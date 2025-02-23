using System.Reflection;
using Blido.Core.Transaction.JsExpression.MethodCallTranslation;

namespace Blido.Core.UnitTests.Mock.Transaction.JsExpression.MethodCallTranslation;

public class MockUnsupportedMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, expression, processExpression) =>
    {
        sb.Append(".IsInterned()");
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.IsInterned), new [] { typeof(string) })!
    };
#nullable restore
}