
using System.Drawing;
using UIFrameWork;

namespace LightMixer.View
{
    public class ItemLineItemViewModel : BaseViewModel
    {
        private double width;
        private Color color;
        private double displayLenght;

        public Color Color { get => color; set => color = value; }
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