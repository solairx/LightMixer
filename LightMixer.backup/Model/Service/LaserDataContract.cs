using LaserDisplay;
using System.Runtime.Serialization;

namespace LightMixer.Model.Service
{
    [DataContract]
    public class LaserDataContract
    {
        [DataMember]
        public bool AutoChangeEvent { get; set; }

        [DataMember]
        public bool AutoChangeEventLaser { get; set; }

        [DataMember]
        public int AutoMixDelay { get; set; }

        [DataMember]
        public bool Blue { get; set; }

        [DataMember]
        public bool Green { get; set; }

        [DataMember]
        public bool LaserPause { get; set; }

        [DataMember]
        public int LaserSpeedAdj { get; set; }

        [DataMember]
        public int LaserSpeedRatio { get; set; }

        [DataMember]
        public bool ManualBeat { get; set; }

        [DataMember]
        public bool ManualBeatOnly { get; set; }

        [DataMember]
        public bool OnBeat { get; set; }

        [DataMember]
        public bool OnBeatReverse { get; set; }

        [DataMember]
        public bool Red { get; set; }

        [DataMember]
        public bool UseBeatTurnOff { get; set; }

        [DataMember]
        public ColorMode LaserColorMode { get; set; }

        [DataMember]
        public string LedCurrentEventID { get; set; }
    }
}