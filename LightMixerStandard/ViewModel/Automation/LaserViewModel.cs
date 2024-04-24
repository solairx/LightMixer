using LightMixer.Model;
using LightMixerStandard.Model.Fixture.Laser;
using System.Collections.ObjectModel;
using UIFrameWork;

namespace LightMixer.View
{
    public class LaserViewModel : BaseViewModel
    {
        public string Name => "Laser";

        private Laser Model { get; set; }

        public bool LaserOn { get; set; }
        public bool UseCustomIlda { get; set; }

        public ObservableCollection<LaserEffect> Effects => Model.Effects;

        public LaserEffect SelectedEffect
        {
            get
            {
                return Model.SelectedEffect;
            }
            set
            {
                Model.SelectedEffect = value;
            }
        }

        public LaserViewModel(LaserFixtureCollection lasersCollections )
        {
            Model = lasersCollections.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<Laser>().FirstOrDefault();
        }
    }
}
