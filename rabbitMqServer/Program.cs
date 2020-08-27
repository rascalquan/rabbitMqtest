using System;
using System.Text;
using RabbitMQ.Client;

namespace rabbitMqServer
{
    class Program
    {
        //服务端/生产者只需要关心交换机，无需关心queue，不用声明队列，
        //消费者也只需关心queue，无需关心交换机
        static void Main(string[] args)
        {
            string exchangeName = "rabbittopictest";

            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "iccode.top"
            };

            //创建连接
            var connection = factory.CreateConnection("rabbitmqserver");
            //创建通道
            var channel = connection.CreateModel();
            //定义交换机
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, false, false, null);
            ////声明队列
            //channel.QueueDeclare(queueName, false, false, false, null);

            //channel.QueueBind(queueName, exchangeName, routeKey, null);


            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");

            string input;
            do
            {
                input = Console.ReadLine();

                var sendByte = Encoding.UTF8.GetBytes(input);
                
                //发布消息
                var topicName = "sayhello." + new Random().Next(1, 5);
                Console.WriteLine("Topic:" + topicName);
                channel.BasicPublish(exchangeName, topicName, null, sendByte);

            } while (input.Trim().ToLower() != "exit");

            channel.Close();
            connection.Close();
        }
    }
}
