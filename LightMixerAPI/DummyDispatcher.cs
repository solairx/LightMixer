using LightMixer;

namespace LightMixerAngular
{
    public class DummyDispatcher : IDispatcher
    {
        public void Invoke(Action action)
        {
            action.Invoke();
        }
    }

}