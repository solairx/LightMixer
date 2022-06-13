using LaserDisplay;
using System;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public abstract class MovingHeadProgram : IMovingHeadProgram
    {
        private ResamplingService Resampler = new ResamplingService();
        public int position { get; set; }

        public double PositionRatio
        {
            get
            {
                if (position == 0 || Resampler.Size == 0)
                {
                    return 0;
                }
                return Resampler.Size / position;
            }
        }

        public abstract Program LegacyProgram { get; }
        public bool IsSlave { get; }

        public MovingHeadProgram(FixtureBase owner)
        {
            Owner = owner;
            IsSlave = false;//!owner.IsMaster;
            InitialSize = 1;
        }

        protected ushort[] PanList;
        protected ushort[] TiltList;
        protected ushort[] MaxDimmer;

        protected virtual ushort[] PanListDesign { get; set; }
        protected virtual ushort[] TiltListDesign { get; set; }
        protected virtual ushort[] MaxDimmerDesign { get; set; }

        protected virtual ushort[] PanListDesignSlave { get; set; }
        protected virtual ushort[] TiltListDesignSlave { get; set; }
        protected virtual ushort[] MaxDimmerDesignSlave { get; set; }

        protected virtual int InitialSize { get; set; }
        public FixtureBase Owner { get; }

        protected void Setup()
        {
            SetSize(InitialSize);
        }

        public void ResetTo(double masterPositionRatio, double groupPosition)
        {
            SetSize(Resampler.Size, masterPositionRatio, groupPosition);
        }

        protected void SetSize(int newSize)
        {
            double groupPosition = 0;

            double masterPositionRatio = position / ((Resampler.Size == 0) ? InitialSize : Resampler.Size);
            double effectiveRatio = masterPositionRatio + groupPosition;
            SetSize(newSize, masterPositionRatio, masterPositionRatio + groupPosition);
        }
        protected void SetSize(int newSize, double masterPositionRatio, double groupPosition)
        {
            double effectiveRatio = masterPositionRatio + groupPosition;
            if (effectiveRatio > 1)
            {
                effectiveRatio = effectiveRatio - 1;
            }

            var localPan = IsSlave ? PanListDesignSlave : PanListDesign;
            var localTilt = IsSlave ? TiltListDesignSlave : TiltListDesign;
            var dimmer = IsSlave ? MaxDimmerDesignSlave : MaxDimmerDesign;



            if (localPan.Length > newSize)
            {
                newSize = localPan.Length;
            }
            //reposition
            if (Resampler.Size != 0 && newSize != 0 && position != 0)
            {
                position = Convert.ToInt32(effectiveRatio * position);

            }
            Resampler.Size = newSize;
            Resampler.Filter = ResamplingFilters.Box;
            PanList = new ushort[newSize];
            TiltList = new ushort[newSize];
            MaxDimmer = Resampler.Resample(dimmer);
            LinearInterpolation(PanList, localPan);
            LinearInterpolation(TiltList, localTilt);
        }

        public void RenderOn(MovingHeadFixture fixture)
        {
            RenderOn(fixture, position / Resampler.Size, 0);
        }
        public void RenderOn(MovingHeadFixture fixture, double masterPositionRatio, double groupPosition)
        {
            if (fixture.Speed != 0)
            {
                double expectedSize = this.Resampler.Size;
                if (fixture.Speed > 128)
                {
                    expectedSize = InitialSize * (1 - (fixture.Speed - 128d) / 128d);

                }
                else
                {
                    expectedSize = (128d / fixture.Speed) * InitialSize;
                }

                if (Math.Abs(this.Resampler.Size - expectedSize) > 2)
                {
                    SetSize(Convert.ToInt32(expectedSize), masterPositionRatio, groupPosition);
                }
            }
            if (position >= PanList.Length)
            {
                position = 0;
            }
            fixture.Pan = PanList[position];
            fixture.Tilt = TiltList[position];

            var dimmerValue = MaxDimmer[position] / 100d;
            if (dimmerValue < 1 && dimmerValue > 0)
            {
                fixture.Dimmer = dimmerValue;
            }
            else
            {
                fixture.Dimmer = 1;
            }
            position++;

        }



        protected void LinearInterpolation(ushort[] destination, ushort[] source)
        {
            if (source.Length == 0)
            {
                LinearInterpolation(destination, new ushort[] { 0, 0 });
                return;
            }

            if (source.Length == 1)
            {
                LinearInterpolation(destination, new ushort[] { source[0], source[0] });
                return;
            }


            if (destination == null)
            {
                throw new Exception("Destination Array must be non null, perhap SetSize was not called");
            }
            destination[0] = source[0];
            int jPrevious = 0;
            for (int i = 1; i < source.Length; i++)
            {
                int j = i * (destination.Length - 1) / (source.Length - 1);
                InterpolateFrame(destination, jPrevious, j, source[i - 1], source[i]);

                jPrevious = j;
            }
        }

        protected void InterpolateFrame(ushort[] destination, int destFrom, int destTo, float valueFrom, float valueTo)
        {
            int destLength = destTo - destFrom;
            if (destLength == 0)
                destLength = 1;
            float valueLength = valueTo - valueFrom;
            for (int i = 0; i <= destLength; i++)
                destination[destFrom + i] = Convert.ToUInt16(valueFrom + (valueLength * i) / destLength);
        }
    }
}
