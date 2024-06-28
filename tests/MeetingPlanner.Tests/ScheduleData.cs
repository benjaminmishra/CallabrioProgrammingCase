namespace MeetingPlanner.Tests
{
    public static class ScheduleData
    {
        public static ScheduleResult Data => new()
        {
            Schedules =
            [
                new Schedule
                {
                    ContractTimeMinutes = 480,
                    Date = new DateTime(2023, 06, 01),
                    IsFullDayAbsence = false,
                    Name = "John Doe",
                    PersonId = Guid.NewGuid().ToString(),
                    Projection =
                    [
                        new Projection
                        {
                            Color = "#1E90FF", Description = "Task A", Start = new DateTime(2023, 06, 01, 8, 0, 0),
                            Minutes = 120
                        },
                        new Projection
                        {
                            Color = "#FF0000", Description = "Break", Start = new DateTime(2023, 06, 01, 10, 0, 0),
                            Minutes = 15
                        },
                        new Projection
                        {
                            Color = "#1E90FF", Description = "Task B", Start = new DateTime(2023, 06, 01, 10, 15, 0),
                            Minutes = 105
                        },
                        new Projection
                        {
                            Color = "#FFFF00", Description = "Lunch", Start = new DateTime(2023, 06, 01, 12, 0, 0),
                            Minutes = 60
                        },
                        new Projection
                        {
                            Color = "#1E90FF", Description = "Task C", Start = new DateTime(2023, 06, 01, 13, 0, 0),
                            Minutes = 120
                        },
                        new Projection
                        {
                            Color = "#FF0000", Description = "Break", Start = new DateTime(2023, 06, 01, 15, 0, 0),
                            Minutes = 15
                        },
                        new Projection
                        {
                            Color = "#1E90FF", Description = "Task D", Start = new DateTime(2023, 06, 01, 15, 15, 0),
                            Minutes = 105
                        }
                    ]
                },

                new Schedule
                {
                    ContractTimeMinutes = 480,
                    Date = new DateTime(2023, 06, 01),
                    IsFullDayAbsence = false,
                    Name = "Jane Smith",
                    PersonId = Guid.NewGuid().ToString(),
                    Projection =
                    [
                        new Projection
                        {
                            Color = "#80FF80", Description = "Meeting", Start = new DateTime(2023, 06, 01, 9, 0, 0),
                            Minutes = 90
                        },
                        new Projection
                        {
                            Color = "#FF0000", Description = "Break", Start = new DateTime(2023, 06, 01, 10, 30, 0),
                            Minutes = 15
                        },
                        new Projection
                        {
                            Color = "#80FF80", Description = "Meeting", Start = new DateTime(2023, 06, 01, 10, 45, 0),
                            Minutes = 120
                        },
                        new Projection
                        {
                            Color = "#FFFF00", Description = "Lunch", Start = new DateTime(2023, 06, 01, 12, 45, 0),
                            Minutes = 60
                        },
                        new Projection
                        {
                            Color = "#80FF80", Description = "Meeting", Start = new DateTime(2023, 06, 01, 13, 45, 0),
                            Minutes = 90
                        },
                        new Projection
                        {
                            Color = "#FF0000", Description = "Break", Start = new DateTime(2023, 06, 01, 15, 15, 0),
                            Minutes = 15
                        },
                        new Projection
                        {
                            Color = "#80FF80", Description = "Meeting", Start = new DateTime(2023, 06, 01, 15, 30, 0),
                            Minutes = 120
                        }
                    ]
                },

                new Schedule
                {
                    ContractTimeMinutes = 0,
                    Date = new DateTime(2023, 06, 01),
                    IsFullDayAbsence = true,
                    Name = "Mark Johnson",
                    PersonId = new Guid().ToString(),
                    Projection = [],
                },

                new Schedule
                {
                    ContractTimeMinutes = 480,
                    Date = new DateTime(2023, 06, 01),
                    IsFullDayAbsence = false,
                    Name = "Emily Brown",
                    PersonId = new Guid().ToString(),
                    Projection =
                    [
                        new Projection
                        {
                            Color = "#80FF80", Description = "Research", Start = new DateTime(2023, 06, 01, 8, 30, 0),
                            Minutes = 120
                        },
                        new Projection
                        {
                            Color = "#FF0000", Description = "Break", Start = new DateTime(2023, 06, 01, 10, 30, 0),
                            Minutes = 15
                        },
                        new Projection
                        {
                            Color = "#80FF80", Description = "Research", Start = new DateTime(2023, 06, 01, 10, 45, 0),
                            Minutes = 90
                        },
                        new Projection
                        {
                            Color = "#FFFF00", Description = "Lunch", Start = new DateTime(2023, 06, 01, 12, 15, 0),
                            Minutes = 60
                        },
                        new Projection
                        {
                            Color = "#80FF80", Description = "Research", Start = new DateTime(2023, 06, 01, 13, 15, 0),
                            Minutes = 120
                        },
                        new Projection
                        {
                            Color = "#FF0000", Description = "Break", Start = new DateTime(2023, 06, 01, 15, 15, 0),
                            Minutes = 15
                        },
                        new Projection
                        {
                            Color = "#80FF80", Description = "Research", Start = new DateTime(2023, 06, 01, 15, 30, 0),
                            Minutes = 90
                        }
                    ]
                }
            ]
        };
    }
}
