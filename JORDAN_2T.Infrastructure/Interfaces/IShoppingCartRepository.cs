using JORDAN_2T.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Interfaces
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncrementQuantity(ShoppingCart shoppingCart, int quantity);
        int DecrementQuantity(ShoppingCart shoppingCart, int quantity);
    }
}
