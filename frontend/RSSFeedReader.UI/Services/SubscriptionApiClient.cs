using System.Net.Http.Json;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsAsync(
        CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<SubscriptionDto[]>(
                "subscriptions",
                cancellationToken)
            ?? throw new InvalidOperationException("The subscriptions API returned no list.");
    }

    public async Task<SubscriptionDto> AddSubscriptionAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "subscriptions",
            new AddSubscriptionRequest(url),
            cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<SubscriptionDto>(
                cancellationToken)
            ?? throw new InvalidOperationException("The subscriptions API returned no subscription.");
    }

    private sealed record AddSubscriptionRequest(string Url);
}

public sealed record SubscriptionDto(string Url);
