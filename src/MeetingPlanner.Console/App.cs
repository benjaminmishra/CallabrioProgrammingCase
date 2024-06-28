using System.Globalization;

namespace MeetingPlanner.Console;

public class App
{
    private readonly ExpertsScheduleReader _scheduleReader;
    private readonly ScheduleAnalyzerService _analyzerService;

    public App(ExpertsScheduleReader scheduleReader, ScheduleAnalyzerService analyzerService)
    {
        _scheduleReader = scheduleReader;
        _analyzerService = analyzerService;
    }

    public async Task RunAsync(CancellationToken stoppingToken)
    {
        System.Console.WriteLine("Enter the start date for the date range between which you want to find suitable time slots in yyyy-MM-dd format : ");

        if (!DateTime.TryParse(System.Console.ReadLine(), out var startDate))
        {
            System.Console.WriteLine("Failed to recognize the date entered");
            System.Console.WriteLine("\n\n");
            return;
        }

        System.Console.WriteLine("Enter the end date for the date range between which you want to find suitable time slots in yyyy-MM-dd format : ");

        if (!DateTime.TryParse(System.Console.ReadLine(), out var endDate))
        {
            System.Console.WriteLine("Failed to recognize the date entered");
            System.Console.WriteLine("\n\n");
            return;
        }

        if (startDate > endDate)
        {
            System.Console.WriteLine("Start Date cannot be greater than end date.");
            System.Console.WriteLine("\n\n");
            return;
        }

        System.Console.WriteLine("Enter the minimum number of experts that you expect to see at the meeting : ");

        if (!int.TryParse(System.Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture,
                out var minNumberOfExperts))
        {
            System.Console.WriteLine("Failed to read the input");
            System.Console.WriteLine("\n\n");
            return;
        }

        if (minNumberOfExperts < 1)
        {
            System.Console.WriteLine("Minimum number of experts can not be less than 0");
            System.Console.WriteLine("\n\n");
            return;
        }

        System.Console.WriteLine("Getting experts' schedule .....");

        ScheduleResult scheduleResult;

        try
        {
            scheduleResult = await _scheduleReader.ReadAsync(stoppingToken);
        }
        catch (Exception)
        {
            System.Console.WriteLine("Failed to get experts' schedule");
            System.Console.WriteLine("\n\n");
            return;
        }

        System.Console.WriteLine("Analyzing schedule ...");

        // execute as task to keep main thread responsive
        // hardcoding to 15 min time slots as per requirement
        var suitableTimeSlots = await Task.Run(
            () => _analyzerService.FindMeetingTimeslots(
                startDate.Date,
                endDate.Date,
                15,
                minNumberOfExperts,
                scheduleResult.Schedules), stoppingToken);

        if (suitableTimeSlots.Count == 0)
        {
            System.Console.WriteLine("No suitable timeslot found. Try again !!");
            System.Console.WriteLine("\n\n");
            return;
        }

        System.Console.WriteLine("Potential time slots are following :");

        foreach (var timeSlot in suitableTimeSlots)
        {
            System.Console.WriteLine($"Date : {timeSlot.StartTime.Date.ToShortDateString()} - Start Time : {timeSlot.StartTime.ToShortTimeString()} - End Time : {timeSlot.EndTime.ToShortTimeString()}");
        }

        System.Console.WriteLine("\n\n");
    }
}