//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Structure;
//using Emgu.CV.Util;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;


namespace ShapeDatasetGeneratorWPF.Models
{
    public class BoundingBox : ObservableObject
    {
        private Rectangle _rect;
        public Rectangle Rect
        {
            get => _rect;
            set => Set(ref _rect, value);
        }

        // TODO: implement own RotatedRect
        //private RotatedRect _minAreaRect;
        //public RotatedRect MinAreaRect
        //{
        //    get => _minAreaRect;
        //    set => Set(ref _minAreaRect, value);
        //}


        private System.Windows.Point[] _minAreaRect;
        public System.Windows.Point[] MinAreaRect
        {
            get => _minAreaRect;
            set => Set(ref _minAreaRect, value);
        }

        public BoundingBox() : this(new Rectangle(), new System.Windows.Point[4])
        {
            for(int i = 0; i < this.MinAreaRect.Length; i++)
            {
                this.MinAreaRect[i] = new System.Windows.Point();
            }
        }

        public BoundingBox(Rectangle rect, System.Windows.Point[] minAreaRect)
        {
            this.Rect = rect;
            this.MinAreaRect = minAreaRect;
        }
    }
}
