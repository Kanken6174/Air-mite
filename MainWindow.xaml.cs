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
        private int clonecount = 0;
        public MainWindow()
        {
            InitializeComponent();


            foreach (object obj in Tripletirage.Children) // tires for each object in children of canvas named grappesB
            {
                try
                {
                    Rectangle img = (Rectangle)obj;
                    Uri test = new Uri("0.png", UriKind.Relative);
 
                    img.PreviewMouseDown += new MouseButtonEventHandler(myimg_MouseDown);
                        img.PreviewMouseMove += new MouseEventHandler(myimg_MouseMove);
                        img.PreviewMouseUp += new MouseButtonEventHandler(myimg_MouseUp);
                        img.TextInput += new TextCompositionEventHandler(myimg_TextInput);
                        img.LostMouseCapture += new MouseEventHandler(myimg_LostMouseCapture);
                        img.SetValue(Canvas.LeftProperty, 0.0);
                        img.SetValue(Canvas.TopProperty, 0.0);
                }
                catch// (notacardposition e) TODO: check if card landed on a correct poistion, else, put it back where it was before!
                {
                    //do something
                }
            }


        }



        private void Reset_clicked(object sender, RoutedEventArgs e)
        {

                canvasLeft = 0.0;
                canvasTop = 0.0;
                int counter = 0;
                int Offset = 0;

                while (counter < 3)
                {
                    counter++;
                    Offset -= 110;
                    string name = "ditto" + counter;

                    (this.FindName(name) as ImageBrush).ImageSource = null;   // erases ImageSource
                    (this.FindName(name) as Rectangle).SetValue(Canvas.LeftProperty, -100.0 - Offset); // only accepts floats and doubles!
                    (this.FindName(name) as Rectangle).SetValue(Canvas.TopProperty, canvasTop);
                }
            

        }

        void myimg_LostMouseCapture(object sender, MouseEventArgs e) 
        {
            ((Rectangle)sender).ReleaseMouseCapture();
        }

        void myimg_TextInput(object sender, TextCompositionEventArgs e)
        {
            ((Rectangle)sender).ReleaseMouseCapture();
        }

        void myimg_MouseUp(object sender, MouseButtonEventArgs e) // disables Hook on mouse position
        {
            ((Rectangle)sender).ReleaseMouseCapture();
            
        }

        void myimg_MouseMove(object sender, MouseEventArgs e) // changes variables accordingly if mouse moves
        {
            if (((Rectangle)sender).IsMouseCaptured)
            {
                Point mouseCurrent = e.GetPosition(null);
                double Left = mouseCurrent.X - canvasLeft - 830; // yes i know substracting the card center afterwards is ugly,
                double Top = mouseCurrent.Y - canvasTop - 80;  // but it'll work for now
                ((Rectangle)sender).SetValue(Canvas.LeftProperty, canvasLeft + Left); // Sets new position on canvas for clicked image
                ((Rectangle)sender).SetValue(Canvas.TopProperty, canvasTop + Top); //
                canvasLeft = Canvas.GetLeft(((Rectangle)sender));
                canvasTop = Canvas.GetTop(((Rectangle)sender));
            }
        }

        void myimg_MouseDown(object sender, MouseButtonEventArgs e) // checks for mouse click on children of canvas (drag)
        {
            mouseClick = e.GetPosition(null);
            canvasLeft = Canvas.GetLeft(((Rectangle)sender));   // get left coordinates of clicked picture
            canvasTop = Canvas.GetTop(((Rectangle)sender));     // get top coordinates of clicked picture
            ((Rectangle)sender).CaptureMouse();
        }

        private void NvTirage(object sender, RoutedEventArgs e)
        {
            canvasLeft = 0.0;
            canvasTop = 0.0;
            int counter = 0;
            int Offset = 00;
            Random rnd = new Random();
            /*while (counter < 3)
            {*/
                counter++;
                Offset -= 110;
                string name = "ditto" + counter;
                clonecount = rnd.Next(1, 53);
                if (clonecount > 52 || counter > 1)
                {
                    clonecount = 0;
                }

                /*Uri Carte = new Uri(clonecount + ".png", UriKind.Relative);     //CCreates Uri element, (sort of fileHandle for c#)*/


                /*ImageBrush ib = new ImageBrush();
                ib.ImageSource = new ImageBrush(new BitmapImage(Carte)); // NEEDS FIXING*/
                Rectangle ditto4 = new Rectangle();
                Canvas.SetLeft(ditto4, 150);
                Canvas.SetTop(ditto4, 130);
                ditto4.StrokeThickness = 10;
                ditto4.Height = 200;
                ditto4.Width = 140;
                ditto4.SetValue(Canvas.LeftProperty, 100d);
                ditto4.SetValue(Canvas.TopProperty, 100d);
                Uri Carte = new Uri("pack://AirMite:,,,/Resources/"+ clonecount + ".png", UriKind.Relative);
                BitmapImage CardFace = new BitmapImage(Carte);
                ditto4.Fill = new ImageBrush(CardFace);
                Tripletirage.Children.Add(ditto4);  //is a valid canvas
                /*(this.FindName(name) as Rectangle).SetValue(Canvas.LeftProperty, -100.0 - Offset); // only accepts floats and doubles!
                (this.FindName(name) as Rectangle).SetValue(Canvas.TopProperty, canvasTop);*/
            //}
            /*if (ditto2.ImageSource == ditto3.ImageSource)
            {
                clause.Content = ditto1.ImageSource;
            }
            else
            {
                clause.Content = "nope";
            }
            c2.Content = "ImageSource de Ditto1 : " + ditto1.ImageSource;
            c3.Content = "ImageSource de Ditto2 : " + ditto2.ImageSource;
            c4.Content = "ImageSource de Ditto3 : " + ditto3.ImageSource;
            */
        }

    }
}

