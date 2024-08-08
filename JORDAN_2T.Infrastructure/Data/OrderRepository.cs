using JORDAN_2T.Infrastructure.Interfaces;
using JORDAN_2T.ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.Infrastructure.Data
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private MvcMovieContext _context;

        public OrderRepository(MvcMovieContext context) : base(context)
        {
            _context = context;
            OrdersStatusList = new Dictionary<OrderStatus, string>
            {
                {OrderStatus.Approved, "Approved" },
                {OrderStatus.Cancelled, "Cancelled" },
                {OrderStatus.Pending, "Pending" },
                {OrderStatus.Processing, "Processing" },
                {OrderStatus.Refunded, "Refunded" },
                {OrderStatus.Shipped, "Shipped" },
            
            };
            PaymentStatusList = new Dictionary<PaymentStatus, string>
            {
                {PaymentStatus.Approved, "Approved" },
                {PaymentStatus.ApprovedForDelayedPayment, "ApprovedForDelayedPayment" },
                {PaymentStatus.Pending, "Pending" },
                {PaymentStatus.Rejected, "Rejected" },
            
            };
        }


        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }
        public IEnumerable<Order> GetOrders(OrderStatus orderStatus,int pg)
        {
             IEnumerable<Order> orders;
            var ordersList = _dbSet.Where(p => p.ApplicationUser != null).Where(p=>p.OrderStatus==orderStatus);

            const int pageSize=8;
            if(pg<1)
                pg=1;
                ;

            int recsCount =ordersList.Count();

            var pager=new Pager(recsCount,pg,pageSize);

            int recSkip=(pg-1)*pageSize;
            orders= ordersList.OrderByDescending(p => p.Id).Skip(recSkip).Take(pageSize).ToList();
            return orders;
        }
        public  Order GetOrder(int id)
        {
            var order = _dbSet.Single(i => i.Id == id);
            // Populate the photo collection. Lazy loading is not
            // turned on so we have to do it explicitly. When you
            // read up on eager, lazy, and explicit loading, make
            // sure you are reading about EF Core, not just EF.
           
            return order;
        }
        // public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        // {
        //     var orderFromDb = _context.Orders.FirstOrDefault(u => u.Id == id);
        //     if (orderFromDb != null)
        //     {
        //         orderFromDb.OrderStatus = orderStatus;
        //         if (paymentStatus != null)
        //         {
        //             orderFromDb.PaymentStatus = paymentStatus;
        //         }
        //     }
        // }
          public void UpdateStatus(int id, OrderStatus orderStatus, PaymentStatus paymentStatus )
        {
            var orderFromDb = _context.Orders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public Order GetOrder(object id)
        {
            throw new NotImplementedException();
        }
        public Dictionary<OrderStatus, string> OrdersStatusList
        {
            get;
            private set;
        }
        public Dictionary<PaymentStatus, string> PaymentStatusList
        {
            get;
            private set;
        }
    }
}
