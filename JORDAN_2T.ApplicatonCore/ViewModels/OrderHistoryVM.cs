using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JORDAN_2T.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JORDAN_2T.ApplicationCore.ViewModels;

    public class OrderHistoryVM
    {
        
        public IEnumerable<OrderDetail> OrderDetails {get;set;}
        public IEnumerable<Category> category { get; set; }
        public  IEnumerable<SubCategory> subCategory {get;set;}
        public IEnumerable<Order> Order { get; set; }
          public IEnumerable<SelectListItem> StatusList { get; set; }
        public OrderStatus Status { get; set; }
    }

