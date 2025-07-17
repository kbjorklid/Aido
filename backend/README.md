# Aido

This is a 'todo list / packing list / check list' application that will use AI to generate suggestions
based on previous list items, list description, and list name.

## Architecture

The solution follows Clean Architecture principles with strict dependency flow inward:

- **Aido.Core**: Domain layer with entities, aggregates, and domain events (no dependencies)
- **Aido.Application**: Application layer with use cases and interface definitions (depends on Core)
- **Aido.Infrastructure**: Infrastructure layer with concrete implementations (depends on Application)
- **Aido.Presentation**: ASP.NET Core Web API entry point and composition root (depends on Application/Infrastructure)
- **Aido.SharedKernel**: Shared domain concepts across bounded contexts, including the Result pattern for explicit error handling

## Key Technologies

- **.NET 9**: Target framework across all projects
- **xUnit**: Testing framework with NSubstitute for mocking
- **Microsoft Semantic Kernel**: AI orchestration (planned)
- **PostgreSQL**: Database (planned)
- **ASP.NET Core**: Web API with OpenAPI support

## Testing Practices

### Test Data Builders

The project uses the Builder pattern for creating test data with sensible defaults and fluent customization:

```csharp
// Simple with defaults
var jobPosting = new JobPostingBuilder().Build();

// Customized 
var jobPosting = new JobPostingBuilder()
    .WithListingItem(new JobListingItemBuilder()
        .WithTitle("Senior Software Engineer")
        .WithJobPostingSite("LinkedIn")
        .Build())
    .Build();
```

**Always use test object builders for creating test data.** This is the primary pattern for all test data creation - not a special case, but the standard approach.

Do not create helper methods like:
```csharp
var user = CreateValidUser("John", "Doe");
```

Always use builders instead:
```csharp
var user = new UserBuilder().WithFirstName("John").WithLastName("Doe").Build();
```

**Key principle: Only specify properties that are relevant to your test.** If first name is asserted but last name is not, omit the irrelevant data:
```csharp
var user = new UserBuilder().WithFirstName("John").Build();
```

**Use builders for nested objects too.** When objects contain complex nested structures, use builders at every level:
```csharp
// Good - builders all the way down
.WithComplexProperty(
    new ComplexObjectBuilder().WithRelevantField("value").Build(),
    new ComplexObjectBuilder().WithDifferentField("other").Build()
)

// Avoid - mixing builders with manual construction
.WithComplexProperty(
    ("manual", new[] { "construction" }),
    ("makes", new[] { "tests", "harder", "to", "read" })
)
```

This pattern makes tests self-documenting - a reader can immediately see what data matters for the test's assertions.


### Testing Guidelines

- **Test object builders are the default** - Always use builders for creating test data, never constructors or helper methods
- **Builders all the way down** - Use builders for nested objects and complex structures too
- **Only specify test-relevant data** - If a property isn't asserted, don't specify it in the builder call
- **Avoid reflection** in test helpers - use public APIs to set up state
- **Test state transitions** through proper domain methods, not by bypassing encapsulation
- **Follow Arrange/Act/Assert** pattern with descriptive test names
- **Use Theory tests** with InlineData for testing multiple scenarios

### System Testing Patterns

**System Testing Principles:**
- **Use REST API for data creation** - Tests should use actual API endpoints, not direct database seeding. Where possible, treat the system as a black box.
- **Apply builder pattern everywhere** - Use test object builders for all test data, including DTOs, complex nested objects, and collections
- **Show only relevant data** - Only specify properties that are being tested; let sensible defaults handle the rest

The builder pattern naturally extends to system tests. Every test object should have a corresponding builder that provides sensible defaults and fluent configuration.

```csharp
// Natural approach - builders everywhere
await PostAnalysisRequestsAsync(
    new RequestDtoBuilder()
        .WithComplexData(
            new ComplexDataBuilder().WithRelevantProperty("test-value").Build(),
            new ComplexDataBuilder().WithDifferentProperty("other-value").Build()
        )
        .Build()
);
```

This approach makes system tests as readable and maintainable as unit tests, with the added benefit of testing through real API endpoints.

## Project Structure Notes

- All projects target .NET 9 with nullable reference types enabled
- Test projects include coverlet.collector for code coverage and reference `FThisJob.TestHelpers`
- TestHelpers project provides builder pattern for consistent test data creation
- Presentation layer currently has basic weather [.idea](.idea)forecast endpoint as template
- Clean Architecture dependency constraints are enforced through project references
- Solution is organized with `src/` and `tests/` folders for clear separation

## Documentation

- Write class-level documentation.
- Do not write documentation for methods unless something needs to be clarified.
- Keep documentation relatively succinct.

Example of bad documentation:
```csharp
public enum RequestStatus
{ 
    /// <summary>
    /// Request is currently being processed.
    /// </summary>
    Processing,
```
This documentation mainly just repeats what the reader can see form the code. Avoid this.