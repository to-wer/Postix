using System.Globalization;
using System.Reflection;

namespace Postix.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Assembly"/> class.
/// </summary>
public static class AssemblyExtensions
{
        /// <summary>
    /// Gets the build date of the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns>The build date, or <see cref="DateTimeOffset.MinValue"/> if the build date cannot be determined.</returns>
    public static DateTimeOffset GetBuildDate(this Assembly assembly)
    {
        const string buildVersionMetadataPrefix = "+build";

        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (attribute?.InformationalVersion == null) return default;
        var value = attribute.InformationalVersion;
        var index = value.IndexOf(buildVersionMetadataPrefix, StringComparison.Ordinal);
        if (index <= 0) return default;
        value = value.Substring(index + buildVersionMetadataPrefix.Length);
        return DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result) ? new DateTimeOffset(result, TimeSpan.Zero) : default;
    }
}