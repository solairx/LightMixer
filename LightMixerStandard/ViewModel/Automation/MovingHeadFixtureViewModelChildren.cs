using LightMixer.Model.Fixture;

namespace LightMixer.View
{
    public class MovingHeadFixtureViewModelChildren : MovingHeadFixtureViewModelBase
    {
        private MovingHeadFixture movingHeadFixture;

        public string Name => "Child";

        public MovingHeadFixtureViewModelChildren(MovingHeadFixture fixtures)
        {
            this.movingHeadFixture = fixtures;
            this.movingHeadFixture.CurrentEffectChanged += MovingHeadUpdated;
        }

        public bool ChildrenVisibility => false;

        public override MovingHeadFixture.Program CurrentMovingHeadProgram
        {
            get
            {
                return this.movingHeadFixture.ProgramMode;
            }
            set
            {
                this.movingHeadFixture.ProgramMode = value;

            }
        }

        public override MovingHeadFixture.Gobo CurrentMovingHeadGobo
        {
            get
            {
                return this.movingHeadFixture.GoboPaturn;
            }
            set
            {
                this.movingHeadFixture.GoboPaturn = value;
            }
        }

        public override bool? UseDelatedPosition
        {
            get
            {
                return this.movingHeadFixture.EnableDelayedPosition;
            }
            set
            {
                this.movingHeadFixture.EnableDelayedPosition = value == true;
            }
        }

        public override bool? UseAlternateColor
        {
            get
            {
                return this.movingHeadFixture.EnableAlternateColor;
            }
            set
            {
                this.movingHeadFixture.EnableAlternateColor = value == true;
            }
        }
    }
}
