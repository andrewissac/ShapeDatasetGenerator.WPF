using System;
using System.Windows;
//using Emgu.CV.Structure;

namespace ShapeDatasetGeneratorWPF.Extensions
{
    public static class RotatedRectExt
    {
        //public static RotatedRect Deepcopy(this RotatedRect r)
        //{
        //    return new RotatedRect
        //        (
        //            new System.Drawing.PointF(r.Center.X, r.Center.Y), 
        //            new System.Drawing.SizeF(r.Size.Width, r.Size.Height), 
        //            r.Angle
        //        );
        //}

        //public static bool MouseInRotatedRect( this RotatedRect r, Point mouse)
        //{
        //    //difference vector from rotation center to mouse
        //    var localMouse = new Vector(mouse.X - r.Center.X, mouse.Y - r.Center.Y).Rotate(-r.Angle);

        //    if (localMouse.X > -r.Size.Width / 2 &&
        //        localMouse.X < r.Size.Width / 2 &&
        //        localMouse.Y > -r.Size.Height / 2 &&
        //        localMouse.Y < r.Size.Height / 2)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        private const double DegToRad = Math.PI / 180;

        private static Vector Rotate(this Vector v, double degrees)
        {
            return v.RotateRadians(degrees * DegToRad);
        }

        public static Vector RotateRadians(this Vector v, double radians)
        {
            var ca = Math.Cos(radians);
            var sa = Math.Sin(radians);
            return new Vector(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
        }
    }
}
