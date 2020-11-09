using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public int card_number = 0;
        public MainWindow()
        {
            InitializeComponent();

        }

        private List<Rectangle> _Deck = new List<Rectangle>();

        
        private void Reset_clicked(object sender, RoutedEventArgs e)
        {         
         
        }


        public void NvTirage(object sender, RoutedEventArgs e)
        {
            card_number = 0;
            foreach (Rectangle Cards in _Deck)
            {
                c3.Content = card_number;
                card_number++;
            }
            int facevalue;
            double Xpos = 0.0,Ypos = 100.0;
            Random rnd = new Random();
            facevalue = rnd.Next(1, 53);
            Rectangle NewClone = MakeRectangle(1,false,facevalue, Xpos, Ypos);; // écriture de la carte
            _Deck.Add(NewClone);
        }

        private Rectangle MakeRectangle(int CardID, bool Flipped, int facevalue, double X, double Y) // écriture de la carte
        {
        string ID = "Clone" + CardID.ToString();
        Rectangle Clone1 = new Rectangle();
            { 
                Tripletirage.Children.Add(Clone1);  // WHY THE **FUCK** DOES RESIZING THE RECTANGLE HERE CAUSES THE WINDOW TO RESIZE ???
                Canvas.SetLeft(Clone1,X);
                Canvas.SetTop(Clone1,Y);
            }
        if (Flipped != false)
            {
                Air.Flip.Set(Clone1, Flipped);
                Paint(facevalue, Clone1); //écrit à la carte cID la valeur visible donnée
                Air.Mite.Set(Clone1, facevalue);
            } else {
                Air.Flip.Set(Clone1, Flipped);  // Setup custom flipped = false
                Paint(0, Clone1); //écrit à la carte cID la valeur visible donnée
                Air.Mite.Set(Clone1, facevalue);    // Setup custom facevalue = vraie valeur cachée
                 }

            c4.Content = Air.Mite.Get(Clone1);  // charge dans c4 la valeur cachée de face
            clause.Content = Flipped;   //charge dans clause la valeur de flipped = false

            Flip(Clone1);// inverse l'état de clone, charge la vraie valeur de face sur la carte
        Flipped = Air.Flip.Get(Clone1);
        clause.Content = Flipped;
            return Clone1;
        }

        private void Paint(int facevalue, Rectangle cID) //écrit à la carte cID la valeur visible donnée, passe l'objet rectangle directement
        {;
            Uri Carte = new Uri("pack://application:,,,/AirMite;component/" + facevalue + ".png", UriKind.RelativeOrAbsolute);
            BitmapImage CardFace = new BitmapImage(Carte);
            c2.Content = Carte;
            cID.Fill = new ImageBrush(CardFace);
            cID.Height = 200;
            cID.Width = 140;    // WHY THE **FUCK** DOES RESIZING THE RECTANGLE HERE DOESN'T CAUSES THE WINDOW TO RESIZE ???
        }

        private void Flip(Rectangle ID) //change la valeur visible de la carte vers celle de la valeur custom cachée
        {

            int facevalue = Air.Mite.Get(ID);
            bool isflipped = Air.Flip.Get(ID);

            Uri flip = new Uri("pack://application:,,,/AirMite;component/" + facevalue + ".png", UriKind.RelativeOrAbsolute);
            BitmapImage CardFace = new BitmapImage(flip);
            ID.Fill = new ImageBrush(CardFace);
            isflipped = !isflipped;
            clause.Content = isflipped;
        }

    }
}

namespace Air  // Ici, on définit un namespace, une classe, puis une propriété qui stockera en XAML la valeur de la carte, même cachée, ainsi qu'un bool qui décrit l'état caché.
{
    public static class Mite
    {

        public static readonly DependencyProperty FaceValueProperty = DependencyProperty.RegisterAttached("Face",typeof(int), typeof(Mite), new FrameworkPropertyMetadata(null)); // définit l'attribut custom

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

    public static class Flip
    {

        public static readonly DependencyProperty FlippedProperty = DependencyProperty.RegisterAttached("Flipped", typeof(bool), typeof(Mite), new FrameworkPropertyMetadata(null)); // définit l'attribut custom

        public static bool Get(UIElement element)  //à utiliser si on veut savoir si la carte est retournée
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (bool)element.GetValue(FlippedProperty);
        }
        public static void Set(UIElement element, bool value)  // à utiliser pour décrire si la carte est retournée
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(FlippedProperty, value);
        }
    }
}

namespace Cardinal // s'occupera de toutes les fonctions et classes liés au mouvement.
{
    public class Movement   // Classe principale du moteur de mouvement
    {
        private bool drag = true;
        private Point StartPt;
        private Point EndPt;
        private double newX, newY;
    }
}