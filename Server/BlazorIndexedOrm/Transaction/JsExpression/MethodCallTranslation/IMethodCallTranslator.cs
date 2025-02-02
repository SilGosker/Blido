using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

public interface IMethodCallTranslator
{
    public static abstract TranslateMethodCall TranslateMethodCall { get; }
    public static abstract MethodInfo[] SupportedMethods { get; }
}