using System.Linq.Expressions;

namespace Blido.Core.Transaction.JsExpression.UnaryTranslation;

public interface IUnaryTranslatorFactory : IReadOnlyDictionary<UnaryExpression, TranslateUnary>
{
    public void AddUnaryTranslator(UnaryExpression unaryExpression, TranslateUnary translator);
    public void AddUnaryTranslator<TTranslator>() where TTranslator : IUnaryTranslator;
    public void Confirm();
}