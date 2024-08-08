using JORDAN_2T.ApplicationCore.Models;

namespace JORDAN_2T.Infrastructure.Interfaces
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Movie>> GetMovies(string sterm = "");
    }
}