using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Psm32.CustomControls
{
    /// <summary>
    /// Interaction logic for QSUnitChannel.xaml
    /// </summary>
    public partial class QSUnitChannel : UserControl
    {
        public QSUnitChannel()
        {
            InitializeComponent();
        }

        public string ChannelColor
        {
            get { return (string)GetValue(ChannelColorProperty); }
            set { SetValue(ChannelColorProperty, value); }
        }

        public static readonly DependencyProperty ChannelColorProperty =
            DependencyProperty.Register("ChannelColor", typeof(string), typeof(QSUnitChannel), new PropertyMetadata("Black"));


        public string StatusColor
        {
            get { return (string)GetValue(StatusColorProperty); }
            set { SetValue(StatusColorProperty, value); }
        }

        public static readonly DependencyProperty StatusColorProperty =
            DependencyProperty.Register("StatusColor", typeof(string), typeof(QSUnitChannel), new PropertyMetadata("#27a045"));

        public string ChannelId
        {
            get { return (string)GetValue(ChannelIdProperty); }
            set { SetValue(ChannelIdProperty, value); }
        }

        public static readonly DependencyProperty ChannelIdProperty =
            DependencyProperty.Register("ChannelId", typeof(string), typeof(QSUnitChannel), new PropertyMetadata(""));

        public string ChannelIdColor
        {
            get { return (string)GetValue(ChannelIdColorProperty); }
            set { SetValue(ChannelIdColorProperty, value); }
        }

        public static readonly DependencyProperty ChannelIdColorProperty =
            DependencyProperty.Register("ChannelIdColor", typeof(string), typeof(QSUnitChannel), new PropertyMetadata("Black"));
    }
}
