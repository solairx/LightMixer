using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace LightMixer.Model.Fixture
{
    public class FixtureGroup
    {
        private FixtureBase masterFixture;

        public FixtureBase MasterFixture { 
            get => masterFixture ?? FixtureInGroup.FirstOrDefault();
            set => masterFixture = value; 
        }

        public FixtureGroup()
        {
            FixtureInGroup.CollectionChanged += FixtureInGroup_CollectionChanged;
        }

        private void FixtureInGroup_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (FixtureBase item in e.NewItems)
                {
                    item.OwnerGroup = this;
                    item.InternalInit();
                }
            }
        }

        public ObservableCollection<FixtureBase> FixtureInGroup { get; set; } = new ObservableCollection<FixtureBase>();
    }

    public enum GroupZone
    {
        DjBooth,
        Bar,
        DanceFloor

    }
}
