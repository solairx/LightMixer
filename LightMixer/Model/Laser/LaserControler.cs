using System;
using System.Collections.Generic;
using VisualControler;

namespace LaserDisplay
{
    public class LaserControler
    {
        private List<LaserDisplay> LaserCollection = new List<LaserDisplay>();
        private LaserDisplay GreenLaser;
        public static LaserControler LaserControlerAccess;
        private dj djSetting = new dj();

        public LaserDisplay GreenLaser_
        {
            get
            {
                return GreenLaser;
            }
        }

        public void ReadSetting()
        {
            GreenLaser.SpinningEffectFullScanning.ReadSetting();
            GreenLaser.AudienceScanningEffect.ReadSetting();
            GreenLaser.AudienceScanningREffect.ReadSetting();
            GreenLaser.BeatSkySanning.ReadSetting();
            GreenLaser.SpinningEffectRScanning.ReadSetting();
            GreenLaser.SpinningEffectScanning.ReadSetting();
            GreenLaser.TunnelEffectScanning.ReadSetting();
            GreenLaser.StaticSkyEffect.ReadSetting();
        }
        public LaserControler(int Device)
        {
            LaserControlerAccess = this;
            this.Init();
            VisualControler.ServiceExchangeSingleton.Instance.LaserPause = true;


        }

        public void Init()
        {

            GreenLaser = new LaserDisplay(WaveLib.SpeakerPosition.SPEAKER_FRONT_LEFT, WaveLib.SpeakerPosition.SPEAKER_FRONT_RIGHT, WaveLib.SpeakerPosition.SPEAKER_FRONT_CENTER, WaveLib.SpeakerPosition.SPEAKER_BACK_RIGHT, WaveLib.SpeakerPosition.SPEAKER_BACK_LEFT, djSetting.LaserDevice, false);
            ReadSetting();
        }


        public void Reset()
        {


            try
            {
                Stop();
                GreenLaser = null;
                this.Init();
                ReadSetting();
                Start();


            }
            catch (Exception)
            {
                //  System.Windows.Forms.MessageBox.Show(ex.ToString());
            }


        }

        public void Start()
        {
            GreenLaser.Start();
        }

        public void Stop()
        {

            try
            {

                GreenLaser.Stop();
            }
            catch (Exception)
            {
            }
        }
        public void OnMusic(int Pourcentage, bool IsBeat, double BPM)
        {
            if (GreenLaser != null)
            {
                GreenLaser._OnBeat = IsBeat;
                GreenLaser.BPM = BPM;
            }

        }

        public void OnMusic(int Pourcentage, bool IsBeat)
        {
            if (GreenLaser != null)
            {
                GreenLaser._OnBeat = IsBeat;

            }

        }

        public void OnMusic(int Pourcentage, double BPM)
        {
            if (GreenLaser != null)
            {

                GreenLaser.BPM = BPM;
            }

        }





    }
}
