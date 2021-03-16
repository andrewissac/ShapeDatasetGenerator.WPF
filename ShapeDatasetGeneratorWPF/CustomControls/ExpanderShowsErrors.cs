using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ShapeDatasetGeneratorWPF.CustomControls
{
    public class ExpanderShowsErrors : Expander
    {

        public static readonly DependencyProperty HasErrorsProperty = DependencyProperty.Register("HasErrors", typeof(bool), typeof(ExpanderShowsErrors), new PropertyMetadata(false));

        public bool HasErrors
        {
            get { return (bool)GetValue(HasErrorsProperty); }
            set { SetValue(HasErrorsProperty, value); }
        }

        private ObservableCollection<string> errors { get; set; }

        public ExpanderShowsErrors()
        {
            this.errors = new ObservableCollection<string>();

            Validation.AddErrorHandler(this, (s, args) => {
                if (args.Action == ValidationErrorEventAction.Added)
                {
                    this.errors.Add(args.Error.ErrorContent.ToString());
                   // this.ToolTip = args.Error.ErrorContent;
                    HasErrors = true;
                }
                else if(args.Action == ValidationErrorEventAction.Removed)
                {;
                    this.errors.RemoveAt(this.errors.Count - 1);
                    if(errors.Count < 1)
                    {
                        //this.ToolTip = null;
                        HasErrors = false;
                    }
                    else
                    {
                        //this.ToolTip = this.errors.Last();
                    }
                }
            });
        }
    }

}
