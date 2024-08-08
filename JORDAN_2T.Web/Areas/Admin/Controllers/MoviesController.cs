using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.Infrastructure.Data;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using JORDAN_2T.ApplicationCore.ViewModels;
using JORDAN_2T.ApplicationCore.ViewModels.Admin;
using JORDAN_2T.ApplicationCore.Utilities;
using JORDAN_2T.Web.Controllers;



namespace JORDAN_2T.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for administrative functions. Enable authorization on this controller to restrict who can modify website content.
    /// </summary>
    [Area("Admin")]
    
    
    [Authorize(Roles = WebsiteRole.Managers)]
    public class MoviesController : BaseController
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uow"></param>
        public MoviesController(IUnitOfWork uow) : base(uow)
        {
        }
        #endregion

        /// <summary>
        /// Display our default view. 
        /// </summary>
        /// <param name="MovieStatus"></param>
        /// <returns></returns>
        public ActionResult Index(MovieStatus Status = MovieStatus.Active,string SearchString="",int pg=1)
        {
            AdminMovieListVM vm = new AdminMovieListVM();
            // Let's see how complicated we can make it just to populate a simple drop down.
            // But seriously, you really might want to obtain all your data from a repository for larger projects.
            vm.StatusList = new SelectList(((MovieRepository)_uow.Movies).MovieStatusList, "Key", "Value", Status);
            vm.Status = Status;
            // Now we get all the movies that have the specified status. Note that EF core does not do lazy loading
            // by default. This means we obtain a collection of movies without their photos. You can change this behavior in
            // the repository call to GetAdminMovies by adding .include(p => p.Photos) to the query string. Alternatively, 
            // you could activate ILazyLoader service globally.

            const int pageSize=8;
            if(pg<1)
                pg=1;
                

            var querry =_uow.Movies.GetAll(p=>p.Id!=null);
            int recsCount =querry.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            
            this.ViewBag.pager=pager;
            vm.search=SearchString;
            vm.Movies = ((MovieRepository)_uow.Movies).GetAdminMovies(Status,SearchString,pg);
            
            return View(vm);
        }

        /// <summary>
        /// Display the item we wish to edit
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <param name="isPhoto">If true we make photo tab active</param>
        /// <returns></returns>
        public ActionResult Edit(int id, bool isPhoto = false)
        {
            // Get the movie along with its photo collection.
            Movie movie = ((MovieRepository)_uow.Movies).GetMovie(id);
            if (movie == null)
            {
                movie = new Movie();
            }
            MovieDetailsVM vm = new MovieDetailsVM(movie);
            vm.MovieStatusList = new SelectList(((MovieRepository)_uow.Movies).MovieStatusList, "Key", "Value", movie.Status);

            vm.CategoryList = new SelectList(_uow.Categorys.GetCategories().ToList(), "Id", "Name");
            vm.SubCategoryList = new SelectList(_uow.SubCategorys.GetSubCategories().ToList(), "Id", "Name");
            
            vm.ShowPhoto = isPhoto;

            return View("Details", vm);
        }


        public ActionResult Delete(int id)
        {

            Movie movie = ((MovieRepository)_uow.Movies).GetMovie(id);

            ((MovieRepository)_uow.Movies).Remove(movie);

            _uow.Save();


            return RedirectToAction("Index");

        }

        /// <summary>
        /// Save the changes to the movie and return to the index screen
        /// </summary>
        /// <param name="id">Movie id</param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Movie movie = null;
            MovieStatus initStatus;
            MovieStatus newStatus;
            
            try
            {
                if (ModelState.IsValid)
                {
                    movie = ((MovieRepository)_uow.Movies).GetMovie(id);
                    initStatus = movie.Status;
                    
                    movie.Name = collection["Name"];
                    movie.Description = collection["Description"];
                    movie.Price = decimal.Parse(collection["Price"]);
                    movie.CategoryId=int.Parse(collection["CategoryId"]);
                    movie.SubCategoryId=int.Parse(collection["SubCategoryId"]);
                    if (Enum.TryParse<MovieStatus>(collection["Status"], out newStatus))
                    {
                        movie.Status = newStatus;
                    }

                    _uow.Save();
                    TempData["message"] = string.Format("Movie {0} has been updated", movie.Number);
                    return RedirectToAction("Index", new { Status = initStatus });
                }
            }
            catch (Exception ex)
            {
            }
            return View("Edit", new MovieDetailsVM(movie));
        }

        /// <summary>
        /// Create a new movie and store it to the database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            // Movie movie;
            // try
            // {
            //     movie = _uow.Movies.CreateNewMovie();
            //     movie.Name = "New Product";
            //     movie.Status = MovieStatus.Draft;
                
            //     movie.CategoryId=1;
            //     movie.SubCategoryId=1;
            //     _uow.Movies.Add(movie);
            //     _uow.Save();
            //     TempData["message"] = string.Format("New movie {0} has been added", movie.Number);
            // }
            // catch (Exception ex)
            // {
            //     TempData["message"] = string.Format("Could not add new movie.");
            // }
            // return RedirectToAction(nameof(Index), new { Status = MovieStatus.Draft });
            AdminMovieListVM adminMovieListVM =new AdminMovieListVM();
            Movie movie =new Movie();
           
            movie.Number=((MovieRepository)_uow.Movies).GetNumber(0);
            adminMovieListVM.Number=movie.Number;
            adminMovieListVM.CategoryList = new SelectList(_uow.Categorys.GetCategories().ToList(), "Id", "Name");
            adminMovieListVM.StatusList = new SelectList(((MovieRepository)_uow.Movies).MovieStatusList, "Key", "Value", movie.Status);
            return View(adminMovieListVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( IFormCollection collection)
        {
            Movie movie = new Movie();
            MovieStatus initStatus;
            MovieStatus newStatus;
            
            try
            {
                if (ModelState.IsValid)
                {
                    
                    initStatus = movie.Status;
                    movie.Number=int.Parse(collection["Number"]);
                    movie.Name = collection["Name"];
                    movie.Description = collection["Description"];
                    movie.Price = decimal.Parse(collection["Price"]);
                    movie.CategoryId=int.Parse(collection["CategoryId"]);
                    movie.SubCategoryId=int.Parse(collection["SubCategoryId"]);
                    // if (Enum.TryParse<MovieStatus>(collection["Status"], out newStatus))
                    // {
                    //     movie.Status = newStatus;
                    // }
                    movie.Status=MovieStatus.Draft;
                    _uow.Movies.Add(movie);
                    _uow.Save();
                    TempData["message"] = string.Format("Movie {0} has been updated", movie.Number);
                    return RedirectToAction("Index", new { Status = initStatus });
                }else
                {
                    return View("Create");
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = string.Format("Could not add new movie.");
            }
            return RedirectToAction(nameof(Index), new { Status = MovieStatus.Draft });
        }

      
        public List<SelectListItem> GetSubCategories(int categoryId = 1)
        {
            List<SelectListItem> list = ((SubCategoryRepository)_uow.SubCategorys).GetSubCategories()
                .Where(c => c.CategoryId == categoryId)
                .OrderBy(n => n.Name)
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                }).ToList();
            var item = new SelectListItem()
            {
                Value = "",
                Text = "--- Select Sub Category ---"
            };
            list.Insert(0, item);
            return list;
        }
        [HttpGet]
        public JsonResult GetSubCategoriesByCategory(int categoryId)
        {
            List<SelectListItem> subCategories = GetSubCategories(categoryId);
            return Json(subCategories);
        }

    }

}
