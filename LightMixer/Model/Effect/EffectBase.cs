﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using LightMixer.Model.Fixture;
using Microsoft.Practices.Unity;

namespace LightMixer.Model
{
    public abstract class EffectBase
    {
        public abstract string Name
        {
            get;
        }
        public event DmxFrameHandler DmxFrameEvent;
        public delegate void DmxFrameHandler(ObservableCollection<DmxChannelStatus> currentValue, object caller);
        protected FixtureCollection CurrentValue;
        private readonly Func<double> intensityGetter;
        private readonly Func<double> intensityFlashGetter;
        protected ObservableCollection<Fixture.FixtureGroup> fixtureGroup;
        public SharedEffectModel _sharedEffectModel;
        protected bool isBeat = false;
        protected bool isSimulatedBeat = false;
        public virtual WledEffect CurrentWledEffect => WledEffect.FX_MODE_CHASE_COLOR;

        protected double bpm = 60;

        public byte SetValue(byte tentativeValue)
        {
            return Convert.ToByte(tentativeValue * ((intensityGetter.Invoke()) / 100d));
        }

        public byte SetValueFlash(byte tentativeValue)
        {
            return Convert.ToByte(tentativeValue * ((intensityFlashGetter.Invoke()) / 100d));
        }

        public byte SetValueMovingHead(byte tentativeValue)
        {
            return Convert.ToByte(tentativeValue * (intensityGetter.Invoke() / 100d));
        }

        public EffectBase(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
        {
            _sharedEffectModel = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            CurrentValue = currentValue;
            this.intensityGetter = intensityGetter;
            this.intensityFlashGetter = intensityFlashGetter;
          //  detector.BpmEvent += new BeatDetector.BeatDetector.BpmHandler(mBpmDetector_BpmEvent);
           // detector.BeatEvent += new BeatDetector.BeatDetector.BeatHandler(mBpmDetector_BeatEvent);
        }


        public byte GetMaxedByte(int val)
        {
            if (val > 255)
                return 255;
            else if (val < 0)
                return 0;
            return Convert.ToByte(val);

        }
        public byte GetMaxedByte(double val)
        {
            if (val > 255)
                return 255;
            else if (val < 0)
                return 0;
            return Convert.ToByte(val);

        }

        private double _lastProcessedBeat = 0;
        private double lastBeatPos = 0;
        private Stopwatch lastBeatPosRunTime = new Stopwatch();

        public void DmxFrameCall(IEnumerable<BeatDetector.VdjEvent> values)
        {
            var currentDeck = values?.FirstOrDefault(); 
            if (currentDeck !=null)
            {
                /*  if (_lastProcessedBeat != currentDeck.BeatPos)
                  {
                      lastBeatPosRunTime = new Stopwatch();
                      lastBeatPos = currentDeck.BeatPos;
                  }*/

                int beatFactor = Convert.ToInt32(BeatDetector.BeatDetector.Instance.BeatRepeat);

                double beatOffSet = currentDeck.BeatPos + 0.0;
                var beatPos = beatOffSet - Math.Truncate(beatOffSet );
                
                var ulpValue = Ulp(beatPos, 1d/BeatDetector.BeatDetector.Instance.BeatRepeat);
                var currentBeatRounded = Math.Truncate(beatOffSet) + ulpValue ;
                if ( _lastProcessedBeat != currentBeatRounded)
                {
                    _lastProcessedBeat = currentBeatRounded;
                    if (isBeat != true)
                    {
                        lastBeatPosRunTime = new Stopwatch();
                        lastBeatPosRunTime.Start();
                        isBeat = true;
                        Debug.WriteLine(DateTime.Now.Millisecond + "BEAT");
                    }
                }
                else if (beatFactor >1)
                {
                    double beatPerSec = currentDeck.BpmAsDouble / 60;
                    var timeBeforeNextBeat = (1000 / beatPerSec) / beatFactor;
                    if (lastBeatPosRunTime.ElapsedMilliseconds > timeBeforeNextBeat)
                    {
                        lastBeatPosRunTime = new Stopwatch();
                        lastBeatPosRunTime.Start();
                        isSimulatedBeat = true;
                        Debug.WriteLine(DateTime.Now.Millisecond + "Simulated BEAT");
                    }
                }
                this.bpm = currentDeck.BpmAsDouble;

            }
            RenderEffect(values);
        }

        public static double Ulp(double source, double epsilon)
        {
            double candidate = 1;
            while (source < candidate)
            {
                candidate -= epsilon;
                if (candidate < 0)
                    return 0;
            }
            return Math.Round(candidate, 3);
        }
        public abstract void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values);


        public void StartCalculation()
        {
            //  if (runningThread.ThreadState == ThreadState.Suspended) 
            //    runningThread.Resume();
        }
        public void StopCalculation()
        {
            //runningThread.Suspend();
        }

        void mBpmDetector_BeatEvent(bool Beat, object caller)
        {

            try
            {

                isBeat = Beat;
            }
            catch (Exception)
            {
            }
        }

        void mBpmDetector_BpmEvent(double Beat, object caller)
        {
            bpm = Beat;
        }

        public void RaiseEvent()
        {
            if (this.DmxFrameEvent != null)
            {
                this.DmxFrameEvent(null, this);
            }
        }

    }
}
