using System.Reflection;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation;

public interface IMethodCallTranslator
{
    public static abstract TranslateMethodCall TranslateMethodCall { get; }
    public static abstract MethodInfo[] SupportedMethods { get; }
}