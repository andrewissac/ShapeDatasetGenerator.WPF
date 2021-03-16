//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Structure;
using System;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShapeDatasetGeneratorWPF.Extensions
{
    public static class BitmapSourceExt
    {
        //public static Mat ToMat(this BitmapSource source)
        //{
        //    Mat result = new Mat();
        //    if (source.Format == PixelFormats.Bgra32 | source.Format == PixelFormats.Pbgra32)
        //    {
        //        result.Create(source.PixelHeight, source.PixelWidth, DepthType.Cv8U, 4);
        //    }
        //    else if (source.Format == PixelFormats.Bgr24)
        //    {
        //        result.Create(source.PixelHeight, source.PixelWidth, DepthType.Cv8U, 3);
        //    }
        //    else
        //    {
        //        throw new Exception(String.Format("Convertion from BitmapSource of format {0} is not supported.", source.Format));
        //    }
        //    source.CopyPixels(Int32Rect.Empty, result.DataPointer, result.Step * result.Rows, result.Step);
        //    return result;
        //}
    }
}
