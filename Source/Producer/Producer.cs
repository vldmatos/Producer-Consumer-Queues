using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace Producer
{
	class Producer
	{
		static ConnectionFactory ConnectionFactory;

		static void Main()
		{
			Console.WriteLine("Start Message Producer!");

			Initialize();

			SendMessages();

			Console.ReadKey();
		}

		private static void Initialize()
		{
			ConnectionFactory = new ConnectionFactory()
			{ 
				HostName = "127.0.0.1",
				UserName = "guest",
				Password = "guest"
			};
		}

		private static void SendMessages()
		{
			using var channel = ConnectionFactory.CreateConnection()
												 .CreateModel();
			
			for (int index = 1; index <= 50; index++)
			{
				var message = $"Message id: {index}";

				var properties = channel.CreateBasicProperties();
				properties.Persistent = true;

				Thread.Sleep(200);

				channel.BasicPublish(exchange: string.Empty, routingKey: "Log", basicProperties: properties, body: Encoding.UTF8.GetBytes(message));

				Console.WriteLine($"{DateTime.UtcNow} - Send: {message}");
			}
		}
	}
}
