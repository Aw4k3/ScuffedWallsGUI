using ScuffedWalls_GUI.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Assimp;
using WinForms = System.Windows.Forms;
using System.Windows.Media;
using Win3D = System.Windows.Media.Media3D;

namespace ScuffedWalls_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class ScuffedEvent
    {
        public enum SFUNCTION_TYPE
        {
            TextToWall,
            ModelToWall,
            ImageToWall,
            Environment,
            ClonerFromWorkspace,
            Blackout,
            AppendToAllWallsBetween,
            AppendToAllNotesBetween,
            AppendToAllEventsBetween,
            Import,
            Run,
            Wall,
            Note,
            AnimateTrack,
            AssignPathAnimation,
            AssignPlayerToTrack,
            ParentTrack,
            PointDefinition
        }

        public SFUNCTION_TYPE SFunctionType { get; set; }
        public SFunction ScuffedFunction { get; set; }

        public ScuffedEvent(SFUNCTION_TYPE ScuffedFunc, SFunction sFunc)
        {
            SFunctionType = ScuffedFunc;
            ScuffedFunction = sFunc;
        }
    }

    public partial class MainWindow : Window
    {
        /* GLOBAL VARIABLES */
        List<ScuffedEvent> scuffedEvents = new List<ScuffedEvent>();
        string ScuffedFilePath;

        /* MAIN CODE */
        public MainWindow()
        {
            InitializeComponent();
            Outliner.ItemsSource = scuffedEvents;
            DragDropFileOverlay.Visibility = Visibility.Visible;
            MainViewModel ViewportViewModel = new MainViewModel();
            Viewport.DataContext = new MainViewModel();
            Viewport.Items.Add(new DirectionalLight3D()
            {
                Direction = new Win3D.Vector3D(-0.3, -0.5, -1),
                Color = Colors.White
            });
            ViewportViewModel.LoadModel(@"C:\Program Files (x86)\Steam\steamapps\common\Beat Saber\Beat Saber_Data\CustomWIPLevels\Suzua\Spooral.dae");
            foreach (Element3D component in ViewportViewModel.ModelToWallGeometry)
            {
                Viewport.Items.Add((MeshGeometryModel3D)component.SceneNode.Items[1]);
            }
        }

        /* GLOBAL KEY BINDS */
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.O)
            {
                OpenScuffedWallsFile();
            }
            
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.S)
            {
                SaveScuffeedWallsFile();
            }

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.S)
            {
                SaveScuffeedWallsFile(true);
            }
            
        }

        #region FUNCTIONS
        /*
        private float[] ParseFloatArray(string Arr, int Size)
        {
            float[] parsed = new float[Size];
            Regex Delimiter = new Regex(@"(\[|\]| )+");
            Arr = Delimiter.Replace(Arr, "");
            string[] split = Arr.Split(',');
            for (int i = 0; i < Size; i++) { parsed[i] = float.Parse(split[i]); }
            return parsed;
        }
        */
        private void OpenScuffedWallsFile(string Filepath = "")
        {
            WinForms.OpenFileDialog openFileDialog = new WinForms.OpenFileDialog { Filter = "Scuffed Walls (.sw)|*.sw" };

            if (Filepath == "")
            {
                if (openFileDialog.ShowDialog() == WinForms.DialogResult.OK)
                {
                    ScuffedFilePath = openFileDialog.FileName;
                }
            }
            else
            {
                ScuffedFilePath = Filepath;
            }

            scuffedEvents.Clear();

            string[] SData = File.ReadAllLines(ScuffedFilePath);

            Regex REGEX_SFUNCTION = new Regex("([0-9]+: [A-Z|a-z]+)|([0-9]+:[A-Z|a-z]+)");

            for (int i = 0; i < SData.Length; i++) // For every line in the Scuffed Walls file
            {
                if (REGEX_SFUNCTION.IsMatch(SData[i])) // If the line is a function
                {
                    string[] FunctionInfo = SData[i].Split(':'); // Split line into Beat and Function
                    List<string> FunctionBlock = new List<string>();
                    switch (FunctionInfo[1].Trim()) // What type of function is it?
                    {
                        case "Import":
                            Import import = new Import(float.Parse(FunctionInfo[0]));
                            
                            for (int j = i; j < SData.Length; j++)
                            {
                                if (SData[j].ToLower().Contains("path")) { import.ResolvePath(SData[j].Split(':')[1]); }
                                if (SData[j] == "") { break; }
                            }

                            scuffedEvents.Add(new ScuffedEvent(ScuffedEvent.SFUNCTION_TYPE.Import, import));
                            break;

                        case "TextToWall":
                            TextToWall textToWall = new TextToWall(float.Parse(FunctionInfo[0])); // Create new SFuncion
                            for (int j = i + 1; j < SData.Length; j++) { if (!REGEX_SFUNCTION.IsMatch(SData[j])) { FunctionBlock.Add(SData[j]); } else { break; } }
                            textToWall.ReadFunctionBlock(FunctionBlock);
                            /*
                            for (int j = i; j < SData.Length; j++)
                            {
                                if (SData[j].ToLower().Contains("path:")) { textToWall.ResolvePath(SData[j].Split(':')[1]); } // Get font Path
                                if (SData[j].ToLower().Contains("line:")) { textToWall.Lines.Add(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("letting:")) { textToWall.Letting = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("leading:")) { textToWall.Leading = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("size:")) { textToWall.Size = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("thicc:")) { textToWall.Thicc = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("duration:")) { textToWall.Duration = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("definitedurationbeats:")) { textToWall.DefiniteDurationInBeats = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("definitedurationseconds:")) { textToWall.DefiniteDurationInSeconds = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("definitetime:")) { textToWall.DefiniteTime = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("position:")) { textToWall.Position = ParseFloatArray(SData[j].Split(':')[1], 2); }
                                if (SData[j] == "") { break; }
                            }
                            */
                            scuffedEvents.Add(new ScuffedEvent(ScuffedEvent.SFUNCTION_TYPE.TextToWall, textToWall)); // Add SFunction to SEvent List
                            break;

                        case "ModelToWall":
                            ModelToWall modelToWall = new ModelToWall(float.Parse(FunctionInfo[0]));
                            for (int j = i + 1; j < SData.Length; j++) { if (!REGEX_SFUNCTION.IsMatch(SData[j])) { FunctionBlock.Add(SData[j]); } else { break; } }
                            modelToWall.ReadFunctionBlock(FunctionBlock);
                            /*
                            for (int j = i; j < SData.Length; j++)
                            {
                                if (SData[j].ToLower().Contains("path:")) { modelToWall.ResolvePath(SData[j].Split(':')[1]); } // Get Model Path
                                if (SData[j].ToLower().Contains("hasanimation:")) { modelToWall.HasAnimation = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("duration:")) { modelToWall.Duration = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("definitedurationbeats:")) { modelToWall.DefiniteDurationInBeats = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("definitedurationseconds:")) { modelToWall.DefiniteDurationInSeconds = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("spreadspawntime:")) { modelToWall.SpreadSpawnTime = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("normal:")) { modelToWall.Normal = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("createtracks:")) { modelToWall.CreateTracks = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("colormult:")) { modelToWall.ColorMult = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("preservetime:")) { modelToWall.PreserveTime = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("cameratoplayer:")) { modelToWall.CameraToPlayer = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("createnotes:")) { modelToWall.CreateNotes = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("spline:")) { modelToWall.Spline = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("type:")) { modelToWall.Type = int.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("alpha:")) { modelToWall.Alpha = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("thicc:")) { modelToWall.Thicc = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("deltaposition:")) { modelToWall.DeltaPosition = ParseFloatArray(SData[j].Split(':')[1], 3); }
                                if (SData[j].ToLower().Contains("deltarotaion:")) { modelToWall.DeltaRotation = ParseFloatArray(SData[j].Split(':')[1], 3); }
                                if (SData[j].ToLower().Contains("deltascale:")) { modelToWall.DeltaScale = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("setdeltaposition:")) { modelToWall.SetDeltaPosition = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("setdeltascale:")) { modelToWall.SetDeltaScale = bool.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("repeat:")) { modelToWall.Repeat = int.Parse(SData[j].Split(':')[1]); }
                                if (SData[j].ToLower().Contains("repeataddtime:")) { modelToWall.RepeatAddTime = float.Parse(SData[j].Split(':')[1]); }
                                if (SData[j] == "" ) { break; }
                            }
                            */
                            scuffedEvents.Add(new ScuffedEvent(ScuffedEvent.SFUNCTION_TYPE.ModelToWall, modelToWall));
                            break;
                    }
                }
            }

            Outliner.Items.Refresh();
            DragDropFileOverlay.Visibility = Visibility.Collapsed;
            StatusLabel.Content = "Scuffed Walls file loaded!";
        }

        private void SaveScuffeedWallsFile(bool SaveAs = false)
        {
            string SaveLocation = ScuffedFilePath;

            if (SaveAs)
            {
                WinForms.SaveFileDialog saveFileDialog = new WinForms.SaveFileDialog { Filter = "Scuffed Walls File (.sw)|*.sw" };

                if (saveFileDialog.ShowDialog() == WinForms.DialogResult.OK) { SaveLocation = saveFileDialog.FileName; }
            }

            List<string> Lines = new List<string>
            {
                "# ScuffedWalls v1.0.1",
                "",
                "# Documentation on functions can be found at",
                "# https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md",
                "",
                "# DM @thelightdesigner#1337 for more help?",
                "",
                "# Using this tool requires an understanding of Noodle Extensions.",
                "# https://github.com/Aeroluna/NoodleExtensions/blob/master/Documentation/AnimationDocs.md",
                "",
                "# Playtest your maps",
                "",
                "Workspace:Default",
                ""
            };

            for (int i = 0; i < scuffedEvents.Count; i++)
            {
                Lines.AddRange(scuffedEvents[i].ScuffedFunction.ToScuffedFormat());
                Lines.Add("");
            }

            File.WriteAllLines(SaveLocation, Lines);

            StatusLabel.Content = "Scuffed Walls file saved!";
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e) => OpenScuffedWallsFile();

        private void SaveFile_Click(object sender, RoutedEventArgs e) => SaveScuffeedWallsFile();

        private void SaveFileAs_Click(object sender, RoutedEventArgs e) => SaveScuffeedWallsFile(true);

        private void BeatNumberLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Outliner.SelectedIndex > -1)
            {
                EditTextField NewBeat = new EditTextField();

                if (NewBeat.ShowDialog() == true)
                {
                    float.TryParse(NewBeat.Text, out float parsed);
                    scuffedEvents[Outliner.SelectedIndex].ScuffedFunction.BeatNumber = parsed;
                    Outliner.Items.Refresh();
                }
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (((string[])e.Data.GetData(DataFormats.FileDrop))[0].EndsWith(".sw"))
                {
                    OpenScuffedWallsFile(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
                }
            }
        }

        private void Window_DragLeave(object sender, DragEventArgs e) => DragDropFileOverlay.Visibility = Visibility.Collapsed;

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (((string[])e.Data.GetData(DataFormats.FileDrop))[0].EndsWith(".sw"))
            {
                DDOverlayText.Content = "Scuffed Walls File (.sw)";
            }
            else
            {
                DDOverlayText.Content = "That's not a Scuffed Walls file you pepega!";
            }

            DragDropFileOverlay.Visibility = Visibility.Visible;
        }

        private void Outliner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (scuffedEvents[Outliner.SelectedIndex].SFunctionType)
            {
                case ScuffedEvent.SFUNCTION_TYPE.ModelToWall:
                    {
                        /* Load Data into property Editor */
                        ModelToWall temp = (ModelToWall)scuffedEvents[Outliner.SelectedIndex].ScuffedFunction;
                        PROPERTY_BeatNumber.Text = temp.BeatNumber.ToString();
                        if (temp.Path != null) { PROPERTY_Path.Text = temp.Path; }
                        if (temp.FullPath != null) { PROPERTY_Path.Text = temp.FullPath; }

                        /* Load Model into Viewport */
                        Importer importer = new Importer();
                        //HelixToolkitScene preview = importer.Load(Path.GetDirectoryName(ScuffedFilePath) + "\\" + temp.Path);
                        break;
                    }

                case ScuffedEvent.SFUNCTION_TYPE.TextToWall:
                    {
                        TextToWall temp = (TextToWall)scuffedEvents[Outliner.SelectedIndex].ScuffedFunction;
                        PROPERTY_BeatNumber.Text = temp.BeatNumber.ToString();
                        if (temp.Path != null) { PROPERTY_Path.Text = temp.Path; }
                        if (temp.FullPath != null) { PROPERTY_Path.Text = temp.FullPath; }
                        break;
                    }
            }
        }
        #endregion
    }
}
