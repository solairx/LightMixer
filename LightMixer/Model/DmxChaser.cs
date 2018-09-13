using System;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Practices.Unity;
using LightMixer.Model.Fixture;
using UIFrameWork;


namespace LightMixer.Model
{
    public class DmxChaser : BaseViewModel,IDisposable 
    {
        private BeatDetector.BeatDetector mBpmDetector;
        private DmxModel mModel;
        private Thread runningThread;
        private FixtureCollection fixtureCollection = new FixtureCollection();
        private ObservableCollection<FixtureGroup> fixtureGroupCollection = new ObservableCollection<FixtureGroup>();
        private bool isRunning = true;
        private ObservableCollection<EffectBase> _ledEffectCollection = new System.Collections.ObjectModel.ObservableCollection<EffectBase>();
        private ObservableCollection<EffectBase> _boothEffectCollection = new System.Collections.ObjectModel.ObservableCollection<EffectBase>();
        private ObservableCollection<EffectBase> _movingHeadEffectCollection = new System.Collections.ObjectModel.ObservableCollection<EffectBase>();
        

        private EffectBase _CurrentLedEffect;
        private EffectBase _CurrentBoothEffect;
        private EffectBase _CurrentMovingHeadEffect;

        public EffectBase CurrentBoothEffect
        {
            get
            {
                return _CurrentBoothEffect;
            }
            set
            {

                _CurrentBoothEffect = value;
                this.OnPropertyChanged(o => this.CurrentBoothEffect);
            }
        }

        public EffectBase CurrentLedEffect
        {
            get
            {
                return _CurrentLedEffect;
            }
            set
            {
               
                _CurrentLedEffect = value;
                this.OnPropertyChanged(o => this.CurrentLedEffect);
            }
        }

        public EffectBase CurrentMovingHeadEffect
        {
            get
            {
                return _CurrentMovingHeadEffect;
            }
            set
            {

                _CurrentMovingHeadEffect = value;
                this.OnPropertyChanged(o => this.CurrentMovingHeadEffect);
            }
        }

        public ObservableCollection<EffectBase> LedEffectCollection
        {
            get
            {
                return _ledEffectCollection;
            }
            set
            {
                _ledEffectCollection = value;
                this.OnPropertyChanged(o => this.LedEffectCollection);
            }
        }

        public ObservableCollection<EffectBase> BoothEffectCollection
        {
            get
            {
                return _boothEffectCollection;
            }
            set
            {
                _boothEffectCollection = value;
                this.OnPropertyChanged(o => this._boothEffectCollection);
            }
        }

        public ObservableCollection<EffectBase> MovingHeadEffectCollection
        {
            get
            {
                return _movingHeadEffectCollection;
            }
            set
            {
                _movingHeadEffectCollection = value;
                this.OnPropertyChanged(o => this.MovingHeadEffectCollection);
            }
        }

        public DmxChaser(DmxModel model) 
        {
            
            RgbFixture fixtureLed3 = new RgbFixture(0);
            RgbFixture fixtureLed4 = new RgbFixture(3);
            RgbFixture fixtureLed5 = new RgbFixture(6);
            RgbFixture fixtureLed6 = new RgbFixture(9);
            RgbFixture fixtureLed1 = new RgbFixture(12);
            RgbFixture fixtureLed2 = new RgbFixture(15);
            RgbFixture fixtureLed7 = new RgbFixture(18);
            RgbFixture fixtureLed8 = new RgbFixture(21);

            RgbFixture bootDjLed1 = new RgbFixture(24);
            RgbFixture bootDjLed2 = new RgbFixture(27);
            RgbFixture bootDjLed3 = new RgbFixture(30);
            RgbFixture bootDjLed4 = new RgbFixture(33);
            RgbFixture bootDjLed5 = new RgbFixture(36);
            RgbFixture bootDjLed6 = new RgbFixture(39);
            RgbFixture bootDjLed7 = new RgbFixture(42);
            RgbFixture bootDjLed8 = new RgbFixture(45);
            //RgbFixture bootDjLed9 = new RgbFixture(48);
            fixtureCollection.FixtureList.Add(fixtureLed1);
            fixtureCollection.FixtureList.Add(fixtureLed2);
            fixtureCollection.FixtureList.Add(fixtureLed3);
            fixtureCollection.FixtureList.Add(fixtureLed4);
            fixtureCollection.FixtureList.Add(fixtureLed5);
            fixtureCollection.FixtureList.Add(fixtureLed6);
            fixtureCollection.FixtureList.Add(fixtureLed7);
            fixtureCollection.FixtureList.Add(fixtureLed8);

            fixtureCollection.FixtureList.Add(bootDjLed1);
            fixtureCollection.FixtureList.Add(bootDjLed2);
            fixtureCollection.FixtureList.Add(bootDjLed3);
            fixtureCollection.FixtureList.Add(bootDjLed4);
            fixtureCollection.FixtureList.Add(bootDjLed5);
            fixtureCollection.FixtureList.Add(bootDjLed6);
            fixtureCollection.FixtureList.Add(bootDjLed7);
            fixtureCollection.FixtureList.Add(bootDjLed8);
            

            fixtureCollection.FixtureList.Add(new MovingHeadFixture(400));
            fixtureCollection.FixtureList.Add(new MovingHeadFixture(300));

            FixtureGroup group1 = new FixtureGroup();
            group1.FixtureInGroup.Add(fixtureLed1);
            group1.FixtureInGroup.Add(fixtureLed2);
            fixtureGroupCollection.Add(group1);

            FixtureGroup group2 = new FixtureGroup();
            group2.FixtureInGroup.Add(fixtureLed3);
            group2.FixtureInGroup.Add(fixtureLed4);
            fixtureGroupCollection.Add(group2);

            FixtureGroup group3 = new FixtureGroup();
            group3.FixtureInGroup.Add(fixtureLed5);
            group3.FixtureInGroup.Add(fixtureLed6);
            fixtureGroupCollection.Add(group3);

            FixtureGroup group4 = new FixtureGroup();
            group4.FixtureInGroup.Add(fixtureLed7);
            group4.FixtureInGroup.Add(fixtureLed8);
            fixtureGroupCollection.Add(group4);

            FixtureGroup boothGroup1 = new FixtureGroup();
            boothGroup1.FixtureInGroup.Add(bootDjLed1);
            boothGroup1.FixtureInGroup.Add(bootDjLed2);
            boothGroup1.Schema = "booth";
            fixtureGroupCollection.Add(boothGroup1);

            FixtureGroup boothGroup2 = new FixtureGroup();
            boothGroup2.FixtureInGroup.Add(bootDjLed3);
            boothGroup2.FixtureInGroup.Add(bootDjLed4);
            boothGroup2.Schema = "booth";
            fixtureGroupCollection.Add(boothGroup2);

            FixtureGroup boothGroup3 = new FixtureGroup();
            boothGroup3.FixtureInGroup.Add(bootDjLed5);
            boothGroup3.FixtureInGroup.Add(bootDjLed6);
            boothGroup3.Schema = "booth";
            fixtureGroupCollection.Add(boothGroup3);

            FixtureGroup boothGroup4 = new FixtureGroup();
            boothGroup4.FixtureInGroup.Add(bootDjLed7);
            boothGroup4.FixtureInGroup.Add(bootDjLed8);
            boothGroup4.Schema = "booth";
            fixtureGroupCollection.Add(boothGroup4);


            mModel = model;
            ((LightMixer.App)LightMixer.App.Current).UnityContainer.RegisterInstance<BeatDetector.BeatDetector>(new BeatDetector.BeatDetector());
            mBpmDetector = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<BeatDetector.BeatDetector>();
            mBpmDetector.BeatEvent += new BeatDetector.BeatDetector.BeatHandler(mBpmDetector_BeatEvent);

            LedEffectCollection.Add(new AllOffEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "default"));
            LedEffectCollection.Add(new FlashAllEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "default"));
            LedEffectCollection.Add(new RandomEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "default"));
            LedEffectCollection.Add(new BreathingEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "default"));
            LedEffectCollection.Add(new AllOnEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "default"));
            LedEffectCollection.Add(new ZoneFlashEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "default"));
            LedEffectCollection.Add(new ZoneRotateEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "default"));
            LedEffectCollection.Add(new StaticColorFlashEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "default"));


            BoothEffectCollection.Add(new AllOffEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "booth"));
            BoothEffectCollection.Add(new FlashAllEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "booth"));
            BoothEffectCollection.Add(new AllOnEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "booth"));
            BoothEffectCollection.Add(new RandomEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "booth"));
            BoothEffectCollection.Add(new BreathingEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "booth"));
            BoothEffectCollection.Add(new ZoneFlashEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "booth"));
            BoothEffectCollection.Add(new ZoneRotateEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "booth"));
            BoothEffectCollection.Add(new StaticColorFlashEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection, "booth"));



            MovingHeadEffectCollection.Add(new MovingHeadOffEffect(mBpmDetector, fixtureCollection, fixtureGroupCollection));
            MovingHeadEffectCollection.Add(new MovingHeadFlashAll(mBpmDetector, fixtureCollection, fixtureGroupCollection));
            MovingHeadEffectCollection.Add(new MovingHeadAllOn(mBpmDetector, fixtureCollection, fixtureGroupCollection));

            
            CurrentMovingHeadEffect = MovingHeadEffectCollection[0];
            CurrentLedEffect = LedEffectCollection[0];
            CurrentBoothEffect = BoothEffectCollection[0];




            runningThread = new Thread(new ThreadStart(Run));
            runningThread.IsBackground = true;
            runningThread.Start();
        }

        void mBpmDetector_BeatEvent(bool Beat, object caller)
        {
            if (this.LedEffectCollection.Count !=0)
                if (Beat && this.LedEffectCollection[0]._sharedEffectModel.AutoChangeColorOnBeat)
            {
                this.LedEffectCollection[0]._sharedEffectModel.RotateColor();
            }
        }

        private void Run()
        {
            while (isRunning)
            {
                try
                {
                    this.CurrentLedEffect.DmxFrameCall(LedType.HeadLed);
                    this.CurrentMovingHeadEffect.DmxFrameCall(LedType.MovingHead);
                    this.CurrentBoothEffect.DmxFrameCall(LedType.BoothLed);
                    byte?[] ledArray = fixtureCollection.render();
                    
                    ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<LightService.DmxServiceClient>().UpdateAllDmxValue(ConvertByteArray(ledArray));
                    Thread.Sleep(25);

                }
                catch (Exception vexp )
                {
                }
            }
        }

        public enum LedType
        {
            HeadLed,
            BoothLed,
            MovingHead
        }

        /// <summary>
        /// Called as a callback from a effect base
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="caller"></param>
        void DmxFrameEvent(ObservableCollection<DmxChannelStatus> currentValue, object caller)
        {
            if (caller != this.CurrentLedEffect)
            {
                EffectBase effect = caller as EffectBase;
                return;
            }
            
            byte?[] ledArray = fixtureCollection.render();
               ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<LightService.DmxServiceClient>().UpdateAllDmxValue(ConvertByteArray(ledArray));
        }


        #region IDisposable Members

        public void Dispose()
        {
            isRunning = false;
            mBpmDetector.Stop();
        }

        #endregion

        private byte[] ConvertByteArray(byte?[] arrayToConvert)
        {
            byte [] res = new byte[arrayToConvert.Length];
            int x =0;
            for (x = 0; x < arrayToConvert.Length; x++)
            {
                if (arrayToConvert[x].HasValue)
                    res[x] = arrayToConvert[x].Value;
            }
            return res;
        }
    }
}
