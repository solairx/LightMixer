using System;
using System.Collections.Generic;
using System.Drawing;

namespace LaserDisplay
{
    public class LineCollection : ICollection<Line>
    {
        System.Collections.Generic.List<Line> _InnerList = new List<Line>();

        #region Interface Implementation

        #region ICollection<Line> Members


        public void Add(Line item)
        {
            this._InnerList.Add(item);
        }

        void ICollection<Line>.Clear()
        {
            this._InnerList.Clear();
        }

        bool ICollection<Line>.Contains(Line item)
        {
            return this._InnerList.Contains(item);
        }

        void ICollection<Line>.CopyTo(Line[] array, int arrayIndex)
        {
            this._InnerList.CopyTo(array, arrayIndex);
        }

        int ICollection<Line>.Count
        {
            get { return this._InnerList.Count; }
        }

        bool ICollection<Line>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<Line>.Remove(Line item)
        {
            if (this._InnerList.Contains(item))
                this._InnerList.Remove(item);
            else return false;
            return true;
        }

        #endregion

        #region IEnumerable<Line> Members

        IEnumerator<Line> IEnumerable<Line>.GetEnumerator()
        {
            return this._InnerList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #endregion

        public void Draw(Graphics G, System.Drawing.Pen P)
        {
            Line p = this._InnerList[0];
            bool first = true;
            lock (this._InnerList)
            {
                foreach (Line l in this._InnerList)
                {
                    if (!first)
                    {
                        if (!l.Transparent)
                            G.DrawLine(P, (p.x_int + 32767) / 256, (p.y_int + 32767) / 256, (l.x_int + 32767) / 256, (l.y_int + 32767) / 256);
                        p = l;
                    }
                    else
                    {
                        first = false;
                    }
                }
            }
        }
        public short[] GetAllXCoordonate(bool DrawReverse)
        {
            Line p = this._InnerList[0];

            //   bool first = true;
            short[] list = new short[_InnerList.Count];
            lock (this._InnerList)
            {
                int x = 0;
                if (DrawReverse)
                {

                    for (x = 0; x < _InnerList.Count; x++)
                    {

                        list[x] = ConvertIntToShort(_InnerList[x].x_int);
                        //   System.Windows.Forms.MessageBox.Show(_InnerList[x].y_int.ToString() + "r, " + list[x].ToString());

                    }
                }
                else
                {
                    for (x = _InnerList.Count - 1; x != 0; x--)
                    {
                        list[x] = ConvertIntToShort(_InnerList[x].x_int);
                        //  System.Windows.Forms.MessageBox.Show(_InnerList[x].y_int.ToString() + "f, " + list[x].ToString());
                    }
                }
            }
            return list;
        }
        public short[] GetAllYCoordonate(bool DrawReverse)
        {
            Line p = this._InnerList[0];
            short[] list = new short[_InnerList.Count];
            lock (this._InnerList)
            {
                int x = 0;
                if (DrawReverse)
                {

                    for (x = 0; x < _InnerList.Count; x++)
                    {
                        list[x] = ConvertIntToShort(_InnerList[x].y_int);
                        //System.Windows.Forms.MessageBox.Show(_InnerList[x].y_int.ToString() + ", " + list[x].ToString());
                    }
                }
                else
                {
                    for (x = _InnerList.Count - 1; x != 0; x--)
                    {
                        list[x] = ConvertIntToShort(_InnerList[x].y_int);
                    }

                }
            }
            return list;
        }
        public short[] GetAllTransparent(bool DrawReverse)
        {
            Line p = this._InnerList[0];
            short[] list = new short[_InnerList.Count];
            lock (this._InnerList)
            {
                int x = 0;
                if (DrawReverse)
                {
                    for (x = 0; x < _InnerList.Count; x++)
                    {
                        if (_InnerList[x].Transparent) list[x] = 32767;
                        else list[x] = -32767;

                    }
                }
                else
                {
                    for (x = _InnerList.Count - 1; x != 0; x--)
                    {
                        if (_InnerList[x].Transparent) list[x] = 32767;
                        else list[x] = -32767;
                    }
                }
            }
            return list;
        }
        private short ConvertIntToShort(int r)
        {
            if (r > short.MaxValue) return short.MaxValue;
            else if (r < short.MinValue) return short.MinValue;
            else return Convert.ToInt16(r);
        }
        public bool Translate(int x, int y)
        {
            bool outofrange = false;
            foreach (Line l in this._InnerList)
            {

                l.x += x;
                l.y += y;
                if ((l.x + x) > 32767) outofrange = true;
                if ((l.x + x) < -32767) outofrange = true;
                if ((l.y + y) > 32767) outofrange = true;
                if ((l.y + y) < -32767) outofrange = true;
            }
            return outofrange;
        }

        public void Zoom(double x, double y, double ratio)
        {
            foreach (Line l in this._InnerList)
            {

                l.x *= ratio;
                l.y *= ratio;
            }
        }

        public void Rotate(int x, int y, double angle)
        {
            foreach (Line l in this._InnerList)
            {
                double c = Math.Sqrt(l.x * l.x + l.y * l.y);
                if (c == 0) c = 0.0000001;

                l.y = Math.Sin(Math.Tanh(l.y / l.x) + angle) / c;
                l.x = Math.Cos(Math.Tanh(l.y / l.x) + angle) / c;
            }
        }
        public void Clean(Graphics g)
        {
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, 255, 255));
        }


    }
}
