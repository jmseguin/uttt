# Testing Conventions

## Tools
- **Framework**: xUnit
- **Assertion Library**: `Assert` (xUnit) or `FluentAssertions` (if available)
- **Mocking**: `Moq` or `NSubstitute`
- **Coverage**: `dotnet-coverage` or `coverlet`

## Coverage Requirements
- **Core Logic**: 100% Unit Test coverage
- **Scanners**: 100% coverage via mocks of file system and process runner
- **CLI**: Functional tests for argument parsing and orchestration

## Best Practices
- **Test Discovery**: Use `[Fact]` for single cases and `[Theory]` with `[InlineData]` for parameterized tests.
- **Naming**: `MethodName_StateUnderTest_ExpectedBehavior` (e.g., `ScanAsync_PythonNotFound_ReturnsEmpty`).
- **Isolation**: Use interfaces (`IFileSystem`, `IProcessRunner`) to mock external dependencies.
- **Data Driven**: Test scanners against varied directory structures and process lists.

## Command Line
Run all tests:
```powershell
dotnet test
```

Generate coverage report:
```powershell
dotnet test /p:CollectCoverage=true
```
