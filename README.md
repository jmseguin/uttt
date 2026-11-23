# Ultimate Tic Tac Toe (.NET Core)

A cross-platform implementation of Ultimate Tic Tac Toe with 2-player and AI modes, featuring a modern GUI.

## Features
- **Two Player Mode**: Play locally with a friend.
- **AI Opponent**: Three difficulty levels (Easy, Medium, Hard).
- **Cross-Platform**: Runs on Windows, macOS, and Linux via .NET Core and Raylib.
- **Fluid GUI**: Responsive and modern game interface using Raylib-cs.
- **100% Test Coverage**: Core game logic is fully tested.

## Prerequisites
- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or later.
- Graphics drivers with OpenGL support (standard on most systems).

## Quick Start

1. **Build the project**:
   ```bash
   dotnet build UTTT.slnx
   ```

2. **Run the game**:
   ```bash
   dotnet run --project UTTT.Gui
   ```

3. **Run tests**:
   ```bash
   dotnet test
   ```

4. **Publish Packaging**:
   To build and package standalone `.zip` archives for Windows, Linux, and macOS:
   ```powershell
   .\publish.ps1
   ```
   Packages will be output to the `publish/` directory.

5. **How to Play**:
   Refer to [UTTT-Instructions.md](file:///c:/GitHub/uttt/UTTT-Instructions.md) for detailed rules.

## Development
This project uses [Antigravity](file:///c:/GitHub/uttt/agents.md) for AI assistance.
Refer to [.agents/rules/coding_standards.md](file:///c:/GitHub/uttt/.agents/rules/coding_standards.md) for contribution guidelines and [.agents/rules/testing-conventions.md](file:///c:/GitHub/uttt/.agents/rules/testing-conventions.md) for testing standards.
