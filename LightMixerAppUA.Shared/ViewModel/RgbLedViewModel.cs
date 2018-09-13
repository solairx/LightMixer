using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using Windows.Data.Json;

namespace LightMixerAppUA.ViewModel
{
    public class RgbLedViewModel : INotifyPropertyChanged
    {

        private static RgbLedViewModel viewModel;
        private string _LaserCurrentEventId;
        private bool _autoChangeEvent;
        private bool _autoChangeEventLaser;
        private int _autoMixDelay;
        private bool _laserBlue;
        private bool _laserGreen;
        private bool _laserPause;
        private int _laserSpeedAdj;
        private int _laserSpeedRatio;
        private bool _manualBeat;
        private bool _manualBeatOnly;
        private bool _onBeat;
        private bool _onBeatReverse;
        private bool _red;
        private bool _useBeatTurnOff;

        public static RgbLedViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new RgbLedViewModel();
                }
                return viewModel;
            }
        }

        private HttpClient httpClient;
        private string selectedStyle;
        private double ledIntensity;
        private double headIntensity;
        private double flashIntensity;
        private int green;
        private int red;
        private int blue;
        private string movingHeadSelectedStyle;
        private string movingHeadGoboSelectedStyle;
        private string movingHeadPaternSelectedStyle;
        private bool autoChangeColor;
        private double beatRepeat;
        private int secondBetweenProgramChange;
        private int secondBetweenGoboChange;
        private double movingHeadMaxSpeed;
        private bool autoChangeGobo;
        private bool autoChangeProgram;
        private string _LaserColorMode;

        public ICommand ResetBeatRepeat { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RgbLedViewModel()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://192.168.1.2:8088/");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ResetBeatRepeat = new DelegateCommand(ResetBeatRepeatExecute);
            // Limit the max buffer size for the response so we don't get overwhelmed

            httpClient.MaxResponseContentBufferSize = 266000;

            EffectStyleList = new List<string> { "Loading...", };
            MovingHeadStyles = new List<string> { "Loading...", };
            MovingHeadParternStyles = new List<string> { "Loading...", };
            MovingHeadGoboStyles = new List<string> { "Loading...", };
            LaserColorModeList = new List<string> { "Loading...", };
            LaserEffectList = new List<string> { "Loading...", };
            GetLaserColorList();
            GetLaserEffectList();
            GetEffectList();
            GetMovingHeadStylesList();
            GetMovingHeadPaternStylesList();
            GetMovingHeadGoboStylesList();
            flashIntensity = 100;
            ledIntensity = 100;
            headIntensity = 100;
            beatRepeat = 1;
            green = 100;
            blue = 100;
            red = 100;
            _laserPause = true;
            _laserGreen = true;
            _laserBlue = true;

        }


        public List<string> LaserColorModeList { get; set; }
        public List<string> LaserEffectList { get; set; }

        private void ResetBeatRepeatExecute()
        {
            BeatRepeat = 1;
        }

        private async void GetEffectList()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/LedEffectCollection");
            var jsonString = await response.Content.ReadAsStringAsync();

            var stringArray = JsonArray.Parse(jsonString).Select(a => a.GetString()).ToList();


            EffectStyleList = stringArray;
            RaisePropertyChange("EffectStyleList");
        }

        private async void GetLaserEffectList()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/LaserEffectList");
            var jsonString = await response.Content.ReadAsStringAsync();

            var stringArray = JsonArray.Parse(jsonString).Select(a => a.GetString()).ToList();


            LaserEffectList = stringArray;
            RaisePropertyChange("LaserEffectList");
            _LaserCurrentEventId = LaserEffectList.First();
            RaisePropertyChange("LaserCurrentEventID");
        }

        private async void GetLaserColorList()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/LaserColorModeListString");
            var jsonString = await response.Content.ReadAsStringAsync();

            var stringArray = JsonArray.Parse(jsonString).Select(a => a.GetString()).ToList();


            LaserColorModeList = stringArray;
            _LaserColorMode = LaserColorModeList.First();
            RaisePropertyChange("LaserColorModeList");
            RaisePropertyChange("LaserColorMode");
        }

        private async void GetMovingHeadStylesList()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/MovingHeadEffectCollection");
            var jsonString = await response.Content.ReadAsStringAsync();

            var stringArray = JsonArray.Parse(jsonString).Select(a => a.GetString()).ToList();


            MovingHeadStyles = stringArray;
            RaisePropertyChange("MovingHeadStyles");
        }

        private async void GetMovingHeadPaternStylesList()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/MovingHeadProgramString");
            var jsonString = await response.Content.ReadAsStringAsync();

            var stringArray = JsonArray.Parse(jsonString).Select(a => a.GetString()).ToList();


            MovingHeadParternStyles = stringArray;
            RaisePropertyChange("MovingHeadParternStyles");
        }

        private async void GetMovingHeadGoboStylesList()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/MovingHeadGoboString");
            var jsonString = await response.Content.ReadAsStringAsync();

            var stringArray = JsonArray.Parse(jsonString).Select(a => a.GetString()).ToList();


            MovingHeadGoboStyles = stringArray;
            RaisePropertyChange("MovingHeadGoboStyles");
        }

        public List<String> EffectStyleList
        {
            get;
            set;
        }

        public List<String> MovingHeadStyles
        {
            get;
            set;
        }

        public List<String> MovingHeadParternStyles
        {
            get;
            set;
        }

        public List<String> MovingHeadGoboStyles
        {
            get;
            set;
        }

        public string SelectedStyle
        {
            get
            {
                return selectedStyle;
            }
            set
            {
                selectedStyle = value;
                UpdateLedStyle();
                RaisePropertyChange("SelectedStyle");
            }
        }

        public string MovingHeadSelectedStyle
        {
            get
            {
                return movingHeadSelectedStyle;
            }
            set
            {
                movingHeadSelectedStyle = value;
                UpdateMovingHead();
                RaisePropertyChange("MovingHeadSelectedStyle");
            }
        }

        public string MovingHeadPaternSelectedStyle
        {
            get
            {
                return movingHeadPaternSelectedStyle;
            }
            set
            {
                movingHeadPaternSelectedStyle = value;
                UpdateMovingHead();
                RaisePropertyChange("MovingHeadPaternSelectedStyle");
            }
        }

        public string MovingHeadGoboSelectedStyle
        {
            get
            {
                return movingHeadGoboSelectedStyle;
            }
            set
            {
                movingHeadGoboSelectedStyle = value;
                UpdateMovingHead();
                RaisePropertyChange("MovingHeadGoboSelectedStyle");
            }
        }

        public double LedIntensity
        {
            get
            {
                return ledIntensity;
            }
            set
            {
                ledIntensity = value;
                UpdateIntensity();
                RaisePropertyChange("LedIntensity");
            }
        }

        public double BeatRepeat
        {
            get
            {
                return beatRepeat;
            }
            set
            {
                beatRepeat = value;
                UpdateIntensity();
                RaisePropertyChange("BeatRepeat");
            }
        }

        public double HeadIntensity
        {
            get
            {
                return headIntensity;
            }
            set
            {
                headIntensity = value;
                UpdateIntensity();
                RaisePropertyChange("HeadIntensity");
            }
        }

        public double FlashIntensity
        {
            get
            {
                return flashIntensity;
            }
            set
            {
                flashIntensity = value;
                UpdateIntensity();
                RaisePropertyChange("FlashIntensity");
            }
        }

        public bool AutoChangeColor
        {
            get
            {
                return autoChangeColor;
            }
            set
            {
                autoChangeColor = value;
                UpdateColor();
                RaisePropertyChange("AutoChangeColor");
            }
        }

        public bool AutoChangeProgram
        {
            get
            {
                return autoChangeProgram;
            }
            set
            {
                autoChangeProgram = value;
                UpdateMovingHead();
                RaisePropertyChange("AutoChangeProgram");
            }
        }

        public bool AutoChangeGobo
        {
            get
            {
                return autoChangeGobo;
            }
            set
            {
                autoChangeGobo = value;
                UpdateMovingHead();
                RaisePropertyChange("AutoChangeGobo");
            }
        }

        public int Red
        {
            get
            {
                return red;
            }
            set
            {
                red = value;
                UpdateColor();
                RaisePropertyChange("Red");
            }
        }

      

        public int Green
        {
            get
            {
                return green;
            }
            set
            {
                green = value;
                UpdateColor();
                RaisePropertyChange("Green");
            }
        }

        public int Blue
        {
            get
            {
                return blue;
            }
            set
            {
                blue = value;
                UpdateColor();
                RaisePropertyChange("Blue");
            }
        }

        public int SecondBetweenProgramChange
        {
            get
            {
                return secondBetweenProgramChange;
            }
            set
            {
                secondBetweenProgramChange = value;
                UpdateMovingHead();
                RaisePropertyChange("SecondBetweenProgramChange");
            }
        }

        public int SecondBetweenGoboChange
        {
            get
            {
                return secondBetweenGoboChange;
            }
            set
            {
                secondBetweenGoboChange = value;
                UpdateMovingHead();
                RaisePropertyChange("SecondBetweenGoboChange");
            }
        }

        public double MovingHeadMaxSpeed
        {
            get
            {
                return movingHeadMaxSpeed;
            }
            set
            {
                movingHeadMaxSpeed = value;
                UpdateMovingHead();
                RaisePropertyChange("MovingHeadMaxSpeed");
            }
        }



        public bool AutoChangeEvent
        {
            get { return _autoChangeEvent; }
            set { _autoChangeEvent = value; UpdateLaserServer(); }
        }

        public bool AutoChangeEventLaser
        {
            get { return _autoChangeEventLaser; }
            set { _autoChangeEventLaser = value; UpdateLaserServer(); }
        }

        public int AutoMixDelay
        {
            get { return _autoMixDelay; }
            set { _autoMixDelay = value; UpdateLaserServer(); }
        }

        public bool LaserBlue
        {
            get { return _laserBlue; }
            set { _laserBlue = value; UpdateLaserServer(); }
        }

        public bool LaserGreen
        {
            get { return _laserGreen; }
            set { _laserGreen = value; UpdateLaserServer(); }
        }

        public bool LaserPause
        {
            get { return _laserPause; }
            set { _laserPause = value; UpdateLaserServer(); }
        }

        public int LaserSpeedAdj
        {
            get { return _laserSpeedAdj; }
            set { _laserSpeedAdj = value; UpdateLaserServer(); }
        }

        public int LaserSpeedRatio
        {
            get { return _laserSpeedRatio; }
            set { _laserSpeedRatio = value; UpdateLaserServer(); }
        }

        public string LaserCurrentEventID
        {
            get { return _LaserCurrentEventId; }
            set { _LaserCurrentEventId = value; UpdateLaserServer(); }
        }

        public string LaserColorMode
        {
            get { return _LaserColorMode; }
            set { _LaserColorMode = value; UpdateLaserServer(); }
        }

        



        public bool ManualBeat
        {
            get { return _manualBeat; }
            set { _manualBeat = value; UpdateLaserServer(); }
        }

        public bool ManualBeatOnly
        {
            get { return _manualBeatOnly; }
            set { _manualBeatOnly = value; UpdateLaserServer(); }
        }

        public bool OnBeat
        {
            get { return _onBeat; }
            set { _onBeat = value; UpdateLaserServer(); }
        }

        public bool OnBeatReverse
        {
            get { return _onBeatReverse; }
            set { _onBeatReverse = value; UpdateLaserServer(); }
        }


        public bool UseBeatTurnOff
        {
            get { return _useBeatTurnOff; }
            set { _useBeatTurnOff = value; UpdateLaserServer(); }
        }

        private async void UpdateLedStyle()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/CurrentLedEffect?cahche=" + Guid.NewGuid() + "&effect=" + SelectedStyle, HttpCompletionOption.ResponseHeadersRead);
            var jsonString = await response.Content.ReadAsStringAsync();
        }

        private async void UpdateLaserServer()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/UpdateLaserRest?cahche="
                + Guid.NewGuid()
                + "&AutoChangeEvent=" + AutoChangeEvent
                + "&AutoChangeEventLaser=" + AutoChangeEventLaser
                + "&AutoMixDelay=" + AutoMixDelay
                + "&Blue=" + LaserBlue 
                + "&Green=" + LaserGreen
                + "&LaserPause=" + LaserPause
                + "&LaserSpeedAdj=" + LaserSpeedAdj
                + "&LaserSpeedRatio=" + LaserSpeedRatio
                + "&ManualBeat=" + ManualBeat
                + "&ManualBeatOnly=" + ManualBeatOnly
                + "&OnBeat=" + OnBeat
                + "&OnBeatReverse=" + OnBeatReverse
                + "&UseBeatTurnOff=" + UseBeatTurnOff
                
                + "&LaserCurrentEventID=" + LaserCurrentEventID
                + "&LaserColorMode=" + LaserColorMode
                , HttpCompletionOption.ResponseHeadersRead);
            var jsonString = await response.Content.ReadAsStringAsync();
        }


        private async void UpdateMovingHead()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/UpdateMovingHead?cahche=" 
                + Guid.NewGuid() 
                + "&effect=" + MovingHeadSelectedStyle  
                + "&program=" + MovingHeadPaternSelectedStyle 
                + "&gobo=" + MovingHeadGoboSelectedStyle
                + "&secondBetweenGoboChange=" + SecondBetweenGoboChange
                + "&secondBetweenProgramChange=" + SecondBetweenProgramChange
                + "&movingHeadMaxSpeed=" + MovingHeadMaxSpeed
                + "&autoChangeGobo=" + AutoChangeGobo
                + "&autoChangeProgram=" + AutoChangeProgram
                , HttpCompletionOption.ResponseHeadersRead);
            var jsonString = await response.Content.ReadAsStringAsync();
        }


        private async void UpdateIntensity()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/Intensity?cahche=" + Guid.NewGuid() + "&led=" + LedIntensity + "&flash=" + FlashIntensity + "&head=" + HeadIntensity + "&beatRepeat=" + BeatRepeat, HttpCompletionOption.ResponseHeadersRead);
            var jsonString = await response.Content.ReadAsStringAsync();
        }

        private async void UpdateColor()
        {
            var response = await httpClient.GetAsync("LightMixer/RemoteLightService/Color?cahche=" + Guid.NewGuid() + "&red=" + Red + "&green=" + Green + "&blue=" + Blue + "&autoChangeColor=" + AutoChangeColor, HttpCompletionOption.ResponseHeadersRead);
            var jsonString = await response.Content.ReadAsStringAsync();
        }



        private void RaisePropertyChange(string element)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(element));
        }

        public void Turn(string voiceText)
        {
            var on = voiceText.Contains("on");
            var everything = voiceText.Contains("everything");
            var red = voiceText.Contains("red");
            var green = voiceText.Contains("green");
            var blue = voiceText.Contains("blue");

            if (red)
                Red = on ? 100 : 0;
            if (green)
                Green = on ? 100 : 0;
            if (blue)
                Blue = on ? 100 : 0;
            if (everything)
            {
                if (on)
                {
                    Red = 100;
                    Green = 100;
                    Blue = 100;
                }
                else
                {
                    SelectedStyle = "AllOff";

                }
                LedIntensity = on ? 100 : 0;
            }
            if (on)
            {
                LedIntensity = 100;
                SelectedStyle = "AllOn";
            }
            UpdateIntensity();
            UpdateColor();
            UpdateLedStyle();

        }

        public void Leave(string voiceText)
        {
            var red = voiceText.Contains("red");
            var green = voiceText.Contains("green");
            var blue = voiceText.Contains("blue");

            if (red)
            {
                Red = 100;
                Green = 0;
                Blue = 0;
            }
            if (green)
            {
                Red = 0;
                Green = 100;
                Blue = 0;
            }
            if (blue)
            {
                Red = 0;
                Green = 0;
                Blue = 100;
            }
            SelectedStyle = "AllOn";
            LedIntensity = 100;
            UpdateIntensity();
            UpdateColor();
            UpdateLedStyle();

        }

        public void FollowTheBeat(string voiceText)
        {

            SelectedStyle = "FlashAll";
            FlashIntensity = 100;

            if (voiceText.Contains("zone"))
            {
                SelectedStyle = "Zone Flash Effect";
                FlashIntensity = 100;
            }

            if (voiceText.Contains("rotate"))
            {
                SelectedStyle = "Zone Rotate Effect";
                LedIntensity = 100;
            }

            UpdateLedStyle();

        }
    }
}
