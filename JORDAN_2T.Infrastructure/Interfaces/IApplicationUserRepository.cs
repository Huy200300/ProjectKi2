using JORDAN_2T.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        ApplicationUser GetMovie(string id);
        IEnumerable<ApplicationUser> GetUsers(string SearchString,int pg);
    }
}
