using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
