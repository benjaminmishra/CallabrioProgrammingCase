using MeetingPlanner.Console.Models;
using Microsoft.Extensions.Options;

namespace MeetingPlanner.Console;

public class ScheduleAnalyzerService
{
    private readonly TimeOnly _workHourStartTime;

    public ScheduleAnalyzerService(IOptions<MeetingSchedulerOptions> options)
    {
        _workHourStartTime = options.Value.WorkDayStartTime;
    }

    public IReadOnlyCollection<TimeSlot> FindMeetingTimeslots(
        DateTime startDate,
        DateTime endDate,
        int meetingDurationMinutes,
        int minParticipants,
        List<Schedule> schedules)
    {
        // validations
        if (startDate == DateTime.MinValue)
            throw new ArgumentException("Start Date cannot be minimum value", nameof(startDate));

        if (endDate == DateTime.MinValue)
            throw new ArgumentException("End Date cannot be minimum value", nameof(endDate));

        if (schedules.Count == 0)
            return [];

        if (meetingDurationMinutes < 1)
            throw new ArgumentException("Meeting duration cannot be less than 1 minute", nameof(meetingDurationMinutes));

        if (minParticipants < 1)
            throw new ArgumentException("Meeting participants cannot be less than 1 minute", nameof(minParticipants));

        var suitableTimeSlots = new List<TimeSlot>();

        var meetingDuration = TimeSpan.FromMinutes(meetingDurationMinutes);

        // if min number of required participants greater than the schedule cout i.e. number of experts
        if (minParticipants > schedules.Count)
            return [];

        // As long as the schedule falls on either or in between either of the dates user has passed we can include it
        // Don't consider time here as the user is allowed to enter only dates
        schedules = schedules
            .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
            .ToList();

        // if schedules do not fall in the date range passed, return empty
        if (schedules.Count == 0)
            return [];

        var scheduleStartDateTime = schedules.Min(x => x.Date);
        var scheduleEndDateTime = schedules.Max(x => x.Date);

        // if they fall on the same day , increment the end date by 1
        if (scheduleEndDateTime == scheduleStartDateTime)
            scheduleEndDateTime = scheduleEndDateTime.AddDays(1);

        var slotStart = scheduleStartDateTime;

        while (slotStart.Add(meetingDuration) <= scheduleEndDateTime)
        {
            var slotEnd = slotStart.Add(meetingDuration);
            var availableParticipants = 0;

            foreach (var schedule in schedules)
            {
                // if not within working hours or expert is on full day absence then no need to do any further checks
                if (!IsWithinWorkingHours(slotStart, slotEnd, schedule.Date, schedule.ContractTimeMinutes) || schedule.IsFullDayAbsence)
                    continue;

                var isAvailable = true;
                foreach (var projection in schedule.Projection)
                {
                    var projectionStart = projection.Start;
                    var projectionEnd = projectionStart.AddMinutes(projection.Minutes);

                    if (slotEnd <= projectionStart || slotStart >= projectionEnd)
                        continue;

                    isAvailable = false;
                    break;
                }

                if (isAvailable)
                {
                    availableParticipants++;
                }
            }

            if (availableParticipants >= minParticipants)
            {
                suitableTimeSlots.Add(new TimeSlot(slotStart, slotEnd));
            }

            slotStart = slotStart.AddMinutes(meetingDurationMinutes);
        }

        return suitableTimeSlots;
    }

    private bool IsWithinWorkingHours(DateTime slotStart, DateTime slotEnd, DateTime scheduleDate, int contractTimeMinutes)
    {
        var workDayStart = scheduleDate + _workHourStartTime.ToTimeSpan();

        // if we did not get the work day start time from the config then assume its 9 AM
        if (_workHourStartTime == TimeOnly.MinValue)
            workDayStart = scheduleDate.AddHours(9);

        var workDayEnd = workDayStart.Add(TimeSpan.FromMinutes(contractTimeMinutes));

        return slotStart >= workDayStart && slotEnd <= workDayEnd;
    }
}
