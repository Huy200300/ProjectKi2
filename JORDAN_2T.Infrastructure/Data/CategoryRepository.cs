using Microsoft.EntityFrameworkCore;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace JORDAN_2T.Infrastructure.Data;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private MvcMovieContext _context;
    public CategoryRepository(MvcMovieContext context) : base(context)
    {
        _context=context;
        CategoryStatusList = new Dictionary<CategoryStatus, string>
        {
            {CategoryStatus.Active, "Active" },
            {CategoryStatus.Draft, "Draft" },
        };
    }

    public Category CreateNewCategory()
    {
        Category category = new Category();
        
        category.Name= "New Category";

        return category;
    }

    public Category GetCategory(int id)
    {
        var category = _dbSet.Single(i => i.Id == id);
        
        return category;
    }
    
     public IEnumerable<Category> GetCategories(CategoryStatus status,string SearchString,int pg)
    {
        IEnumerable<Category> categories;
        var CategoriessList = _dbSet.Where(p=>p.Status==status).Where(p => p.Id != null).Where(p=>p.Name.ToLower().Contains(SearchString));;

        const int pageSize=8;
        if(pg<1)
            pg=1;
            ;

        int recsCount =CategoriessList.Count();

        var pager=new Pager(recsCount,pg,pageSize);

        int recSkip=(pg-1)*pageSize;

        categories= CategoriessList.OrderByDescending(p => p.Id).Skip(recSkip).Take(pageSize).ToList();
        
        return categories;
    }
    public IEnumerable<Category> GetCategories(){
         var category = _dbSet.Where(p=>p.Status==CategoryStatus.Active);
        return category;
    }
    public Dictionary<CategoryStatus, string> CategoryStatusList
    {
        get;
        private set;
    }
}