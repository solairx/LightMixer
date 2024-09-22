using LightMixer.Model;
using LightMixerStandard.Model.Fixture.Laser;
using System.Collections.ObjectModel;
using UIFrameWork;

namespace LightMixer.View
{
    public class LaserViewModel : BaseViewModel
    {
        private bool loop;

        public string Name => "Laser";

        private Laser Model { get; set; }

        public bool LaserOn { get => Model.LaserOn; set => Model.LaserOn = value; }

        public bool Loop { get => Model.Loop; set => Model.Loop = value; }
        public bool UseCustomIlda { get; set; }

        public ObservableCollection<LaserEffect>  Effects => new ObservableCollection<LaserEffect>( Model.Effects.Where(o=>!o.Stretch));

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

            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.AsyncOnPropertyChange(nameof(Loop));
            this.AsyncOnPropertyChange(nameof(SelectedEffect));
        }
    }
}
