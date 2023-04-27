using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Shapes;
using Tiphon_2._0.Models;

namespace Tiphon_2._0.Forms
{
    /// <summary>
    /// Логика взаимодействия для AddTiphon.xaml
    /// </summary>
    public partial class AddTiphon : Window
    {
        public Product Product { get; set; }
        public AddTiphon()
        {
            InitializeComponent();
            Product = new Product();
            DataContext = Product;
            Product.Id = Guid.NewGuid();
        }


        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Metods.Metod.AddProduct (Product.Id, Product.Name, Product.Price, Product.Description);
            MainWindow mainWindow = new MainWindow();
            Close();         
            mainWindow.ShowDialog();

        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;

        }

    }
}
