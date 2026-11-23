---
description: Ensure feature development happens in a dedicated branch
---

# Feature Branch Workflow

This workflow ensures that all development work is performed in a feature branch, keeping the `main` branch stable and ready for production.

## 1. Check Current Branch
Verify your current branch before starting any work.
// turbo
```powershell
git branch --show-current
```

## 2. Start a New Feature
If you are on `main`, create a new branch for your feature. Use a descriptive name like `feature/your-task-name`.
// turbo
```powershell
git checkout -b feature/new-feature-name
```

## 3. Development Loop
- Make changes.
- Commit frequently with descriptive messages.
- Regularly pull from `main` to stay up to date:
// turbo
```powershell
git pull origin main
```

## 4. Prepare for Review
Once your feature is complete and verified:
1. Ensure all tests pass.
2. Ensure no secrets are committed.
3. Call the `/review` workflow to perform the final checks and prepare the PR.

> [!IMPORTANT]
> Never commit directly to `main`. Always use a feature branch and a Pull Request.
