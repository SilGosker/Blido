using System.Collections;
using System.Collections.Frozen;
using System.Reflection;
using Blido.Core.Helpers;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation;

public class JsMethodCallTranslatorFactory : IMethodCallTranslatorFactory
{
    private IReadOnlyDictionary<MethodInfo, TranslateMethodCall> _translators;

    public JsMethodCallTranslatorFactory()
    {
        var translators = new Dictionary<MethodInfo, TranslateMethodCall>(new MethodInfoEqualityComparer());
        var blazorIndexedOrmTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in blazorIndexedOrmTypes.Where(x => x.IsClass && x.GetInterface(nameof(IMethodCallTranslator)) != null))
        {
            if (type.GetProperty(nameof(IMethodCallTranslator.SupportedMethods))!.GetValue(null)
                is not MethodInfo[] supportedMethods) continue;

            if (type.GetProperty(nameof(IMethodCallTranslator.TranslateMethodCall))!.GetValue(null)
                is not TranslateMethodCall translateMethod) continue;

            foreach (MethodInfo method in supportedMethods)
            {
                translators.Add(method, translateMethod);
            }
        }

        _translators = translators;
    }

    public void AddCustomMethodTranslator<TTranslator>() where TTranslator : IMethodCallTranslator
    {
        foreach (MethodInfo supportedMethod in TTranslator.SupportedMethods)
        {
            AddCustomMethodTranslator(supportedMethod, TTranslator.TranslateMethodCall);
        }
    }

    public void AddCustomMethodTranslator(MethodInfo method, TranslateMethodCall translateMethod)
    {
        ThrowHelper.ThrowDictionaryIsNotReadonlyException(_translators, out Dictionary<MethodInfo, TranslateMethodCall> dict);
        dict[method] = translateMethod;
    }

    void IMethodCallTranslatorFactory.Confirm()
    {
        _translators = _translators.ToFrozenDictionary();
    }

    public bool ContainsKey(MethodInfo key)
    {
        return _translators.ContainsKey(key);
    }

    public bool TryGetValue(MethodInfo method, out TranslateMethodCall translateMethod)
    {
        return _translators.TryGetValue(method, out translateMethod!);
    }

    public TranslateMethodCall this[MethodInfo key] => _translators[key];

    public IEnumerable<MethodInfo> Keys => _translators.Keys;

    public IEnumerable<TranslateMethodCall> Values => _translators.Values;

    public IEnumerator<KeyValuePair<MethodInfo, TranslateMethodCall>> GetEnumerator()
    {
        return _translators.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_translators).GetEnumerator();
    }

    public int Count => _translators.Count;
}