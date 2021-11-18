using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Hosting;
using NLogWait;

public static class Program
{
    public static async Task Main()
    {
        Console.Title = $"LogDemo - {Environment.ProcessId} (PID)";

        var builder = Host.CreateDefaultBuilder();

        var host = builder
                   .ConfigureServices( services =>
                   {
                       services.AddLogging();
                       services.AddHostedService<Worker>();
                   } )
                   .ConfigureLogging( logging => logging.ClearProviders() )
                   .UseNLog()
                   .Build();

        try
        {
            await host.RunAsync();
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}