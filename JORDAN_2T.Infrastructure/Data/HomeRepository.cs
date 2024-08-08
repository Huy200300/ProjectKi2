using Microsoft.EntityFrameworkCore;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace JORDAN_2T.Infrastructure.Data
{
    public class HomeRepository : IHomeRepository
    {
        private readonly MvcMovieContext _db;

        public HomeRepository(MvcMovieContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Movie>> GetMovies(string sterm = "")
        {
            sterm = sterm.ToLower();
            IEnumerable<Movie> movies = await (from movie in _db.Movies
                where string.IsNullOrWhiteSpace(sterm) || (movie != null && movie.Name.ToLower().StartsWith(sterm))
                select new Movie
                {
                    Id = movie.Id,
                    Number = movie.Number,
                    Name = movie.Name,
                    Price = movie.Price,
                }
                ).ToListAsync();
            return movies;

        }
    }
}
