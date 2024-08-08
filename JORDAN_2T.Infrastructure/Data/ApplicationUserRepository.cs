using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Data
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private MvcMovieContext _context;

        public ApplicationUserRepository(MvcMovieContext context) : base(context)
        {
            _context = context;
        }
        public ApplicationUser GetMovie(string id)
        {
            var movie = _dbSet.Single(i => i.Id == id);
            // Populate the photo collection. Lazy loading is not
            // turned on so we have to do it explicitly. When you
            // read up on eager, lazy, and explicit loading, make
            // sure you are reading about EF Core, not just EF.
            
            return movie;
        }
         public IEnumerable<ApplicationUser> GetUsers(string SearchString,int pg)
        {
            IEnumerable<ApplicationUser> users;
            var CategoriessList = _dbSet.Where(p => p.Id != null).Where(p=>p.FullName.ToLower().Contains(SearchString));;

            const int pageSize=8;
            if(pg<1)
                pg=1;
                ;

            int recsCount =CategoriessList.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;

            users= CategoriessList.OrderByDescending(p => p.Id).Skip(recSkip).Take(pageSize).ToList();
            
            return users;
        }

    }
}
