using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ShapeDatasetGeneratorWPF.Helper;

namespace ShapeDatasetGeneratorWPF.Models
{
    public class MouseState : NotifyObject
    {

        public MouseState() : this(0, 0, MouseButtonState.Released, MouseButtonState.Released, MouseButtonState.Released, null)
        {

        }

        public MouseState(double mx, double my, MouseButtonState left, MouseButtonState right, MouseButtonState middle, MouseButton? changed)
        {
            this.Pos = new Point(mx, my);
            this.LeftButton = left;
            this.RightButton = right;
            this.MiddleButton = middle;
            this.ChangedButton = changed;
        }

        // copy constructor
        public MouseState(MouseState toCopy)
        {
            this.Pos = new Point(toCopy.Pos.X, toCopy.Pos.Y);
            this.LeftButton = toCopy.LeftButton;
            this.RightButton = toCopy.RightButton;
            this.MiddleButton = toCopy.MiddleButton;
            this.ChangedButton = toCopy.ChangedButton;
        }

        private Point _pos;
        public Point Pos
        {
            get { return _pos; }
            set { Set(ref _pos, value); }
        }

        private MouseButtonState _leftButton;
        public MouseButtonState LeftButton
        {
            get { return _leftButton; }
            set { Set(ref _leftButton, value); }
        }

        private MouseButtonState _rightButton;
        public MouseButtonState RightButton
        {
            get { return _rightButton; }
            set { Set(ref _rightButton, value); }
        }

        private MouseButtonState _middleButton;
        public MouseButtonState MiddleButton
        {
            get { return _middleButton; }
            set { Set(ref _middleButton, value); }
        }

        private MouseButton? _changedButton;
        public MouseButton? ChangedButton
        {
            get { return _changedButton; }
            set { Set(ref _changedButton, value); }
        }

        public override string ToString()
        {
            return "MouseX: " + Pos.X + " MouseY: " + Pos.Y + " LeftMouse: " + LeftButton + " RightMouse: " + RightButton + " ChangedButton: " + ChangedButton;
        }
    }
}
