using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace RabbitCconsumer
{
    class Program
    {

        /// <summary>
        /// 消费者
        /// </summary>
        static void Main(string[] args)
        {
         
            //Console.WriteLine("Hello Cconsumer Direct!");
            //Direct("DirectQueue");

            //Console.WriteLine("Hello Cconsumer FanoutQueue!");
            //Fanout();

            //Console.WriteLine("Hello Cconsumer RutoingQueue!");
            //Console.WriteLine("输入routeKey! 默认 Rutoing");
            //string routeKey = Console.ReadLine();
            //Rutoing(routeKey, "RutoingQueue");

            //Console.WriteLine("Hello Cconsumer TopicQueue!");
            //Console.WriteLine("输入routeKey! 格式 单词1.单词2.单词3 ");
            //string routeKey = Console.ReadLine();
            //Topic(routeKey, "TopicQueue");

            Console.WriteLine("Hello Cconsumer RPCQueue!");
            Console.WriteLine("输入routeKey! 格式 单词1.单词2.单词3 ");
            string routeKey = Console.ReadLine();
            RPC(routeKey);

        }

        /// <summary>
        /// 工作队列
        /// </summary>
        public static void Direct(string queueName)
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

            //事件基本消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            //接收到消息事件
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息： {message}");
                //延时模拟业务操作
                Thread.Sleep(3 * 1000);
                Console.WriteLine($"完成消息： {message}");
                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
            };
            //启动消费者 设置为手动应答消息
            channel.BasicConsume(queueName, false, consumer);
            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            channel.Dispose();
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

            //定义一个Direct类型交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, false, false, null);

            var queueName = channel.QueueDeclare().QueueName;
            //将队列绑定到交换机
            channel.QueueBind(queue: queueName,
                                exchange: exchangeName,
                                routingKey: routeKey);

            //事件基本消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            //接收到消息事件
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息： {message}");
                //延时模拟业务操作
                Thread.Sleep(2 * 1000);
                Console.WriteLine($"完成消息： {message}");
                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
            };
            //启动消费者 设置为手动应答消息
            channel.BasicConsume(queueName, false, consumer);
            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            channel.Dispose();
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

            queueName = channel.QueueDeclare().QueueName;

            //定义一个Direct类型交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, false, false, null);

            //将队列绑定到交换机
            channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: routeKey);

            //事件基本消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            //接收到消息事件
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息： {message}");
                //延时模拟业务操作
                Thread.Sleep(3 * 1000);
                Console.WriteLine($"完成消息： {message}");
                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
            };
            //启动消费者 设置为手动应答消息
            channel.BasicConsume(queueName, false, consumer);
            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            channel.Dispose();
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

            queueName = channel.QueueDeclare().QueueName;

            //定义一个Direct类型交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, false, false, null);

            //将队列绑定到交换机
            channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: routeKey);

            //事件基本消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            //接收到消息事件
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息： {message}");
                //延时模拟业务操作
                Thread.Sleep(3 * 1000);
                Console.WriteLine($"完成消息： {message}");
                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
            };
            //启动消费者 设置为手动应答消息
            channel.BasicConsume(queueName, false, consumer);
            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();
        }

        /// <summary>
        /// RPC
        /// </summary>
        public static void RPC(string routeKey, bool isPersistent = true)
        {
            string exchangeName = "RPCExchange";
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
            //queueName = channel.QueueDeclare().QueueName;

            channel.QueueDeclare(queue: routeKey,
                                 durable: false,
                                 exclusive: false, 
                                 autoDelete: false, 
                                 arguments: null);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: routeKey,
                                autoAck: false, 
                                consumer: consumer);

            ////定义一个Direct类型交换机
            //channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, false, false, null);

            ////将队列绑定到交换机
            //channel.QueueBind(queue: queueName,
            //                  exchange: exchangeName,
            //                  routingKey: routeKey);

           
            //接收RPC服务端反馈的信息
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                var response = Encoding.UTF8.GetString(body);
                Console.WriteLine($"收到消息： {response}");
                //延时模拟业务操作
                Thread.Sleep(3 * 1000);
                Console.WriteLine($"完成消息： {response}");

                response += "CallBack";

                channel.BasicPublish(exchange: "", 
                                    routingKey: props.ReplyTo,
                                    basicProperties: replyProps, 
                                    body: Encoding.UTF8.GetBytes(response));
                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                    multiple: false);
            };

            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();

        }

    }
}
