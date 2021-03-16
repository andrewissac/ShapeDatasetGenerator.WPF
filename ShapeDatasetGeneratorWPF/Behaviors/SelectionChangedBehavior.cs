using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ShapeDatasetGeneratorWPF.Behaviors
{
    public class SelectionChangedBehaviour
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(SelectionChangedBehaviour), new PropertyMetadata(PropertyChangedCallback));

        public static void PropertyChangedCallback(DependencyObject depObj, DependencyPropertyChangedEventArgs args)
        {
            var selector = (Selector) depObj;
            if (selector != null) selector.SelectionChanged += SelectionChanged;
        }

        public static ICommand GetCommand(UIElement element)
        {
            return (ICommand) element.GetValue(CommandProperty);
        }

        public static void SetCommand(UIElement element, ICommand command)
        {
            element.SetValue(CommandProperty, command);
        }

        private static void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = (Selector) sender;
            if (selector != null)
            {
                var command = selector.GetValue(CommandProperty) as ICommand;
                if (command != null) command.Execute(selector.SelectedItem);
            }
        }
    }
}