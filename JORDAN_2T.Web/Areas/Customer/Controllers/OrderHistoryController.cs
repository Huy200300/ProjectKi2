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
    public class OrderHistoryController : BaseController
    {
        [BindProperty]
        public ShoppingCartVM CartVM { get; set; }
        public OrderHistoryVM orderHistoryVM {get;set;}
        public OrderHistoryController(IUnitOfWork uow) : base (uow)
        {   
            //_emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
  
             orderHistoryVM = new OrderHistoryVM()
            {
                Order = _uow.Orders.GetAll(u => u.ApplicationUserId == claim.Value),
                category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active),
                subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active),
            };
            
           
            return View("Index",orderHistoryVM);
        }
        public IActionResult OrderDetails(int id){

            orderHistoryVM= new OrderHistoryVM{ 
                Order = _uow.Orders.GetAll(u => u.Id == id),
                OrderDetails=_uow.OrderDetails.GetAll(p=>p.OrderId==id),
                category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active),
                subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active),
            };

            foreach(var item in orderHistoryVM.OrderDetails){
                 item.Movie=_uow.Movies.GetMovie(item.MovieId);

             }
             
             return View("OrderDetails",orderHistoryVM);
        }
    }
}