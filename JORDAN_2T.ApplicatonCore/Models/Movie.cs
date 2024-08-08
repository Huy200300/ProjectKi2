using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JORDAN_2T.ApplicationCore.Models;
    public enum MovieStatus
    {
        Draft,
        Active,
        Sold
    }

    public enum MovieSortKey
    {
        New,
        PriceLow,
        PriceHigh
    }
    public enum CategoryStatus
    {
        Draft,
        Active
    }
    public class Movie
    {
        public Movie()
        {
            Photos = new Collection<Photo>();
        }
        
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(1.00, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        
        // public int Quantity {get;set;}

        public decimal? Price { get; set; }

        
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }

        public MovieStatus Status { get; set; }
        public string FormattedPrice
        {
            get { return String.Format("{0:C}", Price); }
        }
        // public SubCategory SubCategory {get;set;}
        public ICollection<Photo> Photos { get; set; }
        
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
    }

public class Category
{
    
    public int Id {get;set;}
    public string? Name {get;set;}
    public CategoryStatus Status { get; set; }
   
}

public class SubCategory
{
    
    public int Id {get;set;}

    public int CategoryId {get;set;}
    
    public string? Name{get;set;}

    public Category Category {get;set;}

    public CategoryStatus Status { get; set; }

}
public class Pager{

    public int TotalItems {get;set;}
    public int CurrentPage {get;set;}
    public int PageSize {get;set;}
    public int TotalPages {get;set;}
    public int StartPage {get;set;}
    public int EndPage {get;set;}
    
    public Pager(){

    }
    public Pager(int totalItems,int page,int pageSize=10){
        int totalPages=(int)Math.Ceiling((decimal)totalItems/(decimal)pageSize);
        int currentPage=page;

        int startPage=currentPage-5;
        int endPage =currentPage+4;

        if(startPage<=0){
            endPage=endPage-(startPage-1);
            startPage=1;
        }

        if(endPage> totalPages){
            endPage=totalPages;
            if (endPage>10)
            {
                startPage=endPage-9;
            }
        }
        TotalItems=totalItems;
        CurrentPage=currentPage;
        PageSize=pageSize;
        TotalPages=totalPages;
        StartPage=startPage;
        EndPage=endPage;
    }
}