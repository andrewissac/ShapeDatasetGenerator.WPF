//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShapeDatasetGeneratorWPF.Extensions
{
    public static class WriteableBitmapExt
    {
        //public static Mat ToMat(this WriteableBitmap bmp)
        //{
        //    Mat result = new Mat();
        //    result.Create(bmp.PixelHeight, bmp.PixelWidth, DepthType.Cv8U, 4);
        //    bmp.CopyPixels(Int32Rect.Empty, result.DataPointer, result.Step * result.Rows, result.Step);
        //    return result;
        //}

        //public static void DrawRotatedRect(this WriteableBitmap bmp, RotatedRect r, Color color)
        //{
        //    // need to move the min area bounding box, because sometimes its intersecting the contour by 1 pixel
        //    if (r.Size.Width == 0 | r.Size.Height == 0) return;
        //    int shift = 1;
        //    r.Size.Width += shift;
        //    r.Size.Height += shift;
        //    r.Center.X += shift;
        //    r.Center.Y += shift;
        //    var vert = r.GetVertices();
        //    var bottomright = vert[0];
        //    var bottomleft = vert[1];
        //    var topleft = vert[2];
        //    var topright = vert[3];
        //    using (bmp.GetBitmapContext())
        //    {
        //        bmp.DrawLine((int)topleft.X, (int)topleft.Y, (int)topright.X, (int)topright.Y, color);
        //        bmp.DrawLine((int)topright.X, (int)topright.Y, (int)bottomright.X, (int)bottomright.Y, color);
        //        bmp.DrawLine((int)bottomright.X, (int)bottomright.Y, (int)bottomleft.X, (int)bottomleft.Y, color);
        //        bmp.DrawLine((int)bottomleft.X, (int)bottomleft.Y, (int)topleft.X, (int)topleft.Y, color);
        //    }
        //}

        public static void DrawRotatedRect(this WriteableBitmap bmp, System.Windows.Point[] vertices, Color color)
        {
            if(vertices != null)
            {
                var bottomright = vertices[0];
                var bottomleft = vertices[1];
                var topleft = vertices[2];
                var topright = vertices[3];
                using (bmp.GetBitmapContext())
                {
                    bmp.DrawLine((int)topleft.X, (int)topleft.Y, (int)topright.X, (int)topright.Y, color);
                    bmp.DrawLine((int)topright.X, (int)topright.Y, (int)bottomright.X, (int)bottomright.Y, color);
                    bmp.DrawLine((int)bottomright.X, (int)bottomright.Y, (int)bottomleft.X, (int)bottomleft.Y, color);
                    bmp.DrawLine((int)bottomleft.X, (int)bottomleft.Y, (int)topleft.X, (int)topleft.Y, color);
                }
            }
        }

        public static void DrawRectangle(this WriteableBitmap bmp, System.Drawing.Rectangle r, Color color)
        {
            using (bmp.GetBitmapContext())
            {
                if(r.Width > 0 && r.Height > 0)
                bmp.DrawRectangle(r.Left, r.Top, r.Right, r.Bottom, color);
            }
        }

        public static void FillRectangle(this WriteableBitmap bmp, System.Drawing.Rectangle r, Color color, int enlargeRect = 0)
        {
            using (bmp.GetBitmapContext())
            {
                bmp.FillRectangle(r.Left - enlargeRect, r.Top - enlargeRect, r.Right + enlargeRect, r.Bottom + enlargeRect, color);
            }
        }

        //public static void FillRotatedRect(this WriteableBitmap bmp, RotatedRect r, Color color, int enlargeRect = 0)
        //{
        //    int[] vertInt = GetVertices(r, enlargeRect);
        //    using (bmp.GetBitmapContext())
        //    {
        //        bmp.FillPolygon(vertInt, color);
        //    }
        //}

        //private static int[] GetVertices(RotatedRect r, int enlargeRect)
        //{
        //    var v = r.GetVertices();
        //    var vertInt = new int[]
        //    {
        //        (int)v[2].X - enlargeRect, (int)v[2].Y - enlargeRect, // topleft
        //        (int)v[3].X + enlargeRect, (int)v[3].Y - enlargeRect, // topright
        //        (int)v[0].X + enlargeRect, (int)v[0].Y + enlargeRect,  // bottomright
        //        (int)v[1].X - enlargeRect, (int)v[1].Y + enlargeRect, // bottomleft
        //        (int)v[2].X - enlargeRect, (int)v[2].Y - enlargeRect, // topleft (needed to close polygon)
        //    };
        //    return vertInt;
        //}

        // TODO: maybe improve performance by writing directly into an int[] instead of list and then converting to int[]
        public static void FillPolygon(this WriteableBitmap bmp, List<Point> points, Color color)
        {
            if (points != null && points.Any())
            {
                var cHull = new List<int>();
                foreach (var p in points)
                {
                    cHull.Add((int)p.X);
                    cHull.Add((int)p.Y);
                }
                cHull.Add((int)points[0].X);
                cHull.Add((int)points[0].Y);

                using (bmp.GetBitmapContext())
                {
                    bmp.FillPolygon(cHull.ToArray(), color);
                }
            }
        }
    }
}
