using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;
using VisualControler.Visual.Laser;
namespace LaserDisplay
{
    public class StaticSky : ILaserEffet
    {
        private double _Bpm = 70;
        bool Direction = false;
        private bool DrawReverse = false;
        long waveSync = 0;
        private int max_x_pos  ;
        private int sizex ;
        private LaserSetting laserSetting = new LaserSetting();
        
        public StaticSky()
        {
            
        }
        public void ReadSetting()
        {
            max_x_pos = laserSetting.SS_Max_X;
            sizex = laserSetting.SS_Size;
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
        
        
        /*public short[] DrawOnLaser(int HorizontalPosition, int VerticalPosition, int TurnOnPosition, int BufferSize, ResamplingService Sampler, bool Paused, double bpm, bool IsBeat)
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
                        Buffer[i + HorizontalPosition] = ShortFromDouble(Math.Cos(((double)waveSync++) / sizex) * max_x_pos);
                        Buffer[i + VerticalPosition] = 0;
                    }

                }
                DrawReverse = !DrawReverse;


                return Buffer;
            }
            catch (Exception d)
            {
                return ReturnNull(BufferSize, HorizontalPosition, VerticalPosition, TurnOnPosition);
            }

        }*/

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
                        Buffer[i + HorizontalPosition] = ShortFromDouble(Math.Cos(((double)waveSync++) / sizex) * max_x_pos);
                        Buffer[i + VerticalPosition] = 0;
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
