using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Context;
using MassTransit.Definition;

namespace ZaNetDev.MassTransit.Helpers.MessageEnumerationClassSample
{
    public class SubmitOrderConsumerDefinition : ConsumerDefinition<SubmitOrderConsumer>
    {
        protected override void ConfigureConsumer(
            IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<SubmitOrderConsumer> consumerConfigurator)
        {
            EndpointConvention.Map<SubmitOrder>(endpointConfigurator.InputAddress);
        }
    }
    
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            var messageBody = GetMessageBody(context);
            await WriteMessageToConsole(messageBody);
            
            await Console.Out.WriteLineAsync($"Submit Order Consumer: {context.Message.OrderId} ({context.ConversationId})");
        }

        public static string GetMessageBody<T> (ConsumeContext<T> ctx) where T : class
        {
            var receiveContext = ((BaseConsumeContext) ctx).ReceiveContext;
            var body = System.Text.Encoding.Default.GetString(receiveContext.GetBody());
            return body;
        }
        
        public static async Task WriteMessageToConsole (string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            await Console.Out.WriteLineAsync(
                "MESSAGE BODY\r" +
                "===================================================================\r" +
                $"{message}\r" + 
                "===================================================================");
            Console.ResetColor();
        }
    }
}