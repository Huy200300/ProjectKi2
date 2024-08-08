using JORDAN_2T.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;


namespace JORDAN_2T.ApplicationCore.ViewModels;


    public class CartItemDetailsVM
    {
        private ShoppingCart _cart;
        public CartItemDetailsVM(ShoppingCart cart)
        {
            _cart = cart;
            CreateImageList(cart.Movie);
        }

        public string PageTitle { get; set; }

        public int MovieId
        {
            get { return _cart.Movie.Id; }
        }

        public string Name
        {
            get { return _cart.Movie.Name; }
        }

        public string FormattedPrice
        {
            get { return _cart.Movie.FormattedPrice; }
        }

        public string Description
        {
            get { return _cart.Movie.Description; }
        }

        public int Number
        {
            get { return _cart.Movie.Number; }
        }

        public int Quantity
        {
            get { return _cart.Quantity; }
        }

        public IEnumerable<string> MovieImages
        {
            get;
            private set;

        }

        public IEnumerable<string> Thumbnails
        {
            get;
            private set;
        }

        private void CreateImageList(Movie movie)
        {
            List<string> images = new List<string>();
            List<string> thumbs = new List<string>();

            foreach (var image in movie.Photos)
            {
                images.Add(String.Format("{0}", image.LinkToLargeImage));
                thumbs.Add(String.Format("{0}", image.LinkToSmallImage));
            }

            if (images.Count == 0)
            {
                images.Add("~/images/product-sample-thumb.jpg");
                thumbs.Add("~/images/product-sample-thumb.jpg");
            }

            MovieImages = images;
            Thumbnails = thumbs;
        }
        public IEnumerable<Category> category { get; set; }
        public  IEnumerable<SubCategory> subCategory {get;set;}
    }

