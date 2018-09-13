using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LaserDisplay
{
    public interface ILaserEffet
    {
         void Draw(Graphics g, Pen p);
         void StopDrawing();
         void Beat();
         short[] DrawOnLaser(int HorizontalPosition, int VerticalPosition, int TurnOnPosition, int BufferSize, ResamplingService Sampler, bool Paused, double Bpm, bool IsBeat);
         void Transform();
    }
}
