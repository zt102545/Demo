using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using System;
using System.Threading.Tasks;

namespace KafkaClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const string topicName = "test";
            var options = new KafkaOptions(new Uri("http://localhost:9092"));


            Task.Run(() =>
            {
                //创建一个消费者
                var consumer = new Consumer(new ConsumerOptions(topicName, new BrokerRouter(options)));
                foreach (var data in consumer.Consume())
                {
                    Console.WriteLine("Response: PartitionId={0},Offset={1} :Value={2}", data.Meta.PartitionId, data.Meta.Offset, data.Value.ToUtf8String());
                }
            });

            Console.ReadLine();
        }
    }
}
