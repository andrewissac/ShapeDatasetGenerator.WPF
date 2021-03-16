using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Models
{
    public class OutputImgDimension : ObservableObject, IEquatable<OutputImgDimension>
    {
        public OutputImgDimension() : this(0,0)
        {

        }

        public OutputImgDimension(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        private int _width;
        public int Width
        {
            get { return _width; }
            set { Set(ref _width, value); }
        }

        private int _height;
        public int Height
        {
            get { return _height; }
            set { Set(ref _height, value); }
        }

        public override string ToString()
        {
            return Width.ToString() + "x" + Height.ToString();
        }

        public override int GetHashCode()
        {
            int hashCode = 859600377;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        public bool Equals(OutputImgDimension other)
        {
            if (other == null) return false;
            return other is OutputImgDimension dimension &&
                    Width == dimension.Width &&
                    Height == dimension.Height;
        }
    }
}
