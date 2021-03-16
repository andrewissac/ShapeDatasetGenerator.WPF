//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.ImgHash;
//using Emgu.CV.Structure;
//using Emgu.CV.Util;
//using ShapeDatasetGeneratorWPF.Models;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ShapeDatasetGeneratorWPF.Extensions
//{
//    public static class EmguExt
//    {
//        /// <summary>
//        /// Copy ROI (Region Of Interest)
//        /// </summary>
//        /// <typeparam name="TColor"></typeparam>
//        /// <typeparam name="TDepth"></typeparam>
//        /// <param name="img"></param>
//        /// <param name="ROI"></param>
//        /// <returns></returns>
//        public static Image<TColor, TDepth> CopyROI<TColor, TDepth>(this Image<TColor, TDepth> img, Rectangle ROI) where TColor : struct, IColor where TDepth : new()
//        {
//            return img.Copy(ROI);
//        }

//        /// <summary>
//        /// Copy ROI (Region Of Interest)
//        /// </summary>
//        /// <typeparam name="TColor"></typeparam>
//        /// <typeparam name="TDepth"></typeparam>
//        /// <param name="img"></param>
//        /// <param name="ROI"></param>
//        /// <returns></returns>
//        public static Image<TColor, TDepth> CopyROI<TColor, TDepth>(this Image<TColor, TDepth> img, RotatedRect boundingBox) where TColor : struct, IColor where TDepth : new()
//        {
//            // rotate them accordingly, sothat object is always vertically aligned
//            boundingBox = RotateBoundingBoxToStraight(boundingBox);
//            return img.Copy(boundingBox);
//        }

//        private static RotatedRect RotateBoundingBoxToStraight(RotatedRect boundingBox)
//        {
//            if (boundingBox.Angle < -45.0F)
//            {
//                boundingBox.Angle += 90.0F;
//                var temp = boundingBox.Size.Height;
//                boundingBox.Size.Height = boundingBox.Size.Width;
//                boundingBox.Size.Width = temp;
//            }
//            return boundingBox;
//        }

//        public static Mat Binarize_(this Mat mat, int threshold)
//        {
//            Mat outputBinary = new Mat();
//            using (var outputGray = new Mat())
//            {
//                CvInvoke.CvtColor(mat, outputGray, ColorConversion.Bgra2Gray);
//                CvInvoke.Threshold(outputGray, outputBinary, threshold, 255, ThresholdType.Binary);
//            }
//            return outputBinary;
//        }

//        public static Image<Gray, byte> Binarize(this Mat mat, int threshold)
//        {
//            var output = mat.ToImage<Gray, byte>();
//            output._ThresholdBinary(new Gray(threshold), new Gray(255));
//            return output;
//        }

//        public static string GetHash(this IInputArray input, ImgHashBase hasher)
//        {
//            return GenerateHash(input, hasher);
//        }

//        public static string GetAverageHash(this IInputArray input)
//        {
//            return GenerateHash(input, new AverageHash());
//        }

//        public static string GetBlockMeanHash(this IInputArray input)
//        {
//            return GenerateHash(input, new BlockMeanHash());
//        }

//        public static string GetColorMomentHash(this IInputArray input)
//        {
//            return GenerateHash(input, new ColorMomentHash());
//        }

//        public static string GetMarrHildrethHash(this IInputArray input)
//        {
//            return GenerateHash(input, new MarrHildrethHash());
//        }

//        public static string GetPHash(this IInputArray input)
//        {
//            return GenerateHash(input, new PHash());
//        }

//        public static string GetRadialVarianceHash(this IInputArray input)
//        {
//            return GenerateHash(input, new RadialVarianceHash());
//        }

//        private static string GenerateHash(IInputArray input, ImgHashBase hasher)
//        {
//            var hashResult = new Mat();
//            hasher.Compute(input, hashResult);
//            // Get the data from the unmanage memeory
//            var data = new byte[hashResult.Width * hashResult.Height];
//            System.Runtime.InteropServices.Marshal.Copy(hashResult.DataPointer, data, 0, hashResult.Width * hashResult.Height);

//            // Concatenate the Hex values representation
//            return BitConverter.ToString(data).Replace("-", string.Empty);
//        }

//        //public static ObservableCollection<BoundingBox> GetBoundingBoxes(this IInputOutputArray sourceImg)
//        //{
//        //    var bb = new ObservableCollection<BoundingBox>();
//        //    using (var contours = new VectorOfVectorOfPoint())
//        //    {
//        //        Mat contourHierarchy = new Mat();
//        //        CvInvoke.FindContours(sourceImg, contours, contourHierarchy, RetrType.External, ChainApproxMethod.ChainApproxNone);
//        //        for (int i = 0; i < contours.Size; i++)
//        //        {
//        //            using (VectorOfPoint contour = contours[i])
//        //            {
//        //                bb.Add(new BoundingBox(CvInvoke.BoundingRectangle(contour), CvInvoke.MinAreaRect(contour)));
//        //            }
//        //        }
//        //    }
//        //    return bb;
//        //}

//    }
//}
