using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using VisualControler.Visual.Laser;
using VisualControler;

namespace LaserDisplay
{
    public class AudienceScanning : ILaserEffet
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
        bool Direction = false;
        private bool DrawReverse = false;
        private long waveSync = 0;

        public AudienceScanning()
        {

        }
        public void ReadSetting()
        {
            max_x_pos = laserSetting.AS_Max_X_pos;
            max_x_neg = laserSetting.AS_Max_X_neg;
            max_y_pos = laserSetting.AS_Max_Y_pos;
            max_y_neg = laserSetting.AS_Max_Y_neg;
            size = laserSetting.AS_SIZE;
            yPos = 0;
            Direction = false;
            DrawReverse = false;
            waveSync = 0;

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

                        Buffer[i + HorizontalPosition] = ShortFromDouble(Math.Cos(((double)waveSync++) / size) * max_x_pos);
                        Buffer[i + VerticalPosition] = ShortFromDouble(yPos);
                        if (Direction)
                        {
                            yPos -= bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6000;
                            if (yPos < max_y_neg)
                            {
                                yPos = max_y_neg;
                                Direction = false;
                            }
                        }
                        else
                        {
                            //System.Windows.Forms.MessageBox.Show(bpm.ToString() + " " + (bpm*(SolairXDj.Controler.VisualControler.ServiceExchangeSingleton.LaserSpeedAdj) / 6000).ToString());
                            yPos += bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6000; ;
                            if (yPos > max_y_pos)
                            {
                                yPos = max_y_pos;
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
            if (d >= 32767) return 32767;
            else if (d <= -32767) return -32767;
            else return Convert.ToInt16(d);

        }
    }
}
