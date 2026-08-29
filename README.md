[![](https://img.shields.io/nuget/v/soenneker.managers.hashchecking.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.hashchecking/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.hashchecking/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.managers.hashchecking/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.managers.hashchecking.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.hashchecking/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.hashchecking/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.managers.hashchecking/actions/workflows/codeql.yml)

# Soenneker.Managers.HashChecking

Handles hashing and verification of binaries.

## Install

```bash
dotnet add package Soenneker.Managers.HashChecking
```

## Quick start

```csharp
using Soenneker.Managers.HashChecking.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddHashCheckingManagerAsSingleton();
```

Adds `IHashCheckingManager` as a singleton service.

## What you get

- `IHashCheckingManager` — Handles hashing and verification of binaries.
- `HashCheckingManagerRegistrar` — Handles hashing and verification of binaries.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IHashCheckingManager.CheckForHashDifferences(gitDirectory, filePath, hashFileName, cancellationToken)` | Checks whether the specified file's hash differs from the stored hash in the given Git directory and determines if an update is required. | A value task that returns a tuple containing a boolean indicating whether an update is needed, and the new hash value if a difference is detected. |
| `IHashCheckingManager.CheckForHashDifferencesOfDirectory(gitDirectory, inputDirectory, hashFileName, cancellationToken)` | Compares the hash of the specified input directory with the hash stored in the given Git directory to determine if an update is required. | A tuple containing a Boolean value indicating whether the input directory needs to be updated (`true` if an update is required; otherwise, `false`), and a string representing the newly computed hash of the input directory. |
| `HashCheckingManagerRegistrar.AddHashCheckingManagerAsSingleton(services)` | Adds `IHashCheckingManager` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `HashCheckingManagerRegistrar.AddHashCheckingManagerAsScoped(services)` | Adds `IHashCheckingManager` as a scoped service. | The same service collection, so additional registrations can be chained. |

## Important behavior

- `IHashCheckingManager.CheckForHashDifferences(gitDirectory, filePath, hashFileName, cancellationToken)`: If the hash file does not exist or the file cannot be read, the method may indicate that an update is needed. This method does not modify any files.
- `IHashCheckingManager.CheckForHashDifferencesOfDirectory(gitDirectory, inputDirectory, hashFileName, cancellationToken)`: This method performs a hash comparison between the input directory and the reference hash file in the Git directory. If the hashes differ, the method indicates that an update is needed and provides the new hash value. The operation is asynchronous and can be cancelled via the provided cancellation token.

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
