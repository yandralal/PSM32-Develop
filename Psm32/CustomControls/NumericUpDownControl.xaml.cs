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
    /// Interaction logic for NumericUpDownControl.xaml
    /// </summary>
    public partial class NumericUpDownControl : UserControl
    {
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDownControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)); //new PropertyMetadata(0.0));


        public string PropLabel
        {
            get { return (string)GetValue(PropLabelProperty); }
            set { SetValue(PropLabelProperty, value); }
        }

        public static readonly DependencyProperty PropLabelProperty =
            DependencyProperty.Register("PropLabel", typeof(string), typeof(NumericUpDownControl), new PropertyMetadata(string.Empty));


        public string UnitsLabel
        {
            get { return (string)GetValue(UnitsLabelProperty); }
            set { SetValue(UnitsLabelProperty, value); }
        }

        public static readonly DependencyProperty UnitsLabelProperty =
            DependencyProperty.Register("UnitsLabel", typeof(string), typeof(NumericUpDownControl), new PropertyMetadata(string.Empty));


        public NumericUpDownControl()
        {
            InitializeComponent();
        }

        public void OnUpClicked(object sender, RoutedEventArgs e)
        {
            Value += 1;
        }

        public void OnDownClicked(object sender, RoutedEventArgs e)
        {
            Value -= 1;
        }
    }
}
