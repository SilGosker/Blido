namespace Blido.Core.Transaction.Materialization;

public class MaterializationArgumentsTests
{
    [Theory]
    [InlineData("  ", "  ", 0, "  ", null)]
    [InlineData("database", "objectStore", 18, "", null)]
    [InlineData("", "", ulong.MinValue, null, "parsedExpressions")]
    [InlineData(null, null, ulong.MaxValue, "selector", "parsedExpressions", "parsedExpressions")]

    public void Properties_ShouldSetValues(string database, string objectStore, ulong version, string selector, params string[]? parsedExpressions)
    {
        // Arrange
        var arguments = new MaterializationArguments()
        {
            Database = "database",
            ObjectStore = "objectStore",
            Version = 0,
            ParsedExpressions = null
        };

        // Act
        arguments.Database = database;
        arguments.ObjectStore = objectStore;
        arguments.Version = version;
        arguments.ParsedExpressions = parsedExpressions;
        arguments.Selector = selector;

        // Assert
        Assert.Equal(database, arguments.Database);
        Assert.Equal(objectStore, arguments.ObjectStore);
        Assert.Equal(version, arguments.Version);
        Assert.Equal(parsedExpressions, arguments.ParsedExpressions);
        Assert.Equal(selector, arguments.Selector);
    }
}