using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VisualControler;

namespace SolairXDj
{
    public class VisualEffect : IDisposable
    {
        private double Bpm = 80;
        private bool BeatQueued = false;
        private bool Indice = false;
        private bool BeatExec = false;
        private int LightNumber = 0;
        private int rotate = 0;
        private int target = 0;
        private int b1 = 0;
        private int queuedled = 8;
        private OpenDmx.OpenDMX dmx;
        private dj djSetting = new dj();
        private LedDemo ledDemo = new LedDemo();
        private LaserDisplay.LaserControler _LaserControler;
        public VisualEffect()
        {
            try
            {
                if (!djSetting.EnableLaser)
                {
                    MessageBox.Show("Laser Disable");
                }
                else
                {
                    _LaserControler = new LaserDisplay.LaserControler(djSetting.LaserDevice);
                    _LaserControler.Start();
                }
            }
            catch (Exception d)
            {
                //MessageBox.Show("Failled to start laser service \r\n\r\n"+d.ToString());
            }
            dmx = new OpenDmx.OpenDMX();
            dmx.Start();
            Thread VisThread = new Thread(new ThreadStart(Start));
            VisThread.IsBackground = true;
            VisThread.Start();

        }

        public void OnMusic(int Pourcentage, bool IsBeat, double BPM, bool UseMonitor)
        {
            if (VisualControler.ServiceExchangeSingleton.Instance.ManualBeat) IsBeat = true;
            if ((Pourcentage > 20))
            {
                this.Bpm = BPM / 4;
                if (IsBeat) BeatQueued = true;
                _LaserControler.OnMusic(Pourcentage, IsBeat, Bpm);
            }
        }
        private void Start()
        {
            /*while (Controler.ControlerAccess == null)
            {
                Thread.Sleep(1000);
            }*/
            Thread.Sleep(500);
            /*while (!Controler.ControlerAccess.IsHandleCreated)
            {
                Thread.Sleep(1000);
            }*/
            try
            {


                while (true)
                {
                    rotate++;
                    if ((rotate > 100 * djSetting.TimeBetweenEvent) && (VisualControler.ServiceExchangeSingleton.Instance.AutoChangeEvent))
                    {
                        VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID++;
                        rotate = 0;
                    }
                    if (!VisualControler.ServiceExchangeSingleton.Instance.OnBeat) NoLed();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 0) NoLed();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 1) Effect1();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 2) Effect2();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 3) Effect3();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 4) Effect4();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 5) Effect5();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 6) Effect6();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 7) Effect7();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 8) Effect8();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 9) Effect9();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 10) Effect10();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 11) Effect11();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 12) Effect12();
                    else if (VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID == 13) Effect13();
                    else VisualControler.ServiceExchangeSingleton.Instance.LedCurrentEventID = 0;

                    /* if (rotate < 1000) Effect4();
                     else if (rotate < 2000) Effect2();
                     else if (rotate < 3000) Effect1();
                     else if (rotate < 3000) Effect3();
                     else rotate = 0;*/

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                {

                }
                throw;
            }
        }
        public void Stop()
        {
            try
            {
                _LaserControler.Stop();
                this.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void NoLed()
        {
            int x = 0;
            for (x = 0; x < 16; x++)
            {
                ledDemo.ChangeLed(x, false);
            }
        }
        private void Effect1()
        {
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 2)
                {
                    ledDemo.ChangeLed(x, Indice);
                    ledDemo.ChangeLed(x + 1, !Indice);

                }

                for (x = 0; x < 16; x += 2)
                {
                    dmx.SetDmxValue(1, 255);
                    dmx.SetDmxValue(1, 255);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        private void Effect4()
        {
            if (BeatQueued)
            {
                int x = 0;
                if (target >= 4) target = 0;
                if (target < 0) target = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x + target, true);
                    { if (target != 0)ledDemo.ChangeLed(x, false); }
                    { if (target != 1) ledDemo.ChangeLed(x + 1, false); }
                    { if (target != 2) ledDemo.ChangeLed(x + 2, false); }
                    { if (target != 3) ledDemo.ChangeLed(x + 3, false); }
                }
                target++;

                BeatQueued = false;
            }
        }
        private void Effect2()
        {
            int x = 0;
            if (BeatExec)
            {
                for (x = 0; x < 16; x++)
                {
                    ledDemo.ChangeLed(x, false);
                }
            }

            if (BeatQueued)
            {
                for (x = 0; x < 16; x++)
                {
                    ledDemo.ChangeLed(x, true);
                }
                // Indice = !Indice;
                BeatQueued = false;
                BeatExec = true;
            }

        }
        private void Effect3()
        {

            int x = 0;
            target++;
            if (BeatExec)
            {
                for (x = 0; x < 16; x++)
                {
                    if (x == queuedled)
                        ledDemo.ChangeLed(x, true);
                    else
                        ledDemo.ChangeLed(x, false);
                }
                BeatExec = false;
            }
            else if (BeatQueued)
            {
                for (x = 0; x < 16; x++)
                {
                    ledDemo.ChangeLed(x, true);
                }
                BeatQueued = false;
                BeatExec = true;
            }
            else if ((500 / (Bpm + 1)) < target)
            {
                target = 0;
                //MessageBox.Show("t");
                this.queuedled++;
                if (queuedled == 16) queuedled = 0;
                for (x = LightNumber; x < 16; x++)
                {
                    if (x == queuedled)
                        ledDemo.ChangeLed(x, true);
                    else
                        ledDemo.ChangeLed(x, false);
                }
            }
        }

        private void Effect5()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x, true);
                    ledDemo.ChangeLed(x + 1, false);
                    ledDemo.ChangeLed(x + 2, false);
                    ledDemo.ChangeLed(x + 3, false);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        private void Effect6()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x, false);
                    ledDemo.ChangeLed(x + 1, false);
                    ledDemo.ChangeLed(x + 2, false);
                    ledDemo.ChangeLed(x + 3, true);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        private void Effect7()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x, false);
                    ledDemo.ChangeLed(x + 1, false);
                    ledDemo.ChangeLed(x + 2, true);
                    ledDemo.ChangeLed(x + 3, false);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        private void Effect8()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x, false);
                    ledDemo.ChangeLed(x + 1, true);
                    ledDemo.ChangeLed(x + 2, false);
                    ledDemo.ChangeLed(x + 3, false);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        /// <summary>
        /// color blink
        /// </summary>

        private void Effect10()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x, Indice);
                    ledDemo.ChangeLed(x + 1, false);
                    ledDemo.ChangeLed(x + 2, false);
                    ledDemo.ChangeLed(x + 3, false);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        private void Effect11()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x, false);
                    ledDemo.ChangeLed(x + 1, false);
                    ledDemo.ChangeLed(x + 2, false);
                    ledDemo.ChangeLed(x + 3, Indice);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        private void Effect12()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x, false);
                    ledDemo.ChangeLed(x + 1, false);
                    ledDemo.ChangeLed(x + 2, Indice);
                    ledDemo.ChangeLed(x + 3, false);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        private void Effect13()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 4)
                {
                    ledDemo.ChangeLed(x, false);
                    ledDemo.ChangeLed(x + 1, Indice);
                    ledDemo.ChangeLed(x + 2, false);
                    ledDemo.ChangeLed(x + 3, false);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void Effect9()
        {
            //Color On
            if (BeatQueued)
            {

                int x = 0;
                for (x = 0; x < 16; x += 1)
                {
                    ledDemo.ChangeLed(x, true);
                }
                Indice = !Indice;
                BeatQueued = false;
            }
        }



        #region IDisposable Members

        public void Dispose()
        {
            dmx.Stop();
        }

        #endregion
    }
}