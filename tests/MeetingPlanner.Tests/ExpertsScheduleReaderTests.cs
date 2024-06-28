using MeetingPlanner.Console;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MeetingPlanner.Tests;

[Category("Unit Tests")]
public class ExpertsScheduleReaderTests
{
    [Fact]
    public async Task ReadAsync_ReturnsScheduleResult_OnSuccess()
    {
        // Arrange
        var scheduleUrl = "http://example.com/schedule";
        var expectedScheduleResult = new ScheduleResult();
        var root = new Root { ScheduleResult = expectedScheduleResult };

        var options = new MeetingSchedulerOptions { ScheduleUrl = scheduleUrl };
        var meetingSchedulerIOptions = Options.Create(options);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(root)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var reader = new ExpertsScheduleReader(meetingSchedulerIOptions, httpClient);

        // Act
        var result = await reader.ReadAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedScheduleResult.Schedules, result.Schedules);
    }

    [Fact]
    public async Task ReadAsync_ThrowsDataException_OnBadJson()
    {
        // Arrange
        var scheduleUrl = "http://example.com/schedule";
        var options = new MeetingSchedulerOptions { ScheduleUrl = scheduleUrl };
        var meetingSchedulerIOptions = Options.Create(options);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ invalid json }")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var reader = new ExpertsScheduleReader(meetingSchedulerIOptions, httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(() => reader.ReadAsync(CancellationToken.None));
    }

    [Fact]
    public async Task ReadAsync_ThrowsArgumentException_WhenUrlIsNotSet()
    {
        // Arrange
        var options = new MeetingSchedulerOptions { ScheduleUrl = string.Empty };
        var meetingSchedulerIOptions = Options.Create(options);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var reader = new ExpertsScheduleReader(meetingSchedulerIOptions, httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => reader.ReadAsync(CancellationToken.None));
    }
}
