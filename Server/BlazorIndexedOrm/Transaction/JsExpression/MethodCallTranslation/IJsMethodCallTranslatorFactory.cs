using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

public interface IJsMethodCallTranslatorFactory
{
    public bool TryGetValue(MethodInfo method, out TranslateMethodCall translateMethod);
    public void AddCustomMethodTranslator<TTranslator>() where TTranslator : IMethodCallTranslator; 
    public void AddCustomMethodTranslator(MethodInfo method, TranslateMethodCall translateMethod);
    internal void Confirm();
}