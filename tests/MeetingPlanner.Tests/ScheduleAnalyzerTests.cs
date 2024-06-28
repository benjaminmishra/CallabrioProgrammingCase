using MeetingPlanner.Console;
using Microsoft.Extensions.Options;
using System.ComponentModel;

namespace MeetingPlanner.Tests
{
    [Category("Unit Tests")]
    public class ScheduleAnalyzerTests
    {
        private MeetingSchedulerOptions CreateOptions()
        {
            return new MeetingSchedulerOptions { WorkDayStartTime = TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)) };
        }

        private ScheduleAnalyzerService CreateService()
        {
            var options = Options.Create(CreateOptions());
            return new ScheduleAnalyzerService(options);
        }

        [Fact]
        public void FindMeetingTimeSlots_DateRangeOutsideOfScheduleData_ZeroSuitableTimeSlotsFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2020, 06, 01);
            var endDate = new DateTime(2020, 06, 01);

            var results = service.FindMeetingTimeslots(startDate, endDate, 15, 2, ScheduleData.Data.Schedules);

            Assert.Empty(results);
        }

        [Fact]
        public void FindMeetingTimeSlots_MeetingDurationLessThanZero_ThrowsArgumentException()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 01);

            Assert.Throws<ArgumentException>(() => service.FindMeetingTimeslots(startDate, endDate, -1, 2, ScheduleData.Data.Schedules));
        }

        [Fact]
        public void FindMeetingTimeSlots_MinParticipantsLessThanZero_ThrowsArgumentException()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 01);

            Assert.Throws<ArgumentException>(() => service.FindMeetingTimeslots(startDate, endDate, 15, -1, ScheduleData.Data.Schedules));
        }

        [Fact]
        public void FindMeetingTimeSlots_NoScheduleData_ZeroSuitableTimeSlotsFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 01);

            var results = service.FindMeetingTimeslots(startDate, endDate, 15, 2, []);

            Assert.Empty(results);
        }

        [Fact]
        public void FindMeetingTimeSlots_EveryoneIsAbsentAllDay_ZeroSuitableTimeSlotsFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2020, 12, 01);
            var endDate = new DateTime(2020, 12, 30);

            var results = service.FindMeetingTimeslots(startDate, endDate, 15, 2, ScheduleData.AbsentData.Schedules);

            Assert.Empty(results);
        }

        [Fact]
        public void FindMeetingTimeSlots_StartDateEndDateSame_SuitableTimeSlotsFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 01);

            var results = service.FindMeetingTimeslots(startDate, endDate, 15, 2, ScheduleData.Data.Schedules);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void FindMeetingTimeSlots_RequiredMinimumParticipantsMoreThanAvailableExperts_NoSuitableTimeSlotsFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 01);

            var results = service.FindMeetingTimeslots(startDate, endDate, 15, 5, ScheduleData.Data.Schedules);

            Assert.Empty(results);
        }

        [Fact]
        public void FindMeetingTimeSlots_OneParticipant_SuitableTimeSlotsFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 01);

            var results = service.FindMeetingTimeslots(startDate, endDate, 15, 1, ScheduleData.Data.Schedules);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void FindMeetingTimeSlots_ValidMeetingSlotFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 02);

            var results = service.FindMeetingTimeslots(startDate, endDate, 30, 2, ScheduleData.Data.Schedules);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void FindMeetingTimeSlots_MeetingDurationExceedsAvailableTime_ZeroSuitableTimeSlotsFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 01);

            var results = service.FindMeetingTimeslots(startDate, endDate, 600, 2, ScheduleData.Data.Schedules);

            Assert.Empty(results);
        }

        [Fact]
        public void FindMeetingTimeSlots_MultipleDayRange_SuitableTimeSlotsFound()
        {
            var service = CreateService();
            var startDate = new DateTime(2023, 06, 01);
            var endDate = new DateTime(2023, 06, 05);

            var results = service.FindMeetingTimeslots(startDate, endDate, 15, 2, ScheduleData.Data.Schedules);

            Assert.NotEmpty(results);
        }
    }
}
