# Data Model: MVP RSS/Atom Subscription Management

## Subscription

Represents one URL submitted by the user and displayed in the subscription list.

| Field | Type | Required | Rules |
|---|---|---:|---|
| `url` | String | Yes | Must contain at least one non-whitespace character. Otherwise, preserve the submitted value without URL or feed validation. |

### Collection behavior

- The collection starts empty when the API process starts.
- Entries remain available across requests while that process is running.
- Entries are returned in insertion order.
- Duplicate URLs are allowed and represented as separate entries.
- Stopping the API process discards all entries.
- No feed items, metadata, or persistence state are part of this model.

### State transition

`Not present → Added to in-memory collection → Listed on subsequent reads`

An empty or whitespace-only submission does not cause a state transition.
