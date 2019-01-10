using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using productor.modelo;

namespace productor
{
    class Program
    {
        public static void Main(string[] args)
        {
            Usuario usuario = new Usuario(){
                id = 1,
                nombre = "Alvaro Fuentes",
                mensaje = "Mi mensaje"
            };
            var factory = new ConnectionFactory() { 
                HostName = "localhost" 
            };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "cola-fuentes",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string mensaje = JsonConvert.SerializeObject(usuario);
                var body = Encoding.UTF8.GetBytes(mensaje);

                channel.BasicPublish(exchange: "",
                                     routingKey: "cola-fuentes",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($"Enviado por : {usuario.nombre}");
            }
            Console.WriteLine(" Mensaje enviado");
            Console.ReadLine();
        }
    }
}
