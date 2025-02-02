using BlazorIndexedOrm.Core.UnitTests.Mock.ObjectStore.ObjectStore;
using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression;

public class JsMemberExpressionBuilderTests
{
    [Fact]
    public void AppendMember_WithPropertyExpression_AppendsMemberName()
    {
        // Arrange
        var sb = new StringBuilder();
        var objectStore = new MockObjectStore();
        var expression = Expression.Property(Expression.Constant(objectStore), "Name");
        var processExpression = new ProcessExpression(_ => { });

        // Act
        JsMemberExpressionBuilder.AppendMember(sb, expression, processExpression);

        // Assert
        Assert.Equal(".Name", sb.ToString());
    }

    [Fact]
    public void AppendMember_WithAttributeExpression_AppendsAttributeValue()
    {
        // Arrange
        var sb = new StringBuilder();
        var objectStore = new MockObjectStoreWithFieldAttribute();
        var expression = Expression.Property(Expression.Constant(objectStore), "Name");
        var processExpression = new ProcessExpression(_ => { });

        // Act
        JsMemberExpressionBuilder.AppendMember(sb, expression, processExpression);

        // Assert
        Assert.Equal(".__name", sb.ToString());
    }
}