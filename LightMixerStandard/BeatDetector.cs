using System.ComponentModel;
using System.Drawing;

namespace BeatDetector
{
    public class BeatDetector : INotifyPropertyChanged
    {
        public static BeatDetector Instance;

        public event PropertyChangedEventHandler PropertyChanged;

        private double _beatRepeat = 1;
        private Color _beatBackground;
        private Color _blackColor = Color.FromArgb(0,0,0);
        private Color _redColor = Color.Red;

        public double BeatRepeat
        {
            get
            {
                return _beatRepeat;
            }
            set
            {
                _beatRepeat = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(BeatRepeat)));
                }
                if (value > 1.05 || value < 0.95)
                {
                    BeatBackground = _redColor;
                }
                else
                {
                    BeatBackground = _blackColor;
                }
            }
        }

        public Color BeatBackground
        {
            get
            {
                return _beatBackground;
            }
            set
            {
                _beatBackground = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(BeatBackground)));
                }
            }
        }

        public BeatDetector()
        {
            Instance = this;
            _beatBackground = _blackColor;
        }
    }
}