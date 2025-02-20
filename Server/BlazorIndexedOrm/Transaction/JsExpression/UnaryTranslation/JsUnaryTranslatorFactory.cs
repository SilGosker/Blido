using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using BlazorIndexedOrm.Core.Helpers;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.UnaryTranslation;

public class JsUnaryTranslatorFactory : IUnaryTranslatorFactory
{
    private IReadOnlyDictionary<UnaryExpression, TranslateUnary> _translators;

    public JsUnaryTranslatorFactory()
    {
        var translators = new Dictionary<UnaryExpression, TranslateUnary>(new UnaryExpressionEqualityComparer());
        var blazorIndexedOrmTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in blazorIndexedOrmTypes
                     .Where(x => x.IsClass && x.GetInterface(nameof(IUnaryTranslator)) != null))
        {
            if (type.GetProperty(nameof(IUnaryTranslator.SupportedUnaries))!.GetValue(null)
                is not UnaryExpression[] supportedUnaries) continue;

            if (type.GetProperty(nameof(IUnaryTranslator.TranslateUnary))!.GetValue(null)
                is not TranslateUnary translateMethod) continue;

            foreach (var expression in supportedUnaries)
            {
                translators.Add(expression, translateMethod);
            }
        }

        _translators = translators;
    }

    public IEnumerator<KeyValuePair<UnaryExpression, TranslateUnary>> GetEnumerator()
    {
        return _translators.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _translators.Count;
    public bool ContainsKey(UnaryExpression key)
    {
        return _translators.ContainsKey(key);
    }

    public bool TryGetValue(UnaryExpression key, [MaybeNullWhen(false)] out TranslateUnary value)
    {
        return _translators.TryGetValue(key, out value);
    }

    public TranslateUnary this[UnaryExpression key] => _translators[key];

    public IEnumerable<UnaryExpression> Keys => _translators.Keys;
    public IEnumerable<TranslateUnary> Values => _translators.Values;
    public void AddUnaryTranslator(UnaryExpression unaryExpression, TranslateUnary translator)
    {
        ThrowHelper.ThrowDictionaryIsNotReadonlyException(_translators, out var translators);
        translators[unaryExpression] = translator;
    }

    public void AddUnaryTranslator<TTranslator>() where TTranslator : IUnaryTranslator
    {
        foreach (UnaryExpression unaryExpression in TTranslator.SupportedUnaries)
        {
            AddUnaryTranslator(unaryExpression, TTranslator.TranslateUnary);
        }
    }

    public void Confirm()
    {
        _translators = _translators.ToFrozenDictionary();
    }
}