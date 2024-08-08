using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using JORDAN_2T.ApplicationCore.ViewModels;
using JORDAN_2T.ApplicationCore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.Infrastructure.Data;
using JORDAN_2T.ApplicationCore.ViewModels.Admin;
using JORDAN_2T.Web.Controllers;


namespace JORDAN_2T.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartsController : BaseController
    {
        //private readonly IEmailSender _emailSender;
        [BindProperty]
        public ShoppingCartVM CartVM { get; set; }
        public OrderHistoryVM orderHistoryVM {get;set;}
        public CartsController(IUnitOfWork uow) : base (uow)
        {   
            //_emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            decimal? subTotal = 0;
            decimal? total = 0;

            CartVM = new ShoppingCartVM()
            {
                CartList = _uow.ShoppingCarts.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Movie"),
                category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active),
                subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active),
                Order = new Order()
            };
            foreach(var cart in CartVM.CartList)
            {
                // Get movie detail with photos
                cart.Movie = _uow.Movies.GetMovie(cart.MovieId);
                cart.Price = cart.Movie.Price;
                subTotal = cart.Price * cart.Quantity;
                total = total + subTotal;
            }
            CartVM.Order.OrderTotal = total;
            return View(CartVM);
        }
        

        public IActionResult Checkout()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            

            decimal? subTotal = 0;
            decimal? total = 0;

            CartVM = new ShoppingCartVM()
            {
                CartList = _uow.ShoppingCarts.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Movie"),
                category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active),
                subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active),
                Order = new Order()
            };
            CartVM.Order.ApplicationUser = _uow.ApplicationUsers.GetFirstOrDefault(u => u.Id == claim.Value);

            CartVM.Order.Name = CartVM.Order.ApplicationUser.FullName;
            CartVM.Order.PhoneNumber = CartVM.Order.ApplicationUser.PhoneNumber;
    
            foreach (var cart in CartVM.CartList)
            {
                cart.Price = cart.Movie.Price;
                subTotal = cart.Price * cart.Quantity;
                total = total + subTotal;
            }

            CartVM.Order.OrderTotal = total;
           
            return View(CartVM);
        }

        [HttpPost]
        [ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public IActionResult CheckoutPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            decimal? subTotal = 0;
            decimal? total = 0;

            CartVM.CartList = _uow.ShoppingCarts.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Movie");

            CartVM.Order.OrderDate = System.DateTime.Now;
            CartVM.Order.ApplicationUserId = claim.Value;


            foreach (var cart in CartVM.CartList)
            {
                cart.Price = cart.Movie.Price;
                subTotal = cart.Price * cart.Quantity;
                total = total + subTotal;
            }

            CartVM.Order.OrderTotal = total;
            
            ApplicationUser applicationUser = _uow.ApplicationUsers.GetFirstOrDefault(u => u.Id == claim.Value);

            CartVM.Order.OrderStatus = OrderStatus.Pending;

            _uow.Orders.Add(CartVM.Order);

            _uow.Save();

            foreach (var cart in CartVM.CartList)
            {
                OrderDetail orderDetail = new()
                {
                    MovieId = cart.MovieId,
                    OrderId = CartVM.Order.Id,
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };
                _uow.OrderDetails.Add(orderDetail);
                _uow.Save();
            }

            return RedirectToAction("OrderConfirmation", "Carts", new { id = CartVM.Order.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            Order order = _uow.Orders.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");

            _uow.Orders.UpdateStatus(id, OrderStatus.Approved, PaymentStatus.Approved);
            _uow.Save();

            //_emailSender.SendEmailAsync(order.ApplicationUser.Email, "New Order - Online Shop", "<p>New Order Created</p>");
            List<ShoppingCart> shoppingCarts = _uow.ShoppingCarts.GetAll(u => u.ApplicationUserId == order.ApplicationUserId).ToList();
            _uow.ShoppingCarts.RemoveRange(shoppingCarts);
            _uow.Save();

            var quantity = _uow.ShoppingCarts.GetAll(u => u.ApplicationUserId == order.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32("CartSession", quantity);
            ShoppingCartVM shoppingCartVM =new ShoppingCartVM{
                Order=new Order{
                    Id=id
                },
                category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active),
                subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active),
            };
            
            return View(shoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            ShoppingCart cart = _uow.ShoppingCarts.GetFirstOrDefault(u => u.Id == cartId);
            _uow.ShoppingCarts.IncrementQuantity(cart, 1);
            _uow.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            ShoppingCart cart = _uow.ShoppingCarts.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Quantity <= 1)
            {
                _uow.ShoppingCarts.Remove(cart);
                var quantity = _uow.ShoppingCarts.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count - 1;
                HttpContext.Session.SetInt32("CartSession", quantity);
            }
            else
            {
                _uow.ShoppingCarts.DecrementQuantity(cart, 1);
            }
            _uow.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _uow.ShoppingCarts.GetFirstOrDefault(u => u.Id == cartId);
            _uow.ShoppingCarts.Remove(cart);
            _uow.Save();
            var quantity = _uow.ShoppingCarts.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32("CartSession", quantity);
            return RedirectToAction(nameof(Index));
        }
    }
}
