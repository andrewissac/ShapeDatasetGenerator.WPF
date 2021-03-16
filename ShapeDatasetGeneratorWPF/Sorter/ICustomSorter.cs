using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeDatasetGeneratorWPF.Sorter
{
    public interface ICustomSorter : IComparer
    {
        ListSortDirection? SortDirection { get; set; }
    }
}
