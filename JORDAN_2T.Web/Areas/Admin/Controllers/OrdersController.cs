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


namespace JORDAN_2T.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for administrative functions. Enable authorization on this controller to restrict who can modify website content.
    /// </summary>
    [Area("Admin")]
    
    [Authorize(Roles = WebsiteRole.Managers)]
    public class OrdersController : BaseController
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uow"></param>
        public OrderHistoryVM orderHistoryVM {get;set;}
        public OrdersController(IUnitOfWork uow) : base(uow)
        {
        }
        #endregion
        public IActionResult Index(OrderStatus Status = OrderStatus.Approved,int pg =1)
        {

            const int pageSize=8;
            if(pg<1)
                pg=1;
                

            var querry =_uow.Orders.GetAll(p=>p.Id!=null);
            int recsCount =querry.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            
            this.ViewBag.pager=pager;
            
            orderHistoryVM = new OrderHistoryVM()
            {
                Order = ((OrderRepository)_uow.Orders).GetOrders(Status,pg),
                StatusList=new SelectList(((OrderRepository)_uow.Orders).OrdersStatusList, "Key", "Value", Status),
            };
            
         
            return View(orderHistoryVM);
        }
        public ActionResult Edit(int id){
            
            Order order = ((OrderRepository)_uow.Orders).GetOrder(id);
            if (order == null)
            {
                order = new Order();
            }
            OrdersDetailsVM vm = new OrdersDetailsVM(order);

            vm.Order=((OrderRepository)_uow.Orders).GetAll(p=>p.Id==id);
            vm.OrderDetails=((OrderDetailRepository)_uow.OrderDetails).GetAll(p=>p.OrderId==id);
            vm.OrderStatusList=new SelectList(((OrderRepository)_uow.Orders).OrdersStatusList, "Key", "Value", order.OrderStatus);
            vm.PaymentStatusList=new SelectList(((OrderRepository)_uow.Orders).PaymentStatusList, "Key", "Value", order.PaymentStatus);
            
           foreach (var item in vm.OrderDetails)
           {
                item.Movie=_uow.Movies.GetMovie(item.MovieId);
           }
       
          
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Order order = null;
            OrderStatus initStatus;
            OrderStatus newStatus;
            PaymentStatus paymentNewStatus;
            try
            {
                if (ModelState.IsValid)
                {
                    order = ((OrderRepository)_uow.Orders).GetOrder(id);
                    initStatus = order.OrderStatus;
                   
                    if (Enum.TryParse<OrderStatus>(collection["OrderStatus"], out newStatus))
                    {
                        order.OrderStatus = newStatus;
                    }
                    if (Enum.TryParse<PaymentStatus>(collection["PaymentStatus"], out paymentNewStatus))
                    {
                        order.PaymentStatus = paymentNewStatus;
                    }
                    _uow.Save();
                    TempData["message"] = string.Format("Movie {0} has been updated", order.Id);
                    return RedirectToAction("Index",new { Status = initStatus });
                }
            }
            catch (Exception ex)
            {
            }
            return View("Edit", new OrdersDetailsVM(order));
        }

        
    }
    
}
