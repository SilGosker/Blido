using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

public interface IMethodCallTranslatorFactory : IReadOnlyDictionary<MethodInfo, TranslateMethodCall>
{
    public void AddCustomMethodTranslator<TTranslator>() where TTranslator : IMethodCallTranslator; 
    public void AddCustomMethodTranslator(MethodInfo method, TranslateMethodCall translateMethod);
    internal void Confirm();
}