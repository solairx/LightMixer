using System;
using System.Drawing;
using VisualControler;
using VisualControler.Visual.Laser;

namespace LaserDisplay
{
    public class SpinningEffect : ILaserEffet
    {
        private double _Bpm = 70;
        private long waveSync = 0;
        private bool Direction = false;
        private bool DrawReverse = false;
        private int intervale = 0;
        private short xpos = 0;
        private double ypos = 0;
        private LaserSetting laserSetting = new LaserSetting();
        private dj djSetting = new dj();

        private int max_y_pos;
        private int max_y_neg;
        private short max_x_pos;
        private short max_x_neg;

        public SpinningEffect()
        {
        }

        public void ReadSetting()
        {
            max_y_pos = laserSetting.SE_Max_y_Pos;
            max_y_neg = laserSetting.SE_Max_y_neg;
            max_x_pos = laserSetting.SE_Max_x_Pos;
            max_x_neg = laserSetting.SE_Max_x_Neg;
            waveSync = 0;
            Direction = false;
            DrawReverse = false;
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
                        linex[0] = max_x_pos;
                        linex[1] = max_x_neg;
                        liney[0] = ShortFromDouble(ypos);
                        liney[1] = ShortFromDouble(-ypos);
                    }
                    else
                    {
                        linex[0] = max_x_neg;
                        linex[1] = max_x_pos;
                        liney[0] = ShortFromDouble(-ypos);
                        liney[1] = ShortFromDouble(ypos);
                    }
                    x = Sampler.Resample(linex);
                    y = Sampler.Resample(liney);
                }
                if (Direction)
                {
                    ypos -= bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6;
                    if (ypos < max_y_neg)
                    {
                        ypos = max_y_neg;
                        Direction = false;
                    }
                }
                else
                {
                    ypos += bpm * (VisualControler.ServiceExchangeSingleton.Instance.LaserSpeedAdj) / 6; ;
                    if (ypos > max_y_pos)
                    {
                        ypos = max_y_pos;
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
                        Buffer[i + HorizontalPosition] = x[i / 2];
                        Buffer[i + VerticalPosition] = y[i / 2];
                    }
                }
                DrawReverse = !DrawReverse;

                return Buffer;
            }
            catch (Exception)
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

        #endregion ILaserEffet Members

        public short ShortFromDouble(double d)
        {
            if (d >= 32767) return 32767;
            else if (d <= -32767) return -32767;
            else return Convert.ToInt16(d);
        }
    }
}