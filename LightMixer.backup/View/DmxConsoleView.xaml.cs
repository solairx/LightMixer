using LightMixer.ViewModel;
using System.Windows.Controls;

namespace LightMixer.View
{
    /// <summary>
    /// Interaction logic for DmxConsoleView.xaml
    /// </summary>
    public partial class DmxConsoleView : UserControl
    {
        public DmxConsoleView()
        {
            InitializeComponent();
            DataContext = new DmxConsoleViewModel();
        }
    }
}