using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            //RabbitMQ消息模型的核心思想就是，生产者不把消息直接发送给队列。实际上，生产者在很多情况下都不知道消息是否会被发送到一个队列中。
            //取而代之的是，生产者将消息发送到交换区。交换区是一个非常简单的东西，它一端接受生产者的消息，另一端将他们推送到队列中。
            //交换区必须要明确的指导如何处理它接受到的消息。是放到一个队列中，还是放到多个队列中，亦或是被丢弃。这些规则可以通过交换区的类型来定义。

            //创建RabbitMQ连接对象
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
                Protocol = Protocols.DefaultProtocol,
                AutomaticRecoveryEnabled = true, //自动重连
                RequestedFrameMax = UInt32.MaxValue,
                RequestedHeartbeat = UInt16.MaxValue //心跳超时时间               
            };

            using (var connetcion = factory.CreateConnection())
            {
                using (var channel = connetcion.CreateModel())
                {

                    #region 模式一:简单模式
                    //EasyQ(channel);
                    #endregion

                    #region 模式二:工作模式
                    //WorkQ(channel);
                    #endregion

                    #region 模式三:发布/订阅者模式
                    //PublishSubscribe(channel);
                    #endregion

                    #region 模式四:路由模式
                    //Routing(channel);
                    #endregion

                    #region 模式五:主题模式
                    //Topics(channel);
                    #endregion

                    #region 模式六:RPC模式
                    RPC(channel);
                    #endregion
                }
            }
        }

        static void EasyQ(IModel channel)
        {
            string queueName = "EasyQ";

            //Queue：是消息队列，可以根据需要定义多个队列，设置队列的属性，比如：消息移除、消息缓存、回调机制等设置，实现与Consumer通信；
            //创建一个新的，非持久, 没有排他性，与不自动删除。
            channel.QueueDeclare(queueName, false, false, false, null);
            while (true)
            {
                var sendMessage = Console.ReadLine();
                if (sendMessage.Equals("exit")) return;
                string message = sendMessage;
                var body = Encoding.UTF8.GetBytes(message);
                //routingKey直接指向队列名称***
                channel.BasicPublish("", "EasyQ", null, body);
                Console.WriteLine($"send message : {message}");
            }
        }

        static void WorkQ(IModel channel)
        {
            //设置持久的队列避免消息丢失
            string queueName = "WorkQ";
            //创建一个新的，持久的, 没有排他性，与不自动删除。
            channel.QueueDeclare(queueName, true, false, false, null);

            // 设置消息属性
            var properties = channel.CreateBasicProperties();
            //消息是持久的，存在并不会受服务器重启影响。
            properties.Persistent = true;

            while (true)
            {
                var sendMessage = Console.ReadLine();
                if (sendMessage.Equals("exit")) return;
                string message = sendMessage;
                var body = Encoding.UTF8.GetBytes(message);
                //routingKey直接指向队列名称***
                channel.BasicPublish("", "WorkQ", null, body);
                Console.WriteLine($"send message : {message}");
            }

        }

        static void PublishSubscribe(IModel channel)
        {
            //该模式发布者未将队列绑定到交换，则消息将丢失，但这对我们来说是可以的；如果没有消费者正在监听，我们可以安全地丢弃消息。

            string exchangeName = "PublishSubscribe";
            //Fanout表示该交换机是扇型交换机，可以广播消息给所有绑定到该交换机上的消费者，
            channel.ExchangeDeclare(exchangeName,ExchangeType.Fanout);

            while (true)
            {
                var sendMessage = Console.ReadLine();
                if (sendMessage.Equals("exit")) return;
                string message = sendMessage;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchangeName, "", null, body);
                Console.WriteLine($"send message : {message}");
            }
        }

        static void Routing(IModel channel)
        {
            string exchangeName = "Routing";
            //Direct表示该交换机是直连交换机，交换机将会对绑定键（binding key）和路由键（routing key）进行精确匹配，从而确定消息该分发到哪个队列。
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);

            while (true)
            {
                var sendMessage = Console.ReadLine();
                if (sendMessage.Equals("exit")) return;
                if (sendMessage.Contains("|"))
                {
                    var param = sendMessage.Split("|");
                    string routingKey = param[0];
                    string message = param[1];
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchangeName, routingKey, null, body);
                    Console.WriteLine($"send message : {message}");
                }
            }
        }

        static void Topics(IModel channel)
        {
            string exchangeName = "Topics";
            //Topic表示该交换机是主题模式，交换机将会对绑定键（binding key）和路由键（routing key）进行匹配，匹配方式可以带"*"或"#"，分别表示一个单词或多个单词。
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);

            while (true)
            {
                var sendMessage = Console.ReadLine();
                if (sendMessage.Equals("exit")) return;
                if (sendMessage.Contains("|"))
                {
                    var param = sendMessage.Split("|");
                    string routingKey = param[0];
                    string message = param[1];
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchangeName, routingKey, null, body);
                    Console.WriteLine($"send message : {message}");
                }
            }

        }

        static void RPC(IModel channel)
        {
            string queueName = "RPCQ";
            //创建一个新的，非持久的, 没有排他性，与不自动删除。
            channel.QueueDeclare(queueName, false, false, false, null);
            //basicQos(预取大小：0，预取计数：1，全局：false)
            channel.BasicQos(0, 1, false);
            //创建一个客户端
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString()}: {props.ReplyTo} : Received Message {message}");

                message += "(response)";
                body = Encoding.UTF8.GetBytes(message);
                //发送消息
                channel.BasicPublish("", props.ReplyTo, replyProps, body);
                //消息处理结果应答
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queueName, false, consumer);
            Console.ReadLine();
        }

    }
}
