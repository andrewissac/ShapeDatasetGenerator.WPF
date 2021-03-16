using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Helper
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string strToCompare, StringComparison comp = StringComparison.OrdinalIgnoreCase)
        {
            return source?.IndexOf(strToCompare, comp) >= 0;
        }
    }
}
