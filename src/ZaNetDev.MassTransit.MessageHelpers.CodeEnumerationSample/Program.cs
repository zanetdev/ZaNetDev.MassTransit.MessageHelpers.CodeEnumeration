using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Util;
using Microsoft.Extensions.DependencyInjection;

namespace ZaNetDev.MassTransit.Helpers.MessageEnumerationClassSample
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
            var serviceProvider = ConfigureServiceProvider();
            var bus = serviceProvider.GetRequiredService<IBusControl>();

            try
            {
                bus.Start();
                try
                {
                    Console.WriteLine("Bus started, type 'exit' to exit.");

                    bool running = true;
                    var input = "s";
                    while (running)
                    {
                        switch (input)
                        {
                            case "x":
                            case "q":
                                running = false;
                                break;

                            case "s":
                                TaskUtil.Await(() => SubmitOrder(serviceProvider));
                                break;
                        }
                        input = Console.ReadLine();
                    }
                }
                finally
                {
                    bus.Stop();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        
        static async Task SubmitOrder(IServiceProvider provider)
        {
            var bus = provider.GetRequiredService<IBus>();
            var orderId = NewId.NextGuid();
            var rand = new Random();
            var statuses = MyStatusEnum.GetAll().ToList();
            var status = statuses[rand.Next(statuses.Count)];
            await bus.Send<SubmitOrder>(
                new
                {
                    OrderId = orderId,
                    OrderDateTime = DateTimeOffset.Now,
                    Status = status
                }
                );
        }
        
        static IServiceProvider ConfigureServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddMassTransit( x =>
            {
                x.AddConsumersFromNamespaceContaining<SubmitOrderConsumer>();
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.TransportConcurrencyLimit = 100;
                    cfg.ConfigureEndpoints(context);
                });
            });
            return services.BuildServiceProvider();
        }
    }
}