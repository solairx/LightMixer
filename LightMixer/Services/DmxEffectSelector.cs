﻿using BeatDetector;
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

            var currentPoi = workingEvent?.GetCurrentPoi;
            DisplayElapsed(dmxChaser, workingEvent);

            if (!dmxChaser.AutoChaser)
                return;

            if (currentPoi is AutomatedPoi)
            {
                AutomatedEffect.Get((currentPoi as AutomatedPoi).Automation).Run(workingEvent);
            }
            else if (workingEvent?.IsPoiPlausible == true && currentPoi.ID == 0 && !(currentPoi is ZplanePoi))
            {
                AutomatedEffect.Get(AutomatedEffectEnum.Intro).Run(workingEvent);
            }
            else if (workingEvent?.IsPoiPlausible != true || (currentPoi.ID == 0 && !(currentPoi is ZplanePoi)))
            {
                InvalidTrackInfo(dmxChaser);
            }
            else if (currentPoi != null && workingEvent.IsPoiPlausible)
            {
                PoiIsValidSelectEffect(dmxChaser, workingEvent);
            }
        }

        private static void DisplayElapsed(DmxChaser dmxChaser, VdjEvent workingEvent)
        {
            var elapsed = workingEvent.Elapsed?.ToString();

            if (!string.IsNullOrWhiteSpace(elapsed))
            {
                double elapsedMs;
                if (double.TryParse(elapsed, out elapsedMs))
                {
                    var ts = TimeSpan.FromMilliseconds(elapsedMs);
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

        private void PoiIsValidSelectEffect(DmxChaser dmxChaser, VdjEvent workingEvent)
        {
            var nextPoi = workingEvent?.GetNextPoi;
            var currentPoi = workingEvent?.GetCurrentPoi;
            if (currentPoi.IsBreak)
            {
                if (dmxChaser.UseFlashTransition && nextPoi != null && GetSecondBeforeNextPOI(workingEvent, nextPoi) < 10)
                {
                    AutomatedEffect.Get(AutomatedEffectEnum.BeforeBeatKickIn).Run(workingEvent);
                }
                else if (nextPoi != null && GetSecondBeforeNextPOI(workingEvent, nextPoi) < 20)
                {
                    AutomatedEffect.Get(AutomatedEffectEnum.Before20SecBeatKickIn).Run(workingEvent);
                }
                else
                {
                    AutomatedEffect.Get(AutomatedEffectEnum.Chorus).Run(workingEvent);
                }
            }
            else if (currentPoi.IsEndBreak && GetSecondInCurrentPoi(workingEvent) < 10)
            {
                AutomatedEffect.Get(AutomatedEffectEnum.BeatJustKickIn).Run(workingEvent);
            }
            else if (currentPoi.IsEndBreak)
            {
                AutomatedEffect.Get(AutomatedEffectEnum.Beat).Run(workingEvent);
            }
        }

        private void InvalidTrackInfo(DmxChaser dmxChaser)
        {
            AutomatedEffect.Get(AutomatedEffectEnum.Intro).Run(null);
        }

        private static double GetSecondBeforeNextPOI(VdjEvent workingEvent, VDJPoi nextPoi)
        {
            return (nextPoi.Position - workingEvent.Position) / workingEvent.BpmAsDouble / 1000;
        }

        private static double GetSecondInCurrentPoi(VdjEvent workingEvent)
        {
            return (workingEvent.Position - workingEvent.GetCurrentPoi.Position) / workingEvent.BpmAsDouble / 1000;
        }
    }
}