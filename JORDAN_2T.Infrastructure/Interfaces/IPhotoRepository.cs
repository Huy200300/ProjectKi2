using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;

namespace JORDAN_2T.Infrastructure.Interfaces
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        IEnumerable<Photo> GetMovieImages(int movieID);
    }
}
