using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiphon_2._0.Models;

namespace Tiphon_2._0.Metods
{
    class Metod
    {
        public static string AddProduct(Guid Id, string name, double price, string description)
        {

            string result = "Уже существует";
            using (BDContext db = new BDContext())
            {
                db.Database.Migrate();
                bool check = db.Products.Any(el => el.Name == name && el.Price == price && el.Description == description);
                if (!check)
                {
                    Product newproduct = new Product
                    {
                        Name = name,
                        Price = price,
                        Description = description,
                        Id = Guid.NewGuid()
                    };
                    db.Products.Add(newproduct);
                    db.SaveChanges();
                    result = "Сделано";
                }
                return result;
            }
        }
    }
}
