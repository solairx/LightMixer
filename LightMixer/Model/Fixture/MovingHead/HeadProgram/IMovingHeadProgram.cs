using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public interface IMovingHeadProgram
    {
        void RenderOn(MovingHeadFixture fixture, double masterPositionRatio, double groupPosition);
        void RenderOn(MovingHeadFixture fixture);
        Program LegacyProgram { get; }
        double PositionRatio { get; }
        void ResetTo(double masterPositionRatio, double groupPosition);

    }
}
