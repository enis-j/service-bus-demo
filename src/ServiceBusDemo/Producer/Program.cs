using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetSection("serviceBus:connectionString").Value;
var topicName = configuration.GetSection("serviceBus:topicName").Value;

var client = new ServiceBusClient(connectionString);
var sender = client.CreateSender(topicName);

var jsonBody = @"
{
    ""id"": ""9cf184f8-6103-4869-9c70-86027c6b1986"",
    ""name"": ""Enis Jasharaj"",
    ""email"": ""ej@something.com"",
    ""phoneNumber"": ""+47-999-99-999""
}";

try
{
    var message = new ServiceBusMessage
    {
        Body = BinaryData.FromString(jsonBody),
        MessageId = Guid.NewGuid().ToString(),
        Subject = "CustomerCreated",
        ContentType = "application/json"
    };
    
    await sender.SendMessageAsync(message);

    Console.WriteLine("Message sent to Topic");
}
finally
{
    await client.DisposeAsync();
}

Console.ReadKey();