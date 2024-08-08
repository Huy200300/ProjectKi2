using JORDAN_2T.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Interfaces
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        SubCategory CreateNewSubCategory();
        SubCategory GetSubCategory(int id);
        IEnumerable<SubCategory> GetSubCategories();
        IEnumerable<SubCategory> GetSubCategories(CategoryStatus status,string SearchString,int pg);
    }
}
