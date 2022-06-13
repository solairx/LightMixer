using System.Collections.ObjectModel;
using UIFrameWork;


namespace VisualControler
{
    public class ServiceExchangeSingleton : BaseViewModel
    {
        private static ServiceExchangeSingleton _Instance = new ServiceExchangeSingleton();

        public static ServiceExchangeSingleton Instance
        {
            get
            {
                return _Instance;

            }
            set
            {
                _Instance = value;
                _Instance.OnPropertyChanged(o => Instance);
            }
        }
        private bool _green = true;
        private bool _red = true;
        private bool _blue = true;

        private bool _ManualBeat;
        private bool _OnBeat;
        private bool _AutoChangeEvent;
        private bool _AutoChangeEventLaser;
        private bool _ManualBeatOnly;


        private int _AutoMixDelay = 15;
        private int _LedCurrentEventID = 1;
        private int _LaserSpeedRatio = 50;

        private LaserDisplay.ColorMode _LaserColorMode;

        private int _LaserSpeedAdj = 50;


        private bool _LaserPause = true;
        private bool _OnBeatReverse;

        private bool _UseBeatTurnOff;







        public bool OnBeatReverse
        {
            get
            {
                return _OnBeatReverse;
            }
            set
            {
                _OnBeatReverse = value;
                OnPropertyChanged(o => this.OnBeatReverse);
            }
        }

        public bool UseBeatTurnOff
        {
            get
            {
                return _UseBeatTurnOff;
            }
            set
            {
                _UseBeatTurnOff = value;
                OnPropertyChanged(o => this.UseBeatTurnOff);
            }
        }




        public bool ManualBeat
        {
            get
            {
                return _ManualBeat;
            }
            set
            {
                _ManualBeat = value;
                OnPropertyChanged(o => this.ManualBeat);
            }
        }
        public bool OnBeat
        {
            get
            {
                return _OnBeat;
            }
            set
            {
                _OnBeat = value;
                OnPropertyChanged(o => this.OnBeat);
            }
        }

        public bool Green
        {
            get
            {
                return _green;
            }
            set
            {
                _green = value;
                OnPropertyChanged(o => this.Green);
            }
        }

        public bool Blue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = value;
                OnPropertyChanged(o => this.Blue);
            }
        }

        public bool Red
        {
            get
            {
                return _red;
            }
            set
            {
                _red = value;
                OnPropertyChanged(o => this.Red);
            }
        }
        public bool AutoChangeEvent
        {
            get
            {
                return _AutoChangeEvent;
            }
            set
            {
                _AutoChangeEvent = value;
                OnPropertyChanged(o => this.AutoChangeEvent);
            }
        }
        public bool AutoChangeEventLaser
        {
            get
            {
                return _AutoChangeEventLaser;
            }
            set
            {
                _AutoChangeEventLaser = value;
                OnPropertyChanged(o => this.AutoChangeEventLaser);
            }
        }
        public bool ManualBeatOnly
        {
            get
            {
                return _ManualBeatOnly;
            }
            set
            {
                _ManualBeatOnly = value;
                OnPropertyChanged(o => this.ManualBeatOnly);
            }
        }


        public int AutoMixDelay
        {
            get
            {
                return _AutoMixDelay;
            }
            set
            {
                _AutoMixDelay = value;
                OnPropertyChanged(o => this.AutoMixDelay);
            }
        }
        public int LedCurrentEventID
        {
            get
            {
                return _LedCurrentEventID;
            }
            set
            {
                _LedCurrentEventID = value;
                OnPropertyChanged(o => this.LedCurrentEventID);
            }
        }
        public int LaserSpeedRatio
        {
            get
            {
                return _LaserSpeedRatio;
            }
            set
            {
                _LaserSpeedRatio = value;
                OnPropertyChanged(o => this.LaserSpeedRatio);
            }
        }

        public LaserDisplay.ColorMode LaserColorMode
        {
            get
            {
                return _LaserColorMode;
            }
            set
            {
                _LaserColorMode = value;
                OnPropertyChanged(o => this.LaserColorMode);
            }
        }

        public ObservableCollection<LaserDisplay.ColorMode> LaserColorModeList
        {
            get
            {
                ObservableCollection<LaserDisplay.ColorMode> list = new ObservableCollection<LaserDisplay.ColorMode>();
                list.Add(LaserDisplay.ColorMode.Hard);
                list.Add(LaserDisplay.ColorMode.Manual);
                list.Add(LaserDisplay.ColorMode.Smooth);

                return list;
            }
        }

        public ObservableCollection<LaserEffectUIElement> LaserEffectList
        {
            get
            {
                ObservableCollection<LaserEffectUIElement> list = new ObservableCollection<LaserEffectUIElement>();
                list.Add(new LaserEffectUIElement() { ID = 0, Name = "None" });
                list.Add(new LaserEffectUIElement() { ID = 1, Name = "Static Sky" });
                list.Add(new LaserEffectUIElement() { ID = 2, Name = "Beat Sky" });
                list.Add(new LaserEffectUIElement() { ID = 3, Name = "Audiance Scan" });
                list.Add(new LaserEffectUIElement() { ID = 4, Name = "Audiance ScanR" });
                list.Add(new LaserEffectUIElement() { ID = 5, Name = "Tunel" });
                list.Add(new LaserEffectUIElement() { ID = 6, Name = "Spinning" });
                list.Add(new LaserEffectUIElement() { ID = 7, Name = "Spinning R" });
                list.Add(new LaserEffectUIElement() { ID = 8, Name = "Spinning Full" });


                return list;
            }
        }

        public int LaserSpeedAdj
        {
            get
            {
                return _LaserSpeedAdj;
            }
            set
            {
                _LaserSpeedAdj = value;
                OnPropertyChanged(o => this.LaserSpeedAdj);
            }
        }

        public bool LaserPause
        {
            get
            {
                return _LaserPause;
            }
            set
            {
                _LaserPause = value;
                OnPropertyChanged(o => this.LaserPause);
            }
        }
    }

    public class LaserEffectUIElement
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
}
