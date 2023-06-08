using Newtonsoft.Json;

namespace BeatDetector
{
    public class MusicMLModel
    {
        public string Version { get; set; }
        public float[][] SmallSampleRate { get; set; }
        public float[][] BigSampleRate { get; set; }

        public string[] SmallSampleTags { get; set; }
        public string[] BigSampleTags { get; set; }

        [JsonIgnore]
        public List<MusicMLTimeLineItem> TimeLines { get; set; } = new List<MusicMLTimeLineItem>();

        public List<MLTag> MLTags { get; } = new List<MLTag>();

        public MLTag GetOrCreateMLTag(string name)
        {
            var existingItem = MLTags.FirstOrDefault(o => o.Name == name);
            if (existingItem != null)
            {
                return existingItem;
            }
            var newMlTag = new MLTag { Name = name };
            MLTags.Add(newMlTag);
            return newMlTag;
        }


    }

    public class MLTag
    {
        public string Name { get; set; }
        public float AverageLow25 { get; internal set; }
        public float AverageHigh25 { get; internal set; }
        public float Average { get; internal set; }

        public float Median
        {
            get
            {
                return (AverageHigh25 + AverageLow25) / 2;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class MusicMLTimeLineItem
    {
        public MusicMLTagType TagType { get; set; }
        public List<MusicMLTag> Tag { get; set; } = new List<MusicMLTag>();
        public TimeSpan Position { get; set; }

        public override string ToString()
        {
            return TagType + " "+ Position.ToString();
        }

    }

    public class MusicMLTag
    {
        public MLTag Tag { get; set; }
        public float Value { get; set; }

        public override string ToString()
        {
            return Tag.Name + " " + Value;
        }
    }

    public enum MusicMLTagType
    {
        Big,
        Small
    }
}