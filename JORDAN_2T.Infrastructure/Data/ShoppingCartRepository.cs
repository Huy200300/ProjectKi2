using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Data
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private MvcMovieContext _context;

        public ShoppingCartRepository(MvcMovieContext context) : base(context)
        {
            _context = context;
        }

        public int DecrementQuantity(ShoppingCart shoppingCart, int quantity)
        {
            shoppingCart.Quantity -= quantity;
            return shoppingCart.Quantity;
        }

        public int IncrementQuantity(ShoppingCart shoppingCart, int quantity)
        {
            shoppingCart.Quantity += quantity;
            return shoppingCart.Quantity;
        }
    }
}
