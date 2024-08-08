using Microsoft.AspNetCore.Mvc;
using JORDAN_2T.Infrastructure.Interfaces;

namespace JORDAN_2T.Web.Controllers
{
    // This controller contains the uow class containing our MvcMovieContext. 
    public abstract class BaseController : Controller
    {
        protected readonly IUnitOfWork _uow;
        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }
    }
}
