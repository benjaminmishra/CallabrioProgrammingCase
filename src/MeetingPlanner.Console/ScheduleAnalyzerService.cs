using Microsoft.Extensions.Logging;

namespace MeetingPlanner.Console;

public class ScheduleAnalyzerService
{
    private readonly ILogger<ScheduleAnalyzerService> _logger;

    public ScheduleAnalyzerService(ILogger<ScheduleAnalyzerService> logger)
    {
        _logger = logger;
    }

    public IReadOnlyCollection<TimeSlot> FindSuitableTimeSlots(ScheduleResult scheduleResult, int requiredTeamMembers)
    {
        var suitableTimeSlots = new List<TimeSlot>();

        for (var hour = 0; hour < 24; hour++)
        {
            for (var minute = 0; minute < 60; minute += 15)
            {
                var start = new DateTime(2024, 1, 1, hour, minute, 0);
                var end = start.AddMinutes(15);

                var availableExperts = scheduleResult.Schedules.Count(schedule =>
                    schedule.Projection.Any(projection =>
                        projection.Start <= start && projection.Start.AddMinutes(projection.Minutes) >= end &&
                        projection.Description != "Lunch" && projection.Description != "Short break"));

                if (availableExperts >= requiredTeamMembers)
                {
                    suitableTimeSlots.Add(new TimeSlot(start, end));
                }
            }
        }

        return suitableTimeSlots;
    }
}
