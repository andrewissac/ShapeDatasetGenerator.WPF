using GalaSoft.MvvmLight;
using ShapeDatasetGeneratorWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ShapeDatasetGeneratorWPF.Settings
{
    public class UserSettings : ObservableObject
    {
        public UserSettings()
        {
            this.SelectedStrokeColor = pr0Colors["Schrift"];
            this.SelectedMinAreaBoundingBoxColor = pr0Colors["Angenehmes Gruen"];
            this.SelectedBoundingBoxColor = pr0Colors["Bewaehrtes Orange"];
            this.BackgroundColor = pr0Colors["Richtiges Grau"];
            this.OutputImgDimensions = new ObservableCollection<OutputImgDimension> { new OutputImgDimension(64, 64), new OutputImgDimension(128, 128) };
        }

        private string _rootOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public string RootOutputPath
        {
            get { return _rootOutputPath; }
            set { Set(ref _rootOutputPath, value); }
        }

        private string _shapeName = "Circle";
        public string ShapeName
        {
            get { return _shapeName; }
            set { Set(ref _shapeName, value); }
        }

        private int _framerate = 60;
        public int Framerate
        {
            get { return _framerate; }
            set { Set(ref _framerate, value); }
        }

        private int _strokeThickness = 5;
        public int StrokeThickness
        {
            get { return _strokeThickness; }
            set { Set(ref _strokeThickness, value); }
        }

        private int _strokeThicknessMin = 5;
        public int StrokeThicknessMin
        {
            get { return _strokeThicknessMin; }
            set { Set(ref _strokeThicknessMin, value); }
        }

        private int _strokeThicknessMax = 10;
        public int StrokeThicknessMax
        {
            get { return _strokeThicknessMax; }
            set { Set(ref _strokeThicknessMax, value); }
        }

        private int _interpolationStepSize = 1;  // stepSize should be AT LEAST strokeThickness / 2 for it to have a decent effect
        public int InterpolationStepSize
        {
            get { return _interpolationStepSize; }
            set { Set(ref _interpolationStepSize, value); }
        }

        private bool _clearCanvasOnSave = true;
        public bool ClearCanvasOnSave
        {
            get { return _clearCanvasOnSave; }
            set { Set(ref _clearCanvasOnSave, value); }
        }

        private bool _drawBoundingBoxes = true;
        public bool DrawBoundingBoxes
        {
            get { return _drawBoundingBoxes; }
            set { Set(ref _drawBoundingBoxes, value); }
        }

        private bool _exportBoundingBoxes = true;
        public bool ExportBoundingBoxes
        {
            get { return _exportBoundingBoxes; }
            set { Set(ref _exportBoundingBoxes, value); }
        }

        private bool _exportOriginalSizeBoundingBoxes = false;
        public bool ExportOriginalSizeBoundingBoxes
        {
            get { return _exportOriginalSizeBoundingBoxes; }
            set { Set(ref _exportOriginalSizeBoundingBoxes, value); }
        }

        private bool _drawMinAreaBoundingBoxes = true;
        public bool DrawMinAreaBoundingBoxes
        {
            get { return _drawMinAreaBoundingBoxes; }
            set { Set(ref _drawMinAreaBoundingBoxes, value); }
        }

        private bool _exportMinAreaBoundingBoxes = true;
        public bool ExportMinAreaBoundingBoxes
        {
            get { return _exportMinAreaBoundingBoxes; }
            set { Set(ref _exportMinAreaBoundingBoxes, value); }
        }

        private bool _exportOriginalSizeMinAreaBoundingBoxes = false;
        public bool ExportOriginalSizeMinAreaBoundingBoxes
        {
            get { return _exportOriginalSizeMinAreaBoundingBoxes; }
            set { Set(ref _exportOriginalSizeMinAreaBoundingBoxes, value); }
        }

        private bool _randomizeStrokeThickness = false;
        public bool RandomizeStrokeThickness
        {
            get { return _randomizeStrokeThickness; }
            set { Set(ref _randomizeStrokeThickness, value); }
        }

        public Dictionary<string, Color> pr0Colors = new Dictionary<string, Color>()
        {
            { "Richtiges Grau" , (Color)ColorConverter.ConvertFromString("#FF161618") },
            { "Bewaehrtes Orange" , (Color)ColorConverter.ConvertFromString("#FFEE4D2E") },
            { "Altes Pink" , (Color)ColorConverter.ConvertFromString("#FFFF0082") },
            { "Angenehmes Gruen" , (Color)ColorConverter.ConvertFromString("#FF1db992") },
            { "Olivgruen des Friedens" , (Color)ColorConverter.ConvertFromString("#FFBFBC06") },
            { "Mega episches Blau" , (Color)ColorConverter.ConvertFromString("#FF008FFF") },
            { "Gamb" , (Color)ColorConverter.ConvertFromString("#FFFC8833") },
            { "Logo Orange" , (Color)ColorConverter.ConvertFromString("#FFD23C22") },
            { "Schrift" , (Color)ColorConverter.ConvertFromString("#FFF2F5f4") },
            { "Grau" , (Color)ColorConverter.ConvertFromString("#FF888888") },
            { "Link" , (Color)ColorConverter.ConvertFromString("#FF75C0C7") },
            { "Gebannt" , (Color)ColorConverter.ConvertFromString("#FF444444") }
        };

        private Color _selectedStrokeColor;
        public Color SelectedStrokeColor
        {
            get { return _selectedStrokeColor; }
            set { Set(ref _selectedStrokeColor, value); }
        }

        private Color _backgroundColors;
        public Color BackgroundColor
        {
            get { return _backgroundColors; }
            set { Set(ref _backgroundColors, value); }
        }

        private Color _selectedBoundingBoxColor;
        public Color SelectedBoundingBoxColor
        {
            get { return _selectedBoundingBoxColor; }
            set { Set(ref _selectedBoundingBoxColor, value); }
        }

        private Color _selectedMinAreaBoundingBoxColor; 
        public Color SelectedMinAreaBoundingBoxColor
        {
            get { return _selectedMinAreaBoundingBoxColor; }
            set { Set(ref _selectedMinAreaBoundingBoxColor, value); }
        }

        private ObservableCollection<OutputImgDimension> _outputImgDimensions;
        public ObservableCollection<OutputImgDimension> OutputImgDimensions
        {
            get { return _outputImgDimensions; }
            set { Set(ref _outputImgDimensions, value); }
        }

    }
}
