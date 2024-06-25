using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeetingPlanner.Console;

public class ScheduleService
{
    public ScheduleResult GetScheduleFromJson(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(), new DateTimeConverter() }
        };

        var root = JsonSerializer.Deserialize<Root>(json, options) ?? throw new JsonException("Failed to convert");

        return root.ScheduleResult;
    }

    public List<TimeSlot> FindSuitableTimeSlots(ScheduleResult scheduleResult, int requiredTeamMembers)
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
                    suitableTimeSlots.Add(new TimeSlot { Start = start, End = end });
                }
            }
        }

        return suitableTimeSlots;
    }
}

public class TimeSlot
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
