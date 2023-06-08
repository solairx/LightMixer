using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightMixer.View.Automation
{
    /// <summary>
    /// Interaction logic for TimeLine.xaml
    /// </summary>
    public partial class TimeLine : UserControl
    {

        public bool IsEditable { get => (DataContext as TimeLineViewModel).IsEditable; set => (DataContext as TimeLineViewModel).IsEditable = value; }

        public TimeLine()
        {
            InitializeComponent();
            this.SizeChanged += TimeLine_SizeChanged;
        }

        private void TimeLine_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            (DataContext as TimeLineViewModel).TimeLineWidth = this.ActualWidth;
        }

        private void Thumb_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private DragDeltaEventArgs dragDelta ;

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            dragDelta = e;
        }

        private void Thumb_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            (DataContext as TimeLineViewModel).ResizeIndividualItem(((sender as Thumb).DataContext as ItemLineItemViewModel), dragDelta);
        }
    }
}
