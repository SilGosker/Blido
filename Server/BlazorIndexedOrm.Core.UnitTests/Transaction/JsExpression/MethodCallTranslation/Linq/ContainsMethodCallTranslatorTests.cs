﻿using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class ContainsMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = ContainsMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendContains()
    {
        // Arrange
        var sb = new StringBuilder();
        var method = typeof(Enumerable).GetMethods().First(e => e.Name == nameof(Enumerable.Contains) && e.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(int))!;

        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
                Expression.Constant(3)
            }
        );
        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: List<int> })
            {
                sb.Append("[]");
            }
            else if (next is ConstantExpression { Value: int })
            {
                sb.Append('3');
            }
        };

        // Act
        ContainsMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("[].contains(3)", sb.ToString());
    }
}