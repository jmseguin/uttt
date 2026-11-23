---
description: Pull Request Review Protocol
---

# Pull Request Review

## 1. Analysis
- Review the diff to understand intent and scope.

## 2. Security checks
- [ ] No hardcoded secrets, passwords, or API keys (GUARDRAILS SIGN #4)
- [ ] Sensitive data (like API keys) handled via CLI arguments or env vars
- [ ] External API calls use secure communication (HTTPS)

## 3. Code quality checks
- [ ] Follows C# Style Guide (PascalCase, file-scoped namespaces)
- [ ] Efficient use of resources (Span/Memory for heavy parsing)
- [ ] Correct use of `async/await` for all I/O
- [ ] `IScanner` implementations are isolated and robust

## 4. Stability and Performance
- [ ] Exception handling in Scanners prevents app crash (GUARDRAILS SIGN #2)
- [ ] Resource throttling used for intensive tasks (GUARDRAILS SIGN #1)
- [ ] Network calls have retry/backoff (GUARDRAILS SIGN #3)
- [ ] `CancellationToken` supported in all async paths

## 5. Testing checks
- [ ] New scanner logic has 100% unit test coverage
- [ ] Mocks used for file system and process interactions
- [ ] CLI argument validation tested (GUARDRAILS SIGN #5)
- [ ] Functional tests cover the end-to-end scan lifecycle

## 6. Documentation checks
- [ ] README.md updated for new scanners or CLI flags
- [ ] GUARDRAILS.md remains current
- [ ] Inline comments for complex discovery logic

## 7. Change Summary
Create a Markdown table summarising all changes:

| File | Change Type            | Description |
| ---- | ---------------------- | ----------- |
| ...  | Added/Modified/Deleted | ...         |
