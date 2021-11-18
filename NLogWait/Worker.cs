using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NLogWait
{
    public sealed class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly List<Task> _tasks = new(128);

        private readonly List<Thread> _threads = new(128);

        public Worker( ILogger<Worker> logger ) =>
            _logger = logger;

        protected override async Task ExecuteAsync( CancellationToken stoppingToken )
        {
            //await V2( stoppingToken );
            await V3( stoppingToken );

            Parallel.For( 0,
                          100,
                          //new() { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 },
                          _ =>
                          {
                              while ( !stoppingToken.IsCancellationRequested )
                                  _logger.LogInformation( "Test" );
                          } );
        }

        private async Task V2( CancellationToken stoppingToken )
        {
            for ( var i = 0; i < 100; i++ )
                _tasks.Add( Task.Run( () =>
                                      {
                                          while ( !stoppingToken.IsCancellationRequested )
                                              _logger.LogInformation( "Test" );
                                      },
                                      CancellationToken.None ) );

            await Task.WhenAll( _tasks );
        }

        private void V3( CancellationToken stoppingToken )
        {
            for ( var i = 0; i < 100; i++ )
            {
                var t = new Thread( () =>
                {
                    while ( !stoppingToken.IsCancellationRequested )
                        _logger.LogInformation( "Test" );
                } );
                t.Start();
                _threads.Add( t );
            }
        }
    }
}