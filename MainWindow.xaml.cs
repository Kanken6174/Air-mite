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
        public int WorldID = 0;
        public int NombredeCartes = 0;

        public MainWindow()
        {
            InitializeComponent();
            c7.Content = CanvasPrincipal.Children.Count;
            
        }

        public List<Rectangle> _Deck = new List<Rectangle>();  //Liste de tous les rectangles sur le terrain (liste de structures de type rectangle)

        
        private void Reset_clicked(object sender, RoutedEventArgs e)
        {
            string allrect="";
            foreach (Rectangle author in _Deck)
            {
                allrect = allrect + author + "\n";
            }
            c8.Content = allrect;
        }


        public void NvTirage(object sender, RoutedEventArgs e)
        {
            card_number = 0;
            int facevalue = 0;
            double Xpos = 200.0,Ypos = 0.0;
            Random rnd = new Random();

            if (NombredeCartes < 52)
            {
                do
                {
                    facevalue = rnd.Next(1, 53);
                } while ((this.FindName("CARD" + facevalue.ToString()) as Rectangle) != null); // vérifie si la carte existe déjà dans tous les x:name présents

                NombredeCartes++;

                Rectangle NewClone = MakeRectangle(1, false, facevalue, Xpos, Ypos); ; // écriture de la carte
                RegisterName("CARD" + facevalue.ToString(), NewClone);
                _Deck.Add(NewClone);

                
            
                c3.Content = "nombre de cartes dans deck : " + _Deck.Count;
            }
            else
            {
                c3.Content = "ALL CARDS ON TERRAIN (" + NombredeCartes + ")";
            }
            foreach (Rectangle carte in CanvasPrincipal.Children)   //déplacé ici car il doit s'exécuter à chaque création de carte (placement des hooks ou [ODEV] sur la carte)
            {
                try
                {
                    Rectangle Card = (Rectangle)carte;

                    Card.PreviewMouseDown += new MouseButtonEventHandler(MaCarte_MouseDown);    //créé un nouvel observateur d'évenement (ODEV) de souris [event handler] pour quand la souris enfonce le click
                    Card.PreviewMouseMove += new MouseEventHandler(MaCarte_MouseMove);          // nouveau ODEV pour quand la souris bouge
                    Card.PreviewMouseUp += new MouseButtonEventHandler(MaCarte_MouseUp);        // nv ODEV pour si le bouton de la souris est relâché

                    Card.TextInput += new TextCompositionEventHandler(MaCarte_TextInput);       // ODEV pour si on tape du texte avec le clavier
                    Card.LostMouseCapture += new MouseEventHandler(MaCarte_LostCursor);         // ODEV pour si la souris sort du bord ou si on perd l'emplacement de la souris en général

                }

                catch
                {
                    c3.Content = "ERROR";
                }
            }
        }

        private Rectangle MakeRectangle(int CardID, bool Flipped, int facevalue, double X, double Y) // écriture de la carte
        {
        
        Rectangle Clone1 = new Rectangle();
            {

            }
            CanvasPrincipal.Children.Add(Clone1);  // WHY THE **FUCK** DOES RESIZING THE RECTANGLE HERE CAUSES THE WINDOW TO RESIZE ???
            Canvas.SetLeft(Clone1, X);
            Canvas.SetTop(Clone1, Y);

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

            c2.Content = Air.Mite.Get(Clone1);  // charge dans c4 la valeur cachée de face
            c1.Content = Flipped;   //charge dans clause la valeur de flipped = false

            Flip(Clone1);// inverse l'état de clone, charge la vraie valeur de face sur la carte
        Flipped = Air.Flip.Get(Clone1);
        c1.Content = Flipped;
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
            bool FaceCachee = Air.Flip.Get(ID);
            if (FaceCachee == true)                  // /!\ ATTENTION /!\ : false = face nombre visible; true = face cachée!!!
            {
                facevalue = 0;
            }

            Uri flip = new Uri("pack://application:,,,/AirMite;component/" + facevalue + ".png", UriKind.RelativeOrAbsolute);
            BitmapImage CardFace = new BitmapImage(flip);
            ID.Fill = new ImageBrush(CardFace);
            FaceCachee = !FaceCachee;
            c1.Content = FaceCachee;

        }
        // En-dessous : code pour le mouvement (très buggé)

        void MaCarte_LostCursor(object sender, MouseEventArgs e)    //arrête la capture de mouvement de la souris si le curseur n'est plus visible
        {
            ((Rectangle)sender).ReleaseMouseCapture();
        }

        void MaCarte_TextInput(object sender, TextCompositionEventArgs e) //arrête la capture de mouvement de la souris si on tape du texte
        {
            ((Rectangle)sender).ReleaseMouseCapture();
        }

        void MaCarte_MouseUp(object sender, MouseButtonEventArgs e) // arrête la capture de mouvement de la souris si on relache le bouton de la souris
        {
            ((Rectangle)sender).ReleaseMouseCapture();
        }

        void MaCarte_MouseMove(object sender, MouseEventArgs e) // changes variables accordingly if mouse moves
        {
            if (((Rectangle)sender).IsMouseCaptured)
            {
                c4.Content = "Souris en cours de capture";
                Point mouseCurrent = e.GetPosition(null);
                double Left = mouseCurrent.X - canvasLeft - 40; // yes i know substracting the card center afterwards is ugly,
                double Top = mouseCurrent.Y - canvasTop - 40;  // but it'll work for now
                ((Rectangle)sender).SetValue(Canvas.LeftProperty, canvasLeft + Left); // Sets new position on canvas for clicked image
                ((Rectangle)sender).SetValue(Canvas.TopProperty, canvasTop + Top); //
                canvasLeft = Canvas.GetLeft(((Rectangle)sender));
                canvasTop = Canvas.GetTop(((Rectangle)sender));
            }
            else
            {
                c4.Content = "Souris non capturée";
            }
        }

        void MaCarte_MouseDown(object sender, MouseButtonEventArgs e) // checks for mouse click on children of canvas (drag)
        {
            mouseClick = e.GetPosition(null);
            canvasLeft = Canvas.GetLeft(((Rectangle)sender));   // get left coordinates of clicked picture
            canvasTop = Canvas.GetTop(((Rectangle)sender));     // get top coordinates of clicked picture
            ((Rectangle)sender).CaptureMouse();
        }

        void Mute_Clicked(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (Rectangle carte in CanvasPrincipal.Children)
            {
                count++;
                c7.Content = count;
            }
            c7.Content = count;
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



// CODE MORT ICI :

/*  // Mauvais code de mouvement, mis au coin
bool drag = false; 

if (Mouse.LeftButton == MouseButtonState.Pressed)
{
    drag = true;
    Cursor = Cursors.Hand;
    MousePos = Mouse.GetPosition(Application.Current.MainWindow);
    c2.Content = "Pressed";
}

if (Mouse.LeftButton != MouseButtonState.Pressed)
{
    drag = false;
    Cursor = Cursors.Arrow;
    Mouse.GetPosition(null);
}

if (drag)
{
    var fatherposition = e.GetPosition(Tripletirage);

    MousePos = fatherposition;

    Canvas.SetLeft(MovableShape, MousePos.X);
    Canvas.SetTop(MovableShape, MousePos.Y);


}

private void MovShp_MouseMove(object sender, MouseEventArgs e)
          {




          }// Fin du code de mouvement

*/