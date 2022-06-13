using LightMixer.ViewModel;
using System.Windows.Controls;

namespace LightMixer.View
{
    /// <summary>
    /// Interaction logic for ManualDmxControl.xaml
    /// </summary>
    public partial class LaserControlerView : UserControl
    {
        public LaserControlerView()
        {
            InitializeComponent();
            DataContext = new LaserControlerViewModel();
        }
    }
}
