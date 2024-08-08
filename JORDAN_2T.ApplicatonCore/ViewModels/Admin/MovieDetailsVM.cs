using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;

namespace JORDAN_2T.ApplicationCore.ViewModels.Admin;

    public class MovieDetailsVM
    {
        private Movie _movie;

        public MovieDetailsVM(Movie movie)
        {
            _movie = movie;
            OriginalStatus = _movie.Status;
            ShowPhoto = false;
            foreach (var image in _movie.Photos)
            {
                continue;
            }
        }

        public IEnumerable<SelectListItem> MovieStatusList { get; set; }

        public IEnumerable<SelectListItem> MovieSortKeyList {get;set;}

        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public IEnumerable<SelectListItem> SubCategoryList { get; set; }


        public MovieStatus OriginalStatus { get; set; }

        // Indicates if the photo tab should be active
        public bool ShowPhoto { get; set; }

        public int MovieId
        {
            get
            {
                return (int)_movie.Id;
            }
        }

       
        public int Number 
        { 
            get { return _movie.Number; } 
            set { _movie.Number = value; } 
        }

        public string Name
        {
            get { return _movie.Name; }
            set { _movie.Name = value; }
        }

        public decimal Price
        {
            get { return _movie.Price ?? (decimal)0.00; }
            set { _movie.Price = value; }
        }

        public string Description 
        {
            get { return _movie.Description; } 
            set { _movie.Description = value; }
        }
        public int CategoryId{
            get{return _movie.CategoryId;}
            set{_movie.CategoryId=value;}
        }
        public int SubCategoryId{
            get{return _movie.SubCategoryId;}
            set {_movie.SubCategoryId =value;}
        }
        public MovieStatus Status 
        { 
            get { return _movie.Status; } 
            set { _movie.Status = value; } 
        }

        public IEnumerable<Photo> movieImages
        {
            get { return _movie.Photos; }
        }
    }
