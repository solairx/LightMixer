using BeatDetector;
using System.Collections.Generic;

namespace LightMixer.Model
{
    public abstract class AutomatedEffect
    {
        public static DmxChaser dmxChaser;
        public static SharedEffectModel Model { get; set; }
        public static SceneRenderedService SceneRenderedService { get; set; }

        public AutomatedEffectEnum Name { get; }
        
        public static Dictionary<AutomatedEffectEnum, AutomatedEffect> AutomatedEffectList = new Dictionary<AutomatedEffectEnum, AutomatedEffect>();

        static AutomatedEffect()
        {
            new Beat();
            new BeatJustKickIn();
            new Chorus();
            new Before20SecBeatKickIn();
            new BeforeBeatKickIn();
            new Intro(); 
        }
        public AutomatedEffect(AutomatedEffectEnum name)
        {
            Name = name;
            AutomatedEffectList.Add(name, this);
        }

        public static AutomatedEffect Get(AutomatedEffectEnum name) => AutomatedEffectList[name];

        public abstract void RunInternal(VdjEvent workingEvent);

        public void Run(VdjEvent workingEvent)
        {
            dmxChaser.CurrentAutomationEffect = this;
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