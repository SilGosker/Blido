using System.Reflection;

namespace Blido.Core.Transaction.Mutation.KeyGeneration;

public delegate void GenerateKeyDelegate(object entity, PropertyInfo propertyInfo);
