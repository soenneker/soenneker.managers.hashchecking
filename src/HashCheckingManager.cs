using Soenneker.Managers.HashChecking.Abstract;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Soenneker.Utils.File.Abstract;
using Soenneker.Utils.SHA3.Abstract;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Managers.HashChecking;

/// <inheritdoc cref="IHashCheckingManager"/>
public class HashCheckingManager : IHashCheckingManager
{
    private readonly ILogger<HashCheckingManager> _logger;
    private readonly IFileUtil _fileUtil;
    private readonly ISha3Util _sha3Util;

    public HashCheckingManager(ILogger<HashCheckingManager> logger, IFileUtil fileUtil, ISha3Util sha3Util)
    {
        _logger = logger;
        _fileUtil = fileUtil;
        _sha3Util = sha3Util;
    }

    public async ValueTask<(bool needsUpdate, string? newHash)> CheckForHashDifferences(string gitDirectory, string filePath, string hashFile, CancellationToken cancellationToken = default)
    {
        // Attempt to read the old hash
        string hashFilePath = Path.Combine(gitDirectory, hashFile);
        string? oldHash = await _fileUtil.TryRead(hashFilePath, true, cancellationToken).NoSync();

        // Compute the new hash
        string newHash = await _sha3Util.HashFile(filePath, true, cancellationToken).NoSync();

        if (oldHash == null)
        {
            _logger.LogDebug("Could not read hash from repository, proceeding to update...");
            return (true, newHash);
        }

        // Compare old vs new
        if (oldHash == newHash)
        {
            _logger.LogInformation("Hashes are equal, no need to update, exiting...");
            return (false, null);
        }

        return (true, newHash);
    }
}