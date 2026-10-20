using RSSFeedReader.Api.Models;

namespace RSSFeedReader.Api.Services;

public sealed class SubscriptionStore
{
    private readonly object _sync = new();
    private readonly List<Subscription> _subscriptions = [];

    public bool TryAdd(string? url, out Subscription? subscription)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            subscription = null;
            return false;
        }

        subscription = new Subscription(url);

        lock (_sync)
        {
            _subscriptions.Add(subscription);
        }

        return true;
    }

    public IReadOnlyList<Subscription> GetAll()
    {
        lock (_sync)
        {
            return Array.AsReadOnly(_subscriptions.ToArray());
        }
    }
}
