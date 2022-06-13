using System.ComponentModel;
using System.Windows.Media;

namespace BeatDetector
{
    public class BeatDetector : INotifyPropertyChanged
    {
        public static BeatDetector Instance;


        public event PropertyChangedEventHandler PropertyChanged;
        private double _beatRepeat = 1;
        private Brush _beatBackground;
        private Brush _blackColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private Brush _redColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));


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

        public Brush BeatBackground
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
