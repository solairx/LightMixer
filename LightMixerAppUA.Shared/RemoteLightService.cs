using LightMixer.Model.Fixture;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;


namespace LightMixer.Model.Service
{
   
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
