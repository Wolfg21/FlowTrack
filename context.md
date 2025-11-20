# FlowTrack Context

## Goals
- Deliver a clear, trustworthy view of personal finances (balances, cash flow, spending) across banks and future providers.
- Let users link financial accounts via an external aggregator while keeping vendor details abstracted behind domain/infrastructure boundaries.
- Enable viewing, tracking, and categorizing expenses with sensible defaults plus manual overrides.
- Keep the core domain/app layer reusable for multiple surfaces (web, mobile, services) without leaking implementation details.
- Use domain events and CQRS patterns to keep workflows extensible (notifications, projections, integrations).

## Product Vision
- Fast onboarding: quick account linking, first sync feedback, clear connection status.
- Insightful overview: recent transactions, category spend, and balances by account/connection.
- User control: users can correct categories and see an audit of changes.
- Reliable data: currency-safe amounts, consistent timestamps (UTC), and idempotent ingestion.

## MVP Scope
- Bank account linking through an aggregator (checking/savings/credit); store connection status, cursors, and tokens in infrastructure.
- Account sync: pull accounts with balances and metadata (names, masks, types/subtypes).
- Transaction sync: pull transactions with amounts, dates, merchant/name, categories, payment channel; support incremental fetches via cursors.
- Categorization: accept provider categories, allow manual overrides, persist chosen category per transaction.
- Views: accounts with balances; transactions with filters (date range, category, account); basic spend summary per category.
- Error handling: surface sync errors and connection statuses; allow retry/relink flows.
- Out of MVP: investment account linking/holdings (e.g., TradeRepublic) and investment performance views.

## Post-MVP Priorities
- Investment account linking/import and holdings valuation.
- Budgeting/goals (envelopes or category budgets) and overspend alerts.
- Enrichment: merchant normalization, duplicate detection, recurring expense detection.
- Multi-currency UX and conversions; exports and sharing.

## Functional Requirements (MVP)
- Users: create users; fetch user profile. Authentication/identity is handled by Clerk (outside the domain); application assumes authenticated user context.
- Bank connections: create connection with provider item/access data; track status, connected time, and transaction cursor; update status/cursor; emit connection-created event.
- Accounts: create per connection with balances, holder info, masks, names, types/subtypes; update balances; emit account-created event.
- Transactions: create per account with provider transaction id, amount (`Money`), date, merchant/name, category, payment channel; allow category updates/overrides.
- Queries: get user, list accounts by user/connection, list transactions with filters, category spend summary (to be added in application layer).
- Commands: create user, create bank connection, sync accounts, sync transactions, update transaction category (to be added as handlers).

## Non-Functional Requirements
- Security: never log secrets/tokens; keep access tokens in infrastructure; least-privilege data access; Clerk manages auth.
- Reliability: idempotent ingestion using provider ids (connection/account/transaction); handle retries and partial failures.
- Observability: structured logs for commands/queries; metrics for sync latency/errors; correlation ids per request.
- Privacy: minimize PII in logs; support deletion/anonymization hooks.
- Performance: efficient batch ingestion; pagination for reads; avoid loading full histories into memory.
- Extensibility: new providers or value objects should not leak into the domain; use abstractions for time/email/sql/off-host services.

## Tech Stack
- Backend runtime: .NET 9.0 / C# with nullable reference types and implicit usings enabled.
- Backend libraries: MediatR 13.1.0 for command/query dispatch; MediatR.Contracts 2.0.1 for domain event contracts; logging via Microsoft.Extensions.Logging.Abstractions 9.0.10.
- Frontend: React + Vite + Tailwind CSS for the web app.
- Authentication: Clerk for user auth/session management (domain assumes identity is resolved upstream).
- Solution layout: `FlowTrack.Domain` and `FlowTrack.Application` projects included in `FlowTrack.sln`.

## Architectural Principles
- DDD-centric layers: Domain holds entities/value objects/events with no external dependencies; Application orchestrates use cases and depends on Domain; infrastructure implementations live outside this solution.
- CQRS-style handlers: commands/queries implement `ICommand`/`IQuery` and return `Result`/`Result<T>` instead of throwing for expected flows.
- Strong typing: identifiers and primitives are records (e.g., `UserId`, `Email`, `Money`, `Currency`) to encode invariants and avoid primitive obsession.
- Domain events first: entities raise events on creation to decouple side effects; publishing happens after unit-of-work commits.
- Persistence boundaries: repositories and `IUnitOfWork` abstractions define data access contracts; application handlers do not depend on concrete stores.
- Cross-cutting via abstractions: time (`IDateTimeProvider`), email (`IEmailService`), SQL connections (`ISqlConnectionFactory`), pipeline logging (`LoggingBehavior<,>`), and auth context (provided by host/Clerk) remain pluggable.
- Minimize uncontrolled mutation: records and private setters guard invariants; updates go through explicit methods (`UpdateStatus`, `UpdateTransactionsCursor`, `SetBalance`).

## Domain Model Snapshot
- Users: immutable first/last name and email; creation emits `UserCreatedDomainEvent`.
- Bank Connections: link a user to a financial institution via provider identifiers, track status, cursor, and connection time; creation emits `BankConnectionCreatedDomainEvent`.
- Accounts: tied to a bank connection with balances, holder info, names, types/subtypes; creation emits `AccountCreatedDomainEvent`.
- Transactions: belong to an account, carry provider transaction id, amount (`Money`), date, merchant/name, category, and payment channel.
- Institutions: store provider institution id, name/branding, supported countries/products; creation emits `InstitutionCreatedDomainEvent`.
- Shared value objects: `Money` with currency-safe arithmetic, `Currency` codes (USD/EUR/GBP/CAD/AUD/JPY/CHF/SEK), `Country`, `DateRange`, `Product`, and category descriptors.

## Provider Object Mapping (Plaid reference, kept provider-agnostic)
- BankConnection ˜ Plaid Item: external item/institution ids, status, and transaction cursor; `access_token` is provider-secret and must not live in the domain.
- Account ˜ Plaid Account: external `account_id`, balances (available/current/limit + currency codes), `mask`, `name`, `official_name`, `type`/`subtype`, optional verification status.
- Transaction ˜ Plaid Transaction: external `transaction_id`, `account_id`, `amount` with `iso_currency_code`/`unofficial_currency_code`, `date`/`authorized_date`, `name`, `merchant_name`, personal finance category (primary/detailed), `payment_channel`, `pending` flag.
- Institution ˜ Plaid Institution: `institution_id`, `name`, `country_codes`, `products`, optional branding (logo/primary/secondary/background colors).
- Sync model: Plaid uses cursors for incremental transaction fetch; domain keeps `TransactionsCursor` on the connection and remains provider-neutral.

## Provider-Agnostic Secrets & External Data Strategy
- Access tokens and other provider secrets stay in infrastructure (e.g., secrets store) keyed by `BankConnectionId`; application layer resolves tokens when calling the aggregator. Domain must not expose/store tokens.
- Domain may hold external reference ids but they should be provider-neutral (e.g., `ExternalConnectionId`/`ExternalAccountId`/`ExternalTransactionId` value objects that pair provider key + external id) to allow swapping aggregators.
- Provider-specific response shapes (verification status, pending vs posted, authorized date) should stay in mappers/adapters; only persist what the domain needs.
- Re-auth/relink flows happen in application/infrastructure; domain only tracks connection status and last cursor.

## Application Layer Notes
- `DependencyInjection.AddApplication` registers MediatR handlers and the logging pipeline behavior.
- Example flows: `CreateUserCommandHandler` builds value objects, persists via repository, commits through `IUnitOfWork`, and returns the new `UserId`; `GetUserQueryHandler` retrieves users and maps to `UserResponse` with graceful not-found handling through `Result.Failure`.
- Command logging behavior logs start/success/failure for requests implementing `IBaseCommand` (commands, not queries).

## Domain Actions / TODOs (provider alignment)
- Remove `AccessToken` from the `BankConnection` entity; tokens belong to application/infrastructure, retrieved via `BankConnectionId` when calling providers.
- Rename provider-specific ids (`PlaidItemId`, `PlaidAccountId`, `PlaidTransactionId`) to provider-neutral external id value objects to enable swapping aggregators without leaking names into the domain.
- Consider adding optional fields for transaction `Pending`/`AuthorizedDate` if required by downstream UX while keeping provider-neutral naming.

## Working Agreements
- Preserve layer boundaries when adding features; do not reference infrastructure concerns from Domain or Application.
- Prefer returning `Result`/`Result<T>` and domain-specific `Error` over throwing exceptions for validation/business failures.
- Add new integration points behind abstractions to keep tests fast and deterministic.
- Keep this document current with scope decisions (MVP vs post-MVP) and update when new commands/queries are added.
