using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace rpc_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "cola-fuentes", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "cola-fuentes", autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Esperando RPC peticiones");

                consumer.Received += (model, ea) =>
                {
                    string response = null;
                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        int n = int.Parse(message);
                        Console.WriteLine(" [.] fib({0})", message);
                        response = fib(n).ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };

                Console.WriteLine(" Presione [enter] para salir.");
                Console.ReadLine();
            }
        }
    

        private static int fib(int n)
        {
            
            if (n == 0 || n == 1)
            {
                return n;
            }

            return fib(n - 1) + fib(n - 2);
        }

        private static int suma(int a, int b)
        {
            return a + b;
        }        

    } 
}
