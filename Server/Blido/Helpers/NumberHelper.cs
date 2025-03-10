using System.Numerics;
using System.Reflection;

namespace Blido.Core.Helpers;

internal static class NumberHelper
{
    internal static Type[] NumberTypes => new[]
    {
        typeof(int),
        typeof(long),
        typeof(short),
        typeof(byte),
        typeof(float),
        typeof(double),
        typeof(decimal),
        typeof(uint),
        typeof(ulong),
        typeof(ushort),
        typeof(sbyte)
    };
}