using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using ServiceBusDemo.Consumer.Events;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetSection("serviceBus:connectionString").Value;
var topicName = configuration.GetSection("serviceBus:topicName").Value;
var subscriptionName = configuration.GetSection("serviceBus:subscriptionName").Value;

var client = new ServiceBusClient(connectionString);
var processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

try
{
    processor.ProcessErrorAsync += eventArgs => Task.FromException(eventArgs.Exception);
    
    processor.ProcessMessageAsync += async eventArgs =>
    {
        var updatedCustomer = eventArgs.Message.Body.ToObjectFromJson<CustomerUpdatedEvent>(new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        Console.WriteLine(@$"
            Customer updated: 
            Id: {updatedCustomer.Id}
            Name: {updatedCustomer.Name} 
            Email: {updatedCustomer.Email}
            PhoneNumber: {updatedCustomer.PhoneNumber}");
        
        await eventArgs.CompleteMessageAsync(eventArgs.Message);
    };
    
    await processor.StartProcessingAsync();
    Console.ReadKey();
}
finally
{
    await client.DisposeAsync();
}