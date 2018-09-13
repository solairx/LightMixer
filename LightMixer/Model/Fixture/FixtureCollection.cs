using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace LightMixer.Model.Fixture
{
    public class FixtureCollection
    {
        private ObservableCollection<FixtureBase> _fixtureList = new ObservableCollection<FixtureBase>();

        public FixtureCollection()
        {

        }

     

    

        public ObservableCollection<FixtureBase> FixtureList
        {
            get
            {
                return _fixtureList;
            }
            set
            {
                _fixtureList = value;
            }
        }


        public byte?[] render()
        {
            byte?[] array = new byte?[512];
            
            foreach (FixtureBase fixture in this.FixtureList)
            {

                byte?[] fixtureValue =  fixture.Render();

                int x = 0;
                for (x = 0; x < 512; x++)
                {
                    if (fixtureValue[x].HasValue)
                        array[x] = fixtureValue[x].Value;
                }
            }
            return array;
        }

        public byte?[] render(byte?[] mergeFrom)
        {
            byte?[] array = new byte?[512];

            foreach (FixtureBase fixture in this.FixtureList)
            {

                byte?[] fixtureValue = fixture.Render();

                int x = 0;
                for (x = 0; x < 512; x++)
                {
                    if (fixtureValue[x].HasValue)
                        array[x] = fixtureValue[x].Value;
                    else if (mergeFrom[x].HasValue)
                        array[x] = mergeFrom[x].Value;
                 
                }
            }
            return array;
        }


    }
}
