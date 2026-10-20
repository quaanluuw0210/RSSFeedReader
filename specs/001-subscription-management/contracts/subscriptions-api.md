# Subscriptions API Contract

The API supports only adding and listing subscriptions. Subscription values are inert strings;
the API does not fetch or parse a feed.

## `GET /api/subscriptions`

Returns all subscriptions in insertion order.

**Success response**: `200 OK`, `application/json`

```json
[
  { "url": "https://example.com/feed.xml" },
  { "url": "https://example.com/feed.xml" }
]
```

An empty collection is returned as `[]`. Repeated submissions remain separate array entries.

## `POST /api/subscriptions`

Adds one subscription.

**Request**: `application/json`

```json
{ "url": "https://example.com/feed.xml" }
```

**Success response**: `200 OK`, `application/json`

```json
{ "url": "https://example.com/feed.xml" }
```

The returned URL is the submitted value. No URL syntax or feed-validity check is performed.

**Invalid request**: `400 Bad Request` when the request is missing a URL or its value is empty
or whitespace-only. The collection remains unchanged.

## Cross-origin access

When the API and UI use different local origins, the API allows the configured UI origin only.
The local development defaults documented by the project are API `http://localhost:5151` and
UI `http://localhost:5213`; HTTPS UI origins must also be explicitly configured when used.
