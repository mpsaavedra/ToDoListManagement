using Bootler.Contracts.Responses;
using Bootler.Contracts.Responses.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Tests.IntegrationTest;

public class AuthTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;
    public AuthTests(IntegrationTestFixture fixture)
    {
        _client = fixture.HttpClient;
    }

    [Fact]
    public async Task AuthTest_SignInUser_ShouldReturnOk()
    {
        var requestUrl = "/api/User/SignIn";

        var response = _client.PostAsJsonAsync(requestUrl, new
        {
            Username = "admin",
            Password = "Admin@123"
        }).Result;

        Assert.True(response.IsSuccessStatusCode, "Failed to authenticate test user.");
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        var result = response.Content.ReadFromJsonAsync<BaseResponse<SignInResponse>>().Result;
        Assert.True(result.Success);
        Assert.Equal(result.StatusCode, HttpStatusCode.OK);
    }
}
