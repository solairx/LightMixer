using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace LightMixer.Model.Fixture
{
    public class FixtureGroup
    {
        public FixtureGroup()
        {
        }

        public ObservableCollection<FixtureBase> FixtureInGroup { get; set; } = new ObservableCollection<FixtureBase>();
    }
}
