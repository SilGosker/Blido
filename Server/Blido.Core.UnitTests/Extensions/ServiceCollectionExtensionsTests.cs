using Blido.Core.Transaction;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.JsExpression.BinaryTranslation;
using Blido.Core.Transaction.JsExpression.MemberTranslation;
using Blido.Core.Transaction.JsExpression.MethodCallTranslation;
using Blido.Core.Transaction.JsExpression.UnaryTranslation;
using Blido.Core.Transaction.Mutation.KeyGeneration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;

namespace Blido.Core.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void RegisterIndexedDbDatabase_WithNullServiceCollection_ShouldThrowArgumentNullException()
    {
        // Arrange
        IServiceCollection serviceCollection = null!;

        // Act
        Action action = () => serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();
        
        // Assert
        var exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("serviceCollection", exception.ParamName);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterIndexedDbDatabase()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);

        // Act
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
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);

        // Act
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var methodTranslatorFactory = serviceProvider.GetRequiredService<IMethodCallTranslatorFactory>();

        Assert.NotNull(methodTranslatorFactory);
        Assert.IsType<JsMethodCallTranslatorFactory>(methodTranslatorFactory);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterJsMemberTranslatorFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();
        
        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var memberTranslatorFactory = serviceProvider.GetRequiredService<IMemberTranslatorFactory>();
        Assert.NotNull(memberTranslatorFactory);
        Assert.IsType<JsMemberTranslatorFactory>(memberTranslatorFactory);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterBinaryTranslatorFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var binaryTranslatorFactory = serviceProvider.GetRequiredService<IBinaryTranslatorFactory>();
        Assert.NotNull(binaryTranslatorFactory);
        Assert.IsType<JsBinaryTranslatorFactory>(binaryTranslatorFactory);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterUnaryTranslatorFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        
        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var unaryTranslatorFactory = serviceProvider.GetRequiredService<IUnaryTranslatorFactory>();
        Assert.NotNull(unaryTranslatorFactory);
        Assert.IsType<JsUnaryTranslatorFactory>(unaryTranslatorFactory);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterKeyGeneratorFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();
        
        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var keyGeneratorFactory = serviceProvider.GetRequiredService<IKeyGeneratorFactory>();
        Assert.NotNull(keyGeneratorFactory);
        Assert.IsType<KeyGeneratorFactory>(keyGeneratorFactory);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterKeyGeneratorMiddleware()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var keyGeneratorMiddleware = serviceProvider.GetRequiredService<KeyGeneratorMiddleware>();
        Assert.NotNull(keyGeneratorMiddleware);
    }


    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterJsExpressionBuilder()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;

        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();
        
        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var expressionBuilder = serviceProvider.GetRequiredService<IExpressionBuilder>();
        Assert.NotNull(expressionBuilder);
        Assert.IsType<JsExpressionBuilder>(expressionBuilder);
    }

    [Fact]
    public void RegisterIndexedDbDatabase_ShouldRegisterObjectStoreFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var mockJsRuntime = new Mock<IJSRuntime>().Object;
        // Act
        serviceCollection.AddScoped<IJSRuntime>(_ => mockJsRuntime);
        serviceCollection.RegisterIndexedDbDatabase<MockIndexedDbDatabase>();

        // Assert
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var transactionProviderFactory = serviceProvider.GetRequiredService<IObjectStoreFactory>();
        Assert.NotNull(transactionProviderFactory);
        Assert.IsType<ObjectStoreFactory>(transactionProviderFactory);
    }
}