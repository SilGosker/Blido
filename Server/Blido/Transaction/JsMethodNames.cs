using System.Collections.Frozen;

namespace Blido.Core.Transaction;

public class JsMethodNames
{
    public const string RootObjectName = "blido.";
    public const string GetVersion = RootObjectName + "getVersion";
    public const string ToArray = RootObjectName + "toArray";
    public const string First = RootObjectName + "first";
    public const string FirstOrDefault = RootObjectName + "firstOrDefault";
    public const string Find = RootObjectName + "find";
    public const string Last = RootObjectName + "last";
    public const string LastOrDefault = RootObjectName + "lastOrDefault";
    public const string Single = RootObjectName + "single";
    public const string SingleOrDefault = RootObjectName + "singleOrDefault";
    public const string Count = RootObjectName + "count";
    public const string Any = RootObjectName + "any";
    public const string All = RootObjectName + "all";
    public const string Sum = RootObjectName + "sum";
    public const string Average = RootObjectName + "average";
    public const string Min = RootObjectName + "min";
    public const string Max = RootObjectName + "max";
    public const string Insert = RootObjectName + "insert";
    public const string Update = RootObjectName + "update";
    public const string Delete = RootObjectName + "delete";
    public const string Mutate = RootObjectName + "mutate";

    public static readonly FrozenDictionary<string, string> MaterializerMethodNames = new Dictionary<string, string>
    {
        {nameof(IObjectStore<string>.ToListAsync), ToArray},
        {nameof(IObjectStore<string>.ToArrayAsync), ToArray},
        {nameof(IObjectStore<string>.FirstAsync), First},
        {nameof(IObjectStore<string>.FirstOrDefaultAsync), FirstOrDefault},
        {nameof(IObjectStore<string>.FindAsync), Find},
        {nameof(IObjectStore<string>.LastAsync), Last},
        {nameof(IObjectStore<string>.LastOrDefaultAsync), LastOrDefault},
        {nameof(IObjectStore<string>.SingleAsync), Single},
        {nameof(IObjectStore<string>.SingleOrDefaultAsync), SingleOrDefault},
        {nameof(IObjectStore<string>.CountAsync), Count},
        {nameof(IObjectStore<string>.LongCountAsync), Count},
        {nameof(IObjectStore<string>.AnyAsync), Any},
        {nameof(IObjectStore<string>.AllAsync), All},
        {nameof(IObjectStore<string>.SumAsync), Sum},
        {nameof(IObjectStore<string>.AverageAsync), Average},
        {nameof(IObjectStore<string>.MinAsync), Min},
        {nameof(IObjectStore<string>.MaxAsync), Max},
        {nameof(IMutableObjectStore<string>.InsertAsync), Insert},
        {nameof(IMutableObjectStore<string>.UpdateAsync), Update},
        {nameof(IMutableObjectStore<string>.DeleteAsync), Delete}
    }.ToFrozenDictionary();
}