using EventBusRabbitMQ.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace EventBusRabbitMQ.Producer
{
    public class EventRabbitMQProducer
    {
        readonly IRabbitMQConnection connection;

        public EventRabbitMQProducer(IRabbitMQConnection rabbitMQConnection)
        {
            this.connection = rabbitMQConnection;
        }

        public void PublishBasketCheckout(string queueName, BasketCheckoutEvent publishModel)
        {
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var message = JsonConvert.SerializeObject(publishModel);
            var body = Encoding.UTF8.GetBytes(message);

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            channel.ConfirmSelect();
            channel.BasicPublish(exchange: string.Empty, routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
            channel.WaitForConfirmsOrDie();

            channel.BasicAcks += (sender, eventArgs) => {
                Console.WriteLine("Sent rabbitMQ");
            };
            channel.ConfirmSelect();
        }
    }
}
