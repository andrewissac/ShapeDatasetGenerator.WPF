using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Helper
{
    public static class ListExtension
    {
        public static int IndexOfMin(this IList<double> self)
        {
            if (self == null)
            {
                throw new ArgumentNullException("self");
            }

            if (self.Count == 0)
            {
                throw new ArgumentException("List is empty.", "self");
            }

            var min = self[0];
            int minIndex = 0;

            for (int i = 1; i < self.Count; ++i)
            {
                if (self[i] < min)
                {
                    min = self[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }
    }
}
