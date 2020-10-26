using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TestGrpcService.RabbitMQ
{
    public class Send
    {
        /// <summary>
        /// 工作队列
        /// </summary>
        /// <param name="args"></param>
        public static void SendFun(string args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: "task_queue",
                                     basicProperties: properties,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Thread.Sleep(300);
        }

        /// <summary>
        /// 发布/订阅
        /// </summary>
        /// <param name="args"></param>
        public static void SendLogFun(string args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                Thread.Sleep(300);
            }
        }

        private static string GetMessage(string args)
        {
            return ((args.Length > 0) ? args : "Hello World!");
        }
    }

    /// <summary>
    /// gRPC Call
    /// </summary>
    public static  class RpcClient
    {
        private static readonly IConnection connection;
        private static readonly IModel channel;
        private static readonly string replyQueueName;
        private static readonly EventingBasicConsumer consumer;
        private static readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private static readonly IBasicProperties props;

         static RpcClient() 
        {
            //    var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //    var client = new Rabbit.RabbitClient(channel);
            //    var reply = await client.CallAsync(new RabbitRequest { Param = "Sora：" + i });
            //    Console.WriteLine("Rabbit CallAsync 返回数据: " + reply.Message);


            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };
        }
        public static string Call(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: messageBytes);
            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
            return respQueue.Take();
        }

        public static void Close()
        {
            connection.Close();
        }



    }
}
