﻿using BeatDetector;
using LightMixer.Model.Fixture;
using Unity;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UIFrameWork;

namespace LightMixer.Model
{
    public class DmxChaser : BaseViewModel, IDisposable
    {

        public BeatDetector.BeatDetector mBpmDetector;
        public event EventHandler PoisChanged;
        public event EventHandler PosChanged;

        private SceneService sceneService
        {
            get { return LightMixerBootStrap.UnityContainer.Resolve<SceneService>(); }
        }

        private SceneRenderedService sceneRenderedService
        { get { return LightMixerBootStrap.UnityContainer.Resolve<SceneRenderedService>(); } }
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
        private bool useDarkMode;
        private bool useLightMode;
        private AutomatedEffect currentAutomationEffect;
        private string currentSongPosition;
        private bool useAutomation;
        private bool useZPlane;
        private VDJSong currentVdjSong;

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
                SetCurrentEffectAsync<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, value);
            }
        }

        public string TrackName
        {
            get => trackName;
            set
            {
                if (trackName != value)
                {
                    trackName = value;
                    this.AsyncOnPropertyChange(o => this.TrackName);
                }
            }
        }

        public VDJSong CurrentVdjSong 
        {   get => currentVdjSong;
            set 
            {
                if (currentVdjSong != value)
                {
                    currentVdjSong = value;
                    PoisChanged?.Invoke(this, new EventArgs());
                }
                
            }
        }
        public VdjEvent CurrentVDJEvent { get; private set; }

        public bool AutoChaser
        {
            get => autoChaser; set
            {
                if (autoChaser != value)
                {
                    autoChaser = value;
                    this.AsyncOnPropertyChange(o => this.AutoChaser);
                }
            }
        }

        public bool UseFlashTransition
        {
            get => useFlashTransition;
            set
            {
                if (useFlashTransition != value)
                {
                    useFlashTransition = value;
                    this.AsyncOnPropertyChange(o => this.UseFlashTransition);
                }
            }
        }

        public bool UseDarkMode
        {
            get => useDarkMode;
            set
            {
                ShellyDimmerFixture.UseDarkMode = value;
                if (useDarkMode != value)
                {
                    useDarkMode = value;
                    this.AsyncOnPropertyChange(o => this.UseDarkMode);
                }
            }
        }

        public bool UseLightMode
        {
            get => useLightMode;
            set
            {
                if (useLightMode != value)
                {
                    useLightMode = value;
                    this.AsyncOnPropertyChange(o => this.UseLightMode);
                }
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
                    this.PoisChanged?.Invoke(this, new EventArgs());
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
                    LightMixerBootStrap.Dispatcher.Invoke(() =>
                    {
                        currentPoi = value;
                        foreach (var poi in pois)
                        {
                            poi.IsCurrent = false;
                        }
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

                SetCurrentEffectAsync<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, value);
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

        public string CurrentSongPosition
        {
            get => currentSongPosition;
            set
            {
                this.AsyncOnPropertyChange(o => this.CurrentSongPosition);
                currentSongPosition = value;
                this.PosChanged?.Invoke(this, EventArgs.Empty); 
            }
        }

        public bool UseAutomation
        {
            get => useAutomation;
            set
            {
                if (value != useAutomation)
                {
                    useAutomation = value;
                    var song = this.CurrentVdjSong;
                    if (song != null)
                    {
                        song.UseAutomation = value;
                    }

                    this.AsyncOnPropertyChange(o => this.UseAutomation);
                }
            }
        }

        public bool UseZPlane
        {
            get => useZPlane;
            set
            {
                if (value != useZPlane)
                {
                    useZPlane = value;
                    var song = this.CurrentVdjSong;
                    if (song != null)
                    {
                        song.UseZPlane = value;
                    }
                    this.AsyncOnPropertyChange(o => this.UseZPlane);
                }
            }
        }

        public AutomatedEffect CurrentAutomationEffect
        {
            get => currentAutomationEffect;
            set
            {
                currentAutomationEffect = value;
                this.AutoChaser = false;
                this.AsyncOnPropertyChange(o => this.CurrentAutomationEffect);
            }
        }

        public void SetCurrentAutomationEffectInternal(AutomatedEffect effect)
        {
            currentAutomationEffect = effect;
            this.AsyncOnPropertyChange(o => this.CurrentAutomationEffect);
        }

        public List<AutomatedEffect> AutomationEffects
        {
            get => AutomatedEffect.AutomatedEffectList.Values.ToList();
        }

        public DmxChaser(VirtualDjServer vdjServer)
        {
            mBpmDetector = LightMixerBootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();

            BindViewModelWithScene<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, LedEffectCollection, nameof(CurrentLedEffect));
            BindViewModelWithScene<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, MovingHeadEffectCollection, nameof(CurrentMovingHeadEffect));
            BindViewModelWithScene<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, BoothEffectCollection, nameof(CurrentBoothEffect));
            VdjServer = vdjServer;
        }

        private void Save()
        {
            this.CurrentVdjSong?.Save();
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
                    SelectedPOI.IsDeleted = true;
                    this.POIs.Remove(selectedPOI);

                });
            }
        }

        public ICommand CreateCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (this.CurrentVdjSong == null || this.currentAutomationEffect == null)
                        return;
                    if (this.CurrentVdjSong.AutomatedPois == null)
                    {
                        this.CurrentVdjSong.AutomatedPois = new SortableObservableCollection<VDJPoi>(new List<AutomatedPoi>());
                    }
                    var newPoi = CreatePOI();
                    if (newPoi != null)
                    {

                        this.CurrentVdjSong.AutomatedPois.Add(newPoi);
                        this.CurrentVdjSong.AutomatedPois.Sort(o => o.Position, System.ComponentModel.ListSortDirection.Ascending);
                    }
                });
            }
        }

        private void SetCurrentEffectAsync<T>(string scene, string zone, EffectBase newEffect) where T : FixtureCollection
        {
            LightMixerBootStrap.Dispatcher.Invoke((Action)(() =>
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
            foreach (var effect in fixtureType.EffectList)
            {
                effectList.Add(effect);
            }
        }

        private AutomatedPoi CreatePOI()
        {
            var json = new AutomatedPOIJson();
            json.UseAutomation = this.useAutomation;
            json.ID = this.CurrentVdjSong.AutomatedPois.Any() ? CurrentVdjSong.AutomatedPois.Max(o => o.ID) + 1 : 0;
            json.AutomationEnum = this.currentAutomationEffect.Name;
            var currentPoi = CurrentVDJEvent.GetCurrentPoi;
            var vscanBPM = Convert.ToDouble(CurrentVdjSong?.Scans?.FirstOrDefault()?.Bpm);
            var currentBPM = Convert.ToDouble(CurrentVDJEvent?.BPM);
            var elapsed = Convert.ToDouble(CurrentVDJEvent?.Elapsed);

            json.Position = 60 / vscanBPM / currentBPM * elapsed / 1000;
            //  var poi = new AutomatedPoi(currentPoi.IsBreak ? "End Break " : "Break " + nextId, position.ToString(), "remix", CurrentVdjSong?.Scans?.FirstOrDefault());
            var poi = new AutomatedPoi(json, CurrentVdjSong);
            poi.Automation = this.currentAutomationEffect.Name;
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
                    VdjServer.vdjDataBase.Reload();
                });
            }
        }

        public VirtualDjServer VdjServer { get; }
        public double Elapsed { get; internal set; }

        public void UpdateVDJUiElement(IEnumerable<VdjEvent> activeDeck)
        {
            if (activeDeck.Count() > 0)
            {
                this.TrackName = activeDeck.FirstOrDefault()?.FileName;
                this.CurrentVdjSong = activeDeck.FirstOrDefault()?.VDJSong;
                this.UseAutomation = this.CurrentVdjSong?.UseAutomation ?? false;
                this.UseZPlane = this.UseZPlane;//refresh hack
                this.CurrentVDJEvent = activeDeck.FirstOrDefault();
                this.CurrentPoi = activeDeck.FirstOrDefault()?.GetCurrentPoi;
                if (this.UseAutomation)
                {
                    this.POIs = activeDeck.FirstOrDefault()?.VDJSong?.AutomatedPois;
                }
                else if ((this.UseZPlane || activeDeck.FirstOrDefault()?.VDJSong?.VDJPoiPlausible != true) && activeDeck.FirstOrDefault()?.VDJSong?.ZPlanePois != null && activeDeck.FirstOrDefault()?.VDJSong?.ZPlanePois.Count > 2)
                {
                    this.POIs = activeDeck.FirstOrDefault()?.VDJSong?.ZPlanePois;
                }
                else
                {
                    this.POIs = activeDeck.FirstOrDefault()?.VDJSong?.Pois;
                }

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
        }

        #endregion IDisposable Members
    }
}


