using System.Collections.Generic;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramTest : MovingHeadProgram
    {
        public static List<MovingHeadProgramTest> Instance = new List<MovingHeadProgramTest>();

        public static ushort[] PanListDesignShared = new ushort[] { 0, 500 };
        public static ushort[] TiltListDesignShared = new ushort[] { 0, 5000 };
        public static ushort[] MaxDimmerDesignShared = new ushort[] { 100, 100 };
        public static int InitialSizeShared = 255;



        protected override ushort[] MaxDimmerDesign { get => MaxDimmerDesignShared; set => base.MaxDimmerDesign = value; }
        protected override ushort[] MaxDimmerDesignSlave { get => MaxDimmerDesignShared; set => base.MaxDimmerDesignSlave = value; }
        protected override ushort[] PanListDesign { get => PanListDesignShared; set => base.PanListDesign = value; }
        protected override ushort[] TiltListDesign { get => TiltListDesignShared; set => base.TiltListDesign = value; }
        protected override ushort[] PanListDesignSlave { get => PanListDesignShared; set => base.PanListDesignSlave = value; }

        protected override ushort[] TiltListDesignSlave { get => TiltListDesignShared; set => base.TiltListDesignSlave = value; }
        protected override int InitialSize { get => InitialSizeShared; set => base.InitialSize = value; }


        public MovingHeadProgramTest(FixtureBase owner, List<PointOfInterest> pointOfInterests) : base(owner)
        {
            PanListDesign = PanListDesignSlave = new ushort[] { 0, 0 };
            TiltListDesign = TiltListDesignSlave = new ushort[] { 0, 0 };
            MaxDimmerDesign = MaxDimmerDesignSlave = new ushort[] { 0, 0 };
            InitialSize = 10;
            Setup();
            Instance.Add(this);
        }
        public override Program LegacyProgram => Program.Test;

        internal static void Reset()
        {
            Instance.ForEach(i => i.Setup());
        }
    }
}
