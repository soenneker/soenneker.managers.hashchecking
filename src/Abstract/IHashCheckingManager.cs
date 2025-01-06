using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.HashChecking.Abstract;

/// <summary>
/// Handles hashing and verification of binaries
/// </summary>
public interface IHashCheckingManager
{
    ValueTask<bool> CheckForHashDifferences(string gitDirectory, string filePath, string hashFile, CancellationToken cancellationToken = default);
}
