using GalaSoft.MvvmLight.Messaging;
using ShapeDatasetGeneratorWPF.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Collections;
using GalaSoft.MvvmLight.Threading;
//using Emgu.CV;
//using Emgu.CV.Structure;
using System.IO;
//using Emgu.CV.Util;
//using Emgu.CV.CvEnum;
using ShapeDatasetGeneratorWPF.Extensions;
using ShapeDatasetGeneratorWPF.Models;
using ShapeDatasetGeneratorWPF.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using System.ComponentModel;

namespace ShapeDatasetGeneratorWPF.Views
{

    public partial class CanvasPage : PageBase
    {
        private CanvasPageViewModel vm = SimpleIoc.Default.GetInstance<CanvasPageViewModel>();
        private Size lastImgRenderSize = new Size(0,0);

        public CanvasPage()
        {
            InitializeComponent();
            Messenger.Default.Register<ResizedWindowMessage>(this, OnResizedWindowMessage);
        }

        // TODO: try other solution... this doesn't seem to be robust
        // not pretty using async void... 
        private async void OnResizedWindowMessage(ResizedWindowMessage obj)
        {
            await Task.Delay(100); // needed to wait for the ImgControl to resize before the new RenderSize is set.
            if (!ImgControl.RenderSize.Equals(lastImgRenderSize))
            {
                this.vm.CurrentFrame = this.vm.CurrentFrame.Resize((int)ImgControl.RenderSize.Width, (int)ImgControl.RenderSize.Height, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
                this.lastImgRenderSize = ImgControl.RenderSize;
                this.vm.LastFrame = this.vm.CurrentFrame.Clone();
            }
            Debug.WriteLine("BitmapWidth: " + this.vm.CurrentFrame.PixelWidth, "BitmapHeight: " + this.vm.CurrentFrame.PixelHeight);
        }

        // only needed once
        // SizeChanged event fires everytime the mouse moves while resizing 
        // -> unnecessary resizing of the bitmap if done on every event fire therefore unsubscribe
        private void ImgControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.OnResizedWindowMessage(null);
            this.ImgControl.SizeChanged -= ImgControl_SizeChanged;
        }
    }
}
