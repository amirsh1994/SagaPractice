using System.Reflection;
using System.Timers;
using Framework.OutBoxPublisher;
using OrderManagement.Domain.Contract.DomainEvents;

namespace OrderManagementEventPublisher.Worker;

    public class Worker(ILogger<Worker>logger,OutBoxManager manager) : BackgroundService
    {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Elapsed+= CheckOutBox;
            timer.Interval = 1000;
        }

        public void CheckOutBox(object? sender, ElapsedEventArgs e)
        {
            var contractAssembly = new Assembly[] {typeof(OrderCreatedDomainEvent).Assembly };
            logger.LogCritical("Worker started at: {time}", DateTime.Now);
            manager.Start(contractAssembly);
        }
    }

