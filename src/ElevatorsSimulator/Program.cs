// See https://aka.ms/new-console-template for more information
using ElevatorsSimulator;
using ElevatorsSimulator.Logic;
using ElevatorsSimulator.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services.AddLogging(builder =>
                {
                    builder.AddConsole();
                })
                .AddSingleton<ISimulator, Simulator>()
                .AddSingleton<IElevatorPool, ElevatorPool>()
                .AddSingleton<IOrchestrator, Orchestrator>()
                .AddSingleton<ITaskManager, TaskManager>()
                .AddSingleton<IMovementCostAnalyzer, MovementCostAnalyzer>()
                .AddSingleton<IMenu, Menu>();
        })
        .Build();

        var app = host.Services.GetRequiredService<ISimulator>();

        app.Run();
    }
}
