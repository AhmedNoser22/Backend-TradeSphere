# TradeSphere — Backend

TradeSphere is an **ERP (Enterprise Resource Planning) system** for an import & distribution company. It tracks the full lifecycle of goods — from the moment a purchase is agreed with a supplier, through shipping, customs clearance, quality inspection, and warehousing, to the sale and delivery to a customer, and the financial settlement on both ends.

This repository contains the **backend only** (.NET 10 / ASP.NET Core Web API). It is built with **Clean Architecture**, **CQRS** (via MediatR), and **DDD-flavored building blocks** (rich domain entities, domain events, value objects).

---

## Table of contents

- [The business, in one paragraph](#the-business-in-one-paragraph)
- [The full goods journey](#the-full-goods-journey)
- [Roles](#roles)
- [Architecture](#architecture)
- [Tech stack](#tech-stack)
- [Solution structure](#solution-structure)
- [Design decisions & patterns](#design-decisions--patterns)
- [Feature modules](#feature-modules)
- [How modules talk to each other: Domain Events](#how-modules-talk-to-each-other-domain-events)
- [Request lifecycle, step by step](#request-lifecycle-step-by-step)
- [Authentication & Authorization](#authentication--authorization)
- [Getting started](#getting-started)
- [API surface](#api-surface)
- [Background jobs](#background-jobs)
- [Known simplifications](#known-simplifications)

---

## The business, in one paragraph

TradeSphere is an internal ERP for an import/distribution company: **Buy → Ship → Clear customs → Inspect → Warehouse → Sell → Collect money → Report.** A supplier is not a system user — it's just a record describing a real-world company the negotiation with them (email, calls, WhatsApp) happens outside the system entirely. Every step in the journey has its own status lifecycle, and goods are never allowed into the warehouse until they've passed **both** customs clearance **and** quality inspection.

---

## The full goods journey

```
Supplier (data record only)
    │
    ▼
Purchase Order (Draft → add Lines → Confirm)
    │  Confirm ──────────────► Payment opened automatically (owed to supplier)
    ▼
Shipment (Preparing → Shipped → InTransit → Arrived)
    │  Arrived ──────────────► Customs Clearance opened automatically
    ▼
Customs Clearance (PendingDocuments → UnderClearance → Cleared / Rejected)
    │  (manual: File Declaration → Clear)
    ▼
Quality Inspection (record Accepted/Rejected/Missing per product → Complete)
    │  Complete ─────────────► Inventory receives accepted stock automatically
    │                      ► Purchase Order closed automatically (if fully shipped)
    ▼
Inventory (stock on hand, full movement audit trail)
    │
    ▼
Sales Order (Draft → add Lines → Confirm → Deliver)
    │  Deliver ──────────────► Stock deducted (in the same operation)
    │                      ► Payment opened automatically (owed by customer)
    ▼
Payment (installments recorded against either direction)
    │
    ▼
Dashboard (cross-module reporting)
```

**Golden rule:** goods never enter inventory until they've cleared **both** customs **and** quality inspection.

---

## Roles

| Role | Responsible for |
|---|---|
| SystemAdministrator | Users, roles, system configuration |
| ProcurementOfficer | Suppliers, Purchase Orders |
| LogisticsOfficer | Shipments |
| CustomsClearanceOfficer | Customs Clearance |
| QualityControlOfficer | Quality Inspections |
| WarehouseOfficer | Inventory |
| SalesOfficer | Customers, Sales Orders |
| FinanceOfficer | Payments |
| OperationsManager | Day-to-day oversight across every module, resolves exceptions |
| GeneralManager | Read-only visibility across everything via the Dashboard |

A single user account holds exactly one role at a time. Self-registration (`POST /api/auth/register`) always assigns a fixed default role (`SalesOfficer`) — only an existing SystemAdministrator can place a user in any other role, via the Users management endpoints. The system protects itself from ever being left with zero active administrators.

---

## Architecture

Clean Architecture, five layers, each a separate project, with a strict dependency direction (arrows point toward Domain):

```
TradeSphere.Api  ──────►  TradeSphere.Application  ──────►  TradeSphere.Domain
      │                          ▲
      ▼                          │
TradeSphere.Persistence  ────────┤
TradeSphere.Infrastructure ──────┘
```

- **Domain** depends on nothing — zero NuGet packages, not even MediatR. It contains the actual business rules.
- **Application** depends on Domain only. It orchestrates *what* should happen for each use case (CQRS commands/queries), but knows nothing about databases, SMTP, or HTTP.
- **Persistence** implements Application's data-access contracts using EF Core + SQL Server. This is the only layer that knows a database exists.
- **Infrastructure** implements Application's contracts for everything external: SMTP email, JWT tokens, password hashing, background jobs, file storage.
- **Api** is the entry point: ASP.NET Core controllers, middleware, and `Program.cs`, which wires all the other layers together.

Each layer that references a package lists them in its own `.csproj`, with a `GlobalUsings.cs` file collecting the `using` statements the layer needs — so individual files stay focused on logic.

---

## Tech stack

| Concern | Choice |
|---|---|
| Runtime | .NET 10 / ASP.NET Core Web API |
| ORM | Entity Framework Core 10 + SQL Server |
| CQRS / mediator | MediatR |
| Validation | FluentValidation (as a MediatR pipeline behavior) |
| Object mapping | Mapster (compiled-expression mapping, faster than reflection-based mappers) |
| Auth | Custom JWT (access + rotating refresh tokens), `PasswordHasher<T>` for hashing — **not** full ASP.NET Core Identity, by design (see below) |
| Email | MailKit over SMTP |
| Background jobs | Hangfire (SQL Server storage) |
| Logging | Serilog (console + rolling file) |
| API docs | Swagger / Swashbuckle |

**Why not ASP.NET Core Identity?** Identity brings a full, opinionated membership system (its own tables, its own conventions) that's hard to bend to a custom shape. Since the business needed a specific, simple user model tied to fixed ERP roles, a hand-rolled `User` entity (using only Identity's `PasswordHasher<T>` utility for battle-tested password hashing) gave full control with far less ceremony.

---

## Solution structure

```
src/
  TradeSphere.Domain/
    Entities/            → Supplier, Product, PurchaseOrder(+Line), Shipment,
                            CustomsClearance, QualityInspection(+Line),
                            InventoryStock, InventoryMovement, Customer,
                            SalesOrder(+Line), Payment, User, RefreshToken
    Enums/                → status values for every workflow (PurchaseOrderStatus, etc.)
    ValueObjects/         → Money, Address, ContactInfo
    Events/               → domain events raised by entities
    Exceptions/           → DomainException, BusinessRuleViolationException, InvalidStateTransitionException
    Specifications/       → reusable, named query filters
    Common/               → BaseEntity, AuditableEntity, BusinessConstants

  TradeSphere.Application/
    Common/
      Interfaces/          → contracts Persistence/Infrastructure implement
      Behaviors/            → MediatR pipeline: exception handling, logging, validation, transaction/unit-of-work
      Models/               → Result, Result<T>, PaginatedList<T>, DomainEventNotification<T>
      Exceptions/            → ValidationException, NotFoundException
    Features/
      Auth/ Users/ Suppliers/ Products/ PurchaseOrders/ Shipments/
      CustomsClearance/ QualityInspections/ Inventory/ Customers/
      SalesOrders/ Payments/ Dashboard/
      (each: Commands/Queries + Validators + Handlers + EventHandlers, grouped by feature)

  TradeSphere.Persistence/
    Context/               → ApplicationDbContext (EF Core, dispatches domain events after save)
    Configurations/         → one file per entity (EF Core Fluent API mapping)
    Migrations/
    Repositories/            → generic Repository<T>

  TradeSphere.Infrastructure/
    Email/                  → SmtpEmailService (MailKit)
    Identity/                → JwtTokenService, PasswordHasherService, CurrentUserService
    BackgroundJobs/          → Hangfire job definitions
    FileStorage/             → local file storage (swappable later)

  TradeSphere.Api/
    Controllers/            → one per feature module
    Middlewares/             → GlobalExceptionMiddleware
    Filters/                 → RequireRoleAttribute
    Program.cs
```

---

## Design decisions & patterns

- **CQRS via MediatR** — every use case is a `Command` (write) or `Query` (read), each with its own `Handler`. Controllers only translate HTTP → Command/Query and back; they contain no business logic.
- **Pipeline behaviors**, applied in order to every request: `UnhandledExceptionBehavior` → `LoggingBehavior` → `ValidationBehavior` → `TransactionBehavior`. Validation stops a request before it ever reaches a handler; the transaction behavior calls `SaveChangesAsync` once, after a command handler finishes, implementing the *Unit of Work* half of Repository + Unit of Work.
- **Result pattern** — expected failures (an email already in use, not enough stock) are returned as `Result.Failure(...)`, not thrown as exceptions. Exceptions are reserved for genuinely unexpected situations (`NotFoundException`) or for entity-level rule violations that read naturally as "you broke a rule" (`BusinessRuleViolationException`, `InvalidStateTransitionException`).
- **Self-governing entities (state machines)** — an entity controls its own status transitions. `Shipment.MarkAsArrived()` refuses to run unless the shipment is currently `InTransit`; a handler can never force an invalid jump.
- **Specification pattern** — reusable, named filters (`LowStockProductsSpecification`, `UserByEmailSpecification`) defined once in Domain, executed by the generic `Repository<T>` in Persistence.
- **Repository, used only for single-aggregate reads/writes.** List/report queries that need pagination, joins, or aggregates (`SUM`, `COUNT`) go through `IApplicationDbContext` directly in the query handler — a repository abstraction adds no value there and would hide EF Core's translation to efficient SQL.
- **`AnyAsync` / `FirstOrDefaultAsync` over `ListAsync(...).Count`/`.FirstOrDefault()`** — existence checks translate to a SQL `EXISTS`, not a full row fetch.
- **Domain Events for cross-module reactions**, direct in-handler calls for same-transaction operations — see the dedicated section below.
- **Value Objects** (`Money`, `Address`, `ContactInfo`) for data with no independent identity, stored as owned types (extra columns on the parent table, not separate tables).

---

## Feature modules

Every module below follows the same shape: `Command`/`Query` → `Validator` → `Handler` → (for writes) a call into the Domain entity → a Controller action.

| Module | Responsibility |
|---|---|
| **Auth** | Register, confirm email (SMTP code), resend code, login, JWT refresh-token rotation, forgot/reset password |
| **Users** | Admin-only: list users, create a user directly in any role, change a user's role, activate/deactivate |
| **Suppliers** | CRUD for supplier records (not system users — see below) |
| **Products** | CRUD for the product catalog |
| **Purchase Orders** | Create (Draft), add lines, Confirm, Cancel, list/search, details |
| **Shipments** | Create against a confirmed order, advance status Shipped → InTransit → Arrived |
| **Customs Clearance** | Opened automatically on shipment arrival; file declaration, Clear, Reject, list pending |
| **Quality Inspections** | Create against a cleared shipment, record per-product accepted/rejected/missing, Complete |
| **Inventory** | Stock received automatically after inspection; manual adjustments (with reason); low-stock report |
| **Customers** | CRUD for customer records |
| **Sales Orders** | Create (Draft, stock-checked), add lines, Confirm, Deliver (deducts stock), Cancel |
| **Payments** | Opened automatically (Confirm → owed to supplier; Deliver → owed by customer); record installments |
| **Dashboard** | Cross-module aggregates: sales/purchases totals, inventory value, receivables/payables, low stock, delayed customs |

---

## How modules talk to each other: Domain Events

This is the part of the system that runs "on its own" — worth understanding carefully.

### The problem it solves

`Shipment.MarkAsArrived()` needs to trigger opening a Customs Clearance case. If `Shipment` called into the Customs feature directly, the Shipments module would depend on the Customs module — and any future change to Customs risks breaking Shipments by accident.

### The solution

1. An entity, when something significant happens to it, **raises an event** describing what happened — without knowing who (if anyone) is listening. `Shipment.MarkAsArrived()` raises `ShipmentArrivedEvent(ShipmentId, PurchaseOrderId)`, then goes on with its own logic. Nothing about email, customs, or inventory is referenced here.
2. Events are collected in-memory on the entity (`BaseEntity.RaiseDomainEvent`) until the current unit of work is saved.
3. `ApplicationDbContext.SaveChangesAsync` — **after** the database write has actually succeeded — collects every pending event across all tracked entities and publishes each one through MediatR.
4. Because Domain has zero package references (not even MediatR), a small wrapper, `DomainEventNotification<TDomainEvent>` (defined in Application), bridges a raw `IDomainEvent` to MediatR's `INotification` at publish time via reflection.
5. Any number of `INotificationHandler<DomainEventNotification<TheEvent>>` classes — usually living in a completely different feature folder than the entity that raised the event — react independently.

### Every automatic reaction in the system

| Event raised by | Raised when | Handled by (different module) | Effect |
|---|---|---|---|
| `UserRegisteredEvent` | A new `User` is constructed | Auth's `UserRegisteredEventHandler` | Sends the email-confirmation code via SMTP |
| `EmailConfirmationCodeResentEvent` | Resend requested | Auth's own handler | Sends a new code via SMTP |
| `PasswordResetRequestedEvent` | Forgot-password requested | Auth's own handler | Sends the reset code via SMTP |
| `ShipmentArrivedEvent` | `Shipment.MarkAsArrived()` | Customs's `ShipmentArrivedEventHandler` | Opens a new `CustomsClearance` case (`PendingDocuments`) |
| `QualityInspectionCompletedEvent` | `QualityInspection.Complete()` | Inventory's `QualityInspectionCompletedEventHandler` | Adds accepted quantities to stock; closes the originating `PurchaseOrder` if fully shipped |
| `PurchaseOrderConfirmedEvent` | `PurchaseOrder.Confirm()` | Payments's `PurchaseOrderConfirmedEventHandler` | Opens an **Outgoing** `Payment` (owed to the supplier) |
| `SalesOrderFulfilledEvent` | `SalesOrder.MarkAsDelivered()` | Payments's `SalesOrderFulfilledEventHandler` | Opens an **Incoming** `Payment` (owed by the customer) |

### When something is *not* done via an event

Deducting stock during `SalesOrder.Deliver()` happens as a **direct call inside the same command handler**, not via an event. Rule of thumb: if two operations must succeed or fail together atomically, they run inline in one handler; if the second operation is a genuinely separate reaction to something that already happened (open a case, send an email), it goes through an event.

---

## Request lifecycle, step by step

Using `POST /api/purchaseorders/{id}/confirm` as a concrete example:

1. **Api** — `PurchaseOrdersController.Confirm` receives the HTTP request, wraps the route's `id` into a `ConfirmPurchaseOrderCommand`, and sends it to MediatR. No business logic lives here.
2. **MediatR pipeline** — the command passes through, in order: `UnhandledExceptionBehavior` (catches genuine bugs), `LoggingBehavior` (timing/logging), `ValidationBehavior` (runs `ConfirmPurchaseOrderCommandValidator`; stops here on failure), `TransactionBehavior` (will call `SaveChangesAsync` *after* the handler returns, only because this command implements `ITransactionalRequest`).
3. **Application handler** — `ConfirmPurchaseOrderCommandHandler` loads the `PurchaseOrder` (with its lines, via a `Specification`), calls `purchaseOrder.Confirm()`, and translates any `DomainException` into a `Result.Failure(...)`.
4. **Domain** — `PurchaseOrder.Confirm()` checks its own rules (must be `Draft`, must have at least one line), flips its `Status`, and raises `PurchaseOrderConfirmedEvent`.
5. **Persistence** — back in `TransactionBehavior`, `ApplicationDbContext.SaveChangesAsync()` runs: EF Core writes the status change to SQL, then (only after that succeeds) publishes the collected event.
6. **Cross-module reaction** — `PurchaseOrderConfirmedEventHandler` (in the Payments feature) receives the event and creates a new `Payment` row, with its own nested `SaveChangesAsync`.
7. The response travels back up through the same pipeline to the controller, which returns `200 OK` (or the appropriate error status, mapped centrally by `GlobalExceptionMiddleware`).

---

## Authentication & Authorization

- **JWT access tokens** (short-lived, ~15 min) carry the user's `Id`, `Email`, and `Role` as claims.
- **Refresh tokens** are long-lived, stored per-user, and **rotated** on every use (the old token is revoked and linked to its replacement — never deleted, preserving a full session audit trail).
- **Role-based authorization** via `[RequireRole(UserRole...)]` on controllers/actions, checked against the JWT's role claim by ASP.NET Core's standard authorization middleware.
- The system guarantees it can never be left with **zero active System Administrators** — role changes and deactivation are blocked if they would remove the last one.

---

## Getting started

```bash
# Restore & build
dotnet restore
dotnet build

# Apply migrations
dotnet ef database update --project src/TradeSphere.Persistence --startup-project src/TradeSphere.Api

# Run
dotnet run --project src/TradeSphere.Api
```

Configure `appsettings.json` (or `appsettings.Development.json`) with:
- `ConnectionStrings:DefaultConnection` — SQL Server connection string
- `JwtSettings` — `Secret`, `Issuer`, `Audience`, token lifetimes
- `EmailSettings` — SMTP host/credentials for MailKit
- `FileStorage:RootPath` — local file storage path

Swagger UI is available at `/swagger` in Development. The Hangfire dashboard is available at `/jobs`.

---

## API surface

All endpoints are under `/api/`. Highlights by module:

- `auth/register`, `auth/confirm-email`, `auth/resend-confirmation-code`, `auth/login`, `auth/refresh-token`, `auth/forgot-password`, `auth/reset-password`
- `users` (Admin only) — list, get by id, create with role, change role, activate/deactivate
- `suppliers`, `products` — full CRUD
- `purchaseorders` — list, get, create, add line, confirm, cancel
- `shipments` — list, create, mark-shipped, mark-in-transit, mark-arrived
- `customs-clearances` — list, file-declaration, clear, reject
- `qualityinspections` — list, get, create, add line, complete
- `inventory` — list, low-stock, adjust
- `customers` — full CRUD
- `salesorders` — list, get, create, add line, confirm, deliver, cancel
- `payments` — list, get, record-installment
- `dashboard/summary` — cross-module aggregates

Full request/response shapes are documented in Swagger once the API is running.

---

## Background jobs

Two Hangfire recurring jobs, run daily:

- **Delayed customs clearances** — alerts Operations Managers and Customs Clearance Officers by email if any clearance case has been open longer than the configured threshold.
- **Low stock** — alerts Operations Managers and Warehouse Officers by email when any product's stock falls at or below the configured threshold.

---

## Known simplifications

These were deliberate scope decisions for the current version, not oversights:

- **One shipment per purchase order** — `Shipment` has no line-level quantities, so partial fulfillment isn't tracked; creating a shipment moves the order straight to `FullyShipped`.
- **Inventory valuation on the Dashboard** uses the average historical purchase price per product, not FIFO or actual landed cost per unit — a reporting approximation, not a financial-statement figure.
- **Sales settle in a single base currency**; purchases can be in any currency, but all lines within one purchase order must share the same currency.
- **A cancelled purchase order's associated Payment is deleted only if nothing has been paid against it yet** — if any amount was already paid, cancellation is blocked and requires manual financial resolution.
