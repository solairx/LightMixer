using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using VisualControler.Visual.Laser;
using VisualControler;

namespace LaserDisplay
{
    public class SpinningEffectFull : ILaserEffet
    {
        private double _Bpm = 70;
        private bool Direction = true;
        private bool DoingX = false;
        private bool DrawReverse = false;
        long waveSync = 0;
        private int intervale = 0;
        double xpos = 0;
        double ypos = 0;
        private LaserSetting laserSetting = new LaserSetting();
        private dj djSetting = new dj();
        private int max_x_pos;
        private int max_x_neg;
        private int max_y_pos;
        private int max_y_neg;
        public SpinningEffectFull()
        {
        }

        public void ReadSetting()
        {
            max_x_pos = laserSetting.SEF_Max_x_Pos;
            max_x_neg = laserSetting.SEF_Max_x_neg;
            max_y_pos = laserSetting.SEF_Max_y_Pos;
            max_y_neg = laserSetting.SEF_Max_y_Neg;
            Direction = true;
            DoingX = false;
            DrawReverse = false;
            waveSync = 0;
            intervale = 0;
            xpos = 0;
            ypos = 0;
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
                short[] y = null;
                short[] x = null;
                _Bpm = bpm;
                short[] Buffer = new short[BufferSize / 2];
                if (!Paused)
                {
                    short[] linex = new short[2];
                    short[] liney = new short[2];
                    if (DrawReverse)
                    {
                        linex[0] = ShortFromDouble(xpos);
                        linex[1] = ShortFromDouble(-xpos);
                        liney[0] = ShortFromDouble(ypos);
                        liney[1] = ShortFromDouble(-ypos);
                    }
                    else
                    {
                        linex[0] = ShortFromDouble(-xpos);
                        linex[1] = ShortFromDouble(xpos);
                        liney[0] = ShortFromDouble(-ypos);
                        liney[1] = ShortFromDouble(ypos);
                    }
                    x = Sampler.Resample(linex);
                    y = Sampler.Resample(liney);

                }
                if (Direction)
                {
                    if (DoingX)
                    {
                        ypos += bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6;
                        if (ypos > max_y_pos)
                        {
                            ypos = max_y_pos;
                            xpos = max_x_pos;
                            DoingX = false;
                        }

                    }
                    else
                    {
                        xpos -= bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6; ;
                        if (xpos < max_x_neg)
                        {
                            xpos = max_x_pos;
                            ypos = max_y_neg;
                            DoingX = true;
                        }
                    }
                }
                else
                {
                    if (DoingX)
                    {
                        ypos -= bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6;
                        if (ypos < max_y_neg)
                        {
                            ypos = max_y_pos;
                            xpos = max_x_neg;
                            DoingX = false;
                        }

                    }
                    else
                    {
                        xpos += bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6; ;
                        if (xpos > max_x_pos)
                        {
                            xpos = max_x_pos;
                            ypos = max_y_pos;
                            DoingX = true;
                        }
                    }
                }
               
                for (int i = 0; i < Buffer.Length; i += 2)
                {

                    if (Paused)
                    {
                        Buffer[i + HorizontalPosition] = 0;
                        Buffer[i + VerticalPosition] = 0;
                    }
                    else
                    {
                        Buffer[i + HorizontalPosition] = x[i / 2];
                        Buffer[i + VerticalPosition] = y[i / 2];
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
