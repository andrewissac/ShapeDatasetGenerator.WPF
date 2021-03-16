using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Sorter
{
    public abstract class BaseSorter : ICustomSorter
    {
        public BaseSorter()
        {
            CaseInsensitiveComparer = new CaseInsensitiveComparer();
        }

        protected CaseInsensitiveComparer CaseInsensitiveComparer { get; set; }
        public ListSortDirection? SortDirection { get; set; }

        public abstract int Compare(object x, object y);
    }
}
