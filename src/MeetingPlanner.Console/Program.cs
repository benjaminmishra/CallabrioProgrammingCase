using MeetingPlanner.Console;

var url = "https://rndfiles.blob.core.windows.net/pizzacabininc/2015-12-14.json";
var requiredTeamMembers = 2;

var scheduleService = new ScheduleService();

var json = await GetJsonFromUrl(url);
var scheduleResult = scheduleService.GetScheduleFromJson(json);

var suitableTimeSlots = scheduleService.FindSuitableTimeSlots(scheduleResult, requiredTeamMembers);

Console.WriteLine($"Suitable time slots for {requiredTeamMembers} team members:");
foreach (var slot in suitableTimeSlots)
{
    Console.WriteLine($"{slot.Start:HH:mm} - {slot.End:HH:mm}");
}

static async Task<string> GetJsonFromUrl(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}