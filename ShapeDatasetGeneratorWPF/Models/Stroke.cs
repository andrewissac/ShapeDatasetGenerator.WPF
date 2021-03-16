using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using ShapeDatasetGeneratorWPF.Helper;
using System.Diagnostics;

namespace ShapeDatasetGeneratorWPF.Models
{
    public class Stroke : ObservableObject
    {
        private ObservableCollection<System.Windows.Point> _points;
        public ObservableCollection<System.Windows.Point> Points
        {
            get => _points;
            set => Set(ref _points, value);
        }

        private List<System.Windows.Point> _convexHull;
        public List<System.Windows.Point> ConvexHull
        {
            get => _convexHull;
            set => Set(ref _convexHull, value);
        }

        private BoundingBox _bb;
        public BoundingBox BB
        {
            get => _bb;
            set => Set(ref _bb, value);
        }

        private double _strokeThickness;
        public double StrokeThickness
        {
            get => _strokeThickness;
            set => Set(ref _strokeThickness, value);
        }

        public Stroke() : this(points: new ObservableCollection<System.Windows.Point>(), strokethickness: 5) { }
        public Stroke(double strokethickness) : this(points: new ObservableCollection<System.Windows.Point>(), strokethickness: strokethickness) { }
        public Stroke(ObservableCollection<System.Windows.Point> points, double strokethickness)
        {
            this.BB = new BoundingBox();
            this.Points = points;
            this.ConvexHull = new List<System.Windows.Point>();
            this.Center = new System.Windows.Point();
            this.StrokeThickness = strokethickness;
            Points.CollectionChanged += OnPointsCollectionChanged;
        }

        public Stroke(Stroke other) // copy constructor
        {
            this.BB = other.BB;
            this.Points = new ObservableCollection<System.Windows.Point>();

            foreach(var p in other.Points)
            {
                this.Points.Add(p);
            }

            this.ConvexHull = new List<System.Windows.Point>();
            foreach(var p in other.ConvexHull)
            {
                this.ConvexHull.Add(p);
            }

            this.Center = other.Center;
            this.StrokeThickness = other.StrokeThickness;
            Points.CollectionChanged += OnPointsCollectionChanged;
        }

        private System.Windows.Point _center;
        public System.Windows.Point Center
        {
            get => _center;
            set => Set(ref _center, value);
        }

        private void OnPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.BB.Rect = this.GetAlignedAxisBoundingBox(this.Points);
            //this.ConvexHull = ExpandConvexHull(ConvexHullCreator.GetConvexHull(this.Points.ToList()));
            this.ConvexHull = ConvexHullCreator.GetConvexHull(this.Points.ToList());
            this.BB.MinAreaRect = this.GetMinAreaBoundingBox(this.ConvexHull);
        }

        private List<System.Windows.Point> ExpandConvexHull(List<System.Windows.Point> convexHull)
        {
            // problem: if shape is not circular -> uneven scaling
            // might try scaling with matrix, but this will scale by a factor. Need to calculate the factor based on the size of convex hull... 
            // goal: expand the convex hull by X amount of pixels ( in this case by the StrokeThickness ) 
            var avgX = convexHull.Sum(p => p.X);
            var avgY = convexHull.Sum(p => p.Y);
            this.Center = new System.Windows.Point(avgX / convexHull.Count, avgY / convexHull.Count);
            var inflate = this.StrokeThickness+1;
            var inflatedConvexHull = new List<System.Windows.Point>();
            foreach (var p in convexHull)
            {
                var vec = new Vector(p.X - Center.X, p.Y - Center.Y);
                vec.Normalize();
                vec *= inflate;
                inflatedConvexHull.Add(Vector.Add(vec, p));
            }
            return inflatedConvexHull;
        }

        private System.Drawing.Rectangle GetAlignedAxisBoundingBox(List<System.Windows.Point> points)
        {
            if (points == null) throw new NullReferenceException();
            var minX = (int)points.Min(p => p.X);
            var minY = (int)points.Min(p => p.Y);
            var maxX = (int)points.Max(p => p.X);
            var maxY = (int)points.Max(p => p.Y);

            var result = new System.Drawing.Rectangle(new System.Drawing.Point(minX, minY), new System.Drawing.Size(maxX - minX, maxY - minY));
            result.Inflate((int)this.StrokeThickness, (int)this.StrokeThickness);
            //Debug.WriteLine(result);
            return result;
        }


        private System.Drawing.Rectangle GetAlignedAxisBoundingBox(ObservableCollection<System.Windows.Point> points)
        {
            return this.GetAlignedAxisBoundingBox(points.ToList());
        }

        private System.Windows.Point[] GetMinAreaBoundingBox(List<System.Windows.Point> convexHull)
        {
            if (convexHull.Count > 1)
            {
                var angles = new List<double>();
                var rotations = new List<Matrix>();
                var areas = new List<double>();
                var minX = new List<int>();
                var minY = new List<int>();
                var maxX = new List<int>();
                var maxY = new List<int>();
                var rotatedConvexHulls = new List<List<System.Windows.Point>>();
                var pi2 = Math.PI / 2;

                for (int i = 0; i < convexHull.Count - 1; i++)
                {
                    double x = convexHull[i + 1].X - convexHull[i].X; // edge-vector
                    double y = convexHull[i + 1].Y - convexHull[i].Y; // edge-vector
                    angles.Add(Math.Abs(Math.Atan2(y, x) % pi2));
                }

                angles = angles.Distinct().ToList(); // only leave unique angles to avoid unnecessary computations

                foreach(var angle in angles)
                {
                    rotations.Add(new Matrix(Math.Cos(angle), -Math.Sin(angle), Math.Sin(angle), Math.Cos(angle), 0, 0));
                }

                for (int i = 0; i < rotations.Count; i++)
                {
                    rotatedConvexHulls.Add(convexHull.Select(p => System.Windows.Point.Multiply(p, rotations[i])).ToList());
                }

            
                foreach(var rotatedCHull in rotatedConvexHulls)
                {
                    var minX_ = (int)rotatedCHull.Min(p => p.X);
                    var minY_ = (int)rotatedCHull.Min(p => p.Y);
                    var maxX_ = (int)rotatedCHull.Max(p => p.X);
                    var maxY_ = (int)rotatedCHull.Max(p => p.Y);

                    minX.Add(minX_);
                    minY.Add(minY_);
                    maxX.Add(maxX_);
                    maxY.Add(maxY_);
                    areas.Add((maxX_ - minX_) * (maxY_ - minY_));
                }

                var indexOfSmallestRect = areas.IndexOfMin();
                var inflate = this.StrokeThickness + 2;

                var smallestRectAngle = angles[indexOfSmallestRect];
                var rotBack = new Matrix(Math.Cos(-smallestRectAngle), -Math.Sin(-smallestRectAngle), Math.Sin(-smallestRectAngle), Math.Cos(-smallestRectAngle), 0, 0);
                var bottomright = System.Windows.Point.Multiply(new System.Windows.Point(maxX[indexOfSmallestRect] + inflate, maxY[indexOfSmallestRect] + inflate), rotBack);
                var bottomleft = System.Windows.Point.Multiply(new System.Windows.Point(minX[indexOfSmallestRect] - inflate, maxY[indexOfSmallestRect] + inflate), rotBack);
                var topleft = System.Windows.Point.Multiply(new System.Windows.Point(minX[indexOfSmallestRect] - inflate, minY[indexOfSmallestRect] - inflate), rotBack);
                var topright = System.Windows.Point.Multiply(new System.Windows.Point(maxX[indexOfSmallestRect] + inflate, minY[indexOfSmallestRect] - inflate), rotBack);

                //var size = new SizeF((float)(bottomright.X - topleft.X), (float)(bottomright.Y - topleft.Y));
                //var center = new PointF((float)(topleft.X + size.Width / 2), (float)(topleft.Y + size.Height / 2));

                var vertices = new System.Windows.Point[4];
                // order is based on RotatedRect Vertices from EmguCV
                vertices[0] = bottomright;
                vertices[1] = bottomleft;
                vertices[2] = topleft;
                vertices[3] = topright;

                //var rect = new Emgu.CV.Structure.RotatedRect(center, size, (float)-smallestRectAngle);
                //Debug.WriteLine("center: " + center.ToString() + " - size (w x h): " + size.ToString() + " - angle: " + (smallestRectAngle * (180 / Math.PI)).ToString());
                return vertices;
            }
            else
            {
                return null;
            }
        }
    }
}
