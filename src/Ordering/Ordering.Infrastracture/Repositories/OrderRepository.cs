using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastracture.Data;
using Ordering.Infrastracture.Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastracture.Repositories
{
    public class OrderRepository: Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext)
            :base(dbContext)
        { }

        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            return await dbContext.Orders.Where(x => x.UserName == userName)
                .ToListAsync().ConfigureAwait(false);
        }
    }
}
