# Research: MVP RSS/Atom Subscription Management

## Decisions

### Backend-owned volatile state

- **Decision**: Keep subscriptions in one process-local store owned by the ASP.NET Core API.
  Use a private ordered list, expose add and snapshot/list operations, and coordinate access
  around mutations and snapshots.
- **Rationale**: The stakeholder architecture assigns data management to the API and explicitly
  chooses in-memory storage for MVP. Returning a snapshot prevents callers from mutating the
  store directly. A short lock keeps concurrent requests from racing while preserving insertion
  order and duplicate entries.
- **Alternatives considered**: Client-only browser state would bypass the documented API
  responsibility. A concurrent collection is unnecessary for this small ordered list.
  Persistence is out of scope.

### Minimal add/list HTTP contract

- **Decision**: Expose `GET /api/subscriptions` and `POST /api/subscriptions`. Return the list
  as an array of URL entries; return the created entry from POST. Use `200 OK` for a successful
  add because entries have no addressable resource identifier. Return `400 Bad Request` for a
  missing, empty, or whitespace-only URL.
- **Rationale**: The contract directly expresses the two MVP operations and supports duplicate
  URLs without inventing an identifier or delete route. Empty state is an empty JSON array.
- **Alternatives considered**: `201 Created` is appropriate if a stable resource URI is added
  later; no such URI is needed for the current feature. URL-format and feed-validity checks
  conflict with MVP scope.

### Blazor interaction

- **Decision**: Load the current list when the subscriptions page opens. On submission, reject
  whitespace-only input locally for immediate feedback, POST the non-empty value, and update
  the displayed list from the successful response. Keep server-side validation authoritative.
  Obtain the API base address from application configuration.
- **Rationale**: This keeps the backend as the source of truth, gives immediate visible results,
  and preserves the documented separation of UI and data responsibilities.
- **Alternatives considered**: Optimistically appending before the API confirms success risks
  showing state that the backend did not accept. Hardcoding an API origin conflicts with the
  project configuration guidance.

### Focused validation

- **Decision**: Use the .NET test framework selected by the project (xUnit for the initial
  project), with unit coverage for the store and an in-process ASP.NET Core integration test for
  the request/serialization path. Verify the whole UI-to-API flow with the local browser
  quickstart.
- **Rationale**: This tests state behavior, the actual HTTP contract, and the user journey
  without requiring a database or feed network. The repository has no existing application or
  test framework to preserve.
- **Alternatives considered**: Unit tests alone would miss endpoint routing and serialization.
  A browser automation framework or component test framework adds setup for this simple form;
  add one only if an established browser-test harness is introduced.

## Boundaries and Clarifications

- The in-memory store lasts for the **API process**. To verify that data is cleared on
  application restart, stop and restart the API; merely closing a browser tab while the API
  remains running does not clear backend state.
- “No network connection” in the MVP success criteria means no external feed/internet access is
  required. The browser must still reach the local API to add and list entries.
- The user-provided URL value is preserved as entered if it is not whitespace-only; URL syntax
  is not validated.
- No unresolved technical clarifications remain for planning.

## Source References

- `StakeholderDocuments/ProjectGoals.md` — MVP scope, local single-user use, in-memory state,
  and deferred capabilities.
- `StakeholderDocuments/TechStack.md` — ASP.NET Core API, Blazor WebAssembly, responsibilities,
  local ports, configuration, and CORS guidance.
- `specs/001-subscription-management/spec.md` — user scenarios, functional requirements, and
  success criteria.
- Microsoft Learn, [ASP.NET Core dependency injection service lifetimes](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0#service-lifetimes)
  and [minimal API responses](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-10.0#responses).
- Microsoft Learn, [Call a web API from Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-10.0#get-from-json-getfromjsonasync)
  and [ASP.NET Core integration tests](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0).
