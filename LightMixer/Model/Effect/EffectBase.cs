using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            return Convert.ToByte(tentativeValue * (_sharedEffectModel.MaxLightIntesityMovingHead / 100d));
        }

        public EffectBase(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
        {
            
            _sharedEffectModel = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            CurrentValue = currentValue;
            this.intensityGetter = intensityGetter;
            this.intensityFlashGetter = intensityFlashGetter;
            detector.BpmEvent += new BeatDetector.BeatDetector.BpmHandler(mBpmDetector_BpmEvent);
            detector.BeatEvent += new BeatDetector.BeatDetector.BeatHandler(mBpmDetector_BeatEvent);
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

        public abstract void DmxFrameCall(IEnumerable<BeatDetector.VdjEvent> values);


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
