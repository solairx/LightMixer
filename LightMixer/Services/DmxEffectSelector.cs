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
                dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneRotateEffect>().First();
                dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();

            }
            else  if (currentPoi != null)
            {
                PoiIsValidSelectEffect(dmxChaser, workingEvent, currentPoi, nextPoi);
            }

        }

        private static void PoiIsValidSelectEffect(DmxChaser dmxChaser, VdjEvent workingEvent, VDJPoi currentPoi, VDJPoi nextPoi)
        {
            if (currentPoi.IsBreak)
            {
                if (dmxChaser.UseFlashTransition && nextPoi != null && GetSecondBeforeNextPOI(workingEvent, nextPoi) < 10)
                {
                    dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();
                    if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 5)
                    {
                        dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<FlashAllEffect>().First();
                        dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<FlashAllEffect>().First();
                    }
                    else
                    {
                        dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneFlashEffect>().First();
                        dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneFlashEffect>().First();
                    }
                    if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 8)
                    {
                        dmxChaser.mBpmDetector.BeatRepeat = 2;
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
                    dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneFlashEffect>().First();
                    dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First();
                    dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First();
                }
            }

            else if (currentPoi.IsEndBreak)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 1;
                dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First();
                dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneRotateEffect>().First();
                dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();
            }
        }

        private static double GetSecondBeforeNextPOI(VdjEvent workingEvent, VDJPoi nextPoi)
        {
            return (nextPoi.Position - workingEvent.Position) / workingEvent.BpmAsDouble / 1000;
        }
    }
}
