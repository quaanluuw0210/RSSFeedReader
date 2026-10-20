using RSSFeedReader.Api.Models;
using RSSFeedReader.Api.Services;

namespace RSSFeedReader.Api.Tests;

public class SubscriptionStoreTests
{
    [Fact]
    public void GetAll_starts_empty()
    {
        var store = new SubscriptionStore();

        Assert.Empty(store.GetAll());
    }

    [Fact]
    public void TryAdd_preserves_values_order_and_duplicates()
    {
        var store = new SubscriptionStore();

        Assert.True(store.TryAdd(" first ", out var first));
        Assert.True(store.TryAdd("second", out var second));
        Assert.True(store.TryAdd(" first ", out var duplicate));

        Assert.Equal(" first ", first!.Url);
        Assert.Equal("second", second!.Url);
        Assert.Equal(" first ", duplicate!.Url);
        Assert.Equal([" first ", "second", " first "], store.GetAll().Select(item => item.Url));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t\r\n")]
    public void TryAdd_rejects_null_empty_and_whitespace(string? url)
    {
        var store = new SubscriptionStore();

        Assert.False(store.TryAdd(url, out var added));

        Assert.Null(added);
        Assert.Empty(store.GetAll());
    }

    [Fact]
    public void GetAll_returns_a_read_only_snapshot()
    {
        var store = new SubscriptionStore();
        store.TryAdd("one", out _);

        var snapshot = store.GetAll();

        Assert.IsAssignableFrom<IList<Subscription>>(snapshot);
        Assert.Throws<NotSupportedException>(() => ((IList<Subscription>)snapshot).Clear());
        Assert.Single(store.GetAll());
    }
}
