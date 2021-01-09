using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mapper;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class ChackoutOrderHandler : IRequestHandler<CheckoutOrderCommand, OrderResponse>
    {
        readonly IOrderRepository orderRepository;

        public ChackoutOrderHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = OrderMapper.Mapper.Map<Order>(request);
            if (orderEntity == null)
                throw new ApplicationException($"can not map entity {nameof(orderEntity)}");
            var newOrder = await orderRepository.AddAsync(orderEntity).ConfigureAwait(false);
            return OrderMapper.Mapper.Map<OrderResponse>(newOrder);
        }
    }
}
