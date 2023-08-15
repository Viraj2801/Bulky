using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        

        public void Update(Product obj)
        {
            var objFormDb=_db.Products.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFormDb!=null)
            {
                objFormDb.Title = obj.Title; 
                objFormDb.ISBN = obj.ISBN;
                objFormDb.Price = obj.Price;
                objFormDb.Price50= obj.Price50;
                objFormDb.ListPrice= obj.ListPrice;
                objFormDb.Price100= obj.Price100;
                objFormDb.Description = obj.Description;
                objFormDb.CategoryId = obj.CategoryId;
                objFormDb.Author=obj.Author;
                objFormDb.ProductImages=obj.ProductImages;
                //if(obj.ImageUrl!=null)
                //{
                //    objFormDb.ImageUrl= obj.ImageUrl;
                //}
                
            }
        }
    }
}
