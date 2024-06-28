namespace MeetingPlanner.Console;

public class MeetingSchedulerOptions
{
    public const string ConfigSection = "MeetingSchedulerOptions";

    public string ScheduleUrl { get; set; } = string.Empty;

    public TimeOnly WorkDayStartTime { get; set; } = TimeOnly.FromTimeSpan(TimeSpan.FromHours(9));
}