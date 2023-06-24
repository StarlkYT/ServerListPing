using Bedrock.Framework;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Starlk.Console;

var services = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.SetMinimumLevel(LogLevel.Trace);
        builder.AddConsole();
    })
    .BuildServiceProvider();

var server = new ServerBuilder(services)
    .UseSockets(sockets =>
        sockets.ListenLocalhost(25565,
            configure => configure
                .UseConnectionLogging()
                .UseConnectionHandler<ServerConnectionHandler>()))
    .Build();

var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
await server.StartAsync();
logger.LogInformation("Listening on port 25565");

var taskCompletionSource = new TaskCompletionSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    taskCompletionSource.TrySetResult();
    eventArgs.Cancel = true;
};

await taskCompletionSource.Task;
await server.StopAsync();
logger.LogInformation("Stopped listening");