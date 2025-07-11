# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Aido is a todo/packing/checklist application that uses AI to generate suggestions based on previous list items, descriptions, and names. The backend is built with .NET 9 following Clean Architecture principles.

## Development Commands

### Build
```bash
dotnet build                    # Build entire solution
dotnet build Aido.sln          # Build solution explicitly
```

### Test
```bash
dotnet test                     # Run all tests
dotnet test tests/Aido.Core.UnitTests/
dotnet test tests/Aido.Application.UnitTests/
dotnet test tests/Aido.Infrastructure.UnitTests/
dotnet test tests/Aido.Presentation.UnitTests/
```

### Run Application
```bash
dotnet run --project src/Aido.Presentation/
```

### Restore Dependencies
```bash
dotnet restore                  # Restore NuGet packages
```

## Architecture Overview

The solution follows Clean Architecture with strict dependency flow inward:

- **Aido.SharedKernel**: Shared domain concepts including Result pattern for explicit error handling
- **Aido.Core**: Domain layer with entities, aggregates, and domain events (no dependencies)
- **Aido.Application**: Application layer with use cases and interface definitions (depends on Core)
- **Aido.Infrastructure**: Infrastructure layer with concrete implementations (depends on Application)  
- **Aido.Presentation**: ASP.NET Core Web API entry point and composition root (depends on Application/Infrastructure)

All projects target .NET 9 with nullable reference types enabled.

## Testing Requirements

### Test Data Builders Pattern
**Always use test object builders for creating test data** - this is the primary pattern, not a special case.

```csharp
// Correct approach
var user = new UserBuilder().WithFirstName("John").Build();

// Avoid this
var user = CreateValidUser("John", "Doe");
```

### Key Testing Principles
- **Builders for everything**: Use builders for all test data including nested objects and DTOs
- **Only specify relevant data**: If a property isn't asserted, don't specify it in the builder
- **System tests use REST API**: Create data through actual endpoints, not direct database seeding
- **xUnit with NSubstitute**: Testing framework with mocking library

## Documentation Standards

- Write class-level documentation for public types
- Avoid method documentation unless clarification is needed
- Keep documentation succinct and avoid stating the obvious
- Do not document what is already clear from the code

## Key Technologies

- **.NET 9**: Primary framework
- **ASP.NET Core**: Web API with OpenAPI support
- **xUnit**: Unit testing framework
- **NSubstitute**: Mocking library
- **Microsoft Semantic Kernel**: AI orchestration (planned)
- **PostgreSQL**: Database (planned)