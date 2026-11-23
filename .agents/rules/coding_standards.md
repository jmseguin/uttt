# Coding Standards

To maintain a clean and consistent codebase, all agents and contributors should follow these standards:

## C# Style
- Use PascalCase for methods, properties, and class names.
- Use camelCase for local variables and private fields (with an underscore prefix, e.g., `_myField`).
- Use explicit types instead of `var` when the type is not obvious from the assignment.
- Follow SOLID principles for game logic (e.g., separation of engine and UI).

## Documentation
- Document complex logic, especially the Minimax algorithm in `AiPlayer.cs`.
- Update `UTTT-Instructions.md` if any game rules are modified.

## Platform Compatibility
- Avoid platform-specific libraries.
- Use `Path.Combine` for file paths.
- Ensure terminal output uses standard ANSI colors compatible with multi-platform terminals.
