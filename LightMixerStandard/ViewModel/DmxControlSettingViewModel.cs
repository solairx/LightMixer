using LightMixer.Model;
using LightMixer.Model.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using UIFrameWork;
using Unity;

namespace LightMixer.ViewModel
{
    public class DmxControlSettingViewModel : UIFrameWork.BaseViewModel
    {
        private long mBreak = 27600;
        private long mMbb = 0;
        private long mMab = 200;
        private string panDesign;
        private string tiltDesign;
        private string shutterDesign;
        private string sizeDesign;

        public DmxControlSettingViewModel()
        {
        }

        public DmxModel DmxModel
        {
            get
            {
                return LightMixerBootStrap.UnityContainer.Resolve<DmxModel>();
            }
        }

        public long Break
        {
            get
            {
                return mBreak;
            }
            set
            {
                mBreak = value;
                AsyncOnPropertyChange(o => this.Break);
               // BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().SetBreak(value);
            }
        }

        public long MBB
        {
            get
            {
                return mMbb;
            }
            set
            {
                mMbb = value;
                AsyncOnPropertyChange(o => this.MBB);
            //    BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().SetMBB(value);
            }
        }

        public long Mab
        {
            get
            {
                return mMab;
            }
            set
            {
                this.mMab = value;
                AsyncOnPropertyChange(o => this.Mab);
                //BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().SetMab(value);
            }
        }

        public string PanDesign
        {
            get
            {
                return panDesign;
            }
            set
            {
                this.panDesign = value;
                AsyncOnPropertyChange(o => this.PanDesign);
            }
        }

        public string TiltDesign
        {
            get
            {
                return tiltDesign;
            }
            set
            {
                this.tiltDesign = value;
                AsyncOnPropertyChange(o => this.TiltDesign);
            }
        }

        public string ShutterDesign
        {
            get
            {
                return shutterDesign;
            }
            set
            {
                this.shutterDesign = value;
                AsyncOnPropertyChange(o => this.ShutterDesign);
            }
        }

        public string SizeDesign
        {
            get
            {
                return sizeDesign;
            }
            set
            {
                this.sizeDesign = value;
                AsyncOnPropertyChange(o => this.SizeDesign);
            }
        }

        public DelegateCommand TestDesign => new DelegateCommand(ExecuteTextDesign, CanExecuteTestDesign);

        private bool CanExecuteTestDesign()
        {
            return true;
        }

        private void ExecuteTextDesign()
        {
            MovingHeadProgramTest.PanListDesignShared = StringToIntList(PanDesign).ToArray();
            MovingHeadProgramTest.TiltListDesignShared = StringToIntList(tiltDesign).ToArray();
            MovingHeadProgramTest.MaxDimmerDesignShared = StringToIntList(ShutterDesign).ToArray();
            MovingHeadProgramTest.InitialSizeShared = StringToIntList(SizeDesign).FirstOrDefault();
            MovingHeadProgramTest.Reset();
        }

        public static IEnumerable<ushort> StringToIntList(string str)
        {
            if (String.IsNullOrEmpty(str))
                yield break;

            foreach (var s in str.Split(','))
            {
                ushort num;
                if (ushort.TryParse(s, out num))
                    yield return num;
            }
        }
    }
}