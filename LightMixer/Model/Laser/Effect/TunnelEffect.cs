using System;
using System.Drawing;
using VisualControler;
using VisualControler.Visual.Laser;

namespace LaserDisplay
{
    public class TunnelEffect : ILaserEffet
    {
        private double _Bpm = 70;
        bool Direction = false;
        private bool DrawReverse = false;
        long waveSync = 0;
        private LaserSetting laserSetting = new LaserSetting();
        private int max_x_pos;
        private int max_y_pos;
        private int xsize;
        private int ysize;
        private int currentX;
        private int currentY;

        public TunnelEffect()
        {

        }
        public void ReadSetting()
        {

            max_x_pos = laserSetting.Te_Max_X;
            max_y_pos = laserSetting.TE_Max_Y;
            xsize = laserSetting.Te_SizeX * 2;
            ysize = laserSetting.TE_Size_Y * 2;
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
                        currentX++;
                        currentY++;
                        if (currentX > xsize)
                        {
                            currentX = 50;
                        }
                        if (currentY > ysize)
                        {
                            currentY = 50;
                        }
                        Buffer[i + HorizontalPosition] = ShortFromDouble(Math.Cos(((double)waveSync++) / ServiceExchangeSingleton.Instance.LaserSpeedAdj / 3) * max_x_pos);
                        Buffer[i + VerticalPosition] = ShortFromDouble(Math.Sin(((double)waveSync++) / ServiceExchangeSingleton.Instance.LaserSpeedAdj / 3) * max_y_pos);
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
        #endregion
        public short ShortFromDouble(double d)
        {
            if (d >= 32767) return 32767;
            else if (d <= -32767) return -32767;
            else return Convert.ToInt16(d);
        }
    }
}
