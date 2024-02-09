using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Web.Middleware;
namespace CustomMiddleware.Tests;
public class AuthMiddlewareTests : IAsyncLifetime
{
    IHost? host;
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    public async Task InitializeAsync()
    {
        host = await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
            webBuilder
                .UseTestServer()
                .ConfigureServices(services =>
                {
                })
                .Configure(app =>
                {
                    app.UseMiddleware<AuthMiddleware>();
                    app.Run(async context =>
                    {
                        await context.Response.WriteAsync("Authenticated!");
                    });
                });
        })
        .StartAsync();
    }

    [Fact]
    // no credentials -> not authorized
    public async Task MiddlewareTest_NoCredentials_NotAuthorized()
    {
        // deal with null host warnings
        if (host == null)
        {
            throw new InvalidOperationException("Test host is not initialized.");
        }
        var response = await host!.GetTestClient().GetAsync("/");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }

    [Fact]
    // only username -> not authorized
    public async Task MiddlewareTest_OnlyUsername_NotAuthorized()
    {
        // deal with null host warnings
        if (host == null)
        {
            throw new InvalidOperationException("Test host is not initialized.");
        }
        var response = await host!.GetTestClient().GetAsync("/?username=user1");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }

    [Fact]
    // correct credentials -> authorized
    public async Task MiddlewareTest_CorrectCredentials_Authorized()
    {
        // deal with null host warnings
        if (host == null)
        {
            throw new InvalidOperationException("Test host is not initialized.");
        }
        var response = await host!.GetTestClient().GetAsync("/?username=user1&password=password1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Authenticated!", result);
    }

    [Fact]
    // wrong credentials -> not authorized
    public async Task MiddlewareTest_WrongCredentials_NotAuthorized()
    {
        // deal with null host warnings
        if (host == null)
        {
            throw new InvalidOperationException("Test host is not initialized.");
        }
        var response = await host!.GetTestClient().GetAsync("/?username=user5&password=password2");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Not authorized.", result);
    }
}
