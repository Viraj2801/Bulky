//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Xunit;
//using BulkyBook.Models;
//using BulkyBook.DataAccess.Repository.IRepository;
//using Moq;
//using System.Collections.Generic;
//using BulkyBookWeb.Areas.Admin.Controllers;
//using Microsoft.AspNetCore.Hosting;
//using System.Linq.Expressions;

//namespace UnitTest_BookStore
//{
//    public class ProductControllerUnitTest
//    {
//        [Fact]
//        public void Index_ReturnsAViewResult_WithAListOfProducts()
//        {
//            // Arrange
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(u => u.Product.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
//                .Returns(new List<Product>());

//            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

//            var controller = new ProductController(mockUnitOfWork.Object, mockWebHostEnvironment.Object);

//            // Act
//            var result = controller.Index();

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<List<Product>>(viewResult.ViewData.Model);
//            Assert.Empty(model);
//        }

//    }
//}
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Xunit;
//using BulkyBook.Models;
//using BulkyBook.DataAccess.Repository.IRepository;
//using BulkyBookWeb.Areas.Admin.Controllers;
//using Microsoft.AspNetCore.Hosting;
//using System.Linq.Expressions;

//namespace UnitTest_BookStore
//{
//    public class ProductControllerUnitTest
//    {
//        [Fact]
//        public void Index_ReturnsAllProducts()
//        {

//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
//            var controller = new ProductController(mockUnitOfWork.Object, mockWebHostEnvironment.Object);


//            var mockProducts = new List<Product>
//    {
//        new Product
//        {
//            Id = 1,
//            Title = "Fortune of Time",
//            Description = "Lorem Ipsum...",
//            ISBN = "SWD9999001",
//            Author = "Billy Spark",
//            ListPrice = 99,
//            Price = 90,
//            Price50 = 85,
//            Price100 = 80,
//            CategoryId = 1,
//        },
//        new Product
//        {
//            Id = 2,
//            Title = "Dark Skies",
//            Description = "Lorem Ipsum...",
//            ISBN = "CAW777777701",
//            Author = "Nancy Hoover",
//            ListPrice = 40,
//            Price = 30,
//            Price50 = 25,
//            Price100 = 20,
//            CategoryId = 1,
//        },
//        new Product
//        {
//            Id = 3,
//            Title = "Viraj",
//            Description = "Viraj",
//            ISBN = "Viraj",
//            Author = "Viraj Hoover",
//            ListPrice = 400,
//            Price = 301,
//            Price50 = 225,
//            Price100 = 220,
//            CategoryId = 1,
//        },

//    };
//            mockUnitOfWork.Setup(u => u.Product.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
//                .Returns(mockProducts.AsEnumerable());

//            var result = controller.Index();

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsType<List<Product>>(viewResult.Model);
//            Assert.Equal(mockProducts.Count, model.Count);
//            for (int i = 0; i < mockProducts.Count; i++)
//            {
//                Assert.Equal(mockProducts[i].Id, model[i].Id);
//                Assert.Equal(mockProducts[i].Title, model[i].Title);
//            }
//        }

//    }
//}

//using System.Collections.Generic;
//using BulkyBook.Models;
//using BulkyBook.DataAccess.Repository.IRepository;
//using BulkyBookWeb.Areas.Admin.Controllers;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using Xunit;
//using System.Linq.Expressions;

//namespace Book_Test
//{
//    public class ProductControllerIntegrationTest
//    {
//        [Fact]
//        public void Index_ReturnsListOfProducts()
//        {
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

//            var expectedProducts = new List<Product>
//            {
//                new Product { Id = 1, Title = "Product 1", Category = new Category { Id = 1, Name = "Category 1" } },
//                new Product { Id = 2, Title = "Product 2", Category = new Category { Id = 2, Name = "Category 2" } }
//            };

//            mockUnitOfWork.Setup(u => u.Product.GetAll(
//    It.IsAny<Expression<Func<Product, bool>>>(),
//    It.IsAny<string>()
//)).Returns(expectedProducts);

//            var controller = new ProductController(mockUnitOfWork.Object, mockWebHostEnvironment.Object);

//            var result = controller.Index() as ViewResult;

//            Assert.NotNull(result);

//            var model = result.Model as List<Product>;
//            Assert.NotNull(model);

//            Assert.Equal(expectedProducts.Count, model.Count);

//        }
//    }
//}





//using Microsoft.AspNetCore.Mvc;
//using Xunit;
//using BulkyBook.Models;
//using BulkyBook.DataAccess.Repository.IRepository;
//using System.Collections.Generic;
//using BulkyBookWeb.Areas.Admin.Controllers;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using System.Linq;
//using System;
//using BulkyBook.DataAccess.Repository;
//using BulkyBook.DataAcess.Data;

//namespace Book_Test
//{
//    public class ProductControllerIntegrationTest : IDisposable
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public ProductControllerIntegrationTest()
//        {
//            var services = new ServiceCollection();
//            services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseInMemoryDatabase(databaseName: "TestDatabase"));

//            services.AddScoped<IUnitOfWork, UnitOfWork>();

//            _serviceProvider = services.BuildServiceProvider();

//            SeedTestData();
//        }

//        private void SeedTestData()
//        {
//            using (var scope = _serviceProvider.CreateScope())
//            {
//                var serviceProvider = scope.ServiceProvider;
//                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

//                var products = GetMockProducts(1);
//                dbContext.Products.AddRange(products);
//                dbContext.SaveChanges();
//            }
//        }

//        [Theory]
//        [InlineData(1)]
//        [InlineData(2)]
//        [InlineData(3)]
//        [InlineData(4)]
//        public void IndexProducts(int categoryId)
//        {
//            using (var scope = _serviceProvider.CreateScope())
//            {
//                var serviceProvider = scope.ServiceProvider;
//                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

//                var controller = new ProductController(new UnitOfWork(dbContext), null);

//                var result = controller.Index();

//                var viewResult = Assert.IsType<ViewResult>(result);
//                var model = Assert.IsAssignableFrom<List<Product>>(viewResult.ViewData.Model);
//                var actualData = dbContext.Products.ToList();
//                Assert.Equal(actualData.Count, model.Count);
//            }
//        }

//        public void Dispose()
//        {
//            using (var scope = _serviceProvider.CreateScope())
//            {
//                var serviceProvider = scope.ServiceProvider;
//                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
//                dbContext.Database.EnsureDeleted();
//            }
//        }
//        private List<Product> GetMockProducts(int categoryId)
//        {
//            List<Product> products = new List<Product>();

//            switch (categoryId)
//            {
//                case 1:
//                    products.Add(new Product { Id = 1, Title = "Fortune of Time", CategoryId = 1, Author = "Author1", Description = "Description1", ISBN = "ISBN1" });
//                    products.Add(new Product { Id = 2, Title = "Dark Skies", CategoryId = 1, Author = "Author2", Description = "Description2", ISBN = "ISBN2" });
//                    break;
//                case 2:
//                    products.Add(new Product { Id = 4, Title = "Cotton Candy", CategoryId = 2, Author = "Author3", Description = "Description3", ISBN = "ISBN3" });
//                    products.Add(new Product { Id = 5, Title = "Rock in the Ocean", CategoryId = 2, Author = "Author4", Description = "Description4", ISBN = "ISBN4" });
//                    break;
//                case 3:
//                    products.Add(new Product { Id = 6, Title = "Leaves and Wonders", CategoryId = 3, Author = "Author5", Description = "Description5", ISBN = "ISBN5" });
//                    break;
//                case 4:
//                    products.Add(new Product { Id = 7, Title = "Viraj", CategoryId = 4, Author = "Author6", Description = "Description6", ISBN = "ISBN6" });
//                    break;
//                default:
//                    break;
//            }

//            return products;
//        }

//    }
//}



//using Microsoft.AspNetCore.Mvc;
//using Xunit;
//using BulkyBook.Models;
//using BulkyBook.DataAccess.Repository.IRepository;
//using Moq;
//using System.Collections.Generic;
//using BulkyBookWeb.Areas.Admin.Controllers;
//using Microsoft.AspNetCore.Hosting;
//using System.Linq.Expressions;
//using BulkyBook.DataAcess.Data;
//using Microsoft.EntityFrameworkCore;

//namespace Book_Test
//{
//    public class ProductControllerUnitTest
//    {
//        [Theory]
//        [InlineData(1)]
//        [InlineData(2)]
//        [InlineData(3)]
//        [InlineData(4)]
//        public void IndexProducts(int categoryId)
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//               .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
//               .Options;

//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            var products = GetMockProducts(categoryId);


//            using (var context = new ApplicationDbContext(options))
//            {
//                foreach (var product in products)
//                {
//                    context.Products.Add(product);
//                }
//                context.SaveChanges();
//            }

//            mockUnitOfWork.Setup(u => u.Product.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
//                .Returns(products);

//            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
//            var controller = new ProductController(mockUnitOfWork.Object, mockWebHostEnvironment.Object);

//            var result = controller.Index();

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<List<Product>>(viewResult.ViewData.Model);
//            Assert.Equal(products.Count, model.Count);
//            Assert.Equal(products.Select(p => p.Title), model.Select(p => p.Title));
//        }

//        private List<Product> GetMockProducts(int categoryId)
//        {
//            List<Product> products = new List<Product>();

//            switch (categoryId)
//            {
//                case 1:
//                    products.Add(new Product
//                    {
//                        Id = 1,
//                        Title = "Fortune of Time",
//                        CategoryId = 1,
//                        Author = "John Doe",
//                        Description = "Lorem Ipsum",
//                        ISBN = "123456789"
//                    });
//                    products.Add(new Product
//                    {
//                        Id = 2,
//                        Title = "Dark Skies",
//                        CategoryId = 1,
//                        Author = "Jane Doe",
//                        Description = "Another book",
//                        ISBN = "987654321"
//                    });
//                    break;
//                case 2:
//                    products.Add(new Product
//                    {
//                        Id = 4,
//                        Title = "Cotton Candy",
//                        CategoryId = 2,
//                        Author = "Alice",
//                        Description = "Cotton Candy Adventures",
//                        ISBN = "555555555"
//                    });
//                    products.Add(new Product
//                    {
//                        Id = 5,
//                        Title = "Rock in the Ocean",
//                        CategoryId = 2,
//                        Author = "Bob",
//                        Description = "Oceanic Rocks",
//                        ISBN = "444444444"
//                    });
//                    break;
//                case 3:
//                    products.Add(new Product
//                    {
//                        Id = 6,
//                        Title = "Leaves and Wonders",
//                        CategoryId = 3,
//                        Author = "Wonder Woman",
//                        Description = "Wonders of Nature",
//                        ISBN = "111111111"
//                    });
//                    break;
//                case 4:
//                    products.Add(new Product
//                    {
//                        Id = 7,
//                        Title = "Viraj",
//                        CategoryId = 4,
//                        Author = "Viraj Author",
//                        Description = "Viraj's Book",
//                        ISBN = "999999999"
//                    });
//                    break;
//                default:
//                    break;
//            }

//            return products;
//        }

//    }
//}




using Microsoft.AspNetCore.Mvc;
using Xunit;
using BulkyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using Moq;
using System.Collections.Generic;
using BulkyBookWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Hosting;
using System.Linq.Expressions;

namespace UnitTest_BookStore
{
    public class ProductControllerUnitTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void IndexProducts(int categoryId)
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var products = GetMockProducts(categoryId);

            mockUnitOfWork.Setup(u => u.Product.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string>()))
                .Returns(products);

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var controller = new ProductController(mockUnitOfWork.Object, mockWebHostEnvironment.Object);
            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Product>>(viewResult.ViewData.Model);
            Assert.Equal(products.Count, model.Count);
        }

        private List<Product> GetMockProducts(int categoryId)
        {
            List<Product> products = new List<Product>();

            switch (categoryId)
            {
                case 1:
                    products.Add(new Product { Id = 1, Title = "Fortune of Time", CategoryId = 1 });
                    products.Add(new Product { Id = 2, Title = "Dark Skies", CategoryId = 1 });
                    break;
                case 2:
                    products.Add(new Product { Id = 4, Title = "Cotton Candy", CategoryId = 2 });
                    products.Add(new Product { Id = 5, Title = "Rock in the Ocean", CategoryId = 2 });
                    break;
                case 3:
                    products.Add(new Product { Id = 6, Title = "Leaves and Wonders", CategoryId = 3 });
                    break;
              
                default:
                    break;
            }

            return products;
        }
    }
}

//using BulkyBook.DataAccess.Repository.IRepository;
//using BulkyBook.Models;
//using BulkyBookWeb.Areas.Admin.Controllers;
//using Microsoft.AspNetCore.Hosting;
//using Moq;
//using System.Linq.Expressions;
//using System.Web.Mvc;

//public class ProductControllerIntegrationTest
//{
//    [Theory]
//    [InlineData(1)]
//   // [InlineData(2)]
//    //[InlineData(3)]
//    public void IndexProducts(int categoryId)
//    {
//        var unitOfWorkMock = new Mock<IUnitOfWork>();
//        var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
//        var controller = new ProductController(unitOfWorkMock.Object, hostingEnvironmentMock.Object);

//        var expectedProducts = GetExpectedProducts(categoryId);
//        unitOfWorkMock.Setup(u => u.Product.GetAll(It.IsAny<Expression<Func<Product, bool>>>(), "Category"))
//            .Returns(expectedProducts);

//        var result = controller.Index();

//        var viewResult = Assert.IsType<ViewResult>(result);
//        var model = Assert.IsAssignableFrom<List<Product>>(viewResult.ViewData.Model);
//        Assert.Equal(expectedProducts, model);
//    }

//    private List<Product> GetExpectedProducts(int categoryId)
//    {
//        List<Product> products = new List<Product>();

//        switch (categoryId)
//        {
//            case 1:
//                products.Add(new Product { Id = 1, Title = "Fortune of Time", Description = "<p>It uses a dictionary of over<em> 200 Latin words,</em> combined with a handful of <span style=\"text-decoration: underline;\">model</span> sentence structures, to generate Lorem Ipsum which looks reasonable. The generated<strong> Lorem Ipsum</strong> is therefore always free from repetition, injected humour, or non-characteristic words etc.</p>", ISBN = "SWD9999001", Author = "Billy Spark", ListPrice = 99, Price = 90, Price50 = 85, Price100 = 80, CategoryId = 1 });
//                //  products.Add(new Product { Id = 2, Title = "Dark Skies", CategoryId = 1 });
//                break;
//            //case 2:
//            //    products.Add(new Product { Id = 4, Title = "Cotton Candy", CategoryId = 2 });
//            //    products.Add(new Product { Id = 5, Title = "Rock in the Ocean", CategoryId = 2 });
//            //    break;
//            //case 3:
//            //    products.Add(new Product { Id = 6, Title = "Leaves and Wonders", CategoryId = 3 });
//            //    break;

//            default:
//                break;
//        }
//        return products;
//    }

//}





//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Xunit;
//using BulkyBook.Models;
//using BulkyBook.DataAccess.Repository.IRepository;
//using Moq;
//using System.Collections.Generic;
//using BulkyBookWeb.Areas.Admin.Controllers;
//using Microsoft.AspNetCore.Hosting;
//using System.Linq.Expressions;
//using BulkyBook.DataAccess.Repository;
//using BulkyBook.DataAcess.Data;
//using Microsoft.EntityFrameworkCore;

//namespace Book_Test
//{
//    public class ProductControllerIntegrationTest
//    {
//private readonly ApplicationDbContext _context;
//private readonly IUnitOfWork _unitOfWork;
//private readonly IWebHostEnvironment _webHostEnvironment;

//public ProductControllerIntegrationTest()
//{
//    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//        .UseInMemoryDatabase(databaseName: "Bulky")
//        .Options;

//    _context = new ApplicationDbContext(options);
//    _unitOfWork = new UnitOfWork(_context);
//    _webHostEnvironment = new Mock<IWebHostEnvironment>().Object;

//    SeedTestData();
//}

//private void SeedTestData()
//{
//    var products = Products(1);
//    foreach (var product in products)
//    {
//        _unitOfWork.Product.Add(product);
//    }
//    _unitOfWork.Save();
//}


//public void Dispose()
//{
//    _context.Dispose();
//}
//[Theory]
//[InlineData(1)]
////[InlineData(2)]
////[InlineData(3)]
//public void Index_Products(int categoryId)
//{
//    var controller = new ProductController(_unitOfWork, _webHostEnvironment);

//    var result = controller.Index();

//    var viewResult = Assert.IsType<ViewResult>(result);
//    var model = Assert.IsAssignableFrom<List<Product>>(viewResult.ViewData.Model);
//    Assert.Equal(Products(categoryId).Select(p => p.Title), model.Select(p => p.Title));

//}

//private List<Product> Products(int categoryId)
//{
//    List<Product> products = new List<Product>();

//    switch (categoryId)
//    {
//        case 1:
//            products.Add(new Product { Id = 1, Title = "Fortune of Time",Description= "<p>It uses a dictionary of over<em> 200 Latin words,</em> combined with a handful of <span style=\"text-decoration: underline;\">model</span> sentence structures, to generate Lorem Ipsum which looks reasonable. The generated<strong> Lorem Ipsum</strong> is therefore always free from repetition, injected humour, or non-characteristic words etc.</p>" ,ISBN= "SWD9999001", Author= "Billy Spark",ListPrice=99,Price=90,Price50=85,Price100=80, CategoryId = 1 });
//          //  products.Add(new Product { Id = 2, Title = "Dark Skies", CategoryId = 1 });
//            break;
//        //case 2:
//        //    products.Add(new Product { Id = 4, Title = "Cotton Candy", CategoryId = 2 });
//        //    products.Add(new Product { Id = 5, Title = "Rock in the Ocean", CategoryId = 2 });
//        //    break;
//        //case 3:
//        //    products.Add(new Product { Id = 6, Title = "Leaves and Wonders", CategoryId = 3 });
//        //    break;

//        default:
//            break;
//    }

//    return products;
//}
//    }
//}





//Product p1;
//Product p2;
//List<Product> productList;
//HomeController ctrl;

//Mock<IUnitOfWork> _unitOfWork;

//public HomeControllerUnitTest()
//{
//    p1 = new Product { Title = "FORTUNE OF TIME", Author = "Billy Spark", Price = 90 };
//    p2 = new Product { Title = "DARK SKIES", Description = "Nancy Hoover", Price = 30 };

//    productList = new List<Product>();
//    // productList.Add(p1);
//    // productList.Add(p2);

//    _unitOfWork = new Mock<IUnitOfWork>();
//    ctrl = new HomeController(Mock.Of<ILogger<HomeController>>(), _unitOfWork.Object);
//}

//[Fact]
//public void Index()
//{
//    _unitOfWork.Setup(u => u.Product.GetAll(null, It.IsAny<string>())).Returns(productList);

//    var result = ctrl.Index() as ViewResult;

//    Assert.NotNull(result);
//    Assert.IsType<List<Product>>(result.Model);

//    var model = result.Model as List<Product>;

//    // Assert that the types are equal
//    Assert.Equal(typeof(List<Product>), model.GetType());

//    // Convert the productList to a list for proper comparison
//    Assert.Equal(productList.ToList(), model);
//}
