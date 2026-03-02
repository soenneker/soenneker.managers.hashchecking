using Soenneker.Managers.HashChecking.Abstract;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using Soenneker.Utils.File.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Hashing.Blake3.Abstract;

namespace Soenneker.Managers.HashChecking;

/// <inheritdoc cref="IHashCheckingManager"/>
public sealed class HashCheckingManager : IHashCheckingManager
{
    private readonly ILogger<HashCheckingManager> _logger;
    private readonly IFileUtil _fileUtil;
    private readonly IBlake3Util _blake3Util;

    public HashCheckingManager(ILogger<HashCheckingManager> logger, IFileUtil fileUtil, IBlake3Util blake3Util)
    {
        _logger = logger;
        _fileUtil = fileUtil;
        _blake3Util = blake3Util;
    }

    public async ValueTask<(bool needsUpdate, string newHash)> CheckForHashDifferences(string gitDirectory, string filePath, string hashFileName,
        CancellationToken cancellationToken = default)
    {
        // Attempt to read the old hash
        string hashFilePath = Path.Combine(gitDirectory, hashFileName);

        string newHash = await _blake3Util.HashFile(filePath, cancellationToken).NoSync();

        if (!await _fileUtil.Exists(hashFilePath, cancellationToken).NoSync())
        {
            _logger.LogDebug("Hash file does not exist, proceeding to update...");
            return (true, newHash);
        }

        string? oldHash = await _fileUtil.TryRead(hashFilePath, true, cancellationToken).NoSync();

        if (oldHash == null)
        {
            _logger.LogDebug("Could not read hash from repository, proceeding to update...");
            return (true, newHash);
        }

        // Compare old vs new
        if (oldHash == newHash)
        {
            _logger.LogInformation("Hashes are equal, no need to update, exiting...");
            return (false, newHash);
        }

        return (true, newHash);
    }

    public async ValueTask<(bool needsUpdate, string newHash)> CheckForHashDifferencesOfDirectory(string gitDirectory, string inputDirectory,
        string hashFileName, CancellationToken cancellationToken = default)
    {
        string hashFilePath = Path.Combine(gitDirectory, hashFileName);

        string? oldHash = null;

        if (!await _fileUtil.Exists(hashFilePath, cancellationToken).NoSync())
        {
            _logger.LogDebug("Hash file does not exist, hashing directory");
        }
        else
        {
            oldHash = await _fileUtil.TryRead(hashFilePath, true, cancellationToken).NoSync();
        }

        string newHash = await _blake3Util.HashDirectoryToAggregateString(inputDirectory, cancellationToken).NoSync();

        if (oldHash == null)
        {
            _logger.LogDebug("Could not read hash from repository, proceeding to update...");
            return (true, newHash);
        }

        if (oldHash == newHash)
        {
            _logger.LogInformation("Hashes are equal, no need to update, exiting...");
            return (false, newHash);
        }

        return (true, newHash);
    }
}