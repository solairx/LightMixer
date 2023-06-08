using BeatDetector;
using LightMixer.Model.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public class DmxEffectSelector
    {
        public SceneRenderedService SceneRenderedService { get; }

        public DmxEffectSelector(SceneRenderedService sceneService)
        {
            SceneRenderedService = sceneService;
            AutomatedEffect.SceneRenderedService = sceneService;
            
        }

        public void Select(DmxChaser dmxChaser, IEnumerable<VdjEvent> lastEvent)
        {
            AutomatedEffect.dmxChaser = dmxChaser;
            AutomatedEffect.Model = dmxChaser.LedEffectCollection[0]._sharedEffectModel;

            var workingEvent = lastEvent.FirstOrDefault();
                        
            DisplayElapsed(dmxChaser, workingEvent);
            var manualEffect = dmxChaser.CurrentAutomationEffect;

            if (dmxChaser.AutoChaser)
            {
                SelectEventToRun(dmxChaser, workingEvent,  workingEvent.Position)
                    .Run(workingEvent);
            }
            else if (manualEffect !=null)
            {
                manualEffect.Run(workingEvent);
            }
        }

        

        public AutomatedEffect SelectEventToRun(DmxChaser dmxChaser, VdjEvent workingEvent, long position)
        {
            var currentPoi = workingEvent?.GetNextPoiBasedOnPosition(position);
            if (currentPoi is AutomatedPoi)
            {
                return AutomatedEffect.Get((currentPoi as AutomatedPoi).Automation);
            }
            else if (workingEvent?.IsPoiPlausible == true && currentPoi.ID == 0 && !(currentPoi is ZplanePoi))
            {
                return AutomatedEffect.Get(Intro.ID);
            }
            else if (workingEvent?.IsPoiPlausible != true || (currentPoi.ID == 0 && !(currentPoi is ZplanePoi)))
            {
                return InvalidTrackInfo();
            }
            else if (currentPoi != null && workingEvent.IsPoiPlausible)
            {
                return PoiIsValidSelectEffect(dmxChaser, workingEvent, position);
            }
            return InvalidTrackInfo();
        }
        
        private void DisplayElapsed(DmxChaser dmxChaser, VdjEvent workingEvent)
        {
            var elapsed = workingEvent.Elapsed?.ToString();

            if (!string.IsNullOrWhiteSpace(elapsed))
            {
                double elapsedMs;
                if (double.TryParse(elapsed, out elapsedMs))
                {
                    var ts = TimeSpan.FromMilliseconds(elapsedMs);
                    dmxChaser.Elapsed = elapsedMs;
                    if (ts.Hours > 0)
                    {
                        dmxChaser.CurrentSongPosition = ts.Hours + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + ":" + ts.Milliseconds.ToString("00");
                    }
                    else
                    {
                        dmxChaser.CurrentSongPosition = ts.Minutes + ":" + ts.Seconds.ToString("00") + ":" + ts.Milliseconds.ToString("00");
                    }
                }
            }
        }

        private AutomatedEffect PoiIsValidSelectEffect(DmxChaser dmxChaser, VdjEvent workingEvent, long position)
        {
            var nextPoi = workingEvent?.GetNextPoiBasedOnPosition(position);
            var currentPoi = workingEvent?.GetPoisAtPosition(position); 
            

            if (currentPoi.IsBreak)
            {
                if (dmxChaser.UseFlashTransition && nextPoi != null && GetSecondBeforeNextPOI(workingEvent, nextPoi, position) < 10)
                {
                    return AutomatedEffect.Get(BeforeBeatKickIn.ID);
                }
                else if (nextPoi != null && GetSecondBeforeNextPOI(workingEvent, nextPoi, position) < 20)
                {
                    return AutomatedEffect.Get(Before20SecBeatKickIn.ID);
                }
                else
                {
                    return AutomatedEffect.Get(Chorus.ID);
                }
            }
            else if (currentPoi.IsEndBreak && GetSecondInCurrentPoi(workingEvent, position) < 10)
            {
                return AutomatedEffect.Get(BeatJustKickIn.ID);
            }
            else if (currentPoi.IsEndBreak)
            {
                return AutomatedEffect.Get(Beat.ID);
            }

            return InvalidTrackInfo();
        }

        private AutomatedEffect InvalidTrackInfo()
        {
            return AutomatedEffect.Get(Intro.ID);
        }

        private static double GetSecondBeforeNextPOI(VdjEvent workingEvent, VDJPoi nextPoi, long currentPosition)
        {
            return (nextPoi.Position - currentPosition) / workingEvent.BpmAsDouble / 1000;
        }

        private static double GetSecondInCurrentPoi(VdjEvent workingEvent, long currentPosition)
        {
            return (currentPosition - workingEvent.GetPoisAtPosition(currentPosition).Position) / workingEvent.BpmAsDouble / 1000;
        }
    }
}