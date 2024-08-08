using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JORDAN_2T.ApplicationCore.Models;

namespace JORDAN_2T.ApplicationCore.ViewModels;

    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartList { get; set; }
        public IEnumerable<Category> category { get; set; }
        public  IEnumerable<SubCategory> subCategory {get;set;}
        public  Order Order { get; set; }
    }

