using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Extensions
{
    public static class SystemDrawingRectangleExt
    {
        public static System.Drawing.Rectangle Deepcopy( this System.Drawing.Rectangle r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }
    }
}
