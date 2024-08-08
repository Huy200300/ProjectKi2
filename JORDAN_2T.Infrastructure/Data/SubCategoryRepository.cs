using Microsoft.EntityFrameworkCore;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace JORDAN_2T.Infrastructure.Data;

public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
{
    private MvcMovieContext _context;
    public SubCategoryRepository(MvcMovieContext context) : base(context)
    {
        _context=context;
        SubCategoryStatusList = new Dictionary<CategoryStatus, string>
        {
            {CategoryStatus.Active, "Active" },
            {CategoryStatus.Draft, "Draft" },
        };
    }
    public SubCategory CreateNewSubCategory()
    {
        SubCategory Subcategory = new SubCategory();
        
        Subcategory.CategoryId = 0;

        Subcategory.Name= "New SubCategory";

        return Subcategory;
    }
    public SubCategory GetSubCategory(int id)
    {
        var Subcategory = _dbSet.Single(i => i.Id == id);
        
        return Subcategory;
    }

    public IEnumerable<SubCategory> GetSubCategories(CategoryStatus status,string SearchString,int pg)
    {
            IEnumerable<SubCategory> subCategories;
        var subCategoriessList = _dbSet.Where(p=>p.Status==status).Where(p => p.Id != null).Where(p=>p.Name.ToLower().Contains(SearchString));;

        const int pageSize=8;
        if(pg<1)
            pg=1;
            ;

        int recsCount =subCategoriessList.Count();

        var pager=new Pager(recsCount,pg,pageSize);

        int recSkip=(pg-1)*pageSize;
        subCategories= subCategoriessList.OrderByDescending(p => p.Id).Skip(recSkip).Take(pageSize).ToList();
        return subCategories;
    }
    public IEnumerable<SubCategory> GetSubCategories(){
         var subcategory = _dbSet.Where(p=>p.Status==CategoryStatus.Active);
        return subcategory;
    }
    public Dictionary<CategoryStatus, string> SubCategoryStatusList
    {
        get;
        private set;
    }
}