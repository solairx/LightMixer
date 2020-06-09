using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using LightMixer.Model.Fixture;
using UIFrameWork;
using BeatDetector;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Practices.Prism;

namespace LightMixer.Model
{
    public class DmxChaser : BaseViewModel, IDisposable
    {
        public BeatDetector.BeatDetector mBpmDetector;
        private SceneService sceneService 
        { 
            get { return BootStrap.UnityContainer.Resolve<SceneService>(); } 
        }
        private SceneRenderedService sceneRenderedService { get { return BootStrap.UnityContainer.Resolve<SceneRenderedService>(); } } 
        public ObservableCollection<FixtureGroup> fixtureGroupCollection = new ObservableCollection<FixtureGroup>();
        private ObservableCollection<EffectBase> _ledEffectCollection = new ObservableCollection<EffectBase>();
        private ObservableCollection<EffectBase> _boothEffectCollection = new ObservableCollection<EffectBase>();
        private ObservableCollection<EffectBase> _movingHeadEffectCollection = new ObservableCollection<EffectBase>();

        private BeatDetector.BeatDetector BeatDetector;
        private string trackName;
        private VDJPoi currentPoi;
        private bool autoChaser;
        private bool useFlashTransition = true;
        private SortableObservableCollection<VDJPoi> pois;
        private VDJPoi selectedPOI;

        public VDJPoi SelectedPOI
        {
            get => selectedPOI;
            set
            {
                selectedPOI = value;
                this.AsyncOnPropertyChange(o => this.SelectedPOI);
            }
        }

        public EffectBase CurrentBoothEffect
        {
            get
            {
                return sceneRenderedService.GetCurrentEffect<RGBLedFixtureCollection>(SceneService.outDoorSceneName, SceneService.poolZoneName);
            }
            set
            {
                SetCurrentEffectAsync<RGBLedFixtureCollection>(SceneService.outDoorSceneName, SceneService.poolZoneName,value);
            }
        }

        public string TrackName
        {
            get => trackName;
            set
            {
                trackName = value;
                this.AsyncOnPropertyChange(o => this.trackName);
            }
        }

        public VDJSong CurrentVdjSong { get; private set; }
        public VdjEvent CurrentVDJEvent { get; private set; }

        public bool AutoChaser
        {
            get => autoChaser; set
            {
                autoChaser = value;
                this.AsyncOnPropertyChange(o => this.AutoChaser);
            }
        }

        public bool UseFlashTransition
        {
            get => useFlashTransition; set
            {
                useFlashTransition = value;
                this.AsyncOnPropertyChange(o => this.UseFlashTransition);
            }
        }

        public SortableObservableCollection<VDJPoi> POIs
        {
            get
            {
                return pois;
            }
            set
            {
                if (pois != value)
                {
                    pois = value;
                    this.AsyncOnPropertyChange(o => this.POIs);
                }
            }
        }


        public VDJPoi CurrentPoi
        {
            get => currentPoi;
            set
            {
                if (currentPoi != value)
                {
                    Dispatcher.Invoke(() =>
                    {
                        currentPoi = value;
                        pois?.ForEach(o => o.IsCurrent = false);
                        currentPoi.IsCurrent = true;
                        this.OnPropertyChanged(o => this.CurrentPoi);
                    });
                }
            }
        }

        public EffectBase CurrentLedEffect
        {
            get
            {
                return sceneRenderedService.GetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName);
            }
            set
            {
                SetCurrentEffectAsync<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, value);
            }
        }

        public EffectBase CurrentMovingHeadEffect
        {
            get
            {
                return sceneRenderedService.GetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName);
            }
            set
            {
                SetCurrentEffectAsync<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, value);
            }
        }

        public ObservableCollection<EffectBase> LedEffectCollection
        {
            get
            {
                return _ledEffectCollection;
            }
            set
            {
                _ledEffectCollection = value;
                this.AsyncOnPropertyChange(o => this.LedEffectCollection);
            }
        }

        public ObservableCollection<EffectBase> BoothEffectCollection
        {
            get
            {
                return _boothEffectCollection;
            }
            set
            {
                _boothEffectCollection = value;
                this.AsyncOnPropertyChange(o => this._boothEffectCollection);
            }
        }

        public ObservableCollection<EffectBase> MovingHeadEffectCollection
        {
            get
            {
                return _movingHeadEffectCollection;
            }
            set
            {
                _movingHeadEffectCollection = value;
                this.AsyncOnPropertyChange(o => this.MovingHeadEffectCollection);
            }
        }

        public DmxChaser()
        {
            mBpmDetector = BootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
            
            BindViewModelWithScene<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, LedEffectCollection, nameof(CurrentLedEffect));
            BindViewModelWithScene<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, MovingHeadEffectCollection, nameof(CurrentMovingHeadEffect));
            BindViewModelWithScene<RGBLedFixtureCollection>(SceneService.outDoorSceneName, SceneService.poolZoneName, BoothEffectCollection, nameof(CurrentBoothEffect));
        }
               

        void Save()
        {
            this.BeatDetector.VirtualDjServer.vdjDataBase.Save();
        }

        public ICommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() => Save());
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (selectedPOI == null)
                    {
                        return;
                    }
                    if (selectedPOI.IsNew)
                    {
                        this.POIs.Remove(selectedPOI);
                    }
                    else
                    {
                        SelectedPOI.IsDeleted = true;
                    }
                });
            }
        }

        public ICommand CreateCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var newPoi = CreatePOI();
                    if (newPoi != null)
                    {
                        this.CurrentVdjSong.Pois.Add(newPoi);
                        this.CurrentVdjSong.Pois.Sort(o => o.Position, System.ComponentModel.ListSortDirection.Ascending);
                    }
                });
            }
        }

     
        private void SetCurrentEffectAsync<T>(string scene, string zone, EffectBase newEffect) where T : FixtureCollection
        {
            Dispatcher.Invoke((Action)(() =>
            {
                sceneRenderedService.SetCurrentEffect<T>((string)scene, (string)zone, (EffectBase)newEffect);
            }));
        }

        private void BindViewModelWithScene<T>(string scene, string zone, ObservableCollection<EffectBase> effectList, string propertyChangedEvent) where T : FixtureCollection
        {
            var fixtureType = sceneService.Scenes
                .First(o => o.Name == scene)
                .Zones.Where(z => z.Name == zone)
                .First().FixtureTypes.OfType<T>()
                .First();
            fixtureType.CurrentEffectChanged += () => AsyncOnPropertyChange(propertyChangedEvent);
            effectList.AddRange(fixtureType.EffectList);
        }

        private VDJPoi CreatePOI()
        {
            var nextId = pois.Max(o => o.ID) + 1;
            var currentPoi = CurrentVDJEvent.GetCurrentPoi;
            var vscanBPM = Convert.ToDouble(CurrentVdjSong?.Scans?.FirstOrDefault()?.Bpm);
            var currentBPM = Convert.ToDouble(CurrentVDJEvent?.BPM);
            var elapsed = Convert.ToDouble(CurrentVDJEvent?.Elapsed);
            double position = 60 / vscanBPM / currentBPM * elapsed / 1000;
            var poi = new VDJPoi(currentPoi.IsBreak ? "End Break " : "Break " + nextId, position.ToString(), "remix", CurrentVdjSong?.Scans?.FirstOrDefault());

            poi.IsDeleted = false;
            poi.IsNew = true;
            return poi;
        }

        public ICommand ReloadCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.BeatDetector.VirtualDjServer.vdjDataBase.Reload();
                });
            }
        }

        public void UpdateVDJUiElement(IEnumerable<VdjEvent> activeDeck)
        {
            if (activeDeck.Count() > 0)
            {
                this.TrackName = activeDeck.FirstOrDefault()?.FileName;
                this.CurrentVdjSong = activeDeck.FirstOrDefault()?.VDJSong;
                this.CurrentVDJEvent = activeDeck.FirstOrDefault();
                this.CurrentPoi = activeDeck.FirstOrDefault()?.GetCurrentPoi;
                this.POIs = activeDeck.FirstOrDefault()?.VDJSong?.Pois;
            }
        }

        public enum LedType
        {
            HeadLed,
            BoothLed,
            MovingHead
        }


        #region IDisposable Members

        public void Dispose()
        {
            mBpmDetector.Stop();
        }

        #endregion


    }
}


