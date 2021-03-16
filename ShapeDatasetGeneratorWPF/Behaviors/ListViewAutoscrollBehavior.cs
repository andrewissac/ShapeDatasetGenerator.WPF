using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ShapeDatasetGeneratorWPF.Behaviors
{
    public class ListViewAutoscrollBehavior : Behavior<ListView>
    {
        private bool _autoScrollEnabled;
        private bool _firstTimeLoaded;

        protected override void OnAttached()
        {
            _autoScrollEnabled = true;
            _firstTimeLoaded = true;
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;

            base.OnAttached();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = GetScrollViewer(AssociatedObject);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged -= OnScrollChanged;
                AssociatedObject.Loaded -= OnLoaded;
                AssociatedObject.Unloaded -= OnUnloaded;
            }

            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs eventArgs)
        {
            var scrollViewer = GetScrollViewer(AssociatedObject);
            if (scrollViewer != null)
                if (_firstTimeLoaded)
                {
                    scrollViewer.ScrollChanged += OnScrollChanged;
                    scrollViewer.ScrollToBottom();
                    _firstTimeLoaded = false;
                }
        }

        private ScrollViewer GetScrollViewer(DependencyObject dependencyObject)
        {
            var queue = new Queue<DependencyObject>(new[] {dependencyObject});

            do
            {
                var item = queue.Dequeue();

                if (item is ScrollViewer)
                    return (ScrollViewer) item;

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
                    queue.Enqueue(VisualTreeHelper.GetChild(item, i));
            } while (queue.Count > 0);

            return null;
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = GetScrollViewer(AssociatedObject);

            if (scroll != null)
            {
                if (IsScrollViewerAtBottom(scroll))
                {
                    scroll.ScrollChanged += ScrollChanged;
                    scroll.ScrollToBottom();
                }
            }
            else
            {
                scroll.ScrollChanged -= ScrollChanged;
            }
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = sender as ScrollViewer;

            // User scroll event : set or unset autoscroll mode
            if (e.ExtentHeightChange == 0) _autoScrollEnabled = IsScrollViewerAtBottom(scroll);

            // Content scroll event : autoscroll eventually
            if (_autoScrollEnabled && e.ExtentHeightChange != 0) scroll.ScrollToBottom();
        }

        private bool IsScrollViewerAtBottom(ScrollViewer scroll)
        {
            // this prevents problematic floating point comparison for equality
            return scroll.ScrollableHeight - scroll.VerticalOffset < 1;
        }
    }
}