﻿using System.Linq.Expressions;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.Linq;

public class AllMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = AllMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Fact]
    public void TranslateMethodCall_ShouldAppendEvery()
    {
        // Arrange
        var sb = new StringBuilder();
        Expression<Func<int, bool>> filter = i => i > 3;
        var method = typeof(Enumerable).GetMethod(nameof(Enumerable.All))!.MakeGenericMethod(typeof(int))!;
        var expression = Expression.Call(
            method,
            new Expression[]
            {
                Expression.Constant(new List<int>()),
                filter
            }
        );

        ProcessExpression processExpression = next =>
        {
            if (next is ConstantExpression { Value: List<int> })
            {
                sb.Append("[]");
            } else if (next is LambdaExpression)
            {
                sb.Append("i=>i>3");
            }
        };
        
        // Act
        AllMethodCallTranslator.TranslateMethodCall(sb, expression, processExpression);
        
        // Assert
        Assert.Equal("[].every(i=>i>3)", sb.ToString());
    }
}