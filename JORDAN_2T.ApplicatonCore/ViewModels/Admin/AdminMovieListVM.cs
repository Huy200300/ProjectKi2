using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;

namespace JORDAN_2T.ApplicationCore.ViewModels.Admin;

    public class AdminMovieListVM
    {
        public AdminMovieListVM()
        {
            Status = MovieStatus.Active;
        }
        /// <summary>
        /// Gets or sets the sorting combo box list.
        /// </summary>

        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> SubCategoryList { get; set; }
        /// <summary>
        /// Gets or sets our list of items to display
        /// </summary>
        public IEnumerable<Movie> Movies { get; set; }
        
        /// <summary>
        /// The current display - archived or active listings
        /// </summary>
        public MovieStatus Status { get; set; }

        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        
        // public int Quantity {get;set;}

        public decimal? Price { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string search {get;set;}
    }
