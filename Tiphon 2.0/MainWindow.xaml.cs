using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tiphon_2._0.Forms;
using Tiphon_2._0.Models;

namespace Tiphon_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Product Product { get; set; }
        
        public ObservableCollection<Product> TiphonProduct { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            TiphonProduct = new();
            DataContext = this;
            this.Loaded += Sqlite_Loaded;
        }
        private void Sqlite_Loaded(object sender, RoutedEventArgs e)
        {
            BDContext db = new BDContext();
            db.Database.Migrate();
            List<Product> products = db.Products.ToList();
            LB.ItemsSource = products;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            foreach (Product product in products)
            {
                string str = "Уникальный идентификатор: " + product.Id + "\n" + "Название товара: " + product.Name + "\n" + "Цена товара: " + product.Price + " рублей" + "\n" + "Описание товара: " + product.Description;
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                BitmapImage qrCodeImage = new BitmapImage();
                using (MemoryStream stream = new MemoryStream())
                {
                    qrCode.GetGraphic(20).Save(stream, ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    qrCodeImage.BeginInit();
                    qrCodeImage.CacheOption = BitmapCacheOption.OnLoad;
                    qrCodeImage.StreamSource = stream;
                    qrCodeImage.EndInit();
                }

                TiphonProduct.Add(new Product { Name = product.Name, Price = product.Price, QrCode = qrCodeImage, Description = product.Description, Id = product.Id });
            }

            LB.ItemsSource = TiphonProduct;



        }

        private void btn_add_Click(object sender, RoutedEventArgs e)

        {
            BDContext db = new BDContext();
            db.Database.Migrate();
            Forms.AddTiphon addTiphon = new Forms.AddTiphon();
            Close ();
            addTiphon.ShowDialog ();
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var product = LB.SelectedItem as Product;

            if (new Forms.EditTiphon (product).ShowDialog() == true)
            {
                using (var context = new BDContext())
                {
                    context.Entry(product).State = EntityState.Modified;
                    context.SaveChanges();             
                }
                LB.Items.Refresh();
            }
            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.ShowDialog();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            BDContext db = new BDContext();
            if (Product == null)
            {
                return;
            }
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var product = LB.SelectedItem as Product;
                using (var context = new BDContext())
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                    LB.ItemsSource = context.Products.ToList();
                }
            }
        }
    }
}
