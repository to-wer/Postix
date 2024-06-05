using Postix.Extensions;

namespace Postix.Tests.Extensions;

public class AssemblyExtensionsTest
{
    [Fact]
    public void GetBuildDate_AssemblyWithInformationalVersionAttribute_ReturnsExpectedValue()
    {
        // Arrange
        var assembly = typeof(AssemblyExtensions).Assembly;

        // Act
        var result = assembly.GetBuildDate();

        // Assert
        Assert.InRange(result, new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero), DateTimeOffset.Now);
    }

    [Fact]
    public void GetBuildDate_AssemblyWithoutInformationalVersionAttribute_ReturnsMinValue()
    {
        // Arrange
        var assembly = typeof(string).Assembly;

        // Act
        var result = assembly.GetBuildDate();

        // Assert
        Assert.Equal(DateTimeOffset.MinValue, result);
    }

    [Fact]
    public void GetBuildDate_AssemblyWithInvalidInformationalVersionAttribute_ReturnsMinValue()
    {
        // Arrange
        var assembly = typeof(object).Assembly;

        // Act
        var result = assembly.GetBuildDate();

        // Assert
        Assert.Equal(DateTimeOffset.MinValue, result);
    }
}