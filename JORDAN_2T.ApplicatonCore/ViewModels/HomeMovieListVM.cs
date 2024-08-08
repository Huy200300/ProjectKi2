using Microsoft.AspNetCore.Mvc.Rendering;

using JORDAN_2T.ApplicationCore.Models;

namespace JORDAN_2T.ApplicationCore.ViewModels;


    public class HomeMovieListVM
    {
        public HomeMovieListVM()
        {
            PageTitle = "Products List";
        }
        /// <summary>
        /// Gets or sets the sorting combo box list.
        /// </summary>
        public IEnumerable<SelectListItem> Sorts { get; set; }
        public IEnumerable<Category> category {get;set;}
        public IEnumerable<SubCategory> subCategory {get;set;}
        public MovieSortKey SortBy { get; set; }

        public IEnumerable<Movie> Movies { get; set; }

        /// <summary>
        /// Gets or sets the page label
        /// </summary>
        public string PageTitle { get; private set; }
        public string Search {get;set;}
    }
