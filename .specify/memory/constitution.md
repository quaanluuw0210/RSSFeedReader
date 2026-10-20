<!--
Sync Impact Report
- Version change: unversioned template → 1.0.0 (initial ratification)
- Modified principles: undefined → Security and Safe Data Handling; Maintainable Separation
  of Concerns; Correctness and Code Quality; MVP Scope and Incremental Delivery;
  Cross-Platform Development
- Added sections: Technology and Product Constraints; Engineering Workflow and Quality Gates
- Removed sections: none
- Follow-up TODOs: none
-->

# RSS Feed Reader Constitution

## Core Principles

### I. Security and Safe Data Handling
User-supplied feed URLs and remote feed content MUST be treated as untrusted. In the MVP,
feed URLs are inert subscription data: the application MUST NOT fetch them or render feed
content. Any later feed-fetching feature MUST be explicitly scoped and MUST restrict URL
schemes to HTTP or HTTPS, prevent requests to loopback, private, and link-local destinations
(including after redirects), and enforce timeouts and response-size limits. The UI MUST encode
displayed text and MUST NOT render feed-provided HTML as trusted markup. Secrets MUST NOT be
committed to source control. These controls prevent the feed reader from becoming a path for
server-side request forgery, script injection, or credential exposure.

### II. Maintainable Separation of Concerns
The ASP.NET Core Web API MUST own subscription data and any explicitly approved feed operations;
the Blazor WebAssembly frontend MUST own user interaction and presentation. API contracts and
configuration MUST remain explicit rather than being duplicated or hardcoded across layers.
Implementations MUST prefer the simplest design that meets the approved scope and MUST avoid
adding abstractions, persistence, or background services without a defined requirement. This
keeps the proof of concept easy to understand while preserving a clear path for incremental growth.

### III. Correctness and Code Quality
Changes MUST use clear, idiomatic C# and follow the repository's existing structure and
conventions. Every behavior change MUST have focused automated tests at the appropriate layer;
tests MUST cover the expected behavior and relevant failure cases. Before a change is considered
complete, the affected projects MUST build and their applicable tests MUST pass. Defects MUST be
corrected at their cause rather than hidden by success-shaped fallbacks. These requirements make
the small application reliable and keep future changes safe to review.

### IV. MVP Scope and Incremental Delivery
The MVP is limited to adding a subscription URL and displaying the subscription list, using
in-memory storage. It MUST NOT fetch or parse feeds, persist data, remove subscriptions, or add
other deferred features unless a separate approved specification expands the scope. Work MUST
deliver the smallest complete behavior for the current phase, with later capabilities introduced
incrementally. This preserves the project's proof-of-concept goal and prevents premature complexity.

### V. Cross-Platform Development
The application MUST remain buildable and testable on Windows, macOS, and Linux using supported
.NET tooling. Code and scripts MUST NOT depend on a developer-specific absolute path, shell, or
operating-system behavior when a portable alternative is available. Local API addresses and
frontend origins MUST be configured consistently, and CORS MUST allow only the intended frontend
origins. This supports the documented cross-platform local-development requirement.

## Technology and Product Constraints

- The application uses an ASP.NET Core Web API backend and a Blazor WebAssembly frontend.
- MVP subscription data is held in memory and is lost when the application stops.
- The MVP accepts feed URLs as provided and displays them without network access or feed parsing.
- Feed fetching, parsing, persistence, removal, polling, and rich-content rendering are outside
  the MVP and require an approved scope change before implementation.
- Configuration such as API base addresses MUST be externalized from application code where the
  project provides configuration support. CORS MUST be limited to the configured frontend origins.

## Engineering Workflow and Quality Gates

- Changes MUST be limited to approved requirements; deferred features MUST remain deferred.
- Contributors MUST run the relevant build and automated tests for changed projects and report
  failures explicitly before considering work complete.
- Tests for subscription behavior MUST verify that adding a URL updates the displayed list.
  Network-related tests are required only when feed fetching is explicitly in scope.
- Reviews MUST check scope compliance, safe handling of user and remote data, configuration
  consistency, and adequate tests. Unrelated changes MUST be kept out of the change.
- Local end-to-end verification MUST confirm that the backend is reachable by the frontend and
  that adding a subscription displays it in the UI.

## Governance

This constitution governs product scope, implementation, and review. Amendments MUST be documented
in this file and approved by the project owner before work that depends on the changed rule begins.
Each amendment MUST update the version and last-amended date, and MUST include a Sync Impact Report
for human review; remove that temporary report before committing the amended file.

The constitution uses semantic versioning:

- MAJOR increments for backward-incompatible governance changes, including removing or redefining
  a principle.
- MINOR increments when a principle or governance section is added or materially expanded.
- PATCH increments for clarifications and non-semantic wording changes.

Every implementation change and review MUST check compliance with the principles and quality
gates above. A deviation MUST be identified, justified, and approved in the change description.
Reviews MUST verify that test/build evidence is recorded and that new scope has an approved
specification. When this constitution conflicts with a lower-level project practice, this
constitution takes precedence until it is formally amended.

**Version**: 1.0.0 | **Ratified**: 2026-10-20 | **Last Amended**: 2026-10-20
