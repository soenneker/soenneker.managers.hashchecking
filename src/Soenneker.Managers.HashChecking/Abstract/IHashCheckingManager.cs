using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Managers.HashChecking.Abstract;

/// <summary>
/// Handles hashing and verification of binaries
/// </summary>
public interface IHashCheckingManager
{
    /// <summary>
    /// Checks whether the specified file's hash differs from the stored hash in the given Git directory and determines
    /// if an update is required.
    /// </summary>
    /// <remarks>If the hash file does not exist or the file cannot be read, the method may indicate that an
    /// update is needed. This method does not modify any files.</remarks>
    /// <param name="gitDirectory">The path to the Git directory containing the hash file. Must not be null or empty.</param>
    /// <param name="filePath">The path to the file whose hash will be compared. Must not be null or empty.</param>
    /// <param name="hashFileName">The name of the file that stores the reference hash within the Git directory. Must not be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A value task that returns a tuple containing a boolean indicating whether an update is needed, and the new hash
    /// value if a difference is detected.</returns>
    ValueTask<(bool needsUpdate, string newHash)> CheckForHashDifferences(string gitDirectory, string filePath, string hashFileName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Compares the hash of the specified input directory with the hash stored in the given Git directory to determine
    /// if an update is required.
    /// </summary>
    /// <remarks>This method performs a hash comparison between the input directory and the reference hash
    /// file in the Git directory. If the hashes differ, the method indicates that an update is needed and provides the
    /// new hash value. The operation is asynchronous and can be cancelled via the provided cancellation
    /// token.</remarks>
    /// <param name="gitDirectory">The path to the Git directory containing the reference hash file. Must not be null or empty.</param>
    /// <param name="inputDirectory">The path to the input directory whose contents will be hashed and compared. Must not be null or empty.</param>
    /// <param name="hashFileName">The name of the hash file within the Git directory to use for comparison. Must not be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A tuple containing a Boolean value indicating whether the input directory needs to be updated (<see
    /// langword="true"/> if an update is required; otherwise, <see langword="false"/>), and a string representing the
    /// newly computed hash of the input directory.</returns>
    ValueTask<(bool needsUpdate, string newHash)> CheckForHashDifferencesOfDirectory(string gitDirectory, string inputDirectory, string hashFileName,
        CancellationToken cancellationToken = default);
}