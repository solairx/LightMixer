using System;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadFixture : RgbFixtureBase
    {
        public RgbwMovingHeadMasterFixture Master { get; set; }
        public bool EnableAlternateColor
        {
            get => enableAlternateColor; set
            {
                if (enableAlternateColor != value)
                {
                    enableAlternateColor = value;
                    CurrentEffectChanged?.Invoke();
                }
            }
        }

        public event CurrentEffectChangedEventHandler CurrentEffectChanged;

        public delegate void CurrentEffectChangedEventHandler();

        public bool EnableDelayedPosition
        {
            get => enableDelayedPosition;
            set
            {
                if (enableDelayedPosition != value)
                {
                    if (value && Master != null)
                    {
                        ResetProgram(Master.EffectPositionRatio, GroupPosition);
                    }
                    else
                    {
                        RenderProgram(1, 0);
                    }
                    enableDelayedPosition = value;
                    CurrentEffectChanged?.Invoke();
                }
            }
        }

        public double GroupPosition { get; set; }

        private List<IMovingHeadProgram> internalProgram = new List<IMovingHeadProgram>();

        public MovingHeadFixture(int dmxAddress, List<PointOfInterest> pointOfInterests)
            : base(dmxAddress)
        {
            PointOfInterests = pointOfInterests;
        }

        public override void Init()
        {
            internalProgram.Add(new MovingHeadProgramCircle(this, PointOfInterests));
            internalProgram.Add(new MovingHeadProgramTest(this, PointOfInterests));
            internalProgram.Add(new MovingHeadProgramDj(this, PointOfInterests));
            internalProgram.Add(new MovingHeadProgramDisable(this, PointOfInterests));
            internalProgram.Add(new MovingHeadProgramDiscoBall(this, PointOfInterests));
            internalProgram.Add(new MovingHeadProgramDiscoBallStatic(this, PointOfInterests));
            internalProgram.Add(new MovingHeadProgramBalancing1(this, PointOfInterests));
        }

        public List<PointOfInterest> PointOfInterests { get; }

        public ushort Pan
        {
            get;
            set;
        }

        public ushort Tilt
        {
            get;
            set;
        }

        public byte Speed
        {
            get => speed;
            set
            {
                if (speed != value)
                {
                    speed = value;
                    CurrentEffectChanged?.Invoke();
                }
            }
        }

        public byte SpeedColor
        {
            get => speedColor;
            set
            {
                if (speedColor != value)
                {
                    speedColor = value;
                    CurrentEffectChanged?.Invoke();
                }
            }
        }

        public ColorMacro ColorMode
        {
            get => colorMode;
            set
            {
                if (colorMode != value)
                {
                    colorMode = value;
                    CurrentEffectChanged?.Invoke();
                }
            }
        }

        public Gobo GoboPaturn
        {
            get => goboPaturn;
            set
            {
                if (goboPaturn != value)
                {
                    goboPaturn = value;
                    CurrentEffectChanged?.Invoke();
                }
            }
        }

        public Program ProgramMode
        {
            get => programMode;
            set
            {
                if (programMode != value)
                {
                    programMode = value;
                    CurrentEffectChanged?.Invoke();
                }
            }
        }

        public override int DmxLenght => 13;

        public double Dimmer { get; set; }

        public double EffectPositionRatio = 0;
        private bool enableDelayedPosition;
        private Program programMode;
        private Gobo goboPaturn;
        private ColorMacro colorMode;
        private byte speedColor;
        private byte speed;
        private bool enableAlternateColor;

        public override byte?[] Render()
        {
            var codeProgram = RenderProgram();
            EffectPositionRatio = codeProgram.PositionRatio;
            if (codeProgram == null)
            {
                Dimmer = 1;
            }

            var panByte = BitConverter.GetBytes(Pan);
            var tiltByte = BitConverter.GetBytes(Tilt);
            byte?[] arr = new byte?[512];
            arr[StartDmxAddress] = panByte[1];//x
            arr[StartDmxAddress + 1] = panByte[0]; //y
            arr[StartDmxAddress + 2] = tiltByte[1];
            arr[StartDmxAddress + 3] = tiltByte[0];
            arr[StartDmxAddress + 4] = 0;//Speed;// speed
            arr[StartDmxAddress + 5] = 255;  //shutter
            arr[StartDmxAddress + 6] = Convert.ToByte(RedValue * Dimmer);
            arr[StartDmxAddress + 7] = Convert.ToByte(GreenValue * Dimmer);
            arr[StartDmxAddress + 8] = Convert.ToByte(BlueValue * Dimmer);
            arr[StartDmxAddress + 10] = SpeedColor;
            arr[StartDmxAddress + 11] = codeProgram != null ? (byte)0 : (byte)ProgramMode;
            arr[StartDmxAddress + 12] = (byte)GoboPaturn;
            return arr;
        }

        protected IMovingHeadProgram RenderProgram(double masterPositionRatio, double groupPosition)
        {
            var codeProgram = this.internalProgram.FirstOrDefault(prg => prg.LegacyProgram == this.ProgramMode);
            if (codeProgram != null)
            {
                codeProgram.RenderOn(this, masterPositionRatio, groupPosition);
            }

            return codeProgram;
        }

        protected void ResetProgram(double masterPositionRatio, double groupPosition)
        {
            var codeProgram = this.internalProgram.FirstOrDefault(prg => prg.LegacyProgram == this.ProgramMode);
            if (codeProgram != null)
            {
                codeProgram.ResetTo(masterPositionRatio, groupPosition);
            }
        }

        protected IMovingHeadProgram RenderProgram()
        {
            return RenderProgram(1, 0);
        }

        public enum ColorMacro : byte
        {
            NotSet = 0,
            White = 12,
            Red = 25,
            Green = 40,
            Blue = 57,
            Cyan = 71,
            Magenta = 84,
            Yellow = 100,
            Purple = 115,
            Orange = 128,
            Chartreuse = 138,
            Pink = 155,
            Brown = 168,
            Gold = 182,
            Crimson = 195,
            Violet = 211,
            Crape = 225,
            Auto = 255
        }

        public enum Gobo : byte
        {
            Open = 0,
            FloyerSpiral = 12,
            Spiral = 19,
            TriangleSpiral = 28,
            HearExplosion = 37,
            TriangleSpiral2 = 43,
            Star = 52,
            SpinningArrow = 59,
            SquareSpiral = 68,
            Floyer = 76
        }

        public enum Program : byte
        {
            Disable = 0,
            CodeDisable = 1,
            DJ = 2,
            Balancing1 = 3,
            Circle = 13,
            DiscoBall = 14,
            DiscoBallStatic = 16,
            Auto1 = 15,
            Auto2 = 27,
            Auto3 = 42,
            Auto4 = 60,
            Auto5 = 72,
            Auto6 = 90,
            Auto7 = 105,
            Auto8 = 120,
            SoundAuto1 = 135,
            SoundAuto2 = 150,
            SoundAuto3 = 165,
            SoundAuto4 = 180,
            SoundAuto5 = 195,
            SoundAuto6 = 211,
            SoundAuto7 = 226,
            Test = 254,
            SoundAuto8 = 255,
        }
    }
}