using System;
using System.Drawing;
using VisualControler.Visual.Laser;
namespace LaserDisplay
{
    public class BeatSky : ILaserEffet
    {
        private double _Bpm = 70;
        private long waveSync = 0;


        private int max_x_pos;
        private int size;
        private LaserSetting laserSetting = new LaserSetting();

        private bool DrawReverse = false;
        public BeatSky()
        {

        }
        public void ReadSetting()
        {
            max_x_pos = laserSetting.BS_Max_X;
            size = laserSetting.Bs_Size;
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
        bool abs = false;
        public short[] DrawOnLaser(int HorizontalPosition, int VerticalPosition, int TurnOnPosition, int BufferSize, ResamplingService Sampler, bool Paused, double bpm, bool IsBeat)
        {
            try
            {
                _Bpm = bpm;
                short[] Buffer = new short[BufferSize / 2];
                int w = BufferSize / 4;
                if (IsBeat)
                {
                    abs = !abs;
                }
                for (int i = 0; i < Buffer.Length; i += 2)
                {
                    w--;
                    if (Paused)
                    {
                        Buffer[i + HorizontalPosition] = 0;
                        Buffer[i + VerticalPosition] = 0;
                    }
                    else
                    {




                        if (abs)
                        {
                            Buffer[i + VerticalPosition] = ShortFromDouble(Math.Cos(((double)waveSync++) / size) * max_x_pos);
                            Buffer[i + HorizontalPosition] = 0;
                        }
                        else
                        {
                            Buffer[i + HorizontalPosition] = ShortFromDouble(Math.Cos(((double)waveSync++) / size) * max_x_pos);
                            Buffer[i + VerticalPosition] = 0;
                        }



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
        public short ShortFromDouble(double d)
        {
            if (d >= 32767) return 32767;
            else if (d <= -32767) return -32767;
            else return Convert.ToInt16(d);

        }
        #endregion
    }
}
