using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation;

public interface IUnaryTranslator
{
    public static abstract TranslateUnary TranslateUnary { get; }
    public static abstract UnaryExpression[] SupportedUnaries { get; }
}