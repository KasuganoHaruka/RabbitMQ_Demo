using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using TestGrpcClient.RabbitMQ;
using TestGrpcService;
using TestGrpcService.RabbitMQ;

namespace TestGrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receive Begin ");

            //var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //var client = new Greeter.GreeterClient(channel);
            //var reply =  client.SayHelloAsync(new HelloRequest { Name = "Sora" });
            //Console.WriteLine("Greeter 服务返回数据: " + reply.Message);

            //var catClient = new LuCat.LuCatClient(channel);
            //var catReply = await catClient.SuckingCatAsync(new SuckingCatRequest { Name = "Haru" });
            //Console.WriteLine("Cat 服务返回数据: " + catReply.Name);

            //catReply = await catClient.JumpCatAsync(new SuckingCatRequest { Name = "贪生怕死角斗士" });
            //Console.WriteLine("Cat 服务返回数据: " + catReply.Name);

            //catReply = await catClient.JumpCatAsync(new SuckingCatRequest { Name = "勇往直前少女心" });
            //Console.WriteLine("Cat 服务返回数据: " + catReply.Name);

            //Receive.ReceiveFun();
            //Receive.ReceiveLogFun();

            //-----------------------------------------
            //for (int i = 0; i < 10; i++)
            //{
            //    var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //    var client = new Rabbit.RabbitClient(channel);
            //    var reply = await client.CallAsync(new RabbitRequest { Param = "Sora：" + i });
            //    Console.WriteLine("Rabbit CallAsync 返回数据: " + reply.Message);
            //}

            //-----------------------------------------

            //Receive.ReceiceGrpcCallFun();

            Console.WriteLine("Receive End ");
        }





    }
}
