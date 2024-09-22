using ILDA.net;
using System.Runtime.CompilerServices;

namespace LightMixerStandard.Model.Fixture.Laser
{

    public class LaserEffect
    {
        public string Name { get; set; }
        public HeliosPoint[][] Points { get; set; }

        public LaserEffectMood Mood { get; set; }

        public string FileName { get; set; }

        public bool Stretch { get; set; } = false;



        public static LaserEffect LoadFrom(LaserEffectMood mood, string fileName, string name)
        {

            
            HeliosPoint[][] frames = ReadPointFromFile(fileName).Result;

            return new LaserEffect
            {
                Name = name,
                Points = frames,
                Mood = mood,
                FileName = fileName,
                Stretch = false,
            };
        }

        public static LaserEffect LoadFromAsync(LaserEffectMood mood, string fileName, string name)
        {



            var newLaserEffect = new LaserEffect
            {
                Name = name,
                Mood = mood,
                FileName = fileName,
                Stretch = true
            };


            Task.Run(() =>
            {
                newLaserEffect.Points = ReadPointFromFile(fileName).Result;
            });

            return newLaserEffect;
        }

        private static Task<HeliosPoint[][]> ReadPointFromFile( string fileName)
        {
            return Task.Run<HeliosPoint[][]>(() =>
            {
                var file = IldaFile.Open(fileName);
                if (file == null)
                    return null;
                int frameId = 0;
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

                return frames;
            });
        }

        static ushort GetShortUshortValue(short ildaValue)
        {
            var factor = (Convert.ToDouble(ildaValue) + 32768) / 65535;
            var calculatedFactor = 4000 * factor;
            return Convert.ToUInt16(calculatedFactor);
        }
    }
}