using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        public bool IsConnected => connection != null && connection.IsOpen && !disposed;
        readonly IConnectionFactory connectionFactory;
        IConnection connection;
        readonly bool disposed;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
            if (!IsConnected)
                TryConnect();
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No rabbitMQ connection");
            }
            return connection.CreateModel();
        }

        public void Dispose()
        {
            if (disposed)
                return;

            try
            {
                connection.Dispose();
            }
            catch(Exception e)
            {
                //logger here
                throw;
            }
        }

        public bool TryConnect()
        {
            try
            {
                connection = connectionFactory.CreateConnection();
            }catch(BrokerUnreachableException e)
            {
                Thread.Sleep(2000);
                connection = connectionFactory.CreateConnection();
            }

            return IsConnected;
        }
    }
}
