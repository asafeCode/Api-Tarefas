using System.Globalization;
using System.Net;
using System.Text.Json;
using Shouldly;
using TarefasCrud.Shared.Exceptions;
using TarefasCrud.Shared.Exceptions.ExceptionsBase;
using UsersModule.Application.UseCases.Token.RefreshToken;
using WebApi.Test.InlineData;

namespace WebApi.Test.Token.RefreshToken;

public class GetNewAccessTokenTest : TarefasCrudClassFixture 
{
    private const string METHOD = "token";

    private readonly string _userRefreshToken;

    public GetNewAccessTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userRefreshToken = factory.GetRefreshToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new CreateNewTokenCommand
        {
            RefreshToken = _userRefreshToken
        };

        var response = await DoPost($"{METHOD}/refresh-token", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("accessToken").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("refreshToken").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        var request = new CreateNewTokenCommand
        {
            RefreshToken = "InvalidRefreshToken"
        };

        var response = await DoPost($"{METHOD}/refresh-token", request, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EXPIRED_SESSION", new CultureInfo(culture));
        
        errors.ShouldHaveSingleItem(); 
        errors.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
    }
}