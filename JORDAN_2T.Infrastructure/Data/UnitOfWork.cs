using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.Infrastructure.Data;
using JORDAN_2T.ApplicationCore.Models;

namespace JORDAN_2T.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MvcMovieContext _context;
        public UnitOfWork(MvcMovieContext context)
        {
            _context = context;
            Home = new HomeRepository(_context);
            ApplicationUsers = new ApplicationUserRepository(_context);
            Movies = new MovieRepository(_context);
            MoviePhotos = new PhotoRepository(_context);
            Categorys =new CategoryRepository(_context);
            SubCategorys =new SubCategoryRepository(_context);
            ShoppingCarts = new ShoppingCartRepository(_context);
            Orders = new OrderRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
        }

        public IHomeRepository Home { get; private set; }

        public IApplicationUserRepository ApplicationUsers { get; private set; }

        public IMovieRepository Movies { get; private set; }

        public IPhotoRepository MoviePhotos { get; private set; }

        public ICategoryRepository Categorys {get;private set;}

        public ISubCategoryRepository SubCategorys {get;private set;}

        public IShoppingCartRepository ShoppingCarts { get; private set; }

        public IOrderDetailRepository OrderDetails { get; private set; }
        public IOrderRepository Orders { get; private set; }

        /// <summary>
        /// Here is where we commit a transaction.
        /// </summary>
        /// <returns></returns>

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
