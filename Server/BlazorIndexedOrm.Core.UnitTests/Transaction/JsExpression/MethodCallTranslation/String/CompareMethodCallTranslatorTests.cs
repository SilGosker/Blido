﻿using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation.String;

public class CompareMethodCallTranslatorTests
{
    [Fact]
    public void SupportedMethods_ShouldNotContainNull()
    {
        // Arrange
        var supportedMethods = CompareMethodCallTranslator.SupportedMethods;

        // Act
        var containsNull = supportedMethods.Contains(null);

        // Assert
        Assert.False(containsNull);
    }

    [Theory]
    [InlineData("a", 0, "b", 0, 0, "en-US", CompareOptions.None)]
    [InlineData("a", 0, "b", 0, 0, true, "en-US")]
    [InlineData("a", 0, "b", 0, 0, StringComparison.CurrentCulture)]
    [InlineData("a", 0, "b", 0, 0, true)]
    [InlineData("a", 0, "b", 0, 0)]
    public void TranslateMethodCall_WithIntArguments_ThrowsNotSupportedException(params object?[] parameters)
    {
        // Arrange
        parameters = parameters.Where(e => e != null).Select(e => e is "en-US"
            ? new CultureInfo("en-US")
            : e).ToArray();
        var arguments = parameters.Select(e => e!.GetType()).ToArray();
        var method = typeof(string).GetMethod(nameof(string.Compare), arguments);
        var expression = Expression.Call(null, method!, parameters.Select(Expression.Constant));
        var builder = new StringBuilder();

        // Act
        Action act = () => CompareMethodCallTranslator.TranslateMethodCall(builder, expression, _ => { });

        // Assert
        Assert.Throws<NotSupportedException>(act);
    }
    
    [Fact]
    public void TranslateMethodCall_WithoutCasingArguments_AppendsLocalCompare()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Compare), new[] { typeof(string), typeof(string) })!;
        var expression = Expression.Call(null, method, new []
        {
            Expression.Constant("a"),
            Expression.Constant("b")
        });
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: string s})
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };

        // Act
        CompareMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"a\".localeCompare(\"b\")", builder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithTrueBooleanIgnoreCaseArgument_AppendsLocalCompareWithAccentSensitivity()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Compare),
            new[] { typeof(string), typeof(string), typeof(bool) })!;
        var expression = Expression.Call(null, method, new[]
        {
            Expression.Constant("a"),
            Expression.Constant("b"),
            Expression.Constant(true)
        });
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: string s})
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };

        // Act
        CompareMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"a\".localeCompare(\"b\",undefined,{sensitivity:'accent'})", builder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithFalseBooleanIgnoreCaseArgument_AppendsLocaleCompare()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Compare),
            new[] { typeof(string), typeof(string), typeof(bool) })!;
        var expression = Expression.Call(null, method, new[]
        {
            Expression.Constant("a"),
            Expression.Constant("b"),
            Expression.Constant(false)
        });
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };
        
        // Act
        CompareMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"a\".localeCompare(\"b\")", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCultureIgnoreCase)]
    [InlineData(StringComparison.InvariantCultureIgnoreCase)]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void TranslateMethodCall_WithIgnoreCaseStringComparison_AppendsLocaleCompareWithAccentSensitivity(StringComparison ignoreCaseComparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Compare),
            new[] { typeof(string), typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(null, method, new[]
        {
            Expression.Constant("a"),
            Expression.Constant("b"),
            Expression.Constant(ignoreCaseComparison)
        });
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };
        
        // Act
        CompareMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"a\".localeCompare(\"b\",undefined,{sensitivity:'accent'})", builder.ToString());
    }

    [Theory]
    [InlineData(StringComparison.CurrentCulture)]
    [InlineData(StringComparison.InvariantCulture)]
    [InlineData(StringComparison.Ordinal)]
    public void TranslateMethodCall_WithStringComparison_AppendsLocaleCompare(StringComparison comparison)
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Compare),
            new[] { typeof(string), typeof(string), typeof(StringComparison) })!;
        var expression = Expression.Call(null, method, new[]
        {
            Expression.Constant("a"),
            Expression.Constant("b"),
            Expression.Constant(comparison)
        });
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
        };
        
        // Act
        CompareMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"a\".localeCompare(\"b\")", builder.ToString());
    }

    [Fact]
    public void TranslateMethodCall_WithCultureInfo_AppendsLocaleCompareWithCulture()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Compare),
            new[] { typeof(string), typeof(string), typeof(bool), typeof(CultureInfo) })!;
        var expression = Expression.Call(null, method, new[]
        {
            Expression.Constant("a"),
            Expression.Constant("b"),
            Expression.Constant(false),
            Expression.Constant(new CultureInfo("en-US"))
        });
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            } else if (next is ConstantExpression { Value: CultureInfo cultureInfo })
            {
                builder.Append('\"');
                builder.Append(cultureInfo.TwoLetterISOLanguageName);
                builder.Append('\"');
            }
        };

        // Act
        CompareMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);
        
        // Assert
        Assert.Equal("\"a\".localeCompare(\"b\",\"en\")", builder.ToString());
    }

    [Fact]
    public void
        TranslateMethodCall_WithCultureInfoAndBooleanIgnoreCase_AppendsLocaleCompareWithCultureAndAccentSensitivity()
    {
        // Arrange
        var method = typeof(string).GetMethod(nameof(string.Compare),
            new[] { typeof(string), typeof(string), typeof(bool), typeof(CultureInfo) })!;
        var expression = Expression.Call(null, method, new[]
        {
            Expression.Constant("a"),
            Expression.Constant("b"),
            Expression.Constant(true),
            Expression.Constant(new CultureInfo("en-US"))
        });
        var builder = new StringBuilder();
        ProcessExpression processExpression = (next) =>
        {
            if (next is ConstantExpression { Value: string s })
            {
                builder.Append('\"');
                builder.Append(s);
                builder.Append('\"');
            }
            else if (next is ConstantExpression { Value: CultureInfo cultureInfo })
            {
                builder.Append('\"');
                builder.Append(cultureInfo.TwoLetterISOLanguageName);
                builder.Append('\"');
            }
        };

        // Act
        CompareMethodCallTranslator.TranslateMethodCall(builder, expression, processExpression);

        // Assert
        Assert.Equal("\"a\".localeCompare(\"b\",\"en\",{sensitivity:'accent'})", builder.ToString());
    }


}