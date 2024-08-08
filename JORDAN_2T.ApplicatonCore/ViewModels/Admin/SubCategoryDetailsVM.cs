using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;

namespace JORDAN_2T.ApplicationCore.ViewModels.Admin;

    public class SubCategoryDetailsVm
    {
        private SubCategory _subcategory;

        public SubCategoryDetailsVm(SubCategory subCategory)
        {
            _subcategory = subCategory;
           
        }
        public IEnumerable<SelectListItem> SubCategoryStatusList { get; set; }
        public IEnumerable<SelectListItem> ListCategory { get; set; }
        public IEnumerable<Category> categories{get;set;}
        public int Id
        {
            get
            {
                return (int)_subcategory.Id;
            }
        }
        public int CategoryId
        {

            get {return (int)_subcategory.CategoryId;}
            set {_subcategory.CategoryId=value;}
        }

        public string Name
        {
            get { return _subcategory.Name; }
            set { _subcategory.Name = value; }
        }
         public CategoryStatus Status 
        { 
            get { return _subcategory.Status; } 
            set { _subcategory.Status = value; } 
        }
    }
