using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Managers.HashChecking.Abstract;

namespace Soenneker.Managers.HashChecking.Registrars;

/// <summary>
/// Handles hashing and verification of binaries
/// </summary>
public static class HashCheckingManagerRegistrar
{
    /// <summary>
    /// Adds <see cref="IHashCheckingManager"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddHashCheckingManagerAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<IHashCheckingManager, HashCheckingManager>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IHashCheckingManager"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddHashCheckingManagerAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<IHashCheckingManager, HashCheckingManager>();

        return services;
    }
}
