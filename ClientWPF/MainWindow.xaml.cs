using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections;

namespace ClientWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageCollection imageCollection1;
        private ImageCollection imageCollection2;
        private ListBox dragSource = null;
        private WebServeur.ImageTransfertClient client = new WebServeur.ImageTransfertClient();
        private String loginName;

        public MainWindow()
        {
            
            InitializeComponent();
            
            imageCollection2 = new ImageCollection();
            imageCollection1 = new ImageCollection();
            
            // On lie la collectionau ObjectDataProvider déclaré dans le fichier XAML
            ObjectDataProvider imageSource1 = (ObjectDataProvider)FindResource("ImageCollection1");
            ObjectDataProvider imageSource2 = (ObjectDataProvider)FindResource("ImageCollection2");
            imageSource1.ObjectInstance = imageCollection1;
            imageSource2.ObjectInstance = imageCollection2;
        }

        private static byte[] lireFichier(string chemin)
        {
            byte[] data = null;
            FileInfo fileInfo = new FileInfo(chemin);
            int nbBytes = (int)fileInfo.Length;
            FileStream fileStream = new FileStream(chemin, FileMode.Open,
            FileAccess.Read);
            BinaryReader br = new BinaryReader(fileStream);
            data = br.ReadBytes(nbBytes);
            return data;
        }

        // On initie le Drag and Drop
        private void ImageDragEvent(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        // On ajoute l'objet dans la base de données. 
        private void ImageDropEvent2(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            ImageObjet data = (ImageObjet)e.Data.GetData(typeof(ImageObjet));
            ((IList)dragSource.ItemsSource).Remove(data);
            ((IList)parent.ItemsSource).Add(data);
            WebServeur.ImageInfo imageInfo = new WebServeur.ImageInfo();
            imageInfo.AlbumID = comboBox1.Text;
            imageInfo.imageID = data.Nom;
            MemoryStream imageStreamEnMemoire = new MemoryStream(data.Image);
            client.UploadImage(imageInfo, imageStreamEnMemoire);
        }

        private void ImageDropEvent1(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            ImageObjet data = (ImageObjet)e.Data.GetData(typeof(ImageObjet));
            ((IList)dragSource.ItemsSource).Remove(data);
            ((IList)parent.ItemsSource).Add(data);

            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.Filter = "image(*.gif)|*.gif|image(*.jpg)|*.jpg";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FilterIndex = 2;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String path = saveFileDialog.FileName;
                FileStream fileStream = new FileStream(path, FileMode.Create);
                fileStream.Write(data.Image, 0, data.Image.Length);
            }
        }
        // On récupére l'objet que que l'on a dropé
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = (UIElement)source.InputHitTest(point);
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data =
                    source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = (UIElement)VisualTreeHelper.GetParent(element);
                    }
                    if (element == source)
                    {
                        return null;
                    }
                }
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory="c:\\";
            openFileDialog.Filter="image(*.jpg)|*.jpg|image(*.gif)|*.gif";
            openFileDialog.RestoreDirectory=true;
            openFileDialog.FilterIndex=1;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String path = openFileDialog.FileName;
                String imageID = System.IO.Path.GetFileNameWithoutExtension(path);
                imageCollection2.Add(new ImageObjet(imageID,
                lireFichier(path)));
            }       
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            imageCollection1 = new ImageCollection();
            ObjectDataProvider imageSource1 = (ObjectDataProvider)FindResource("ImageCollection1");
            imageSource1.ObjectInstance = imageCollection1;
            String[] photos = client.getAlbumPhotos(comboBox1.Text);
            for (int i = 0; i < photos.Length; i++)
            {
                WebServeur.ImageInfo info = new WebServeur.ImageInfo();
                info.imageID = photos[i];
                Stream stream = client.DownloadImage(info);
                MemoryStream imageStreamEnMemoire = new MemoryStream();
                stream.CopyTo(imageStreamEnMemoire);
                Byte[] bytes = imageStreamEnMemoire.ToArray();
                imageCollection1.Add(new ImageObjet(photos[i], bytes));
            }
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (client.checkLogin(userName.Text,password.Password))
            {
                loginName = userName.Text;
                user.Visibility = Visibility.Hidden;
                userName.Visibility = Visibility.Hidden;
                pass.Visibility = Visibility.Hidden;
                password.Visibility = Visibility.Hidden;
                login.Visibility = Visibility.Hidden;
                newAlbum.Visibility = Visibility.Visible;
                Add.Visibility = Visibility.Visible;
                valid.Visibility = Visibility.Visible;
                comboBox1.Visibility = Visibility.Visible;
                album.Visibility = Visibility.Visible;
                resultat.Text = String.Format("Welcome, {0}!",userName.Text);
                String[] albums = client.getUserAlbum(userName.Text);

                for (int i = 0; i < albums.Length; i++)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    comboBox1.Items.Add(new ComboBoxItem() { Content = albums[i] });
                }
            }
            else
            {
//                resultat.Text = String.Format("user doesn't exist or password isn't correct.");
                resultat.Text = "user doesn't exist or password isn't correct.";
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            client.createNewAlbum(newAlbum.Text, loginName);
            comboBox1.Items.Add(new ComboBoxItem() { Content = newAlbum.Text});
        }

    }
}
