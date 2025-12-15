using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Dtos;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Domain.Dtos;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.RoutineTask.GetTasks;

public class GetTasksTest : TarefasCrudClassFixture
{
    private const string METHOD = "tasks";

    private readonly Guid _userId;

    public GetTasksTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var filter = new FilterTasksDto
        {
            IsCompleted = false
        };

        var query = FilterTasksDtoBuilder.BuildQuery(filter);
        var url = $"{METHOD}/{query}";
        
        // Act
        var response = await DoGet(url, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var tasks = responseData.RootElement.GetProperty("tasks");

        tasks.GetArrayLength().ShouldBeGreaterThan(0);

        foreach (var task in tasks.EnumerateArray())
        {
            task.GetProperty("isCompleted").GetBoolean().ShouldBe(false);
        }
    }
}