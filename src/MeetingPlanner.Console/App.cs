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
        System.Console.WriteLine("============================================ Meeting Scheduler ===================================================");
        System.Console.WriteLine("Enter the start date for the date range between which you want to find suitable time slots in dd-mm-yyyy format : ");
        var startDateRange = System.Console.ReadLine();

        if (startDateRange is null)
            System.Console.WriteLine("Failed to recognize the date entered");

        DateTime.TryParseExact(startDateRange, "dd-MM-yyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDateTime);

        System.Console.WriteLine("Enter the end date for the date range between which you want to find suitable time slots in dd-mm-yyyy format : ");

        var endDateRange = System.Console.ReadLine();

        if (endDateRange is null)
            System.Console.WriteLine("Failed to recognize the date entered");

        DateTime.TryParseExact(endDateRange, "dd-MM-yyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var enDateTime);

        System.Console.WriteLine("Enter the minimum number of experts that you expect to see at the meeting : ");

        var minNumberOfExpertsInput = System.Console.ReadLine();

        if (minNumberOfExpertsInput is null)
            System.Console.WriteLine("Failed to read the input");

        int.TryParse(minNumberOfExpertsInput, NumberStyles.Number, CultureInfo.InvariantCulture, out var minNumberOfExperts);

        System.Console.WriteLine("Getting experts' schedule .....");

        var scheduleResult = await _scheduleReader.ReadAsync(stoppingToken);

    }


    public void CalculatePotentialTimeSlots(DateTime startDate, DateTime endDate, int minimumNumberOfExpertsRequired)
    {

    }
}