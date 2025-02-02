using System.Reflection;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

namespace BlazorIndexedOrm.Core.UnitTests.Mock.Transaction.JsExpression.MethodCallTranslation;

public class MockContainsMethodCallTranslator : IMethodCallTranslator
{
    public static TranslateMethodCall TranslateMethodCall => (sb, _, _) =>
    {
        sb.Append("mockContains()");
    };

    #nullable disable
    public static MethodInfo[] SupportedMethods => new[]
    {
        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!
    };
    #nullable restore
}