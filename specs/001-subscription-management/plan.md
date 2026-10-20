# Implementation Plan: MVP RSS/Atom Subscription Management

**Branch**: `001-subscription-management` | **Date**: 2026-10-20 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `specs/001-subscription-management/spec.md`

## Summary

Deliver the single-user MVP flow to add feed URL subscriptions and display the current list.
Use an ASP.NET Core Web API as the source of truth, a process-local in-memory store, and a
Blazor WebAssembly page that calls the API. Do not fetch or parse feeds or add persistence.

## Technical Context

**Language/Version**: C# 14 / .NET 10 LTS

**Primary Dependencies**: ASP.NET Core Web API, Blazor WebAssembly, `HttpClient`; xUnit and
`Microsoft.AspNetCore.Mvc.Testing` for focused automated tests

**Storage**: In-memory singleton subscription store; state lasts only for the API process

**Testing**: xUnit unit tests for store behavior and ASP.NET Core in-process API integration
tests; browser-based end-to-end verification per [quickstart.md](./quickstart.md)

**Target Platform**: Windows, macOS, and Linux local development; modern browser with access to
the local API

**Project Type**: Web application (ASP.NET Core API + Blazor WebAssembly frontend)

**Performance Goals**: A submitted subscription is visible in the list immediately after the
API confirms the addition; no throughput target is required for the single-user POC

**Constraints**: Single local user; additions and listing only; reject empty/whitespace values,
otherwise preserve input and allow duplicates; no feed network access, parsing, persistence,
removal, or background work; API and UI origins must be coordinated for CORS

**Scale/Scope**: One local user, one subscription-management page, and two API operations

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Security and Safe Data Handling**: PASS. Subscription values remain inert data; this plan
  performs no feed requests and renders values as ordinary encoded text.
- **Maintainable Separation of Concerns**: PASS. The API owns the list; the UI owns the form
  and presentation. No persistence or unnecessary abstraction is introduced.
- **Correctness and Code Quality**: PASS. Store and API contract behavior have focused automated
  tests; the full user flow has a documented local browser verification.
- **MVP Scope and Incremental Delivery**: PASS. Scope is restricted to adding and listing
  subscriptions, with volatile in-memory state.
- **Cross-Platform Development**: PASS. The stack and validation use supported .NET tooling and
  documented local API/UI origins on Windows, macOS, and Linux.

No constitution violations or complexity exceptions are required.

## Project Structure

### Documentation (this feature)

```text
specs/001-subscription-management/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── subscriptions-api.md
└── tasks.md
```

### Source Code (repository root)

The repository currently contains planning artifacts and stakeholder documents, but no
application source projects. The following is the planned MVP layout, based on
`StakeholderDocuments/TechStack.md`:

```text
backend/
├── RSSFeedReader.Api/
│   ├── Program.cs
│   ├── Endpoints/
│   │   └── SubscriptionsEndpoints.cs
│   └── Services/
│       └── SubscriptionStore.cs
└── RSSFeedReader.Api.Tests/
    ├── SubscriptionStoreTests.cs
    └── SubscriptionsApiTests.cs

frontend/
└── RSSFeedReader.UI/
    ├── Pages/
    │   └── Subscriptions.razor
    ├── Services/
    │   └── SubscriptionApiClient.cs
    └── wwwroot/
        └── appsettings.json
```

**Structure Decision**: Use the API and UI project locations and responsibilities already
specified by the stakeholder technology document. Keep the in-memory store in the backend and
the API client/page in the frontend. Add only the backend test project needed to verify store
and request-pipeline behavior; validate the simple form/list journey in a browser using the
quickstart steps rather than adding a separate component-testing framework.

## Phase 0: Outline & Research

Completed in [research.md](./research.md). Decisions cover the process-local store, minimal
HTTP contract, client interaction, and smallest useful validation set. No unresolved technical
clarifications remain.

## Phase 1: Design & Contracts

- [data-model.md](./data-model.md) defines the in-memory subscription entity and its rules.
- [contracts/subscriptions-api.md](./contracts/subscriptions-api.md) specifies the add/list
  HTTP interface and error response.
- [quickstart.md](./quickstart.md) documents build, test, launch, and end-to-end verification.

### Post-Design Constitution Check

- **Scope and simplicity**: PASS. The design uses one in-memory service and two API operations,
  with no production-only components.
- **Data safety**: PASS. The UI displays escaped URL text, and the MVP makes no outbound feed
  requests.
- **Testability**: PASS. Store and API behavior are covered by automated tests; the browser
  journey is verified using the local quickstart.
- **Cross-platform configuration**: PASS. The API and UI use documented local ports and an
  explicit CORS allow-list; external feed connectivity is not required.

No violations were introduced by the design.

## Complexity Tracking

No constitution violations; no complexity exceptions.
