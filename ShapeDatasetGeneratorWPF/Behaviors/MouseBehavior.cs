using ShapeDatasetGeneratorWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ShapeDatasetGeneratorWPF.Behaviors
{
    public class MouseBehavior : System.Windows.Interactivity.Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty MouseStateProperty = DependencyProperty.Register(
            "MouseState", typeof(MouseState), typeof(MouseBehavior), new FrameworkPropertyMetadata(new MouseState()));

        public MouseState MouseState
        {
            get { return (MouseState)GetValue(MouseStateProperty); }
            set { SetValue(MouseStateProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
            AssociatedObject.PreviewMouseDown += AssociatedObjectOnPreviewMouseDown;
            AssociatedObject.PreviewMouseUp += AssociatedObjectOnPreviewMouseUp;
        }


        private void AssociatedObjectOnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GetMouseState(e);
        }


        private void AssociatedObjectOnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            GetMouseState_(e);
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs e)
        {
            GetMouseState(e);
        }

        private void GetMouseState(MouseEventArgs e)
        {
            MouseState.Pos = e.GetPosition(AssociatedObject);
            MouseState.LeftButton = e.LeftButton;
            MouseState.RightButton = e.RightButton;
            MouseState.MiddleButton = e.MiddleButton;
        }

        private void GetMouseState_(MouseButtonEventArgs e)
        {
            GetMouseState(e);
            MouseState.ChangedButton = e.ChangedButton;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
            AssociatedObject.PreviewMouseDown -= AssociatedObjectOnPreviewMouseDown;
            AssociatedObject.PreviewMouseUp -= AssociatedObjectOnPreviewMouseUp;
        }
    }
}
