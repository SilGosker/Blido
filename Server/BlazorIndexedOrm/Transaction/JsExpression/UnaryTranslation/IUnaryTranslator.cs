using System.Linq.Expressions;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation;

public interface IUnaryTranslator
{
    public static abstract TranslateUnary TranslateUnary { get; }
    public static abstract UnaryExpression[] SupportedUnaries { get; }
}