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

namespace Psm32.CustomControls;

public class ModalControl : ContentControl
{
    public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register("IsOpen", typeof(bool), typeof(ModalControl),
            new PropertyMetadata(false));

    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }

    static ModalControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalControl), new FrameworkPropertyMetadata(typeof(ModalControl)));
        BackgroundProperty.OverrideMetadata(typeof(ModalControl), new FrameworkPropertyMetadata(CreateDefaultBackground()));
    }

    private static object CreateDefaultBackground()
    {
        return new SolidColorBrush(Colors.Black)
        {
            Opacity = 0.3
        };
    }
}