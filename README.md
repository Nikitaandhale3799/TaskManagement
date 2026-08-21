# Task Management

A full-stack task management application: a .NET 8 Web API backend, a React (Vite + TypeScript) frontend, and a SQL Server database.

## Tech Stack

- **Backend:** .NET 8 Web API, Entity Framework Core 8 (Code First), SQL Server
- **Frontend:** React 19 + TypeScript, Vite, plain CSS (no UI framework)
- **Testing:** xUnit + EF Core InMemory provider
- **Database:** SQL Server (via Docker)

## Project Structure

```
TaskManagement/
├── TaskManagement.API/       # .NET 8 Web API
│   ├── Controllers/           # TasksController
│   ├── Services/               # ITaskService / TaskService (business logic)
│   ├── DTOs/                    # Request/response DTOs
│   ├── Models/                   # TaskItem entity, TaskStatus/TaskPriority enums
│   ├── Data/                      # AppDbContext, seed data
│   ├── Migrations/                 # EF Core Code First migrations
│   └── Middleware/                  # Global exception handler
├── TaskManagement.UI/         # React + Vite frontend
│   └── src/
│       ├── api/                    # Fetch-based API client
│       ├── components/              # TaskForm, TaskTable, StatusSelect, etc.
│       ├── hooks/                    # useTasks (data + state)
│       └── types/                     # TypeScript types mirroring backend DTOs
├── TaskManagement.Tests/       # xUnit tests for TaskService
└── TaskManagement.sln
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18+)
- [Docker](https://www.docker.com/) (to run SQL Server)
- `dotnet-ef` CLI tool: `dotnet tool install --global dotnet-ef` (if not already installed)

## Setup

### 1. Start SQL Server

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

Wait a few seconds for it to finish initializing, then confirm it's ready:

```bash
docker logs sqlserver
```

Look for `SQL Server is now ready for client connections.`

> The connection string in `TaskManagement.API/appsettings.json` expects this exact port and password. If you change either, update the connection string to match.

### 2. Apply migrations (creates the database, schema, and seeds 12 sample tasks)

```bash
cd TaskManagement.API
dotnet ef database update
```

### 3. Run the backend

```bash
dotnet run
```

API runs at `http://localhost:5267`. Swagger UI opens automatically at `http://localhost:5267/swagger`.

### 4. Run the frontend

```bash
cd TaskManagement.UI
npm install
npm run dev
```

App runs at `http://localhost:5173`.

> CORS is configured on the backend to allow exactly `http://localhost:5173`. If Vite picks a different port (e.g. because 5173 is already in use), update the CORS policy in `TaskManagement.API/Program.cs` to match.

## API Endpoints

| Method | Route | Description |
|---|---|---|
| GET | `/api/tasks` | List tasks. Optional query params: `status`, `priority`, `sortBy` (`title`\|`status`\|`priority`\|`createdDate`), `descending` |
| GET | `/api/tasks/{id}` | Get a single task by ID (404 if not found or soft-deleted) |
| GET | `/api/tasks/summary` | Task counts grouped by status and priority (raw SQL, not LINQ) |
| POST | `/api/tasks` | Create a task |
| PUT | `/api/tasks/{id}` | Update a task (full replace) |
| DELETE | `/api/tasks/{id}` | Soft-delete a task (sets `IsDeleted = true`; row is never physically removed) |

All error responses (validation failures, not-found, and unexpected exceptions) return a consistent `ProblemDetails`-shaped JSON body.

## Task Entity

| Field | Type |
|---|---|
| Id | int |
| Title | string (required, max 200 chars) |
| Description | string? (max 2000 chars) |
| Status | `ToDo` \| `InProgress` \| `Done` |
| Priority | `Low` \| `Medium` \| `High` \| `Critical` |
| AssignedTo | string? (max 100 chars) |
| CreatedDate | DateTime |
| ModifiedDate | DateTime |
| IsDeleted | bool |

## Running Tests

```bash
cd TaskManagement.Tests
dotnet test
```

13 tests covering `TaskService`: filtering, soft-delete exclusion, create defaults, update behavior, and not-found handling.

## Frontend Features

- Task list in a table, with Status and Priority filters
- Create task form with client + server-side validation
- Inline status update (dropdown per row, optimistic update with rollback on failure)
- Delete button (confirms before calling the soft-delete endpoint)
- Color-coded priority badges
- Loading, error (with retry), and empty states

## Not Implemented (Optional / Bonus)

- Authentication
- Docker Compose for the full application (only SQL Server is documented above; the API and frontend run locally)
- A shared .NET class library between projects

## Notes

- Enums (`Status`, `Priority`) are serialized as strings over the API (e.g. `"Done"`, `"High"`), not integers.
- Soft delete is enforced globally via an EF Core query filter — no query in the codebase can accidentally return a deleted task.
- The summary endpoint (`GET /api/tasks/summary`) uses a hand-written raw SQL query via `Database.SqlQuery<T>`, per the requirement that it not be an EF LINQ-generated query.
