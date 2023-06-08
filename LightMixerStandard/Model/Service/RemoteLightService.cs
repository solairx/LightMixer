using LaserDisplay;
using LightMixer.Model.Fixture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using VisualControler;


namespace LightMixer.Model.Service
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "RemoteLightService" à la fois dans le code et le fichier de configuration.
   /* public class RemoteLightService : IRemoteLightService
    {
        private InterfaceKit kit;

        public ObservableCollection<MovingHeadFixture.Program> MovingHeadProgram()
        {
            return BootStrap.UnityContainer.Resolve<SharedEffectModel>().MovingHeadProgram;
        }

        public ObservableCollection<MovingHeadFixture.Gobo> MovingHeadGobo()
        {
            return BootStrap.UnityContainer.Resolve<SharedEffectModel>().MovingHeadGobo;
        }

        public ObservableCollection<string> LedEffectCollection()
        {
            return new ObservableCollection<string>(BootStrap.UnityContainer.Resolve<DmxChaser>().LedEffectCollection.Select(o => o.Name));
        }

        public ObservableCollection<string> MovingHeadEffectCollection()
        {
            return new ObservableCollection<string>(BootStrap.UnityContainer.Resolve<DmxChaser>().MovingHeadEffectCollection.Select(o => o.Name));
        }

        public ObservableCollection<string> BoothEffectCollection()
        {
            return new ObservableCollection<string>(BootStrap.UnityContainer.Resolve<DmxChaser>().BoothEffectCollection.Select(o => o.Name));
        }

        public ObservableCollection<ColorMode> LaserColorModeList()
        {
            return BootStrap.UnityContainer.Resolve<ServiceExchangeSingleton>().LaserColorModeList;
        }

        public IEnumerable<string> LaserColorModeListString()
        {
            return BootStrap.UnityContainer.Resolve<ServiceExchangeSingleton>().LaserColorModeList.Select(o => o.ToString());
        }

        public IEnumerable<string> LaserEffectList()
        {
            return new ObservableCollection<string>(BootStrap.UnityContainer.Resolve<ServiceExchangeSingleton>().LaserEffectList.Select(o => o.Name));
        }

        public void CurrentMovingHeadProgram(MovingHeadFixture.Program program)
        {
            BootStrap.UnityContainer.Resolve<SharedEffectModel>().CurrentMovingHeadProgram = program;
        }

        public void CurrentMovingHeadGobo(MovingHeadFixture.Gobo gobo)
        {
            BootStrap.UnityContainer.Resolve<SharedEffectModel>().CurrentMovingHeadGobo = gobo;
        }

        public void CurrentLedEffect(string selectedEffect)
        {
            var effect = BootStrap.UnityContainer.Resolve<DmxChaser>()
                .LedEffectCollection.First(o => o.Name == selectedEffect);
            BootStrap.UnityContainer.Resolve<DmxChaser>().CurrentLedEffect = effect;
        }

        public void Intensity(double led, double flash, double head, double beatRepeat)
        {
            var chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
            var shared = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            var beatDetector = BootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
            BootStrap.Dispatcher.Invoke(new Action(() =>
            {
                shared.MaxLightFlashIntesity = flash;
                shared.MaxLightIntesity = led;
                shared.MaxLightIntesityMovingHead = head;
                beatDetector.BeatRepeat = beatRepeat;
            }
                ));
        }

        public void Color(int red, int green, int blue, bool autoChangeColor)
        {
            var chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
            var shared = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            var beatDetector = BootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
            BootStrap.Dispatcher.Invoke(new Action(() =>
            {
                shared.Red = Convert.ToByte(red);
                shared.Green = Convert.ToByte(green);
                shared.Blue = Convert.ToByte(blue);
                shared.AutoChangeColorOnBeat = autoChangeColor;
            }
                ));
        }

        public void CurrentBoothEffect(string selectedEffect)
        {
            var effect = BootStrap.UnityContainer.Resolve<DmxChaser>()
                .BoothEffectCollection.First(o => o.Name == selectedEffect);
            BootStrap.UnityContainer.Resolve<DmxChaser>().CurrentBoothEffect = effect;
        }

        public void CurrentMovingHeadEffect(string selectedEffect)
        {
            var effect = BootStrap.UnityContainer.Resolve<DmxChaser>()
                .MovingHeadEffectCollection.First(o => o.Name == selectedEffect);
            BootStrap.UnityContainer.Resolve<DmxChaser>().CurrentMovingHeadEffect = effect;
        }

        public void UpdateLaserRest(bool AutoChangeEvent, bool AutoChangeEventLaser, int AutoMixDelay, bool Blue, bool Green, bool LaserPause, int LaserSpeedAdj, int LaserSpeedRatio, bool ManualBeat, bool ManualBeatOnly, bool OnBeat, bool OnBeatReverse, bool UseBeatTurnOff, string LaserCurrentEventID, string LaserColorMode)
        {
            var shared = BootStrap.UnityContainer.Resolve<SharedEffectModel>();

            var laser = ServiceExchangeSingleton.Instance;

            BootStrap.Dispatcher.Invoke(new Action(() =>
            {
                laser.AutoChangeEvent = AutoChangeEvent;
                laser.AutoChangeEventLaser = AutoChangeEventLaser;
                laser.AutoMixDelay = AutoMixDelay;
                laser.Blue = Blue;
                laser.Green = Green;
                laser.LaserPause = LaserPause;
                laser.LaserSpeedAdj = LaserSpeedAdj;
                laser.LaserSpeedRatio = LaserSpeedRatio;
                laser.ManualBeat = ManualBeat;
                laser.ManualBeatOnly = ManualBeatOnly;
                laser.OnBeat = OnBeat;
                laser.OnBeatReverse = OnBeatReverse;
                laser.UseBeatTurnOff = UseBeatTurnOff;
                var newLaserEffect = laser.LaserEffectList.FirstOrDefault(o => o.Name == LaserCurrentEventID)
                    .ID;
                if (newLaserEffect != null)
                {
                    laser.LedCurrentEventID = newLaserEffect;
                }
                var colorMode = ColorMode.Hard;
                Enum.TryParse(LaserColorMode, out colorMode);
                laser.LaserColorMode = colorMode;
            }));
        }

        public void UpdateLaser(LaserDataContract contract)
        {
            var shared = BootStrap.UnityContainer.Resolve<SharedEffectModel>();

            var laser = ServiceExchangeSingleton.Instance;

            BootStrap.Dispatcher.Invoke(new Action(() =>
            {
                laser.AutoChangeEvent = contract.AutoChangeEvent;
                laser.AutoChangeEventLaser = contract.AutoChangeEventLaser;
                laser.AutoMixDelay = contract.AutoMixDelay;
                laser.Blue = contract.Blue;
                laser.Green = contract.Green;
                laser.LaserPause = contract.LaserPause;
                laser.LaserSpeedAdj = contract.LaserSpeedAdj;
                laser.LaserSpeedRatio = contract.LaserSpeedRatio;
                laser.ManualBeat = contract.ManualBeat;
                laser.ManualBeatOnly = contract.ManualBeatOnly;
                laser.OnBeat = contract.OnBeat;
                laser.OnBeatReverse = contract.OnBeatReverse;
                laser.Red = contract.Red;
                laser.UseBeatTurnOff = contract.UseBeatTurnOff;
                laser.LedCurrentEventID = laser.LaserEffectList.First(o => o.Name == contract.LedCurrentEventID)
                    .ID;
                laser.LaserColorMode = contract.LaserColorMode;
            }));
        }

        public LaserDataContract GetLaserStatus()
        {
            var laser = new LaserDataContract();
            var contract = ServiceExchangeSingleton.Instance;
            laser.AutoChangeEvent = contract.AutoChangeEvent;
            laser.AutoChangeEventLaser = contract.AutoChangeEventLaser;
            laser.AutoMixDelay = contract.AutoMixDelay;
            laser.Blue = contract.Blue;
            laser.Green = contract.Green;
            laser.LaserPause = contract.LaserPause;
            laser.LaserSpeedAdj = contract.LaserSpeedAdj;
            laser.LaserSpeedRatio = contract.LaserSpeedRatio;
            laser.ManualBeat = contract.ManualBeat;
            laser.ManualBeatOnly = contract.ManualBeatOnly;
            laser.OnBeat = contract.OnBeat;
            laser.OnBeatReverse = contract.OnBeatReverse;
            laser.Red = contract.Red;
            laser.UseBeatTurnOff = contract.UseBeatTurnOff;
            laser.LedCurrentEventID = contract.LaserEffectList.First(o => o.ID == contract.LedCurrentEventID)
                .Name; ;
            laser.LaserColorMode = contract.LaserColorMode;
            return laser;
        }

        public DmxDataContract GetDmxStatus()
        {
            var chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
            var shared = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            var beatDetector = BootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
            var contract = new DmxDataContract
            {
                CurrentMovingHeadGobo = shared.CurrentMovingHeadGobo,
                CurrentMovingHeadProgram = shared.CurrentMovingHeadProgram,
                CurrentBoothEffect = chaser.CurrentBoothEffect.Name,
                CurrentLedEffect = chaser.CurrentLedEffect.Name,
                CurrentMovingHeadEffect = chaser.CurrentMovingHeadEffect.Name,
                AutoChangeColorOnBeat = shared.AutoChangeColorOnBeat,
                AutoChangeGobo = shared.AutoChangeGobo,
                AutoChangeProgram = shared.AutoChangeProgram,
                Blue = shared.Blue,
                BeatRepeat = beatDetector.BeatRepeat,
                Green = shared.Green,
                MaxLightFlashIntesity = shared.MaxLightFlashIntesity,
                MaxLightIntesity = shared.MaxLightIntesity,
                MaxLightIntesityMovingHead = shared.MaxLightIntesityMovingHead,
                MaxSpeed = shared.MaxSpeed,
                Red = shared.Red,
                SecondBetweenGoboChange = shared.SecondBetweenGoboChange,
                SecondBetweenProgramChange = shared.SecondBetweenProgramChange
            };
            return contract;
        }

        public void UpdateMovingHead(string effect, string program, string gobo, int secondBetweenGoboChange, int secondBetweenProgramChange, double movingHeadMaxSpeed, bool autoChangeGobo, bool autoChangeProgram)
        {
            var chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
            var shared = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            BootStrap.Dispatcher.Invoke(new Action(() =>
            {
                shared.CurrentMovingHeadGobo = shared.MovingHeadGobo.FirstOrDefault(o => o.ToString() == gobo);
                shared.CurrentMovingHeadProgram = shared.MovingHeadProgram.FirstOrDefault(o => o.ToString() == program);
                chaser.CurrentMovingHeadEffect = chaser.MovingHeadEffectCollection.FirstOrDefault(o => o.Name == effect);
                shared.MaxSpeed = movingHeadMaxSpeed;
                shared.SecondBetweenGoboChange = secondBetweenGoboChange;
                shared.SecondBetweenProgramChange = secondBetweenProgramChange;
                shared.AutoChangeProgram = autoChangeProgram;
                shared.AutoChangeGobo = autoChangeGobo;
            }));
        }

        public void UpdateDmx(DmxDataContract contract)
        {
            var chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
            var shared = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            var beatDetector = BootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
            BootStrap.Dispatcher.Invoke(new Action(() =>
            {
                shared.CurrentMovingHeadGobo = contract.CurrentMovingHeadGobo;
                shared.CurrentMovingHeadProgram = contract.CurrentMovingHeadProgram;
                chaser.CurrentBoothEffect =
                    chaser.BoothEffectCollection.FirstOrDefault(o => o.Name == contract.CurrentBoothEffect);
                chaser.CurrentLedEffect = chaser.LedEffectCollection.FirstOrDefault(o => o.Name == contract.CurrentLedEffect);
                chaser.CurrentMovingHeadEffect =
                    chaser.MovingHeadEffectCollection.FirstOrDefault(o => o.Name == contract.CurrentMovingHeadEffect);

                shared.AutoChangeColorOnBeat = contract.AutoChangeColorOnBeat;
                shared.AutoChangeGobo = contract.AutoChangeGobo;
                shared.AutoChangeProgram = contract.AutoChangeProgram;

                beatDetector.BeatRepeat = contract.BeatRepeat;

                shared.MaxLightFlashIntesity = contract.MaxLightFlashIntesity;
                shared.MaxLightIntesity = contract.MaxLightIntesity;
                shared.MaxLightIntesityMovingHead = contract.MaxLightIntesityMovingHead;

                shared.MaxSpeed = contract.MaxSpeed;

                shared.Green = contract.Green;
                shared.Blue = contract.Blue;
                shared.Red = contract.Red;

                shared.SecondBetweenGoboChange = contract.SecondBetweenGoboChange;
                shared.SecondBetweenProgramChange = contract.SecondBetweenProgramChange;
            }
                ));
        }

        public IEnumerable<string> MovingHeadProgramString()
        {
            return BootStrap.UnityContainer.Resolve<SharedEffectModel>().MovingHeadProgram.Select(value => value.ToString());
        }

        public IEnumerable<string> MovingHeadGoboString()
        {
            return BootStrap.UnityContainer.Resolve<SharedEffectModel>().MovingHeadGobo.Select(value => value.ToString()); ;
        }

        public RemoteLightService()
        {
        }
    }*/

    [DataContract]
    public class DmxDataContract
    {
        [DataMember]
        public MovingHeadFixture.Gobo CurrentMovingHeadGobo { get; set; }

        [DataMember]
        public MovingHeadFixture.Program CurrentMovingHeadProgram { get; set; }

        [DataMember]
        public string CurrentMovingHeadEffect { get; set; }

        [DataMember]
        public string CurrentBoothEffect { get; set; }

        [DataMember]
        public string CurrentLedEffect { get; set; }

        [DataMember]
        public bool AutoChangeColorOnBeat { get; set; }

        [DataMember]
        public double MaxLightIntesity { get; set; }

        [DataMember]
        public double MaxLightIntesityMovingHead { get; set; }

        [DataMember]
        public int SecondBetweenProgramChange { get; set; }

        [DataMember]
        public int SecondBetweenGoboChange { get; set; }

        [DataMember]
        public double MaxSpeed { get; set; }

        [DataMember]
        public double MaxLightFlashIntesity { get; set; }

        [DataMember]
        public byte Red { get; set; }

        [DataMember]
        public byte Green { get; set; }

        [DataMember]
        public byte Blue { get; set; }

        [DataMember]
        public bool AutoChangeProgram { get; set; }

        [DataMember]
        public bool AutoChangeGobo { get; set; }

        [DataMember]
        public double BeatRepeat { get; set; }
    }
}