using MeetingPlanner.Console;
using Microsoft.Extensions.Options;
using System.ComponentModel;

namespace MeetingPlanner.Tests;

[Category("Unit Tests")]
public class ScheduleAnalyzerTests
{
    [Fact]
    public void FindMeetingTimeSlots_DateRangeOutsideOfScheduleData_ZeroSuitableTimeSlotsFound()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        var startDate = new DateTime(2020, 06, 01);
        var endDate = new DateTime(2020, 06, 01);

        var results = scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, 15, 2, ScheduleData.Data.Schedules);

        Assert.Empty(results);
    }

    [Fact]
    public void FindMeetingTimeSlots_MeetingDurationLessThanZero_ThrowsArgumentException()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        var startDate = new DateTime(2023, 06, 01);
        var endDate = new DateTime(2023, 06, 01);

        Assert.Throws<ArgumentException>(() => scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, -1, 2, ScheduleData.Data.Schedules));
    }

    [Fact]
    public void FindMeetingTimeSlots_MinParticipantsLessThanZero_ThrowsArgumentException()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        var startDate = new DateTime(2023, 06, 01);
        var endDate = new DateTime(2023, 06, 01);

        Assert.Throws<ArgumentException>(() => scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, 15, -1, ScheduleData.Data.Schedules));
    }

    [Fact]
    public void FindMeetingTimeSlots_NoScheduleData_ZeroSuitableTimeSlotsFound()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        var startDate = new DateTime(2023, 06, 01);
        var endDate = new DateTime(2023, 06, 01);

        var results = scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, 15, 2, []);

        Assert.Empty(results);
    }

    [Fact]
    public void FindMeetingTimeSlots_EveryoneIsAbsentAllDay_ZeroSuitableTimeSlotsFound()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        // date range is for the year 2020 which is outside the schedule date range i.e. 2023
        var startDate = new DateTime(2020, 12, 01);
        var endDate = new DateTime(2020, 12, 30);

        var results = scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, 15, 2, ScheduleData.AbsentData.Schedules);

        Assert.Empty(results);
    }

    [Fact]
    public void FindMeetingTimeSlots_StartDateEndDateSame_SuitableTimeSlotsFound()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        var startDate = new DateTime(2023, 06, 01);
        var endDate = new DateTime(2023, 06, 01);

        var results = scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, 15, 2, ScheduleData.Data.Schedules);

        Assert.NotEmpty(results);
    }

    [Fact]
    public void FindMeetingTimeSlots_RequiredMinimumParticipantsMoreThanAvailableExperts_NoSuitableTimeSlotsFound()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        var startDate = new DateTime(2023, 06, 01);
        var endDate = new DateTime(2023, 06, 01);

        // total no. of experts 4
        var results = scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, 15, 5, ScheduleData.Data.Schedules);

        Assert.Empty(results);
    }

    [Fact]
    public void FindMeetingTimeSlots_OneParticipant_SuitableTimeSlotsFound()
    {
        var options = new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        var meetingSchedulerIOptions = Options.Create(options);

        var scheduleAnalyzerService = new ScheduleAnalyzerService(meetingSchedulerIOptions);

        var startDate = new DateTime(2023, 06, 01);
        var endDate = new DateTime(2023, 06, 01);

        // total no. of experts as per mock data 4
        var results = scheduleAnalyzerService.FindMeetingTimeslots(startDate, endDate, 15, 1, ScheduleData.Data.Schedules);

        Assert.NotEmpty(results);
    }
}