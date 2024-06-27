using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeetingPlanner.Console;

public class ExpertsScheduleReader
{
    private readonly string _scheduleUrl;
    private readonly HttpClient _httpClient;

    public ExpertsScheduleReader(IConfiguration configuration, HttpClient httpClient)
    {
        _scheduleUrl = configuration["ScheduleUrl"] ?? throw new InvalidDataException("Schedule not found");
        _httpClient = httpClient;
    }

    public async Task<ScheduleResult> ReadAsync(CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(), new DateTimeConverter() }
        };

        var root = await _httpClient.GetFromJsonAsync<Root>(_scheduleUrl, options, cancellationToken);

        if (root is null)
            throw new DataException("Failed to read schedule");


        return root.ScheduleResult;
    }
}
