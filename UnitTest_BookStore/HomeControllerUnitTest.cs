using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using moq;

namespace UnitTest_BookStore
{
    [TestClass]
    public class HomeControllerUnitTest
    {
        Product p1;
        Product p2;
        Product p3;
        List<Product> productList;
        Category c1;
        Category c1;
        List<Category> categoryList;
        HomeController ctrl;

        Mock<IUnitOfWork> _unitOfWork;

        public HomeControllerUnitTest()
        {
            p1= new Product { ProductId =1, Name="MVC Book",Description = "My MVC Book", UnitPrice = 120, CategoryId = 1 };
            p2= new Product { ProductId = 2, Name = "Note Book", Description = "My Note Book", UnitPrice = 220, CategoryId = 1 };
            p3 = new Product { ProductId = 3, Name = "React Book", Description = "My React Book", UnitPrice = 320, CategoryId = 1 };


            c1 = new Category { CategoryId = 1, Name = "Books", Description = "My Books" };
            c2 = new Category { CategoryId = 2, Name = "NotePads", Description = "My NotePads" };




            productList = new List<Product>();
            productList.Add(p1);
            productList.Add(p2);

            categoryList = new List<Category>();
            categoryList.Add(c1);
            categoryList.Add(c2);

            _unitOfWork = new Mock<IUnitOfWork>;


            ctrl = new HomeController(_unitOfWork.Object);

        }






        [TestMethod]
        public void Index()
        {
            _unitOfWork.Setup(u => u.Product.GetAll().Return(productList));

            var result = ctrl.Index() as ViewResult;

            var model = result.Model as categoryList<Product>;

            CollectionAssert.Contains(model, p1);
            CollectionAssert.Contains(model, p2);

        }
    }
}
