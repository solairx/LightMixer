using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LightMixer.ViewModel;

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
