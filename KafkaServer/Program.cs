using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using System;

namespace KafkaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string topicName = "test";
            var options = new KafkaOptions(new Uri("http://localhost:9092"));

            //创建一个生产者发消息
            using (var producer = new Producer(new BrokerRouter(options)) { BatchSize = 100, BatchDelayTime = TimeSpan.FromMilliseconds(2000) })
            {
                while (true)
                {
                    var message = Console.ReadLine();
                    if (message == "quit") break;

                    if (!string.IsNullOrEmpty(message))
                    {
                        producer.SendMessageAsync(topicName, new[] { new Message(message) });
                    }
                }
            }
        }
    }
}
