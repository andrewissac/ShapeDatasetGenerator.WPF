using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Helper
{
    public static class UniqueTimeId
    {
        public static string GetUniqueTimeId()
        {
            System.Threading.Thread.Sleep(1); // ensure that the next Id will have a different tick
            return DateTime.Now.Ticks.ToString("x");
        }

        public static string GetUniqueTimeId(DateTime d)
        {
            System.Threading.Thread.Sleep(1); // ensure that the next Id will have a different tick
            return d.Ticks.ToString("x");
        }

        public static DateTime GetDateTimeFromUniqueTimeId(string hex)
        {
            return new DateTime(Convert.ToInt64(hex, 16));
        }
    }
}
