using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using JORDAN_2T.Infrastructure.Data;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using JORDAN_2T.ApplicationCore.ViewModels;
using JORDAN_2T.Web.Controllers;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace JORDAN_2T.Web.Areas.Customer.Controllers
{
    /// <summary>
    /// User can browse items in the database but not make changes.
    /// Admin controller allows database updates.
    /// </summary>
    [Area("Customer")]
    public class HomeController : BaseController
    {
        public HomeController(IUnitOfWork uow) : base(uow)
        {
        }

        /// <summary>
        /// Tell us about the website
        /// </summary>
        /// <returns></returns>
        public IActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// List all active movies using the designated sort order
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public IActionResult List(MovieSortKey sortBy = MovieSortKey.New)
        {
            HomeMovieListVM vm = new HomeMovieListVM();
           
            vm.Sorts = new SelectList(((MovieRepository)_uow.Movies).MovieSortList, "Key", "Value", sortBy);
            vm.SortBy = sortBy;
           
            vm.Movies = ((MovieRepository)_uow.Movies).GetInventoryMovies(sortBy);

            vm.category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active);
            vm.subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active);

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Home page
        /// </summary>
        /// <returns></returns>
        /*public async Task<IActionResult> Index(string sterm = "")
        {
            //IEnumerable<Movie> movies = await _homeRepository.GetBooks(sterm);
            IEnumerable<Movie> movies = _uow.Movies.GetInventoryMovies();
            MovieDisplayModel movieModel = new MovieDisplayModel
            {
                Movies = movies,
                Sterm = sterm
            };
            return View(movieModel);
        }*/

        public IActionResult Index(int pg=1 , MovieSortKey sortBy = MovieSortKey.New)
        {
            
            HomeMovieListVM vm = new HomeMovieListVM();
            // Let's see how complicated we can make it just to populate a simple drop down.
            // But seriously, you really might want to obtain all your data from a repository for larger projects.
            vm.Sorts = new SelectList(((MovieRepository)_uow.Movies).MovieSortList, "Key", "Value", sortBy);
            vm.SortBy = sortBy;
            // Retrieve all the active items along with their photos. Due to lazy loading we have to explicitly
            // ask for the photos in the method GetInventoryMovies
            vm.Movies = ((MovieRepository)_uow.Movies).GetInventoryMovies(sortBy,pg);
            
           
            const int pageSize=8;
            if(pg<1)
                pg=1;
                

            var querry =_uow.Movies.GetAll(p=>p.Id!=null);
            int recsCount =querry.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            
            this.ViewBag.pager=pager;
            // var data =querry.Skip(recSkip).Take(pageSize).ToList();
            // vm.Movies=querry.Skip(recSkip).Take(pageSize).ToList();

            vm.category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active);
            vm.subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active);

        

            return View("Index",vm);
        }

        public IActionResult Category(int Id,int pg=1 )
        {
            MovieSortKey sortBy = MovieSortKey.New;
            HomeMovieListVM vm = new HomeMovieListVM();
            
            vm.Sorts = new SelectList(((MovieRepository)_uow.Movies).MovieSortList, "Key", "Value", sortBy);
            vm.SortBy = sortBy;


             const int pageSize=8;
            if(pg<1)
                pg=1;
                

            var querry =_uow.Movies.GetAll(p=>p.Id!=null);
            int recsCount =querry.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            
            this.ViewBag.pager=pager;

            vm.Movies = ((MovieRepository)_uow.Movies).GetCategory(Id,pg);


             vm.category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active);
            vm.subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active);
            
            return View("Category",vm);
        }

        public IActionResult SubCategory(int Id,int pg=1)
        {
            MovieSortKey sortBy = MovieSortKey.New;
            HomeMovieListVM vm = new HomeMovieListVM();
           
            vm.Sorts = new SelectList(((MovieRepository)_uow.Movies).MovieSortList, "Key", "Value", sortBy);
            vm.SortBy = sortBy;
           
             const int pageSize=8;
            if(pg<1)
                pg=1;
                

            var querry =_uow.Movies.GetAll(p=>p.Id!=null);
            int recsCount =querry.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            
            this.ViewBag.pager=pager;
            vm.Movies = ((MovieRepository)_uow.Movies).GetSubCategory(Id,pg);
            

             vm.category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active);
            vm.subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active);
            return View("SubCategory",vm);
        }


        /// <summary>
        /// Display a specific movie record.
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public IActionResult Details(int movieId,int quantity)
        {
            ShoppingCart cartObj = new()
            {
                Quantity = quantity,
                MovieId = movieId,
                // Movie = _uow.Movies.GetFirstOrDefault(u => u.Id == movieId, includeProperties: "Category"),
                //Movie = _uow.Movies.GetFirstOrDefault(u => u.ID == movieId),
                Movie = _uow.Movies.GetMovie(movieId),
            };

            CartItemDetailsVM cartItemDetails = new CartItemDetailsVM(cartObj);
            cartItemDetails.category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active);
            cartItemDetails.subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active);
            return View(cartItemDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _uow.ShoppingCarts.GetFirstOrDefault(u => u.MovieId == shoppingCart.MovieId);

            if (cartFromDb == null)
            {
                
                _uow.ShoppingCarts.Add(shoppingCart);
                _uow.Save();
                HttpContext.Session.SetInt32("CartSession", _uow.ShoppingCarts.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
            }
            else
            {
                _uow.ShoppingCarts.IncrementQuantity(cartFromDb, shoppingCart.Quantity);
                _uow.Save();
            }

            return RedirectToAction(nameof(Index));
        }
       
        public IActionResult Search(string search,int pg=1){

            HomeMovieListVM vm=new HomeMovieListVM();
             const int pageSize=8;
            if(pg<1)
                pg=1;
                

            var querry =_uow.Movies.GetAll(p=>p.Id!=null);
            int recsCount =querry.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            
            this.ViewBag.pager=pager;
            vm.Search=search;

            vm.Movies=((MovieRepository)_uow.Movies).Search(search,pg);

            vm.category=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Status==CategoryStatus.Active);
            vm.subCategory=((SubCategoryRepository)_uow.SubCategorys).GetAll(p=>p.Status==CategoryStatus.Active);
            
            return View("Search",vm);
        }
       
    }
}
