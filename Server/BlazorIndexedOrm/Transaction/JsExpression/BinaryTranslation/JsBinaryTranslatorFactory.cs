using System.Collections;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using BlazorIndexedOrm.Core.Helpers;
using BlazorIndexedOrm.Core.Transaction.JsExpression.MemberTranslation;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.BinaryTranslation;

public class JsBinaryTranslatorFactory : IBinaryTranslatorFactory
{
    private IReadOnlyDictionary<TranslateBinaryHash, TranslateBinary> _translators;
    private readonly List<TryMatchBinary> _matchers = new();

    public JsBinaryTranslatorFactory()
    {
        var translators = new Dictionary<TranslateBinaryHash, TranslateBinary>();
        var blazorIndexedOrmTypes = Assembly.GetExecutingAssembly().GetTypes();

        foreach (Type type in blazorIndexedOrmTypes.Where(x => x.IsClass && x.GetInterface(nameof(IBinaryTranslator)) != null))
        {
            if (type.GetProperty(nameof(IBinaryTranslator.SupportedHashes))!.GetValue(null)
                is not TranslateBinaryHash[] supportedMembers) continue;

            if (type.GetProperty(nameof(IBinaryTranslator.TranslateBinary))!.GetValue(null)
                is not TranslateBinary translateMember) continue;

            if (type.GetProperty(nameof(IBinaryTranslator.TryMatchBinary))!.GetValue(null)
                is not TryMatchBinary tryMatchBinary) continue;
            
            _matchers.Add(tryMatchBinary);

            foreach (var binaryHash in supportedMembers)
            {
                translators.Add(binaryHash, translateMember);
            }
        }

        _translators = translators;
    }
    public void AddCustomBinaryTranslator(TryMatchBinary tryMatchBinary, TranslateBinaryHash matchingHash, TranslateBinary translator)
    {
        ThrowHelper.ThrowDictionaryIsNotReadonlyException(_translators, out var translators);
        _matchers.Add(tryMatchBinary);
        translators[matchingHash] = translator;
    }

    public void AddCustomBinaryTranslator<TTranslator>() where TTranslator : IBinaryTranslator
    {
        foreach (var hash in TTranslator.SupportedHashes)
        {
            AddCustomBinaryTranslator(TTranslator.TryMatchBinary, hash, TTranslator.TranslateBinary);
        }
    }

    public void Confirm()
    {
        _translators = _translators.ToFrozenDictionary();
        _matchers.Reverse();
    }

    public bool TryGetValue(BinaryExpression key, [MaybeNullWhen(false)] out TranslateBinary value)
    {
        TranslateBinaryHash? hash = null;
        value = null;

        var span = CollectionsMarshal.AsSpan(_matchers);
        foreach (var matcher in span)
        {
            if (matcher(key, out var matcherHash))
            {
                hash = matcherHash;
                break;
            }
        }

        if (hash is null)
        {
            return false;
        }

        if (!_translators.TryGetValue(hash.Value, out var translators))
        {
            return false;
        }

        value = translators;
        return true;
    }

    public IEnumerator<KeyValuePair<TranslateBinaryHash, TranslateBinary>> GetEnumerator()
    {
        return _translators.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _translators.Count;
    public bool ContainsKey(TranslateBinaryHash key)
    {
        return _translators.ContainsKey(key);
    }

    public bool TryGetValue(TranslateBinaryHash key, [MaybeNullWhen(false)] out TranslateBinary value)
    {
        return _translators.TryGetValue(key, out value);
    }

    public TranslateBinary this[TranslateBinaryHash key] => _translators[key];

    public IEnumerable<TranslateBinaryHash> Keys => _translators.Keys;
    public IEnumerable<TranslateBinary> Values => _translators.Values;
}