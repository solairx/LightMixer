using System;
using System.Drawing;
using VisualControler.Visual.Laser;
using VisualControler;

namespace LaserDisplay
{
    public class SpinningEffectR : ILaserEffet
    {
        private double _Bpm = 70;
        bool Direction = false;
        private bool DrawReverse = false;
        private int intervale = 0;
        short xpos = 0;
        double ypos = 0;
        long waveSync = 0;

        private LaserSetting laserSetting = new LaserSetting();
        private dj djSetting = new dj();
        private short max_y_pos;
        private short max_y_neg;
        private int max_x_pos;
        private int max_x_neg;
        public SpinningEffectR()
        {
        }
        public void ReadSetting()
        {
            max_y_pos = laserSetting.SER_Max_y_Pos;
            max_y_neg = laserSetting.SER_Max_y_neg;
            max_x_pos = laserSetting.SER_Max_x_Pos;
            max_x_neg = laserSetting.SER_Max_x_neg;
            Direction = false;
            DrawReverse = false;
            intervale = 0;
            xpos = 0;
            ypos = 0;
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
                        linex[0] = max_y_pos;
                        linex[1] = max_y_neg;
                        liney[0] = ShortFromDouble(ypos);
                        liney[1] = ShortFromDouble(-ypos);
                    }
                    else
                    {
                        linex[0] = max_y_neg;
                        linex[1] = max_y_pos;
                        liney[0] = ShortFromDouble(-ypos);
                        liney[1] = ShortFromDouble(ypos);
                    }
                    x = Sampler.Resample(linex);
                    y = Sampler.Resample(liney);

                }
                if (Direction)
                {
                    ypos -= bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6;
                    if (ypos < max_x_neg)
                    {
                        ypos = max_x_neg;
                        Direction = false;
                    }
                }
                else
                {
                    ypos += bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6; ;
                    if (ypos > max_x_pos)
                    {
                        ypos = max_x_pos;
                        Direction = true;
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
                        Buffer[i + HorizontalPosition] = y[i / 2];
                        Buffer[i + VerticalPosition] = x[i / 2];
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

        public short ShortFromDouble ( double d )
        {
            if (d >= 32767) return 32767;
            else if (d <= -32767) return -32767;
            else return Convert.ToInt16(d);

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
    }
}
