using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using TestGrpcService;

namespace TestGrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("gRPC Client Start..");

            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //var client = new Greeter.GreeterClient(channel);
            //var reply =  client.SayHelloAsync(new HelloRequest { Name = "Sora" });
            //Console.WriteLine("Greeter 服务返回数据: " + reply.Message);

            var catClient = new LuCat.LuCatClient(channel);
            var catReply = await catClient.SuckingCatAsync(new SuckingCatRequest { Name = "Haru" });
            Console.WriteLine("Cat 服务返回数据: " + catReply.Name);

            catReply = await catClient.JumpCatAsync(new SuckingCatRequest { Name = "贪生怕死角斗士" });
            Console.WriteLine("Cat 服务返回数据: " + catReply.Name);

            catReply = await catClient.JumpCatAsync(new SuckingCatRequest { Name = "勇往直前少女心" });
            Console.WriteLine("Cat 服务返回数据: " + catReply.Name);


            Console.ReadKey();
        }





    }
}
