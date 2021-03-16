using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ShapeDatasetGeneratorWPF.Helper
{
    // this allows to addRange a whole list to the current list without triggering propertychanged events for each item -> performance improvement!
    public class ObservableCollectionWithAddRange<T> : ObservableCollection<T>
    {
        private bool _suppressNotification = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            _suppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }
            _suppressNotification = false;
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public ObservableCollectionWithAddRange() : base()
        {

        }

        public ObservableCollectionWithAddRange(IEnumerable<T> x) : base(x)
        {

        }

        public ObservableCollectionWithAddRange(List<T> x) : base(x)
        {

        }
    }
}
