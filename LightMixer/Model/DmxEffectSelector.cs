using System.Collections.Generic;
using System.Linq;
using BeatDetector;

namespace LightMixer.Model
{
    public class DmxEffectSelector
    {
        public void Select(DmxChaser dmxChaser, IEnumerable<VdjEvent> lastEvent)
        {
            if (!dmxChaser.AutoChaser)
                return;
            var workingEvent = lastEvent.FirstOrDefault();

            var currentPoi = workingEvent?.GetCurrentPoi;

            var nextPoi = workingEvent?.GetNextPoi;


            if (currentPoi.ID == 0)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 1;
                dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First();
                dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();

            }
            else  if (currentPoi != null)
            {

                if (currentPoi.IsBreak)
                {
                    if (nextPoi != null && GetSecondBeforeNextPOI(workingEvent, nextPoi) < 10)
                    {
                        dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();
                        if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 5)
                        {
                            dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<AllOnEffect>().First();
                        }
                        else
                        {
                            dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneFlashEffect>().First();
                        }
                        if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 8)
                        {
                            dmxChaser.mBpmDetector.BeatRepeat = 1.5;
                        }

                        if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 6)
                        {
                            dmxChaser.mBpmDetector.BeatRepeat = 3;
                        }

                        if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 5)
                        {
                            dmxChaser.mBpmDetector.BeatRepeat = 5;
                        }

                        if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 3)
                        {
                            dmxChaser.mBpmDetector.BeatRepeat = 10;
                        }
                    }
                    else
                    {
                        dmxChaser.mBpmDetector.BeatRepeat = 1;
                        dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First();
                        dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First();
                    }
                }

                else if (currentPoi.IsEndBreak)
                {
                    dmxChaser.mBpmDetector.BeatRepeat = 1;
                    dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First();
                    dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();
                }
            }

        }

        private static double GetSecondBeforeNextPOI(VdjEvent workingEvent, VDJPoi nextPoi)
        {
            return (nextPoi.Position - workingEvent.Position) / workingEvent.BpmAsDouble / 1000;
        }
    }
}
