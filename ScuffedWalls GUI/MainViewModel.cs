using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Animations;
using HelixToolkit.Wpf.SharpDX.Assimp;
using Win3D = System.Windows.Media.Media3D;
using HelixToolkit.Wpf.SharpDX.Controls;
using System.Diagnostics;
using HelixToolkit.Wpf.SharpDX.Model.Scene;

namespace ScuffedWalls_GUI
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Setup Viewport
        public EffectsManager EffectsManager { get; }
        public PerspectiveCamera Camera { get; }
        public HelixToolkitScene ModelToWallScene { get; set; }
        public SceneNodeGroupModel3D ModelToWallSceneNodeGroupModel { get; set; } = new SceneNodeGroupModel3D();
        public ObservableCollection<Element3D> ModelToWallGeometry { get; set; } = new ObservableCollection<Element3D>();
        public ObservableCollection<IAnimationUpdater> ModelToWallAnimations { get; } = new ObservableCollection<IAnimationUpdater>();
        private CompositionTargetEx compositeHelper = new CompositionTargetEx();
        private IAnimationUpdater animationUpdater;

        // Materials
        public PBRMaterial DefaultMaterial { get; } = new PBRMaterial()
        {
            EmissiveColor = new SharpDX.Color4(255)
        };

        public MainViewModel()
        {
            EffectsManager = new DefaultEffectsManager();
            Camera = new PerspectiveCamera
            {
                FieldOfView = 90,
                Position = new Win3D.Point3D(0, 1.8, 1)
            };
            
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string info = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        protected bool Set<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        public void LoadModel(string Path)
        {
            ModelToWallAnimations.Clear();
            ModelToWallSceneNodeGroupModel.Clear();
            Importer importer = new Importer();
            ImporterConfiguration importerConfiguration = new ImporterConfiguration { ImportAnimations = true };
            ModelToWallScene = importer.Load(Path, importerConfiguration);
            try
            {
                ModelToWallSceneNodeGroupModel.AddNode(ModelToWallScene.Root);
                foreach (var item in ModelToWallScene.Root.Traverse())
                {
                    if (item is MeshNode meshNode)
                    {
                        ModelToWallGeometry.Add(item);
                    }
                }

                foreach (var animation in ModelToWallScene.Animations.CreateAnimationUpdaters().Values)
                {
                    ModelToWallAnimations.Add(animation);
                }
            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.ToString());
            }
        }

        public void PlayAnimation() => compositeHelper.Rendering += CompositeHelper_Rendering;
        public void StopAnimation() => compositeHelper.Rendering -= CompositeHelper_Rendering;

        private void CompositeHelper_Rendering(object sender, RenderingEventArgs e) => animationUpdater.Update(Stopwatch.GetTimestamp(), Stopwatch.Frequency);
    }
}
