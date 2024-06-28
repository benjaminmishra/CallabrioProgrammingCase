using MeetingPlanner.Console;
using Microsoft.Extensions.Options;

namespace MeetingPlanner.Tests;

public class ScheduleAnalyzerTests
{
    [Fact]
    public void FindMeetingTimeSlots_DateRangeOutsideOfScheduleData_ZeroSuitableTimeSlotsFound()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        // date range is for the year 2020 which is outside the schedule date range i.e. 2015
        var startDate = new DateTime(2020, 12, 01);
        var endDate = new DateTime(2020, 12, 30);

        var results = scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, 15, 5, ScheduleData.Data.Schedules);

        Assert.Empty(results);
    }
}