---
description: Build and publish the AI Asset Discovery tool
---

// turbo-all

1. Ensure all tests pass
```powershell
dotnet test
```

2. Build the solution in Release mode
```powershell
dotnet build -c Release
```

3. Publish the standalone executable for Windows

4. Verify the executable exists in release folder (adjust path for .NET version)