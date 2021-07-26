using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Consumer
{
	class Consumer
	{
		static ConnectionFactory ConnectionFactory;

		static void Main()
		{
			Console.WriteLine("Start Message Consumer Instance!");

			Initialize();

			CreateChannelConnection();
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

		private static void CreateChannelConnection()
		{
			using var channel = ConnectionFactory.CreateConnection()
												 .CreateModel();

			channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

			var eventingConsumer = new EventingBasicConsumer(channel);

			Console.WriteLine($"Instace: {Guid.NewGuid()}");
			Console.WriteLine("Waiting for messages...");

			eventingConsumer.Received += (model, content) =>
			{
				var interval = new Random().Next(100, 500);
				Thread.Sleep(interval);

				Console.WriteLine($"Time: {interval} - Message: {Encoding.UTF8.GetString(content.Body.ToArray())}");

				channel.BasicAck(deliveryTag: content.DeliveryTag, multiple: false);
			};

			channel.BasicConsume(queue: "Log", autoAck: false, consumer: eventingConsumer);

			Console.ReadLine();
		}
	}
}
