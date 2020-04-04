using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQClient
{
    class Program
    {
        static void Main(string[] args)
        {
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

            #region 模式一:简单模式
            //EasyQ(factory);
            #endregion

            #region 模式二:工作模式
            //多线程模拟多个客户端
            //for (int i = 0; i < 3; i++)
            //{
            //    Thread thread = new Thread(new ParameterizedThreadStart(WorkQ));
            //    thread.Start(i);
            //}
            #endregion

            #region 模式三:发布/订阅者模式
            //多线程模拟多个客户端
            //for (int i = 0; i < 3; i++)
            //{
            //    Thread thread = new Thread(new ParameterizedThreadStart(PublishSubscribe));
            //    thread.Start(i);
            //}
            #endregion

            #region 模式四:路由模式
            //多线程模拟多个客户端
            //for (int i = 0; i < 3; i++)
            //{
            //    Thread thread = new Thread(new ParameterizedThreadStart(Routing));
            //    thread.Start(i);
            //}
            #endregion

            #region 模式五:主题模式
            //多线程模拟多个客户端
            //for (int i = 0; i < 3; i++)
            //{
            //    Thread thread = new Thread(new ParameterizedThreadStart(Topics));
            //    thread.Start(i);
            //}
            #endregion

            #region 模式六:RPC模式
            RPC();
            #endregion

            Console.ReadLine();
        }

        static void EasyQ(ConnectionFactory factory)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string queueName = "EasyQ";

                    //Queue：是消息队列，可以根据需要定义多个队列，设置队列的属性，比如：消息移除、消息缓存、回调机制等设置，实现与Consumer通信；
                    //创建一个新的，非持久, 没有排他性，与不自动删除。
                    channel.QueueDeclare(queueName, false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString() } : Received Message {message}");
                    };
                    //autoAck：是否自动ack，如果不自动ack，需要使用channel.ack、channel.nack、channel.basicReject 进行消息应答
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString() } : Press [enter] to exit.");

                    Console.ReadLine();
                }
            }
        }

        static void WorkQ(object i)
        {
            int num = Convert.ToInt32(i);
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
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string queueName = "WorkQ";

                    //Queue：是消息队列，可以根据需要定义多个队列，设置队列的属性，比如：消息移除、消息缓存、回调机制等设置，实现与Consumer通信；
                    //创建一个新的，持久的, 没有排他性，与不自动删除。
                    channel.QueueDeclare(queueName, true, false, false, null);

                    //basicQos(预取大小：0，预取计数：1，全局：false)
                    channel.BasicQos(0, 1, false);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        //线程休眠i秒，模拟业务处理时间
                        Thread.Sleep(num * 2 * 1000);
                        Console.WriteLine($"{num}: {Thread.CurrentThread.ManagedThreadId.ToString()} : Received Message {message}");
                        //消息处理结果应答
                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //autoAck：是否自动ack，如果不自动ack，需要使用channel.ack、channel.nack、channel.basicReject 进行消息应答
                    //autoAck自动应答无法确认消息是否处理完成，只是应答接受到消息。
                    channel.BasicConsume(queueName, false, consumer);

                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString() } : Press [enter] to exit.");

                    Console.ReadLine();
                }
            }
        }

        static void PublishSubscribe(object i)
        {
            int num = Convert.ToInt32(i);
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
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string exchangeName = "PublishSubscribe";
                    //Fanout表示该交换机是扇型交换机，可以广播消息给所有绑定到该交换机上的消费者，
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

                    //创建一个新的，非持久的, 没有排他性，与不自动删除的随机名称队列。
                    string queueName = channel.QueueDeclare().QueueName;
                    //队列绑定到交换机上
                    channel.QueueBind(queueName, exchangeName, "");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        //线程休眠i秒，模拟业务处理时间
                        Thread.Sleep(num * 2 * 1000);
                        Console.WriteLine($"{num}: {Thread.CurrentThread.ManagedThreadId.ToString()}: {queueName}: Received Message {message}");
                        //消息处理结果应答
                        //channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //autoAck：是否自动ack，如果不自动ack，需要使用channel.ack、channel.nack、channel.basicReject 进行消息应答
                    //autoAck自动应答无法确认消息是否处理完成，只是应答接受到消息。
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine($"{num}: {Thread.CurrentThread.ManagedThreadId.ToString()}: {queueName}: Press [enter] to exit.");

                    Console.ReadLine();
                }
            }
        }

        static void Routing(object i)
        {
            int num = Convert.ToInt32(i);
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
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string exchangeName = "Routing";
                    string bindingKey = num.ToString();
                    //创建一个新的，非持久的, 没有排他性，与不自动删除的随机名称队列。
                    string queueName = channel.QueueDeclare().QueueName;

                    //Direct表示该交换机是直连交换机，交换机将会对绑定键（binding key）和路由键（routing key）进行精确匹配，从而确定消息该分发到哪个队列。
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                    //队列绑定到交换机上
                    channel.QueueBind(queueName, exchangeName, bindingKey);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"{num}: {Thread.CurrentThread.ManagedThreadId.ToString()}: {queueName}: Received Message {message}");
                        //消息处理结果应答
                        //channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //autoAck：是否自动ack，如果不自动ack，需要使用channel.ack、channel.nack、channel.basicReject 进行消息应答
                    //autoAck自动应答无法确认消息是否处理完成，只是应答接受到消息。
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine($"{num}: {Thread.CurrentThread.ManagedThreadId.ToString()}: {queueName}: Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        static void Topics(object i)
        {
            int num = Convert.ToInt32(i);
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
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    string exchangeName = "Topics";
                    string bindingKey = "";
                    switch (num)
                    {
                        case 0:
                            bindingKey = "0.*.*";
                            break;
                        case 1:
                            bindingKey = "*.1.*";
                            break;
                        case 2:
                            bindingKey = "*.*.2";
                            break;
                    }
                    //创建一个新的，非持久的, 没有排他性，与不自动删除的随机名称队列。
                    string queueName = channel.QueueDeclare().QueueName;

                    //Topic表示该交换机是主题模式，交换机将会对绑定键（binding key）和路由键（routing key）进行匹配，匹配方式可以带"*"或"#"，分别表示一个单词或多个单词。
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
                    //队列绑定到交换机上
                    channel.QueueBind(queueName, exchangeName, bindingKey);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"{num}: {Thread.CurrentThread.ManagedThreadId.ToString()}: {queueName}: Received Message {message}");
                        //消息处理结果应答
                        //channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //autoAck：是否自动ack，如果不自动ack，需要使用channel.ack、channel.nack、channel.basicReject 进行消息应答
                    //autoAck自动应答无法确认消息是否处理完成，只是应答接受到消息。
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine($"{num}: {Thread.CurrentThread.ManagedThreadId.ToString()}: {queueName}: Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        static void RPC()
        {
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

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var correlationId = Guid.NewGuid().ToString();

                    //接收服务端消息
                    string queueName = channel.QueueDeclare().QueueName;
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Received:{ea.BasicProperties.CorrelationId}:correlationId");
                        if (ea.BasicProperties.CorrelationId == correlationId)
                        {
                            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString()}: {queueName}: Received Message {message}");
                        }
                    };
                    channel.BasicConsume(queueName, true, consumer);

                    //给服务端发送请求
                    var props = channel.CreateBasicProperties();
                    props.CorrelationId = correlationId;
                    props.ReplyTo = queueName;
                    while (true)
                    {
                        var requestMessage = Console.ReadLine();
                        if (requestMessage.Equals("exit")) return;
                        var requestBody = Encoding.UTF8.GetBytes(requestMessage);
                        channel.BasicPublish("", "RPCQ", props, requestBody);
                    }
                }
            }
        }
    }
}
