//using Emgu.CV;
//using Emgu.CV.CvEnum;
//using Emgu.CV.Structure;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using ShapeDatasetGeneratorWPF.Extensions;
using ShapeDatasetGeneratorWPF.Messages;
using ShapeDatasetGeneratorWPF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using Emgu.CV.ImgHash;
using ShapeDatasetGeneratorWPF.Settings;
using System.Collections.ObjectModel;

namespace ShapeDatasetGeneratorWPF.ViewModel
{
    public class CanvasPageViewModel : ValidatableViewModelBase, INavigationAware
    {

        public CanvasPageViewModel(UserSettings userSettings)
        {
            this.UserSettings = userSettings;
            this.UserSettings.PropertyChanged += PropChanged;

            this.CurrentFrame = BitmapFactory.New(0, 0);
            this.CurrentFrame.Clear(UserSettings.BackgroundColor);
            this.LastFrame = BitmapFactory.New(0, 0);
            this.LastFrame.Clear(UserSettings.BackgroundColor);

            Messenger.Default.Register<ClearCanvasMessage>(this, ClearCanvasMessageReceived);
            Messenger.Default.Register<SaveOutputImagesMessage>(this, SaveOutputMessageReceived);

            this.SetDrawingTimer();
        }

        #region Fields
        private Random _random = new Random();
        private Queue _drawingPoints = new Queue();
        private Timer _drawingTimer;
        private MouseState _initialMouseState = new MouseState();
        #endregion

        #region Properties
        // LastFrame contains only the drawn strokes! No BoundingBoxes are drawn onto it.
        private WriteableBitmap _lastFrame;
        public WriteableBitmap LastFrame
        {
            get => _lastFrame;
            set => Set(ref _lastFrame, value); 
        }

        private WriteableBitmap _currentFrame;
        public WriteableBitmap CurrentFrame
        {
            get => _currentFrame; 
            set => Set(ref _currentFrame, value); 
        }

        private MouseState _mouseState = new MouseState();
        public MouseState MouseState
        {
            get => _mouseState; 
            set => Set(ref _mouseState, value); 
        }

        private UserSettings _userSettings;
        public UserSettings UserSettings
        {
            get => _userSettings; 
            set => Set(ref _userSettings, value); 
        }

        private ObservableCollection<Stroke> _strokes = new ObservableCollection<Stroke>();
        public ObservableCollection<Stroke> Strokes
        {
            get => _strokes;
            set => Set(ref _strokes, value);
        }

        private Stroke _newestStroke = new Stroke();
        public Stroke NewestStroke
        {
            get => _newestStroke;
            set => Set(ref _newestStroke, value);
        }
        #endregion

        #region Methods
        // Main drawing function
        private async void UpdateImg(object sender, ElapsedEventArgs e)
        {
            var points = new List<Point>();
            lock (_drawingPoints.SyncRoot)
            {
                for (int i = 0; i < _drawingPoints.Count; i++)
                {
                    points.Add((Point)_drawingPoints.Dequeue());
                }
            }

            if (points.Count > 0)
            {
                await DispatcherHelper.UIDispatcher.BeginInvoke(new Action(() =>
                {
                    //var roi = new System.Windows.Rect(
                    //    new System.Windows.Point(this.NewestStroke.BB.Rect.X, this.NewestStroke.BB.Rect.Y),
                    //    new System.Windows.Size(this.NewestStroke.BB.Rect.Width, this.NewestStroke.BB.Rect.Height));
                    //this.CurrentFrame.Blit(roi, this.LastFrame, roi, WriteableBitmapExtensions.BlendMode.None);
                    this.CurrentFrame = this.LastFrame;

                    using (this.CurrentFrame.GetBitmapContext())
                    {
                        for (int i = 0; i < points.Count; i++)
                        {
                            this.CurrentFrame.FillEllipseCentered(
                                (int)points[i].X, (int)points[i].Y, (int)this.NewestStroke.StrokeThickness, (int)this.NewestStroke.StrokeThickness, this.UserSettings.SelectedStrokeColor);
                        }
                    }
                    //this.LastFrame.Blit(roi, this.CurrentFrame, roi, WriteableBitmapExtensions.BlendMode.None);
                    this.LastFrame = this.CurrentFrame.Clone();

                    //this.CurrentFrame.FillPolygon(this.NewestStroke.ConvexHull, Colors.Green); //for debug purposes
                    this.DrawRectanglesOnWBmp(this.CurrentFrame);
                }));
            }
        }

        private void DrawRectanglesOnWBmp(WriteableBitmap wBmp)
        {
            wBmp.DrawRectangle(this.NewestStroke.BB.Rect, this.UserSettings.SelectedBoundingBoxColor);
            wBmp.DrawRotatedRect(this.NewestStroke.BB.MinAreaRect, this.UserSettings.SelectedBoundingBoxColor);
            if (this.UserSettings.DrawBoundingBoxes)
            {
                foreach (var stroke in this.Strokes)
                {
                    wBmp.DrawRectangle(stroke.BB.Rect, this.UserSettings.SelectedBoundingBoxColor);
                }
            }
            if (this.UserSettings.DrawMinAreaBoundingBoxes)
            {
                foreach (var stroke in this.Strokes)
                {
                    wBmp.DrawRotatedRect(stroke.BB.MinAreaRect, this.UserSettings.SelectedMinAreaBoundingBoxColor);
                }
            }
        }

        private void ClearCanvasMessageReceived(ClearCanvasMessage obj)
        {
            this.ClearCanvas();
        }

        private void PropChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.UserSettings.Framerate))
            {
                _drawingTimer.Elapsed -= UpdateImg;
                SetDrawingTimer();
            }
            else if(e.PropertyName == nameof(this.UserSettings.BackgroundColor))
            {
                this.ClearCanvas();
            }
        }

        private void SetDrawingTimer()
        {
            _drawingTimer = new Timer(1000 / this.UserSettings.Framerate);
            _drawingTimer.Elapsed += this.UpdateImg;
            _drawingTimer.AutoReset = true;
            _drawingTimer.Enabled = true;
        }

        private void RemoveBoundingBox(Point m)
        {
            var mouse = new System.Drawing.Point((int)m.X, (int)m.Y);
            for (int i = this.Strokes.Count - 1; i >= 0; i--)
            {
                //bool mouseInRect = false;

                //if (this.Strokes[i].BB.Rect.Contains(mouse) || this.Strokes[i].BB.MinAreaRect.MouseInRotatedRect(m))
                //{
                //    mouseInRect = true;
                //}

                //if (mouseInRect)
                //{

                //    if (this.UserSettings.DrawBoundingBoxes)
                //    {
                //        // expand a few pixels to ensure deleting everything
                //        int expandRectByPixels = 2;
                //        this.LastFrame.FillRectangle(this.Strokes[i].BB.Rect, UserSettings.BackgroundColor, expandRectByPixels);
                //    }

                    //if (this.UserSettings.DrawMinAreaBoundingBoxes)
                    //{
                    //    // expand a few pixels to ensure deleting everything
                    //    var mBb = this.Strokes[i].BB.MinAreaRect;
                    //    int expandRectByPixels = 10;
                    //    mBb.Size.Width += expandRectByPixels;
                    //    mBb.Size.Height += expandRectByPixels;
                    //    this.LastFrame.FillRotatedRect(mBb, UserSettings.BackgroundColor);
                    //}

                    //this.Strokes.RemoveAt(i);
                    //var lastFrameWithBoxes = this.LastFrame.Clone();
                    //this.DrawRectanglesOnWBmp(lastFrameWithBoxes);
                    //this.CurrentFrame = lastFrameWithBoxes.Clone();
                    //if (this.UserSettings.DrawBoundingBoxes)
                    //{
                    //    // expand a few pixels to ensure deleting everything
                    //    int expandRectByPixels = 2;
                    //    this.LastFrame.FillRectangle(this.Strokes[i].BB.Rect, UserSettings.BackgroundColor, expandRectByPixels);
                    //}

                    //if (this.UserSettings.DrawMinAreaBoundingBoxes)
                    //{
                    //    // expand a few pixels to ensure deleting everything
                    //    var mBb = this.Strokes[i].BB.MinAreaRect;
                    //    int expandRectByPixels = 10;
                    //    mBb.Size.Width += expandRectByPixels;
                    //    mBb.Size.Height += expandRectByPixels;
                    //    this.LastFrame.FillRotatedRect(mBb, UserSettings.BackgroundColor);
                    //}

                    //this.Strokes.RemoveAt(i);
                    //var lastFrameWithBoxes = this.LastFrame.Clone();
                    //this.DrawRectanglesOnWBmp(lastFrameWithBoxes);
                    //this.CurrentFrame = lastFrameWithBoxes.Clone();
                //}
            }
        }

        private List<Point> GetInterpolatedPoints(Point A, Point B, int stepSize)
        {
            var interpolatedPoints = new List<Point>();
            var vec = Point.Subtract(B, A);
            var vecLength = (int)vec.Length;
            vec.Normalize();

            for (int i = 0; i <= vecLength; i += stepSize)
            {
                interpolatedPoints.Add(Point.Add(A, Vector.Multiply(vec, i)));
            }
            return interpolatedPoints;
        }

        public void ClearCanvas()
        {
            this.CurrentFrame.Clear(UserSettings.BackgroundColor);
            this.LastFrame.Clear(UserSettings.BackgroundColor);
            //this.BoundingBoxes = new ObservableCollection<BoundingBox>();
            this.Strokes = new ObservableCollection<Stroke>();
        }

        private ExportInfo[] GetExportInfos(string folder)
        {
            var files = Directory.GetFiles(folder);
            var exportInfos = new ExportInfo[files.Length];
            for(int i = 0; i < files.Length; i++)
            {
                exportInfos[i] = ExportInfo.GetExportInfoFromFullFileName(files[i]);
            }
            return exportInfos;
        }

        private string[] GetHashesFromFiles(string folder)
        {
            var exportInfos = GetExportInfos(folder);
            var hashes = new string[exportInfos.Length];
            for(int i = 0; i < exportInfos.Length; i++)
            {
                hashes[i] = exportInfos[i].ImgHash;
            }
            return hashes;
        }

        private void SaveOutputMessageReceived(SaveOutputImagesMessage obj)
        {
            //try
            //{
            //    using (var img = this.LastFrame.ToMat().Binarize(127))
            //    {
            //        foreach (var dim in this.UserSettings.OutputImgDimensions)
            //        {
            //            var path = Path.Combine(this.UserSettings.RootOutputPath, this.UserSettings.ShapeName, dim.ToString());

            //            if (UserSettings.ExportBoundingBoxes)
            //            {
            //                var path_ = Path.Combine(path, "BoundingBox");
            //                Directory.CreateDirectory(path_); // only creates dir if it doesn't exist
            //                string[] hashes = this.GetHashesFromFiles(path_);

            //                foreach (var bb in this.BoundingBoxes)
            //                {
            //                    // resized images need to be binarized again
            //                    using (var extractedShape = img.CopyROI(bb.Rect).Resize(dim.Width, dim.Height, Inter.Nearest).ThresholdBinary(new Gray(0), new Gray(255)))
            //                    {
            //                        var hash = extractedShape.GetAverageHash();
            //                        if (!hashes.Contains(hash))
            //                        {
            //                            var exportInfo = new ExportInfo(path_, this.UserSettings.ShapeName, hash, dim, DateTime.Now, ROI.rbb);
            //                            extractedShape.Save(exportInfo.FullFileName);
            //                        }
            //                    }
            //                }
            //            }

            //            if (UserSettings.ExportMinAreaBoundingBoxes)
            //            {
            //                var path_ = Path.Combine(path, "MinAreaBoundingBox");
            //                Directory.CreateDirectory(path_);
            //                string[] hashes = this.GetHashesFromFiles(path_);

            //                foreach (var bb in this.BoundingBoxes)
            //                {
            //                    // resized images need to be binarized again
            //                    using (var extractedShape = img.CopyROI(bb.MinAreaRect).Resize(dim.Width, dim.Height, Inter.Nearest).ThresholdBinary(new Gray(0), new Gray(255)))
            //                    {
            //                        var hash = extractedShape.GetAverageHash();
            //                        if (!hashes.Contains(hash))
            //                        {
            //                            var exportInfo = new ExportInfo(path_, this.UserSettings.ShapeName, hash, dim, DateTime.Now, ROI.rmabb);
            //                            extractedShape.Save(exportInfo.FullFileName);
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        if (UserSettings.ExportOriginalSizeBoundingBoxes || UserSettings.ExportOriginalSizeMinAreaBoundingBoxes)
            //        {
            //            var path = Path.Combine(this.UserSettings.RootOutputPath, this.UserSettings.ShapeName, "OriginalSize");

            //            if (UserSettings.ExportOriginalSizeBoundingBoxes)
            //            {
            //                var path_ = Path.Combine(path, "BoundingBox");
            //                Directory.CreateDirectory(path_);
            //                string[] hashes = this.GetHashesFromFiles(path_);

            //                foreach (var bb in this.BoundingBoxes)
            //                {
            //                    using (var extractedShape = img.CopyROI(bb.Rect))
            //                    {
            //                        var hash = extractedShape.GetAverageHash();
            //                        if (!hashes.Contains(hash))
            //                        {
            //                            var exportInfo = new ExportInfo(path_, this.UserSettings.ShapeName, hash, new OutputImgDimension((int)bb.Rect.Size.Width, (int)bb.Rect.Size.Height), DateTime.Now, ROI.bb);
            //                            extractedShape.Save(exportInfo.FullFileName);
            //                        }
            //                    }
            //                }
            //            }

            //            if (UserSettings.ExportOriginalSizeMinAreaBoundingBoxes)
            //            {
            //                var path_ = Path.Combine(path, "MinAreaBoundingBox");
            //                Directory.CreateDirectory(path_);
            //                string[] hashes = this.GetHashesFromFiles(path_);

            //                foreach (var bb in this.BoundingBoxes)
            //                {
            //                    using (var extractedShape = img.CopyROI(bb.MinAreaRect))
            //                    {
            //                        var hash = extractedShape.GetAverageHash();
            //                        if (!hashes.Contains(hash))
            //                        {
            //                            var exportInfo = new ExportInfo(path_, this.UserSettings.ShapeName, hash, new OutputImgDimension((int)bb.MinAreaRect.Size.Width, (int)bb.MinAreaRect.Size.Height), DateTime.Now, ROI.mabb);
            //                            extractedShape.Save(exportInfo.FullFileName);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    if (UserSettings.ClearCanvasOnSave)
            //    {
            //        this.ClearCanvas();
            //    }
            //}
            //catch(Exception e)
            //{
            //    MessageBox.Show("Failed to save files. Error: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }

        public void OnNavigatedFrom()
        {

        }

        public void OnNavigatedTo()
        {

        }
        #endregion

        #region Commands
        private RelayCommand _previewMouseDownCommand;
        public RelayCommand PreviewMouseDownCommand
        {
            get
            {
                return _previewMouseDownCommand ?? (_previewMouseDownCommand = new RelayCommand(
                    () =>
                    {
                        if (this.MouseState.LeftButton == MouseButtonState.Pressed)
                        {
                            this._initialMouseState = new MouseState(this.MouseState);
                            lock (_drawingPoints.SyncRoot)
                            {
                                this._drawingPoints.Enqueue(this._initialMouseState.Pos);
                            }

                            double strokeThickness = this.UserSettings.RandomizeStrokeThickness ? this._random.Next(this.UserSettings.StrokeThicknessMin, this.UserSettings.StrokeThicknessMax) : this.UserSettings.StrokeThickness;
                            this.NewestStroke = new Stroke(strokethickness: strokeThickness);
                            this.NewestStroke.Points.Add(this._initialMouseState.Pos);
                        }
                    }
                ));
            }
        }

        private RelayCommand _previewMouseUpCommand;
        public RelayCommand PreviewMouseUpCommand
        {
            get
            {
                return _previewMouseUpCommand ?? (_previewMouseUpCommand = new RelayCommand(
                    () => 
                    {
                        if (this.MouseState.ChangedButton == MouseButton.Left & this.MouseState.LeftButton == MouseButtonState.Released)
                        {
                            this.Strokes.Add(new Stroke(this.NewestStroke));
                        }
                        else if (this.MouseState.ChangedButton == MouseButton.Right & this.MouseState.RightButton == MouseButtonState.Released)
                        {
                            this.RemoveBoundingBox(this.MouseState.Pos);
                        }
                        else if (this.MouseState.ChangedButton == MouseButton.Middle & this.MouseState.MiddleButton == MouseButtonState.Released)
                        {
                            Messenger.Default.Send(new FlyoutSettingsMessage());
                        }
                    }
                ));
            }
        }

        private RelayCommand _previewMouseMoveCommand;
        public RelayCommand PreviewMouseMoveCommand
        {
            get
            {
                return _previewMouseMoveCommand ?? (_previewMouseMoveCommand = new RelayCommand(
                    () => 
                    {
                        if (this.MouseState.LeftButton == MouseButtonState.Pressed & this.MouseState.RightButton == MouseButtonState.Released)
                        {
                            lock (_drawingPoints.SyncRoot)
                            {
                                var interPolatedPoints = this.GetInterpolatedPoints(this._initialMouseState.Pos, this.MouseState.Pos, this.UserSettings.InterpolationStepSize);
                                foreach (var p in interPolatedPoints)
                                {
                                    this._drawingPoints.Enqueue(p);
                                }
                            }
                            this._initialMouseState = new MouseState(this.MouseState);
                            this.NewestStroke.Points.Add(this.MouseState.Pos);
                        }
                        else if (this.MouseState.RightButton == MouseButtonState.Pressed & this.MouseState.LeftButton == MouseButtonState.Released)
                        {
                            this.RemoveBoundingBox(this.MouseState.Pos);
                        }
                    }
                ));
            }
        }
        #endregion
    }
}
