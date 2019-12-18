using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadFixture : FixtureBase
    {
        public MovingHeadFixture(int dmxAddress)
            : base(dmxAddress) 
        {

        }

        public short Pan
        {
            get;
            set;
        }

        public short Tilt
        {
            get;
            set;
        }

        public byte Speed
        {
            get;
            set;
        }

        public byte RedValue
        {
            get;
            set;
        }

        public byte GreenValue
        {
            get;
            set;
        }
        
        public byte BlueValue
        {
            get;
            set;
        }

        public byte SpeedColor
        {
            get;
            set;
        }

        public ColorMacro ColorMode
        {
            get;
            set;
        }
        public Gobo GoboPaturn
        {
            get;
            set;
        }
        public Program ProgramMode
        {
            get;
            set;
        }

        public override byte?[] Render()
        {
            var panByte = BitConverter.GetBytes(Pan);
            var tiltByte = BitConverter.GetBytes(Pan);
            byte?[] arr = new byte?[512];
            arr[StartDmxAddress ] = panByte[0];//x
            arr[StartDmxAddress + 1] = panByte[1]; //y
            arr[StartDmxAddress + 2] = tiltByte[0];
            arr[StartDmxAddress +3] = tiltByte[1];
            arr[StartDmxAddress +4] = 255;  //shutter
            arr[StartDmxAddress +5] = RedValue;
            arr[StartDmxAddress +6] = GreenValue;
            arr[StartDmxAddress +7] = BlueValue;
            //arr[StartDmxAddress + 8] = (byte)ColorMode;
            arr[StartDmxAddress + 9] = SpeedColor;
            arr[StartDmxAddress +10] = (byte)ProgramMode;
            arr[StartDmxAddress +11] = (byte)GoboPaturn ;
            
            
            return arr;
        }


        public enum ColorMacro : byte
        {
            NotSet = 0,
            White = 12,
            Red =25,
            Green=40,
            Blue=57,
            Cyan=71,
            Magenta=84,
            Yellow=100,
            Purple=115,
            Orange=128,
            Chartreuse=138,
            Pink=155,
            Brown=168,
            Gold=182,
            Crimson=195,
            Violet=211,
            Crape=225,
            Auto=255
        }

        public enum Gobo : byte
        {
            Open=0,
            FloyerSpiral=12,
            Spiral=19,
            TriangleSpiral=28,
            HearExplosion=37,
            TriangleSpiral2=43,
            Star=52,
            SpinningArrow=59,
            SquareSpiral=68,
            Floyer=76
        }

        public enum Program : byte
        {
            Disable=0,
            Auto1=15,
            Auto2=27,
            Auto3=42,
            Auto4=60,
            Auto5=72,
            Auto6=90,
            Auto7=105,
            Auto8=120,
            SoundAuto1=135,
            SoundAuto2=150,
            SoundAuto3=165,
            SoundAuto4=180,
            SoundAuto5=195,
            SoundAuto6=211,
            SoundAuto7=226,
            SoundAuto8=255,
        }

    }

    
}
