using LightMixer.Model;
using Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using UIFrameWork;

namespace LightMixer.ViewModel
{
    public class DmxConsoleViewModel : BaseViewModel
    {
        private int mSelectedChannel = 0;
        private byte mSelectedValue = 0;
        private DmxChaser _chaser;
        private SharedEffectModel _sharedEffectModel;
        private BeatDetector.BeatDetector _BpmDetector;
        private IntPtr _midiInHandle;
        public static Process MidiController;

        public DmxConsoleViewModel()
        {
            _chaser = LightMixerBootStrap.UnityContainer.Resolve<DmxChaser>();
            _BpmDetector = LightMixerBootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
            _sharedEffectModel = LightMixerBootStrap.UnityContainer.Resolve<SharedEffectModel>();

        }

       
        public DmxChaser Chaser
        {
            get
            {
                return _chaser;
            }
        }

        public SharedEffectModel SharedEffectModel
        {
            get
            {
                return _sharedEffectModel;
            }
        }

        public BeatDetector.BeatDetector BeatDetector
        {
            get
            {
                return _BpmDetector;
            }
        }

        public ICommand ResetBeatCommand
        {
            get
            {
                return new DelegateCommand(() => { BeatDetector.BeatRepeat = 1; });
            }
        }
    }
}