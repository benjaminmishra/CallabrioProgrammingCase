using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MeetingPlanner.Console;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private static readonly Regex DateRegex = new Regex(@"\/Date\((\d+)(?:[+-]\d{4})?\)\/", RegexOptions.Compiled);

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Expected string token");

        var dateString = reader.GetString();

        if (dateString is null)
            throw new JsonException("Date is null");

        var match = DateRegex.Match(dateString);
        if (!match.Success)
            throw new JsonException("Invalid date format");

        var milliseconds = long.Parse(match.Groups[1].Value);
        return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).DateTime;
    }

    // We will never write back to the url , if we do something is wrong, hence exception
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
