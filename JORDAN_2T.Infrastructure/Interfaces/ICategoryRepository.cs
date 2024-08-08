using JORDAN_2T.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category CreateNewCategory();
        Category GetCategory(int id);
        IEnumerable<Category> GetCategories();
        IEnumerable<Category> GetCategories(CategoryStatus status,string SearchString,int pg);
    }
}
