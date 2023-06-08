using System.Windows.Media;
using UIFrameWork;

namespace LightMixer.View
{
    internal class ItemLineItemViewModel : BaseViewModel
    {
        private double width;
        private SolidColorBrush color;
        private double displayLenght;

        public SolidColorBrush Color { get => color; set => color = value; }
        public double Position { get => width; set => width = value; }

        public string Name { get; set; }
        public double DisplayLenght
        {
            get => displayLenght;
            internal set
            {
                displayLenght = value;
                AsyncOnPropertyChange(nameof(DisplayLenght));
            }
        }

    }
}