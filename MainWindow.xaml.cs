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

                    (this.FindName(name) as Rectangle).Fill = new ImageBrush(null); ;   // erases ImageSource
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
            int facevalue = 0;
            int CardID = 0;
            int Xpos = 0,Ypos = 0;
            int c;
            Random rnd = new Random();
            facevalue = rnd.Next(1, 53);
            MakeRectangle(CardID,facevalue, Xpos, Ypos);

            for(c=0; c<2; c++)
            {
                MakeRectangle(CardID, 0, Xpos, Ypos);
                MakeRectangle(CardID, 0, Xpos, Ypos);
            }

            
        }

        private void MakeRectangle(int CardID, int facevalue, int X, int Y)
        {
        Rectangle ditto4 = new Rectangle();
        Canvas.SetLeft(ditto4, 150);
        Canvas.SetTop(ditto4, 130);
        ditto4.StrokeThickness = 10;
        ditto4.Height = 200;
        ditto4.Width = 140;
        ditto4.SetValue(Canvas.LeftProperty, 100d);
        ditto4.SetValue(Canvas.TopProperty, 100d);
        Uri Carte = new Uri("pack://application:,,,/AirMite;component/" + facevalue + ".png", UriKind.RelativeOrAbsolute);
        BitmapImage CardFace = new BitmapImage(Carte);
        ditto4.Fill = new ImageBrush(CardFace);
        Tripletirage.Children.Add(ditto4);

        Air.Mite.Set(ditto4, facevalue);
        c4.Content = Air.Mite.Get(ditto4);
        }
    }
}

namespace Air  // Ici, on définit un namespace, une classe, puis une propriété qui stockera en XAML la valeur de la carte, même cachée
{
    public static class Mite
    {

        public static readonly DependencyProperty FaceValueProperty = DependencyProperty.RegisterAttached("MyProperty",typeof(int), typeof(Mite), new FrameworkPropertyMetadata(null)); // définit l'attribut custom

        public static int Get(UIElement element)  //à utiliser si on veut LIRE la valeur de la carte
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (int)element.GetValue(FaceValueProperty);
        }
        public static void Set(UIElement element, int value)  // à utiliser pour ECRIRE sur la valeur de la carte
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(FaceValueProperty, value);
        }
    }// Cet attribut custom s'appelle avec : Air(namespace).Mite(classe publique).Get/Set, un très bon exemple de POO :)
}

