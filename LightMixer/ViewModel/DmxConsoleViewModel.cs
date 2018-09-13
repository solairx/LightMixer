using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using LightMixer.Model;
using UIFrameWork;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;

namespace LightMixer.ViewModel
{
    public class DmxConsoleViewModel : BaseViewModel  
    {
        private int mSelectedChannel = 0;
        private byte mSelectedValue = 0;
        private DmxChaser _chaser ;
        private SharedEffectModel _sharedEffectModel;
        private BeatDetector.BeatDetector _BpmDetector;
        public DmxConsoleViewModel()
        {
            _chaser = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<DmxChaser>();
            _BpmDetector = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<BeatDetector.BeatDetector>();
            _sharedEffectModel = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<SharedEffectModel>();
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
                return new DelegateCommand(()=> { BeatDetector.BeatRepeat = 1; });
            }
        }





    }
}
