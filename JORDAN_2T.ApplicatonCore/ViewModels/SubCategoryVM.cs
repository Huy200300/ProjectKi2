using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;

namespace JORDAN_2T.ApplicationCore.ViewModels;

public class SubCategoryVM
{
    public int Id {get;set;}

    public int CategoryId {get;set;}
    
    public string? Name{get;set;}
    public string? search{get;set;}
    public IEnumerable<SelectListItem> ListCategory { get; set; }
    public  IEnumerable<SubCategory> subcategory {get;set;}
    public IEnumerable<Category> categories {get;set;}
    public IEnumerable<SelectListItem> Statuslist{get;set;}
    public CategoryStatus Status { get; set; }
}