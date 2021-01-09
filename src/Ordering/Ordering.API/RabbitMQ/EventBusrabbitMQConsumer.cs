using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.API.RabbitMQ
{
    public class EventBusrabbitMQConsumer
    {
        readonly IRabbitMQConnection connection;
        readonly IMediator mediator;
        readonly IMapper mapper;
        readonly IOrderRepository orderRepository;

        public EventBusrabbitMQConsumer(IRabbitMQConnection connection, IMediator mediator, IMapper mapper, IOrderRepository orderRepository)
        {
            this.connection = connection;
            this.mediator = mediator;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }

        public void Consumer()
        {
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.BasketCheckoutQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;
            channel.BasicConsume(queue: EventBusConstants.BasketCheckoutQueue, autoAck: true, consumer: consumer, exclusive: false, arguments: null);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if(e.RoutingKey == EventBusConstants.BasketCheckoutQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                var command = mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);
                var response = await mediator.Send(command).ConfigureAwait(false);

            }
        }

        public void Disconect()
        {
            connection.Dispose();
        }
    }
}
