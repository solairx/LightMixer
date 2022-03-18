using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public interface IMovingHeadProgram
    {
        void RenderOn(MovingHeadFixture fixture);
        Program LegacyProgram { get; }
    }
}
