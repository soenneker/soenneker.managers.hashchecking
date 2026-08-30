[![](https://img.shields.io/nuget/v/soenneker.managers.hashchecking.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.hashchecking/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.hashchecking/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.managers.hashchecking/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.managers.hashchecking.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.managers.hashchecking/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.managers.hashchecking/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.managers.hashchecking/actions/workflows/codeql.yml)

# Soenneker.Managers.HashChecking

Compares BLAKE3 hashes for a file or directory against a hash stored in a repository checkout.

## Install

```bash
dotnet add package Soenneker.Managers.HashChecking
```

## Usage

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Managers.HashChecking.Abstract;
using Soenneker.Managers.HashChecking.Registrars;

services.AddHashCheckingManagerAsSingleton();

IHashCheckingManager hashes =
    serviceProvider.GetRequiredService<IHashCheckingManager>();

(bool needsUpdate, string newHash) = await hashes.CheckForHashDifferences(
    gitDirectory: repositoryPath,
    filePath: downloadedBinaryPath,
    hashFileName: "hash.txt",
    cancellationToken);

if (needsUpdate)
{
    // Validate and publish the new artifact, then persist newHash.
}
```

The methods only compare hashes. They do not write the hash file or update Git.

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

- A missing or unreadable stored hash returns `needsUpdate: true` with the newly calculated hash.
- Stored leading/trailing whitespace is ignored during comparison.
- `hashFileName` may be a relative path inside `gitDirectory`; paths that escape the checkout are rejected.
- Directory comparisons use `IBlake3Util.HashDirectoryToAggregateString`. Use the returned aggregate string as the next stored value.

## Practical notes

- Hashing large files or directories can be expensive; pass cancellation through from the owning runner.
