using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RSSFeedReader.Api.Tests;

public class SubscriptionsApiTests
{
    [Fact]
    public async Task Get_returns_empty_list_then_posted_subscriptions_in_order()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var initial = await client.GetFromJsonAsync<SubscriptionResponse[]>("/api/subscriptions");
        Assert.Empty(initial!);

        var firstResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url = "one" });
        var secondResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url = "two" });
        var duplicateResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url = "one" });

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, duplicateResponse.StatusCode);
        Assert.Equal("one", (await firstResponse.Content.ReadFromJsonAsync<SubscriptionResponse>())!.Url);

        var subscriptions = await client.GetFromJsonAsync<SubscriptionResponse[]>("/api/subscriptions");

        Assert.Equal(["one", "two", "one"], subscriptions!.Select(subscription => subscription.Url));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t ")]
    public async Task Post_rejects_missing_or_whitespace_url_without_changing_list(string? url)
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/subscriptions", new { url });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Empty(await client.GetFromJsonAsync<SubscriptionResponse[]>("/api/subscriptions") ?? []);
    }

    [Fact]
    public async Task Post_rejects_request_without_url_property()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        using var content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/subscriptions", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Empty(await client.GetFromJsonAsync<SubscriptionResponse[]>("/api/subscriptions") ?? []);
    }

    private sealed record SubscriptionResponse(string Url);
}
