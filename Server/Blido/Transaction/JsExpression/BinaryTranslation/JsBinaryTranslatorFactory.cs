using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Blido.Core.Helpers;
using Blido.Core.Transaction.JsExpression.BinaryTranslation.Default;

namespace Blido.Core.Transaction.JsExpression.BinaryTranslation;

public class JsBinaryTranslatorFactory : IBinaryTranslatorFactory
{
    private IReadOnlyDictionary<BinaryExpression, TranslateBinary> _translators;
    private IReadOnlyDictionary<ExpressionType, TranslateBinary> _defaultTranslators;

    public JsBinaryTranslatorFactory()
    {
        var translators = new Dictionary<BinaryExpression, TranslateBinary>(new BinaryExpressionComparer());
        var defaultTranslators = new Dictionary<ExpressionType, TranslateBinary>();

        var blazorIndexedOrmTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.GetInterface(nameof(IBinaryTranslator)) != null);
        
        string defaultNamespace = typeof(EqualBinaryTranslator).Namespace!;

        foreach (Type type in blazorIndexedOrmTypes.Where(x => x.Namespace != defaultNamespace))
        {
            if (type.GetProperty(nameof(IBinaryTranslator.SupportedBinaries))!.GetValue(null)
                is not BinaryExpression[] supportedBinaries) continue;

            if (type.GetProperty(nameof(IBinaryTranslator.TranslateBinary))!.GetValue(null)
                is not TranslateBinary translateMember) continue;
            
            foreach (var binaryExpression in supportedBinaries)
            {
                translators.Add(binaryExpression, translateMember);
            }
        }

        foreach (Type type in blazorIndexedOrmTypes.Where(x => x.Namespace == defaultNamespace))
        {
            if (type.GetProperty(nameof(IBinaryTranslator.SupportedBinaries))!.GetValue(null)
                is not BinaryExpression[] supportedBinaries) continue;

            if (type.GetProperty(nameof(IBinaryTranslator.TranslateBinary))!.GetValue(null)
                is not TranslateBinary translateMember) continue;

            foreach (var binaryExpression in supportedBinaries)
            {
                defaultTranslators.Add(binaryExpression.NodeType, translateMember);
            }
        }

        _translators = translators;
        _defaultTranslators = defaultTranslators;
    }

    public void AddCustomBinaryTranslator(BinaryExpression tryMatchBinary, TranslateBinary translator)
    {
        ThrowHelper.ThrowDictionaryIsNotReadonlyException(_translators, out var translators);
        translators[tryMatchBinary] = translator;
    }

    public void AddCustomBinaryTranslator<TTranslator>() where TTranslator : IBinaryTranslator
    {
        foreach (var binaryExpression in TTranslator.SupportedBinaries)
        {
            AddCustomBinaryTranslator(binaryExpression, TTranslator.TranslateBinary);
        }
    }

    public void Confirm()
    {
        _translators = _translators.ToFrozenDictionary();
        _defaultTranslators = _defaultTranslators.ToFrozenDictionary();
    }

    public bool TryGetValue(BinaryExpression key, [MaybeNullWhen(false)] out TranslateBinary value)
    {
        if (_translators.TryGetValue(key, out value))
        {
            return true;
        }

        return _defaultTranslators.TryGetValue(key.NodeType, out value);
    }

    public IEnumerator<KeyValuePair<BinaryExpression, TranslateBinary>> GetEnumerator()
    {
        return _translators.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _translators.Count + _defaultTranslators.Count;
    public bool ContainsKey(BinaryExpression key)
    {
        return _translators.ContainsKey(key) || _defaultTranslators.ContainsKey(key.NodeType);
    }

    public TranslateBinary this[BinaryExpression key] => _translators[key];

    public IEnumerable<BinaryExpression> Keys => _translators.Keys;
    public IEnumerable<TranslateBinary> Values => _translators.Values;
}