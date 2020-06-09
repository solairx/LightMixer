using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LaserDisplay
{
    public class Line
    {
        private double _x = 0;
        private int MaxVal = 32767;
        private int MinVal = -32767;
        private double _y = 0;
        private bool _Transparent;


        public Line(double x, double y, bool Transparent)
        {
            this._x = x;
            this._y = y;
            this._Transparent = Transparent;
        }
        public bool Transparent
        {
            get { return _Transparent; }
            set { _Transparent = value; }
        }

        public double x
        {
            get { return _x; }
            set
            {
                if (value > MaxVal)
                    _x = MaxVal;
                else if (value < MinVal)
                    _x = MinVal;
                else
                    _x = value;
            }
        }
        public double y
        {
            get { return _y; }
            set
            {
                if (value > MaxVal)
                    _y = MaxVal;
                else if (value < MinVal)
                    _y = MinVal;
                else
                    _y = value;
            }
        }

        public int y_int
        {
            get { return Convert.ToInt32(_y); }
        }
        public int x_int
        {
            get { return Convert.ToInt32(_x); }
        }

    }
}
