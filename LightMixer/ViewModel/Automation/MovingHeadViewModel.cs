using LightMixer.Model;
using LightMixer.Model.Fixture;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LightMixer.View
{
    public class MovingHeadViewModel : MovingHeadFixtureViewModelBase
    {
        private readonly MovingHeadFixtureCollection movingHeadFixtureCollection;

        public string Name => "Moving Head";


        public MovingHeadViewModel(MovingHeadFixtureCollection fixtures)
        {
            this.movingHeadFixtureCollection = fixtures;

            movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().ForEach(o => o.CurrentEffectChanged += MovingHeadUpdated);
            this.movingHeadFixtureCollection.CurrentEffectChanged += () => AsyncOnPropertyChange(nameof(SelectedEffect));
        }

        private void MovingHeadUpdated()
        {
            AsyncOnPropertyChange(nameof(CurrentMovingHeadGobo));
            AsyncOnPropertyChange(nameof(CurrentMovingHeadProgram));
            AsyncOnPropertyChange(nameof(UseAlternateColor));
            AsyncOnPropertyChange(nameof(UseDelatedPosition));
        }

        
        public ObservableCollection<EffectBase> EffectList => movingHeadFixtureCollection.EffectList;

        public IEnumerable<MovingHeadFixtureViewModelChildren> Childrens
        {
            get
            {
                return movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>()
                    .Select(s => new MovingHeadFixtureViewModelChildren(s));
            }
        }

        public EffectBase SelectedEffect
        {
            get
            {
                return movingHeadFixtureCollection.CurrentEffect;
            }
            set
            {
                movingHeadFixtureCollection.CurrentEffect = value;
            }
        }

        public override MovingHeadFixture.Program CurrentMovingHeadProgram
        {
            get
            {
                if (movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().GroupBy(o=>o.ProgramMode).Count() >1)
                {
                    return MovingHeadFixture.Program.CodeDisable;
                }
                return movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().FirstOrDefault().ProgramMode;
            }
            set
            {
                movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().ForEach(f => f.ProgramMode = value);

            }
        }

        public override MovingHeadFixture.Gobo CurrentMovingHeadGobo
        {
            get
            {
                if (movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().GroupBy(o => o.GoboPaturn).Count() > 1)
                {
                    return MovingHeadFixture.Gobo.Open;
                }
                return movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().FirstOrDefault().GoboPaturn;
            }
            set
            {
                movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().ForEach(f => f.GoboPaturn = value);

            }
        }

        public override bool? UseAlternateColor
        {
            get
            {
                if (movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().GroupBy(o => o.EnableAlternateColor).Count() > 1)
                {
                    return null;
                }
                return movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().FirstOrDefault().EnableAlternateColor;
            }
            set
            {
                movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().ForEach(f => f.EnableAlternateColor = value == true);

            }
        }

        public override bool? UseDelatedPosition
        {
            get
            {
                if (movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().GroupBy(o => o.EnableDelayedPosition).Count() > 1)
                {
                    return null;
                }
                return movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().FirstOrDefault().EnableDelayedPosition;
            }
            set
            {
                movingHeadFixtureCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<MovingHeadFixture>().ForEach(f => f.EnableDelayedPosition = value == true);

            }
        }
        public bool ChildrenVisibility => true;

    }
}
