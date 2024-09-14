using BeatDetector;
using System.Collections.Generic;
using System.Drawing;

namespace LightMixer.Model
{
    public abstract class AutomatedEffect
    {
        public static DmxChaser dmxChaser;
        public static SharedEffectModel Model { get; set; }
        public static SceneRenderedService SceneRenderedService { get; set; }

        public Color Color { get; set; }

        public string Name { get; }

        public string DisplayName { get; set; }
        
        public static Dictionary<string, AutomatedEffect> AutomatedEffectList = new Dictionary<string, AutomatedEffect>();

        static AutomatedEffect()
        {
            new Beat();
            new BeatJustKickIn();
            new Chorus();
            new Before20SecBeatKickIn();
            new BeforeBeatKickIn();
            new Intro();
            new LaserHigh();
        }
        public AutomatedEffect(string name)
        {
            Name = name;
            AutomatedEffectList.Add(name, this);
        }

        public static AutomatedEffect Get(string name) => AutomatedEffectList[name];

        public abstract void RunInternal(VdjEvent workingEvent);

        public void Run(VdjEvent workingEvent)
        {
            dmxChaser.SetCurrentAutomationEffectInternal(this);
            RunInternal(workingEvent);
        }

        protected static double GetSecondBeforeNextPOI(VdjEvent workingEvent, VDJPoi nextPoi)
        {
            return (nextPoi.Position - workingEvent.Position) / workingEvent.BpmAsDouble / 1000;
        }

        protected static double GetSecondInCurrentPoi(VdjEvent workingEvent)
        {
            return (workingEvent.Position - workingEvent.GetCurrentPoi.Position) / workingEvent.BpmAsDouble / 1000;
        }
    }
}