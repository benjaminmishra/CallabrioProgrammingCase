using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace MeetingPlanner.Console;

public class ExpertsScheduleReader
{
    private readonly string _scheduleUrl;

    public ExpertsScheduleReader(IConfiguration configuration)
    {
        _scheduleUrl = configuration["ScheduleUrl"] ?? throw new InvalidDataException("Schedule not found");
    }

    public ScheduleResult Read(string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(), new DateTimeConverter() }
        };

        var root = JsonSerializer.Deserialize<Root>(json, options) ?? throw new JsonException("Failed to convert");

        return root.ScheduleResult;
    }

    static async Task<string> GetScheduleJsonFromUrl(string url)
    {
        using var client = new HttpClient();
        return await client.GetStringAsync(url);
    }
}
