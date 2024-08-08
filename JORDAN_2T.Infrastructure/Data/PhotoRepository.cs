using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace JORDAN_2T.Infrastructure.Data
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(MvcMovieContext context) : base(context)
        {

        }

        public IEnumerable<Photo> GetMovieImages(int movieId)
        {
            IQueryable<Photo> imageList = _context.Set<Photo>();
            imageList = imageList.Where(p => p.MovieId == movieId);
            return imageList.OrderBy(p => p.Sequence);
        }

        public MvcMovieContext MovieContext
        {
            get { return _context as MvcMovieContext; }
        }
    }
}
