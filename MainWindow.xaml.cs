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

namespace AirMite
{

    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point mouseClick;
        private double canvasLeft;
        private double canvasTop;
        public MainWindow()
        {
            InitializeComponent();
            grappeB1.Visibility = Visibility.Collapsed;
            grappeB2.Visibility = Visibility.Collapsed;
            grappeB3.Visibility = Visibility.Collapsed;
            grappeB4.Visibility = Visibility.Collapsed;
            grappeB5.Visibility = Visibility.Collapsed;
            grappeB6.Visibility = Visibility.Collapsed;

            foreach (object obj in gappesB.Children)
            {
                try
                {
                    Image img = (Image)obj;
                    img.PreviewMouseDown += new MouseButtonEventHandler(myimg_MouseDown);
                    img.PreviewMouseMove += new MouseEventHandler(myimg_MouseMove);
                    img.PreviewMouseUp += new MouseButtonEventHandler(myimg_MouseUp);
                    img.TextInput += new TextCompositionEventHandler(myimg_TextInput);
                    img.LostMouseCapture += new MouseEventHandler(myimg_LostMouseCapture);
                    img.SetValue(Canvas.LeftProperty, 0.0);
                    img.SetValue(Canvas.TopProperty, 0.0);
                }
                catch
                {
                    //do something
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Hides "play" menu
            grappeA1.Visibility = Visibility.Collapsed;
            grappeA2.Visibility = Visibility.Collapsed;
            grappeA3.Visibility = Visibility.Collapsed;

            grappeB1.Visibility = Visibility.Visible;
            grappeB2.Visibility = Visibility.Visible;
            grappeB3.Visibility = Visibility.Visible;
            grappeB4.Visibility = Visibility.Visible;
            grappeB5.Visibility = Visibility.Visible;
            grappeB6.Visibility = Visibility.Visible;
        }

        void myimg_LostMouseCapture(object sender, MouseEventArgs e)
{
    ((Image)sender).ReleaseMouseCapture();
}

void myimg_TextInput(object sender, TextCompositionEventArgs e)
{
    ((Image)sender).ReleaseMouseCapture();
}

void myimg_MouseUp(object sender, MouseButtonEventArgs e)
{
    ((Image)sender).ReleaseMouseCapture();
}

void myimg_MouseMove(object sender, MouseEventArgs e)
{
    if (((Image)sender).IsMouseCaptured)
    {
        Point mouseCurrent = e.GetPosition(null);
        double Left = mouseCurrent.X - canvasLeft;
        double Top = mouseCurrent.Y - canvasTop;
        ((Image)sender).SetValue(Canvas.LeftProperty, canvasLeft + Left);
        ((Image)sender).SetValue(Canvas.TopProperty, canvasTop + Top);
        canvasLeft = Canvas.GetLeft(((Image)sender));
        canvasTop = Canvas.GetTop(((Image)sender));
    }
}

void myimg_MouseDown(object sender, MouseButtonEventArgs e)
{
    mouseClick = e.GetPosition(null);
    canvasLeft = Canvas.GetLeft(((Image)sender));
    canvasTop = Canvas.GetTop(((Image)sender));
    ((Image)sender).CaptureMouse();
}
    }
}

