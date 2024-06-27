using MeetingPlanner.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
.ConfigureServices(services =>
{
    services.AddSingleton<App>();
    services.AddHttpClient();
    services.AddSingleton<ExpertsScheduleReader>();
    services.AddSingleton<ScheduleAnalyzerService>();
}).Build();

await host.RunAsync();

var app = host.Services.GetRequiredService<App>();
var cancellationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, _) =>
{
    cancellationTokenSource.Cancel();
};


while (!cancellationTokenSource.IsCancellationRequested)
{
    await app.RunAsync(cancellationTokenSource.Token);
}