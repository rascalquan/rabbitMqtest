using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rabbitMqClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string exchangeName = "rabbittopictest";
            string queueName = "sayhello";
            string routeKey = "sayhello.2";

            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "iccode.top"
            };
            //创建连接
            var connection = factory.CreateConnection("rabbitmqclient");
            //创建管道
            var channel = connection.CreateModel();

            //声明队列
            channel.QueueDeclare(queueName, false, false, false, null);

            channel.QueueBind(queueName, exchangeName, routeKey, null);

            //channel.ConfirmSelect();//回执确认，手动确认

            //事件基本消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine($"收到消息:{message}");
                //确认消息
                channel.BasicAck(e.DeliveryTag, false);
                //Console.WriteLine("已发送回执");
            };

            //启动消费者
            channel.BasicConsume(queueName, false, consumer);
            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();
        }
    }
}
