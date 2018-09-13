using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace LightMixer.Model.Fixture
{
    public class FixtureGroup
    { 
        private ObservableCollection<FixtureBase> _fixtureInGroup = new ObservableCollection<FixtureBase>();
        private string schema = "default";


        public FixtureGroup()
        {
        }

        public FixtureGroup(string vschema)
        {
            this.schema = vschema;
        }

        public string Schema
        {
            get
            {
                return schema;
            }
            set
            {
                schema = value;
            }
        }

        public ObservableCollection<FixtureBase> FixtureInGroup
        {
            get
            {
                return _fixtureInGroup;
            }
            set
            {
                _fixtureInGroup = value;

            }
        }
    }
}
