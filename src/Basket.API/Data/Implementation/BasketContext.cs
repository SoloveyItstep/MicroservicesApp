using Basket.API.Data.Abstract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Data.Implementation
{
    public class BasketContext : IBasketContext
    {
        private readonly IConnectionMultiplexer _redisConnection;

        public BasketContext(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            Redis = redisConnection.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}
