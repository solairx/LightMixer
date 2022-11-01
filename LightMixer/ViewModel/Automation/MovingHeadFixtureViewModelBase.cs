using LightMixer.Model;
using LightMixer.Model.Fixture;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using UIFrameWork;

namespace LightMixer.View
{
    public abstract class MovingHeadFixtureViewModelBase : BaseViewModel
    {
        private SharedEffectModel sharedEffectModel;

        public MovingHeadFixtureViewModelBase()
        {
            sharedEffectModel = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            sharedEffectModel.PropertyChanged += SharedEffectChanged;
        }

        private void SharedEffectChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AsyncOnPropertyChange(o => this.MaxSpeed);
        }

        public ObservableCollection<MovingHeadFixture.Program> MovingHeadProgram => SharedEffectModel.MovingHeadProgram;

        public ObservableCollection<MovingHeadFixture.Gobo> MovingHeadGobo => SharedEffectModel.MovingHeadGobo;

        protected void MovingHeadUpdated()
        {
            AsyncOnPropertyChange(nameof(CurrentMovingHeadGobo));
            AsyncOnPropertyChange(nameof(CurrentMovingHeadProgram));
            AsyncOnPropertyChange(nameof(UseDelatedPosition));
            AsyncOnPropertyChange(nameof(UseAlternateColor));
        }

        public abstract MovingHeadFixture.Gobo CurrentMovingHeadGobo { get; set; }

        public abstract MovingHeadFixture.Program CurrentMovingHeadProgram { get; set; }

        public abstract bool? UseDelatedPosition { get; set; }

        public abstract bool? UseAlternateColor { get; set; }

        public double MaxSpeed
        {
            get
            {
                return sharedEffectModel.MaxSpeed;
            }
            set
            {
                sharedEffectModel.MaxSpeed = value;
                AsyncOnPropertyChange(o => this.MaxSpeed);
            }
        }
    }
}
