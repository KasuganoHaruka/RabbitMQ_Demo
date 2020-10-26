using Grpc.Net.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestGrpcService;

namespace TestGrpcClient.RabbitMQ
{
    public class Receive
    {
        /// <summary>
        /// 工作队列
        /// </summary>
        public static void ReceiveFun()
        {
            Console.WriteLine(" RabbitMQ Receive Run..");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    int dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000);

                    Console.WriteLine(" [x] Done");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: "task_queue",
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }

        /// <summary>
        /// 发布/订阅
        /// </summary>
        public static void ReceiveLogFun()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: "logs",
                                  routingKey: "");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// gRPC回调
        /// </summary>
        public static void ReceiceGrpcCallFun()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
               {
                   string response = null;

                   var body = ea.Body.ToArray();
                   var props = ea.BasicProperties;
                   var replyProps = channel.CreateBasicProperties();
                   replyProps.CorrelationId = props.CorrelationId;

                   try
                   {
                       var message = Encoding.UTF8.GetString(body);
                       int n = int.Parse(message);
                       Console.WriteLine($" Rabbit Receive{message}");
                       response = (n * 10).ToString();//GrpcCall(n).ToString();
                   }
                   catch (Exception e)
                   {
                       Console.WriteLine(" Exception: " + e.Message);
                       response = "";
                   }
                   //finally
                   //{
                       Console.WriteLine($" GRPC Call Result;{response}");
                       var responseBytes = Encoding.UTF8.GetBytes(response);

                       channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                       channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                       //var channel = GrpcChannel.ForAddress("https://localhost:5001");
                       //var client = new Rabbit.RabbitClient(channel);
                       //var reply = await client.CallAsync(new RabbitRequest { Param = "Sora："  });
                       //Console.WriteLine("Rabbit CallAsync 返回数据: " + reply.Message);
                   //}
               };

                Console.WriteLine(" Press [enter] to exit.");
            }
        }

        private static int GrpcCall(int n)
        {
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Rabbit.RabbitClient(channel);
            //var reply = await client.CallAsync(new RabbitRequest { Param = "Sora：" });
            //Console.WriteLine("Rabbit CallAsync 返回数据: " + reply.Message);
            Thread.Sleep(n * 100);
            return n * 10;
        }
    }
}
