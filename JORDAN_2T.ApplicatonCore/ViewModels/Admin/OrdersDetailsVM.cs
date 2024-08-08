using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;

namespace JORDAN_2T.ApplicationCore.ViewModels.Admin;

    public class OrdersDetailsVM
    {
          private Order _order;
        public OrdersDetailsVM(Order order)
        {
             _order = order;
        }
       public int Id
        {
            get
            {
                return (int)_order.Id;
            }
        }
        public string Name 
        { 
            get
            {
                return _order.Name;
            }
            set{
                _order.Name=value;
            } 
        }
        
         public string PhoneNumber 
        { 
            get
            {
                return _order.PhoneNumber;
            }
            set{
                _order.PhoneNumber=value;
            } 
        }
         public string Address 
        { 
            get
            {
                return _order.Address;
            }
            set{
                _order.Address=value;
            } 
        }
         public string City 
        { 
            get
            {
                return _order.City;
            }
            set{
                _order.City=value;
            } 
        }
         public decimal OrderTotal 
        { 
            get
            {
                return (decimal)_order.OrderTotal;
            }
            set{
                _order.OrderTotal=value;
            } 
        }
         public DateTime OrderDate 
        { 
            get
            {
                return _order.OrderDate;
            }
            set{
                _order.OrderDate=value;
            } 
        }
         public DateTime PaymentDate 
        { 
            get
            {
                return _order.PaymentDate;
            }
            set{
                _order.PaymentDate=value;
            } 
        }
         public DateTime ShippingDate 
        { 
            get
            {
                return _order.ShippingDate;
            }
            set{
                _order.ShippingDate=value;
            } 
        }
         public OrderStatus OrderStatus 
        { 
            get { return _order.OrderStatus; } 
            set { _order.OrderStatus = value; } 
        }
         public PaymentStatus PaymentStatus 
        { 
            get { return _order.PaymentStatus; } 
            set { _order.PaymentStatus = value; } 
        }
        public IEnumerable<SelectListItem> OrderStatusList{get;set;}
        public IEnumerable<SelectListItem> PaymentStatusList { get; set; }
        public IEnumerable<Order> Order{get;set;}
        public IEnumerable<OrderDetail> OrderDetails {get;set;}
       
    }
