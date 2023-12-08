using Bulkey.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {

            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int page = 1, int pageSize = 2)
        {
            // Calculate the number of items to skip
            int skip = (page - 1) * pageSize;

           // Retrieve a paginated subset of products
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages")
                                                                 .Skip(skip)
                                                                 .Take(pageSize);
            // IEnumerable<Product> productList = _unitOfWork.Product.GetAll();

            //// Pass the paginated product list and pagination details to the view
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = _unitOfWork.Product.GetAll().Count(); // Total count for pagination

            return View(productList);

        }

        // [Route("/Customer/Home/Task")]

        public IActionResult Task(string q)
        {
            List<Country> accounts = new List<Country>();
            // Add parts to the list.
            accounts.Add(new Country() { Id = 1, Name = "MVP1" });
            accounts.Add(new Country() { Id = 2, Name = "MVP11" });
            accounts.Add(new Country() { Id = 3, Name = "ABC2" });
            accounts.Add(new Country() { Id = 4, Name = "ABC3" });
            accounts.Add(new Country() { Id = 5, Name = "XYZ3" });
            accounts.Add(new Country() { Id = 6, Name = "XYZ4" });


            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                accounts = accounts.Where(x => x.Name.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(new { items = accounts });


        }
   
        public IActionResult Country()
        {
            List<Country> item = _unitOfWork.Country.GetAll().ToList();


            return View(item);


        }

        [HttpPost]
        public IActionResult Country(Country countryObj, string[] languages)
        {
           

                string selectedLanguages = string.Join(", ",languages);
                countryObj.Languages = selectedLanguages;

           
           

            _unitOfWork.Country.Update(countryObj);
              
                _unitOfWork.Save();

                TempData["Message"] = "Submit successfully";


                return RedirectToAction("Country");

         
            //List<Country> item = _unitOfWork.Country.GetAll().ToList();


            //return View(item);
        }

            //[HttpPost]
            //public IActionResult Country(Country country)
            //{
            //    return View();
            //}

            public IActionResult NewCountry()
            {
            var model = new Country()
            {
                Name = "Viraj"
            };
           
                  return View(model);
            }

        [HttpPost]
        public IActionResult NewCountry(Country model)
        {



            var name = model.Name;
            TempData["Message"] = "Submit successfully";
            return RedirectToAction("NewCountry");


        }



        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,ProductImages"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId=userId;

            ShoppingCart cartFromDb= _unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId==userId && u.ProductId==shoppingCart.ProductId);

            if(cartFromDb!=null)
            {
                //shoping cart exist
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();

            }
            else
            {
                //add a cart record
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();

                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetItem(string q)
        {
            List<Country> item = _unitOfWork.Country.GetAll().ToList();
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                item = item.Where(x => x.Name.ToLower().StartsWith(q.ToLower())).ToList();
            }
            // return Json(new { items = item });
            return Json(new { data = item });
        }


        public IActionResult GetAll(string q)
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages").ToList();
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
                   {
                      productList = productList.Where(x => x.Title.ToLower().StartsWith(q.ToLower())).ToList();
                    }

                return Json(new { data = productList });
        }
        #endregion



    }
}