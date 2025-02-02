using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;
using BlazorIndexedOrm.Core.UnitTests.Mock.IndexedDbDatabase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;

namespace BlazorIndexedOrm.Core.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterIndexedDbDatabase()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var database = serviceProvider.GetRequiredService<MockIndexedDbDatabase>();

        Assert.NotNull(database);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegistersJsMethodCallTranslatorFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var methodTranslatorFactory = serviceProvider.GetRequiredService<IJsMethodCallTranslatorFactory>();

        Assert.NotNull(methodTranslatorFactory);
        Assert.IsType<JsMethodCallTranslatorFactory>(methodTranslatorFactory);
    }
}