using Microsoft.EntityFrameworkCore;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace JORDAN_2T.Infrastructure.Data
{
    /*public enum MovieSortKey
    {
        New,
        PriceLow,
        PriceHigh
    }*/
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        /// <summary>
        /// Populate lists needed for drop downs
        /// </summary>
        /// <param name="context"></param>
        public MovieRepository(MvcMovieContext context) : base(context)
        {
            // Create our item status list for the drop downs.
            MovieStatusList = new Dictionary<MovieStatus, string>
            {
                {MovieStatus.Active, "Active" },
                {MovieStatus.Draft, "Draft" },
                {MovieStatus.Sold, "Sold" }
            };
            MovieSortList = new Dictionary<MovieSortKey, string>
            {
                {MovieSortKey.New, "Newly Added"},
                {MovieSortKey.PriceLow, "Low to High Price"},
                {MovieSortKey.PriceHigh, "High to Low Price" }
            };
        }

        /// <summary>
        /// Create a new item and populate fields with default values.
        /// </summary>
        /// <returns></returns>
        public Movie CreateNewMovie()
        {
            Movie movie = new Movie();
            // Figure out the next movie number. This works provided we
            // don't have too many simultaneous users creating movies.
            if (_dbSet.Count() > 0)
            {
                movie.Number = _dbSet.Max(p => p.Number) + 1;
            }
            else
            {
                movie.Number = 100;
            }
            movie.Status = MovieStatus.Draft;
            movie.Name = "New Movie";
            movie.Price = 0;
            movie.SubCategoryId=1;
            return movie;
        }
        public int GetNumber(int i){
            
            Movie movie =new Movie();
            if (_dbSet.Count() > 0)
            {
                movie.Number = _dbSet.Max(p => p.Number) + 1;
                i=movie.Number;
            }
            else
            {
                movie.Number = 100;
                i=movie.Number;
            }
            return i;
        }
        /// <summary>
        /// This is different from Get() in the base class because
        /// it also obtains the photo collection for the specified
        /// item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Movie GetMovie(int id)
        {
            var movie = _dbSet.Single(i => i.Id == id);
            // Populate the photo collection. Lazy loading is not
            // turned on so we have to do it explicitly. When you
            // read up on eager, lazy, and explicit loading, make
            // sure you are reading about EF Core, not just EF.
            _context.Entry(movie).Collection(p => p.Photos).Load();
            return movie;
        }
        

        /// <summary>
        /// For administration screens. Retrieves movies based on item status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public IEnumerable<Movie> GetAdminMovies(MovieStatus status,string SearchString,int pg)
        {
             IEnumerable<Movie> movies;
            var movieList = _dbSet.Where(p => p.Status == status).Where(p=>p.Name.ToLower().Contains(SearchString));

            const int pageSize=8;
            if(pg<1)
                pg=1;
                ;

            int recsCount =movieList.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            
            movies= movieList.OrderByDescending(p => p.Number).Skip(recSkip).Take(pageSize).ToList();

            return movies;
        }
        
        
        /// <summary>
        /// For user screens. Retrieves active movies with photos and sorts
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
       public IEnumerable<Movie> GetInventoryMovies(MovieSortKey sortOrder,int pg)
        {
            IEnumerable<Movie> sorted;
            var movieList = _dbSet.Where(p => p.Status == MovieStatus.Active).Include(p=>p.Photos);

            
            const int pageSize=8;
            if(pg<1)
                pg=1;
                ;

            int recsCount =movieList.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;

            if (sortOrder == MovieSortKey.PriceLow)
            {
                sorted = movieList.OrderBy(p=>p.Price).Skip(recSkip).Take(pageSize).ToList();  
                  
            }
            else if (sortOrder == MovieSortKey.PriceHigh)
            {
                sorted = movieList.OrderByDescending(p => p.Price).Skip(recSkip).Take(pageSize).ToList();    
            }
            else
            {
                sorted = movieList.OrderByDescending(p => p.Id).Skip(recSkip).Take(pageSize).ToList();
                
            }
            return sorted;
        }
        public IEnumerable<Movie> GetInventoryMovies(MovieSortKey sortOrder)
        {
            IEnumerable<Movie> sorted;
            var movieList = _dbSet.Where(p => p.Status == MovieStatus.Active).Include(p=>p.Photos);

            if (sortOrder == MovieSortKey.PriceLow)
            {
                sorted = movieList.OrderBy(p=>p.Price);  
                  
            }
            else if (sortOrder == MovieSortKey.PriceHigh)
            {
                sorted = movieList.OrderByDescending(p => p.Price);    
            }
            else
            {
                sorted = movieList.OrderByDescending(p => p.Id);
                
            }
            return sorted;
        }
        /// <summary>
        /// For user screens. Retrieves active movies with photos.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public IEnumerable<Movie> GetInventoryMovies()
        {
            IEnumerable<Movie> movies;
            movies = _dbSet.Where(p => p.Status == MovieStatus.Active).Include(p=>p.Photos);
            return movies;
        }
        public IEnumerable<Movie> GetCategory(int Id,int pg)
        {
            IEnumerable<Movie> movies;
            var listmovies = _dbSet.Where(p => p.CategoryId == Id)
            .Where(p=>p.Status== MovieStatus.Active)
            .Include(p=>p.Photos);

            const int pageSize=8;
            if(pg<1)
                pg=1;
                ;

            int recsCount =listmovies.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;

            movies= listmovies.Skip(recSkip).Take(pageSize).ToList();

            return movies;
        }

        public IEnumerable<Movie> GetSubCategory(int Id,int pg)
        {
            IEnumerable<Movie> movies;
            var listmovies = _dbSet.Where(p => p.SubCategoryId == Id)
            .Where(p=>p.Status== MovieStatus.Active)
            .Include(p=>p.Photos);
            
            const int pageSize=8;
            if(pg<1)
                pg=1;
                ;

            int recsCount = listmovies.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;

             movies= listmovies.Skip(recSkip).Take(pageSize).ToList();


            return movies;
        }
        
        public IEnumerable<Movie> Search(string search,int pg)
        {
            IEnumerable<Movie> movies;
            var listmovies = _dbSet.Where(p=>p.Name.ToLower().Contains(search))
            .Where(p=>p.Status== MovieStatus.Active)
            .Include(p=>p.Photos);

            const int pageSize=8;
            if(pg<1)
                pg=1;
                ;

            int recsCount =listmovies.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;

             movies= listmovies.Skip(recSkip).Take(pageSize).ToList();

            return movies;
        }
        /// <summary>
        /// Use to populate dropdown
        /// </summary>
        public Dictionary<MovieStatus, string> MovieStatusList
        {
            get;
            private set;
        }

        /// <summary>
        /// Use to populate dropdown
        /// </summary>
        public Dictionary<MovieSortKey, string> MovieSortList
        {
            get;
            private set;
        }

    }
}
