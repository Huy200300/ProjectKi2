using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.Infrastructure.Data;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using JORDAN_2T.ApplicationCore.ViewModels;
using JORDAN_2T.ApplicationCore.ViewModels.Admin;
using JORDAN_2T.ApplicationCore.Utilities;
using JORDAN_2T.Web.Controllers;


namespace JORDAN_2T.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    
    [Authorize(Roles = WebsiteRole.Managers)]
    public class SubCategoryController : BaseController
    {
        public SubCategoryController(IUnitOfWork uow) : base(uow)
        {

        }
        public IActionResult Index(CategoryStatus Status = CategoryStatus.Active,string SearchString="",int pg=1)
        {
             const int pageSize=8;
            if(pg<1)
                pg=1;
                

            var querry =_uow.Orders.GetAll(p=>p.Id!=null);
            int recsCount =querry.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            
            this.ViewBag.pager=pager;

            SubCategoryVM vM= new SubCategoryVM();
            
            vM.Statuslist = new SelectList(((SubCategoryRepository)_uow.SubCategorys).SubCategoryStatusList, "Key", "Value", Status);
            vM.Status = Status;

            vM.search=SearchString;

            vM.subcategory=((SubCategoryRepository)_uow.SubCategorys).GetSubCategories(Status,SearchString,pg);
            
           return View(vM);
        }

        public ActionResult Create()
        {
            SubCategoryVM subCategoryVM =new SubCategoryVM();
            
            subCategoryVM.ListCategory=new SelectList(_uow.Categorys.GetCategories().ToList(),"Id","Name");
             
            return View(subCategoryVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( IFormCollection collection)
        {
            SubCategory Subcategory = new SubCategory();
            
            try
            {
                if (ModelState.IsValid)
                {
                    
                    Subcategory.Name = collection["Name"];
                    Subcategory.Status=CategoryStatus.Draft;
                    Subcategory.CategoryId=int.Parse(collection["CategoryId"]);
                    _uow.SubCategorys.Add(Subcategory);
                    _uow.Save();
                   
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
            }
            return View("Index");
        }

        public ActionResult Edit(int id, bool isPhoto = false)
        {

            SubCategory Subcategory = ((SubCategoryRepository)_uow.SubCategorys).GetSubCategory(id);
            if (Subcategory == null)
            {
                Subcategory = new SubCategory();
            }
            SubCategoryDetailsVm vm = new SubCategoryDetailsVm(Subcategory);

            vm.ListCategory=new SelectList(_uow.Categorys.GetCategories().ToList(),"Id","Name");
            vm.SubCategoryStatusList = new SelectList(((SubCategoryRepository)_uow.SubCategorys).SubCategoryStatusList, "Key", "Value", Subcategory.Status);

            // vm.categories=((CategoryRepository)_uow.Categorys).GetAll(p=>p.Id!=null);

            return View("Details", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            SubCategory Subcategory = new SubCategory();
             CategoryStatus initStatus;
            CategoryStatus newStatus;
            try
            {
                if (ModelState.IsValid)
                {
                    Subcategory = ((SubCategoryRepository)_uow.SubCategorys).GetSubCategory(id);
                    initStatus = Subcategory.Status;

                    Subcategory.Name = collection["Name"];
                    Subcategory.CategoryId=int.Parse(collection["CategoryId"]);
                    if (Enum.TryParse<CategoryStatus>(collection["Status"], out newStatus))
                    {
                        Subcategory.Status = newStatus;
                    }
                    _uow.Save();
                   
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
            }
            return View("Edit",new SubCategoryDetailsVm(Subcategory) );
        }
         public ActionResult Delete(int id){
            
            SubCategory subcategory=((SubCategoryRepository)_uow.SubCategorys).GetSubCategory(id);
            
                ((SubCategoryRepository)_uow.SubCategorys).Remove(subcategory);
            
                _uow.Save();
                
            
            return RedirectToAction("Index");
            
        }
    }
    

}
