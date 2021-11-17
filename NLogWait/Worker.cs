using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NLogWait;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker( ILogger<Worker> logger ) =>
        _logger = logger;

    protected override Task ExecuteAsync( CancellationToken stoppingToken )
    {
        Parallel.For( 0,
                      100,
                      _ =>
                      {
                          while ( !stoppingToken.IsCancellationRequested )
                              _logger.LogInformation( "Test" );
                      } );

        return Task.CompletedTask;
    }
}