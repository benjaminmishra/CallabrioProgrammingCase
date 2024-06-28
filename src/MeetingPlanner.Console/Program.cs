using MeetingPlanner.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hostApplicationBuilder = Host.CreateApplicationBuilder(args);

hostApplicationBuilder.Configuration
    .SetBasePath(Environment.CurrentDirectory)
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables();

hostApplicationBuilder.Services.AddOptions<MeetingSchedulerOptions>()
    .Bind(hostApplicationBuilder.Configuration.GetSection(MeetingSchedulerOptions.ConfigSection));
hostApplicationBuilder.Services.AddSingleton<App>();
hostApplicationBuilder.Services.AddHttpClient();
hostApplicationBuilder.Services.AddSingleton<ExpertsScheduleReader>();
hostApplicationBuilder.Services.AddSingleton<ScheduleAnalyzerService>();

var host = hostApplicationBuilder.Build();

var app = host.Services.GetRequiredService<App>();
var cancellationTokenSource = new CancellationTokenSource();

Console.CancelKeyPress += (_, _) =>
{
    cancellationTokenSource.Cancel();
};


Console.WriteLine("============= Meeting Scheduler Start =================");

while (!cancellationTokenSource.IsCancellationRequested)
{
    await app.RunAsync(cancellationTokenSource.Token);
}

Console.WriteLine("============= Scheduler Shut down ===============");