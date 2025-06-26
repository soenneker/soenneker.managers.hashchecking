using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.HashChecking.Abstract;

/// <summary>
/// Handles hashing and verification of binaries
/// </summary>
public interface IHashCheckingManager
{
    ValueTask<(bool needsUpdate, string? newHash)> CheckForHashDifferences(string gitDirectory, string filePath, string hashFileName,
        CancellationToken cancellationToken = default);

    ValueTask<(bool needsUpdate, string? newHash)> CheckForHashDifferencesOfDirectory(string gitDirectory, string inputDirectory, string hashFileName,
        CancellationToken cancellationToken = default);
}