using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Producer
{
	class Producer
	{
		static void Main()
		{
			Console.WriteLine("Start Message Producer!");

			Initialize();
		}

		private static void Initialize()
		{
			var host = new ConnectionFactory()
			{ 
				HostName = "127.0.0.1",
				UserName = "guest",
				Password = "guest"
			};

			using var connection = host.CreateConnection();
			using var channel = connection.CreateModel();

			while(true)
			{
				var message = $"Operation performed id: {Guid.NewGuid()}";
				
				var properties = channel.CreateBasicProperties();
				properties.Persistent = true;

				Thread.Sleep(200);

				channel.BasicPublish(exchange: string.Empty, routingKey: "Log", basicProperties: properties, body: Encoding.UTF8.GetBytes(message));

				Console.WriteLine($"{DateTime.UtcNow} - Send: {message}");
			}
		}
	}
}
