using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Threading;

namespace LightMixer.Model
{
    public abstract class EffectBase
    {
        public abstract string Name
        {
            get;
        }

        public string Schema
        {
            get;
            private set;
        }

        public event DmxFrameHandler DmxFrameEvent;
        public delegate void DmxFrameHandler(ObservableCollection<DmxChannelStatus> currentValue, object caller);
        private Thread runningThread;
        protected Fixture.FixtureCollection CurrentValue;
        protected ObservableCollection<Fixture.FixtureGroup> fixtureGroup;
        public SharedEffectModel _sharedEffectModel;
        protected bool isBeat = false;
        protected double bpm = 60;

        public byte SetValue(byte tentativeValue, DmxChaser.LedType ledInstance)
        {
            return Convert.ToByte(tentativeValue * ((ledInstance == DmxChaser.LedType.HeadLed?_sharedEffectModel.MaxLightIntesity: _sharedEffectModel.MaxBoothIntesity) / 100d));
        }

        public byte SetValueFlash(byte tentativeValue, DmxChaser.LedType ledInstance)
        {
            return Convert.ToByte(tentativeValue * ((ledInstance == DmxChaser.LedType.HeadLed?_sharedEffectModel.MaxLightFlashIntesity:_sharedEffectModel.MaxBoothFlashIntesity) / 100d));
        }

        public byte SetValueMovingHead(byte tentativeValue, DmxChaser.LedType ledInstance)
        {
            return Convert.ToByte(tentativeValue * (_sharedEffectModel.MaxLightIntesityMovingHead / 100d));
        }

        public EffectBase(BeatDetector.BeatDetector detector, Fixture.FixtureCollection currentValue, ObservableCollection< Fixture.FixtureGroup> vfixtureGroup, string vSchema)
        {
            Schema = vSchema;
            fixtureGroup = vfixtureGroup;
            _sharedEffectModel = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<SharedEffectModel>();
            CurrentValue = currentValue;
            detector.BpmEvent += new BeatDetector.BeatDetector.BpmHandler(mBpmDetector_BpmEvent);
            detector.BeatEvent += new BeatDetector.BeatDetector.BeatHandler(mBpmDetector_BeatEvent);
        //    runningThread = new Thread(new ThreadStart(Run));
         //   runningThread.IsBackground = true;
         //   runningThread.Start();
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

        public abstract void DmxFrameCall(DmxChaser.LedType boothLed, IEnumerable<BeatDetector.VdjEvent> values);
    

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
            catch
                (Exception )
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
