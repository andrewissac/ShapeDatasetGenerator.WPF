//using Emgu.CV.CvEnum;
using ShapeDatasetGeneratorWPF.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Models
{
    public class ExportInfo
    {
        public ExportInfo()
        {

        }

        public ExportInfo(string dir, string shapeName, string imgHash, OutputImgDimension dim, DateTime d, ROI type)
        {
            this.OutputDirectory = dir;
            this.ShapeName = shapeName;
            this.ImgHash = imgHash;
            this.OutputImgDimension = dim;
            this.ExportDateTime = d;
            this.ROIType = type;
            this.FileName = this.BuildFileName();
        }

        private string _outputDirectory;
        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set
            {
                if (!string.IsNullOrWhiteSpace(this.FileName))
                {
                    var fullFileName = Path.Combine(value, this.FileName);
                    if (this.FullFileName != fullFileName) this.FullFileName = fullFileName;
                }
                _outputDirectory = value;
            }
        }
        public string ShapeName { get; set; }
        public string ImgHash { get; set; }
        public OutputImgDimension OutputImgDimension { get; set; }
        public DateTime ExportDateTime { get; set; }
        public ROI ROIType { get; set; }
        private string __fileName;
        public string FileName
        {
            get { return __fileName; }
            set
            {
                if(!string.IsNullOrWhiteSpace(this.OutputDirectory))
                {
                    var fullFileName = Path.Combine(this.OutputDirectory, value);
                    if (this.FullFileName != fullFileName) this.FullFileName = fullFileName;
                }
                __fileName = value;
            }
        }
        public string FullFileName { get; set; }

        public static OutputImgDimension GetOutputImgDimensionFromFileName(string filename)
        {
            return ExportInfo.GetExportInfoFromFileName(filename).OutputImgDimension;
        }

        public static DateTime GetExportDateTimeFromFileName(string filename)
        {
            return ExportInfo.GetExportInfoFromFileName(filename).ExportDateTime;
        }

        public static string GetShapeNameFromFileName(string filename)
        {
            return ExportInfo.GetExportInfoFromFileName(filename).ShapeName;
        }

        public static string GetImgHashFromFileName(string filename)
        {
            return ExportInfo.GetExportInfoFromFileName(filename).ImgHash;
        }

        public static ROI GetROITypeFromFileName(string filename)
        {
            return ExportInfo.GetExportInfoFromFileName(filename).ROIType;
        }

        public string BuildFileName()
        {
            return this.ShapeName + "_" + (int)OutputImgDimension.Width + "x" + (int)OutputImgDimension.Height + "_" + ROIType.ToString() + "_" + this.ImgHash + "_" + UniqueTimeId.GetUniqueTimeId() + "_.png";
        }

        public static ExportInfo GetExportInfoFromFileName(string f)
        {
            var substring = f.Split('_');

            var e = new ExportInfo();
            e.FileName = f;
            e.ShapeName = substring[0];

            var dimSubString = substring[1].Split('x');
            e.OutputImgDimension = new OutputImgDimension(Convert.ToInt32(dimSubString[0]), Convert.ToInt32(dimSubString[1]));

            Enum.TryParse(substring[2], out ROI roi);
            e.ROIType = roi;

            e.ImgHash = substring[3];

            e.ExportDateTime = UniqueTimeId.GetDateTimeFromUniqueTimeId(substring[4]);

            return e;
        }

        public static ExportInfo GetExportInfoFromFullFileName(string f)
        {
            var fileName = Path.GetFileName(f);
            var e = ExportInfo.GetExportInfoFromFileName(fileName);
            e.OutputDirectory = Path.GetDirectoryName(f);
            return e;
        }

        public override string ToString()
        {
            return "OutputDirectory: " + this.OutputDirectory + Environment.NewLine +
                "FileName: " + this.FileName + Environment.NewLine +
                "FullFileName: " + this.FullFileName + Environment.NewLine +
                "ShapeName: " + this.ShapeName + Environment.NewLine +
                "ImgHash: " + this.ImgHash + Environment.NewLine +
                "OutputImgDimension: " + this.OutputImgDimension.ToString() + Environment.NewLine +
                "ExportDateTime: " + UniqueTimeId.GetUniqueTimeId(this.ExportDateTime) + Environment.NewLine +
                "ROIType: " + this.ROIType.ToString();
        }

    }

    public enum ROI
    {
        bb, // bounding box
        rbb, // resized bb
        mabb, // minimal area bb
        rmabb, // resized mabb
    }
}
