using MeetingPlanner.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

var host = Host.CreateDefaultBuilder(args)
.ConfigureServices(services => {
    services.AddHttpClient();
    services.AddSingleton<ExpertsScheduleReader>();
    services.AddSingleton<ScheduleAnalyzerService>();
}).Build();

await host.RunAsync();


var json = await GetJsonFromUrl(url);
var scheduleResult = scheduleService.GetScheduleFromJson(json);

var suitableTimeSlots = scheduleService.FindSuitableTimeSlots(scheduleResult, requiredTeamMembers);

Console.WriteLine($"Suitable time slots for {requiredTeamMembers} team members:");
foreach (var slot in suitableTimeSlots)
{
    Console.WriteLine($"{slot.Start:HH:mm} - {slot.End:HH:mm}");
}