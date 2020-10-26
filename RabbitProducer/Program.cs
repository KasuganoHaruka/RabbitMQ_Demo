using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace RabbitProducer
{
    class Program
    {

        /// <summary>
        /// 生产者
        /// </summary>
        static void Main(string[] args)
        {
           
            //Console.WriteLine("Hello Producer Direct!");
            //Direct("DirectQueue");

            //Console.WriteLine("Hello Producer Fanout!");
            //Fanout();

            //Console.WriteLine("Hello Producer Rutoing!");
            //Console.WriteLine("输入routeKey! 格式 Rutoing");
            //string routeKey = Console.ReadLine();
            //Rutoing(routeKey, "RutoingQueue");

            //Console.WriteLine("Hello Producer TopicQueue!");
            //Console.WriteLine("输入routeKey!  格式 单词1.单词2.单词3  ");
            //string routeKey = Console.ReadLine();
            //Topic(routeKey, "TopicQueue");

            Console.WriteLine("Hello Producer RPCQueue!");
            Console.WriteLine("输入routeKey!  格式 单词1.单词2.单词3  ");
            string routeKey = Console.ReadLine();
            RPC(routeKey);
        }

        /// <summary>
        /// 工作队列
        /// </summary>
        public static void Direct(string queueName, bool isPersistent = true)
        {
            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                //UserName = "admin",//用户名
                //Password = "admin",//密码
                HostName = "localhost"//rabbitmq ip
            };

            //创建连接
            var connection = factory.CreateConnection();
            //创建通道
            var channel = connection.CreateModel();
            //声明一个队列
            channel.QueueDeclare(queue: queueName,
                                    durable: isPersistent,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");

            var properties = channel.CreateBasicProperties();
            properties.Persistent = isPersistent;

            string input;
            do
            {
                input = Console.ReadLine();

                var sendBytes = Encoding.UTF8.GetBytes(input);
                //发布消息
                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: properties,
                                     body: sendBytes);

            } while (input.Trim().ToLower() != "exit");
            channel.Close();
            connection.Close();
        }

        /// <summary>
        /// 发布/订阅
        /// </summary>
        public static void Fanout(bool isPersistent = true)
        {
            string exchangeName = "FanoutExchange";
            string routeKey = "";

            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                //UserName = "admin",//用户名
                //Password = "admin",//密码
                HostName = "localhost"//rabbitmq ip
            };

            //创建连接
            var connection = factory.CreateConnection();
            //创建通道
            var channel = connection.CreateModel();

            var properties = channel.CreateBasicProperties();
            properties.Persistent = isPersistent;

            //定义一个Direct类型交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, false, false, null);

            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");

            string input;
            do
            {
                input = Console.ReadLine();

                var sendBytes = Encoding.UTF8.GetBytes(input);
                //发布消息
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routeKey,
                                     basicProperties: properties,
                                     body: sendBytes);

            } while (input.Trim().ToLower() != "exit");
            channel.Close();
            connection.Close();
        }

        /// <summary>
        /// 路由
        /// </summary>
        public static void Rutoing(string routeKey, string queueName, bool isPersistent = true)
        {
            string exchangeName = "RutoingExchange";
            //string routeKey = "Rutoing";

            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                //UserName = "admin",//用户名
                //Password = "admin",//密码
                HostName = "localhost"//rabbitmq ip
            };

            //创建连接
            var connection = factory.CreateConnection();
            //创建通道
            var channel = connection.CreateModel();
            //声明一个队列
            channel.QueueDeclare(queue: queueName,
                                    durable: isPersistent,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            //定义一个Direct类型交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, false, false, null);

            //将队列绑定到交换机
            channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: routeKey);

            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");

            var properties = channel.CreateBasicProperties();
            properties.Persistent = isPersistent;

            string input;
            do
            {
                input = Console.ReadLine();

                var sendBytes = Encoding.UTF8.GetBytes(input);
                //发布消息
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routeKey,
                                     basicProperties: properties,
                                     body: sendBytes);

            } while (input.Trim().ToLower() != "exit");
            channel.Close();
            connection.Close();
        }

        /// <summary>
        /// 主题
        /// </summary>
        public static void Topic(string routeKey, string queueName, bool isPersistent = true)
        {
            string exchangeName = "TopicExchange";
            //string routeKey = "Rutoing";

            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                //UserName = "admin",//用户名
                //Password = "admin",//密码
                HostName = "localhost"//rabbitmq ip
            };

            //创建连接
            var connection = factory.CreateConnection();
            //创建通道
            var channel = connection.CreateModel();
            //声明一个队列
            channel.QueueDeclare(queue: queueName,
                                    durable: isPersistent,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            //定义一个Direct类型交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, false, false, null);

            //将队列绑定到交换机
            channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: routeKey);

            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");

            var properties = channel.CreateBasicProperties();
            properties.Persistent = isPersistent;

            string input;
            do
            {
                input = Console.ReadLine();

                var sendBytes = Encoding.UTF8.GetBytes(input);
                //发布消息
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routeKey,
                                     basicProperties: properties,
                                     body: sendBytes);

            } while (input.Trim().ToLower() != "exit");
            channel.Close();
            connection.Close();
        }

        /// <summary>
        /// RPC回调
        /// </summary>
        /// <param name="routeKey"></param>
        /// <param name="isPersistent"></param>
        public static void RPC(string routeKey, bool isPersistent = true)
        {
            string exchangeName = "RPCExchange";

            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                //UserName = "admin",//用户名
                //Password = "admin",//密码
                HostName = "localhost"//rabbitmq ip
            };

            //创建连接
            var connection = factory.CreateConnection();
            //创建通道
            var channel = connection.CreateModel();

            ////创建RPC队列-队列名rpc_queue，非持久化，非排他，非自动删除
            //channel.QueueDeclare(queue: queueName,
            //                        durable: isPersistent,
            //                        exclusive: false,
            //                        autoDelete: false,
            //                        arguments: null);
            ////Qos 预期大小：0；预取数：1；非全局
            //channel.BasicQos(0, 1, false);

            ////定义一个Direct类型交换机
            //channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, false, false, null);

            ////将队列绑定到交换机
            //channel.QueueBind(queue: queueName,
            //                  exchange: exchangeName,
            //                  routingKey: routeKey);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = isPersistent;
            var correlationId = Guid.NewGuid().ToString();
            properties.CorrelationId = correlationId;
            var queueName = channel.QueueDeclare().QueueName;
            properties.ReplyTo = queueName;

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            BlockingCollection<string> respQueue = new BlockingCollection<string>();

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                Console.WriteLine($"收到回调： {response}");
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };

            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");
            string input;
            do
            {
                input = Console.ReadLine();

                var sendBytes = Encoding.UTF8.GetBytes(input);
                //发布消息
                channel.BasicPublish(exchange: "",//exchangeName,
                                     routingKey: routeKey,
                                     basicProperties: properties,
                                     body: sendBytes);

                channel.BasicConsume(consumer: consumer,
                                    queue: queueName,
                                    autoAck: true);

            } while (input.Trim().ToLower() != "exit");
            channel.Close();
            connection.Close();
        }


    }





}
