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
            MakeRectangle(1,1,facevalue, Xpos, Ypos);; // écriture de la carte

            
        }

        private void MakeRectangle(int CardID, int flipped, int facevalue, int X, int Y) // écriture de la carte
        {
        string ID = "Clone" + CardID.ToString();
        Rectangle Clone1 = new System.Windows.Shapes.Rectangle();
            { 
            Height = 800;
            Width = 1400;
            }

        if(flipped != 0)
            {
                Draw(facevalue, Clone1); //écrit à la carte cID la valeur visible donnée
            } else {
                Draw(0, Clone1); //écrit à la carte cID la valeur visible donnée
            }
        Tripletirage.Children.Add(Clone1);
        Canvas.SetLeft(Clone1, 150);
        Canvas.SetTop(Clone1, 130);

        Air.Mite.Set(Clone1, facevalue);
        c4.Content = Air.Mite.Get(Clone1);
        }

        private void Draw(int facevalue, Rectangle cID) //écrit à la carte cID la valeur visible donnée
        {;
            Uri Carte = new Uri("pack://application:,,,/AirMite;component/" + facevalue + ".png", UriKind.RelativeOrAbsolute);
            BitmapImage CardFace = new BitmapImage(Carte);
            cID.Fill = new ImageBrush(CardFace);
        }

        private void Flip(int CardID) //change la valeur visible de la carte vers celle de la valeur custom cachée
        {
            string ID = CardID.ToString();

            int facevalue = Air.Mite.Get((this.FindName(ID) as Rectangle));

            Uri flip = new Uri("pack://application:,,,/AirMite;component/" + facevalue + ".png", UriKind.RelativeOrAbsolute);
            BitmapImage CardFace = new BitmapImage(flip);
            (this.FindName(ID) as Rectangle).Fill = new ImageBrush(CardFace);
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

