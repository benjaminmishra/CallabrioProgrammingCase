using Microsoft.Extensions.Options;
using System.Data;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeetingPlanner.Console;

public class ExpertsScheduleReader
{
    private readonly string _scheduleUrl;
    private readonly HttpClient _httpClient;

    public ExpertsScheduleReader(IOptions<MeetingSchedulerOptions> options, HttpClient httpClient)
    {
        _scheduleUrl = options.Value.ScheduleUrl;
        _httpClient = httpClient;
    }

    public async Task<ScheduleResult> ReadAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_scheduleUrl))
            throw new ArgumentException("Schedule url not set", nameof(_scheduleUrl));

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(), new DateTimeConverter() }
        };

        var root = await _httpClient.GetFromJsonAsync<Root>(_scheduleUrl, options, cancellationToken);

        if (root is null)
            throw new DataException("Failed to get schedule");

        return root.ScheduleResult;
    }
}
