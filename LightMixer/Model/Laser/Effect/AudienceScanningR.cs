using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using VisualControler.Visual.Laser;
using VisualControler;

namespace LaserDisplay
{
    public class AudienceScanningR : ILaserEffet
    {

        private int max_x_pos;
        private int max_x_neg;
        private int max_y_pos;
        private int max_y_neg;
        private int size;
        private LaserSetting laserSetting = new LaserSetting();
        private dj djSetting = new dj();
        private double yPos = 0;
        private double _Bpm = 70;
        private long  waveSync = 0;
        bool Direction = false;
        private bool DrawReverse = false;
        
        public AudienceScanningR()
        {
        }
        public void ReadSetting()
        {
            max_x_pos = laserSetting.ASR_Max_X_pos;
            max_x_neg = laserSetting.ASR_Max_X_neg;
            max_y_pos = laserSetting.ASR_Max_y_pos;
            max_y_neg = laserSetting.ASR_Max_y_neg;
            size = laserSetting.ASR_size;
            yPos = 0;
            waveSync = 0;
            Direction = false;
            DrawReverse = false;
        }

        #region ILaserEffet Members

        public void Draw(Graphics g, Pen p)
        {
        }

        public void StopDrawing()
        {
        }
        private void run()
        {
        }
        public void Transform()
        {
            
        }
        public void Beat()
        {

        }

        public short[] DrawOnLaser(int HorizontalPosition, int VerticalPosition, int TurnOnPosition, int BufferSize, ResamplingService Sampler, bool Paused, double bpm, bool IsBeat)
        {
            
       /*     if (bpm == 0) bpm = 130;
            if (bpm >200 ) bpm = 200;
            if (bpm <75 ) bpm = 75;*/
            try
            {
                if (IsBeat & VisualControler.ServiceExchangeSingleton.Instance.OnBeat)
                {
                    Direction = !Direction;
                }
                _Bpm = bpm;
                short[] Buffer = new short[BufferSize / 2];

                for (int i = 0; i < Buffer.Length; i += 2)
                {

                    if (Paused)
                    {
                        Buffer[i + HorizontalPosition] = 0;
                        Buffer[i + VerticalPosition] = 0;
                    }
                    else
                    {

                        Buffer[i + VerticalPosition] = ShortFromDouble(Math.Cos(((double)waveSync++) / size) * max_y_pos);
                        Buffer[i + HorizontalPosition] = ShortFromDouble(yPos);
                        if (Direction)
                        {
                            yPos -= bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6000;
                            if (yPos < max_x_neg)
                            {
                                yPos = max_x_neg;
                                Direction = false;
                            }
                        }
                        else
                        {
                            yPos += bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6000; ;
                            if (yPos > max_x_pos)
                            {
                                yPos = max_x_pos;
                                Direction = true;
                            }
                        }
                    }

                }
                DrawReverse = !DrawReverse;
                return Buffer;
            }
            catch (Exception d)
            {
                return ReturnNull(BufferSize, HorizontalPosition, VerticalPosition, TurnOnPosition);
            }

            

        }

        private short[] ReturnNull(int BufferSize, int HorizontalPosition, int VerticalPosition, int TurnOnPosition)
        {
            short[] Buffer = new short[BufferSize / 2];

            for (int i = 0; i < Buffer.Length; i += 2)
            {
                Buffer[i + HorizontalPosition] = 0;
                Buffer[i + VerticalPosition] = 0;
            }
            return Buffer;
        }
        #endregion
        public short ShortFromDouble(double d)
        {
          //  try
          //  {
                if (d >= 32767) return 32767;
                else if (d <= -32767) return -32767;
                else return Convert.ToInt16(d);
          /*  }
            catch (Exception de)
            {
                yPos = 0;
                try
                {
                    System.Windows.Forms.MessageBox.Show("Message :" + de.ToString() + "-" + d.ToString() + "-");
                }
                catch (Exception ee)
                {
                    System.Windows.Forms.MessageBox.Show("Unagel to display "+ee.ToString());
                }
                return 0;
            }*/
        }
    }
}
