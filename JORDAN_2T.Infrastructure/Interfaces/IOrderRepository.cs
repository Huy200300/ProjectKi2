using JORDAN_2T.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order orderHeader);
        void UpdateStatus(int id, OrderStatus orderStatus, PaymentStatus paymentStatus);
         Order GetOrder(int id);
        IEnumerable<Order> GetOrders(OrderStatus orderStatus,int pg);
    }
}
