namespace LightMixer.View.Automation
{
    public class TimeLinePois : TimeLine
    {
        public TimeLinePois()
        {
            DataContext = new TimeLinePoisViewModel();
        }
    }

    public class TimeLineML : TimeLine
    {
        private int confidenceLevel;

        public int ConfidenceLevel { get => (DataContext as TimeLineMLViewModel).ConfidenceLevel; set => (DataContext as TimeLineMLViewModel).ConfidenceLevel = value; }

        public TimeLineML()
        {
            DataContext = new TimeLineMLViewModel();
        }
    }
}
