---
description: "Implementation tasks for MVP RSS/Atom subscription management"
---

# Tasks: MVP RSS/Atom Subscription Management

**Input**: Design documents from `specs/001-subscription-management/`

**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, and `contracts/`

**Tests**: Included because the constitution requires focused automated tests and the plan
defines store and API integration tests.

**Organization**: Tasks are grouped by the sole prioritized user story so it can be implemented
and validated as an independently useful MVP increment.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel with other marked tasks in the same phase.
- **[Story]**: User story label for story-specific work.
- Each task identifies the exact project or source file it creates or changes.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Initialize the solution and the planned API, UI, and test projects.

- [X] T001 Create the .NET solution file `RSSFeedReader.sln` at the repository root.
- [X] T002 [P] Create the ASP.NET Core Web API project in `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`.
- [X] T003 [P] Create the Blazor WebAssembly project in `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj`.
- [X] T004 Create the xUnit test project in `backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj` and add its project reference to `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Wire projects together and configure local API/UI connectivity before feature work.

- [X] T005 Add the API, UI, and test projects to `RSSFeedReader.sln`.
- [X] T006 Configure the API launch URL and the UI launch URL in `backend/RSSFeedReader.Api/Properties/launchSettings.json` and `frontend/RSSFeedReader.UI/Properties/launchSettings.json`, using the documented local defaults `http://localhost:5151` and `http://localhost:5213`.
- [X] T007 Configure the API CORS policy in `backend/RSSFeedReader.Api/Program.cs` to allow only the documented local UI origins `http://localhost:5213` and `https://localhost:7025`.

**Checkpoint**: API, UI, and test projects are in the solution and local cross-origin requests can be configured.

---

## Phase 3: User Story 1 - Add and View Subscriptions (Priority: P1) 🎯 MVP

**Goal**: Let a local user submit non-empty subscription values and immediately see all entries
in the list, without feed retrieval or durable storage.

**Independent Test**: Start the app with an empty list; add two values (including a duplicate),
verify both submissions appear in order, verify whitespace-only input adds nothing, and restart
the API to verify the list is empty.

### Tests for User Story 1

> Write the tests first and confirm they fail before implementing the corresponding behavior.

- [X] T008 [P] [US1] Add store unit tests in `backend/RSSFeedReader.Api.Tests/SubscriptionStoreTests.cs` for an initially empty list, preserving input and insertion order, allowing duplicate entries, rejecting whitespace-only values, and returning a snapshot that cannot mutate stored state.
- [X] T009 [P] [US1] Add API integration tests in `backend/RSSFeedReader.Api.Tests/SubscriptionsApiTests.cs` for empty GET, successful POST followed by GET, duplicate retention, and empty/whitespace POST returning `400 Bad Request` without changing the collection.

### Implementation for User Story 1

- [X] T010 [P] [US1] Create the `Subscription` data model with its required URL field in `backend/RSSFeedReader.Api/Models/Subscription.cs`.
- [X] T011 [US1] Implement the process-local ordered subscription store in `backend/RSSFeedReader.Api/Services/SubscriptionStore.cs`; reject only null, empty, or whitespace-only values, preserve other input as entered, allow duplicates, and return read-only snapshots.
- [X] T012 [US1] Register the store as a singleton and expose `GET /api/subscriptions` and `POST /api/subscriptions` with the specified `200 OK` JSON results and `400 Bad Request` validation behavior in `backend/RSSFeedReader.Api/Program.cs`.
- [X] T013 [P] [US1] Set the configured API base address to `http://localhost:5151/api/` in `frontend/RSSFeedReader.UI/wwwroot/appsettings.json`.
- [X] T014 [US1] Implement the configured HTTP client operations for listing and adding subscriptions in `frontend/RSSFeedReader.UI/Services/SubscriptionApiClient.cs`.
- [X] T015 [US1] Implement the subscriptions page in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor`; load existing entries, submit non-whitespace values, preserve duplicate entries, display values as encoded text, and update the list only after a successful API response.

**Checkpoint**: User Story 1 can be demonstrated independently using the local UI and API.

---

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Verify the delivered MVP and keep documentation aligned with the runnable layout.

- [X] T016 Update the runnable project paths, launch instructions, and test commands in `specs/001-subscription-management/quickstart.md` if project defaults differ from the documented values.
- [X] T017 Run the API and UI builds and the API test project defined in `specs/001-subscription-management/quickstart.md`; resolve failures in the affected source files.
- [X] T018 Complete the add/list, duplicate, whitespace, offline-feed, CORS, and API-restart checks in `specs/001-subscription-management/quickstart.md`.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No earlier phase dependency. T002 and T003 can be created in parallel;
  T004 requires the API project from T002.
- **Foundational (Phase 2)**: Depends on Setup completion and blocks User Story 1.
- **User Story 1 (Phase 3)**: Depends on Setup and Foundational completion. This is the only
  user story in the MVP.
- **Polish (Phase 4)**: Depends on the implementation of User Story 1.

### User Story Dependencies

- **User Story 1 (P1)**: No dependency on any other user story; starts after the foundation.

### Within User Story 1

- T008 and T009 are independent test files and may be prepared in parallel after project setup.
- T010 and T013 modify separate files and can be prepared independently.
- T011 depends on T010; T012 depends on T011 and the foundational `Program.cs` CORS setup.
- T014 depends on the API contract in T012 and UI configuration in T013.
- T015 depends on T014.
- T016-T018 require the complete user story.

## Parallel Execution Examples

### Setup

```text
T002 Create the API project in backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj
T003 Create the Blazor project in frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj
```

### User Story 1

```text
T008 Add store tests in backend/RSSFeedReader.Api.Tests/SubscriptionStoreTests.cs
T009 Add API tests in backend/RSSFeedReader.Api.Tests/SubscriptionsApiTests.cs
```

```text
T010 Create the model in backend/RSSFeedReader.Api/Models/Subscription.cs
T013 Configure the API base address in frontend/RSSFeedReader.UI/wwwroot/appsettings.json
```

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Setup and Foundational phases.
2. Write the store and API tests; verify that they fail before implementing behavior.
3. Implement the model, in-memory store, and API operations.
4. Implement the configured UI client and subscriptions page.
5. Run the API tests and builds, then complete the independent add/list checks in `quickstart.md`.
6. Stop after User Story 1; feed fetching, parsing, persistence, removal, and polling remain out
   of scope.

### Parallel Team Strategy

Complete Setup and Foundational phases first. Then independent tests (T008-T009) and separate
model/configuration work (T010 and T013) may proceed in parallel. Store, API, UI client, and page
work proceeds in dependency order.

## Notes

- Every implementation task specifies its target file or project path.
- `[P]` marks only work in separate files/projects with no dependency on unfinished tasks.
- `[US1]` maps each story task to the add-and-view user journey.
- The MVP's temporary state is tied to the API process; closing only the browser does not clear it.
