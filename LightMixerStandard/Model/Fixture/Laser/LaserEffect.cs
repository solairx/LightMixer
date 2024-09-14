using ILDA.net;
using System.Runtime.CompilerServices;

namespace LightMixerStandard.Model.Fixture.Laser
{
    public enum LaserEffectMood
    {
        None = 0 ,
        Low = 1 ,
        Mid = 2 ,
        Hight = 3 
    }

    public class LaserEffect
    {
        public string Name { get; set; }
        public HeliosPoint[][] Points { get; set; }

        public LaserEffectMood Mood { get; set; }

        

        public static LaserEffect LoadFrom(LaserEffectMood mood, string fileName, string name)
        {
            
            int frameId = 0;
            //var file = IldaFile.Open("example1114.ild");
            //var file = IldaFile.Open("example1227.ild");
            var file = IldaFile.Open(fileName);
            HeliosPoint[][] frames = new HeliosPoint[file.Count()][];
            foreach (var ildaFrame in file)
            {
                int pointId = 0;
                frames[frameId] = new HeliosPoint[ildaFrame.Count()];
                foreach (var ildaPoint in ildaFrame)
                {
                    frames[frameId][pointId] = new HeliosPoint
                    {
                        Blue = ildaPoint.Color.B,
                        Red = ildaPoint.Color.R,
                        Green = ildaPoint.Color.G,
                        X = GetShortUshortValue(ildaPoint.X),
                        Y = GetShortUshortValue(ildaPoint.Y),
                        Intensity = 0xFF
                    };
                    pointId++;
                }
                frameId++;
            }

            return new LaserEffect
            {
                Name = name,
                Points = frames,
                Mood = mood,
            };
        }
        static ushort GetShortUshortValue(short ildaValue)
        {
            var factor = (Convert.ToDouble(ildaValue) + 32768) / 65535;
            var calculatedFactor = 4000 * factor;
            return Convert.ToUInt16(calculatedFactor);
        }
    }
}