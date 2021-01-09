using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastracture.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryFoAvailability = retry.Value;

            try
            {
                // need migrate first time from manage package console
                orderContext.Database.Migrate();

                if(!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetPreconfiguredOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                if (retryFoAvailability < 3)
                    retryFoAvailability++;
                var log = loggerFactory.CreateLogger<OrderContextSeed>();
                log.LogError(e.Message);
                await SeedAsync(orderContext, loggerFactory, retryFoAvailability);
            }

        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order> { 
                new Order{ UserName = "swn", FirstName = "Solovei", LastName = "Vladimir", EmailAddress = "test@test.com", AddressLine = "test address 1", Country = "test country" }
            };
        }
    }
}
