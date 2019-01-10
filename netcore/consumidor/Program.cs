using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using consumidor.modelo;


namespace consumidor
{
    class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(3000); // esperando 3 segunndos y obtener mensaje
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "cola-fuentes",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var mensaje = Encoding.UTF8.GetString(body);
                    Usuario usuario = JsonConvert.DeserializeObject<Usuario>(mensaje);
                    Console.WriteLine($" Nombre: {usuario.nombre} Mensaje [{usuario.mensaje}]");
                };
                channel.BasicConsume(queue: "cola-fuentes",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Mensaje recibido de la cola, ver consola.");
                Console.ReadLine();
            }
        }
    }
}
