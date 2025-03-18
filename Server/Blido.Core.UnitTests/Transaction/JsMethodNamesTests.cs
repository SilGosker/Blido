using Blido.Core.Helpers;

namespace Blido.Core.Transaction;

public class JsMethodNamesTests
{
    [Fact]
    public void RootObjectName_EqualsBlidoDot()
    {
        Assert.Equal("blido.", JsMethodNames.RootObjectName);
    }

    [Fact]
    public void FirstOrDefault_EqualsBlidoDotFirstOrDefault()
    {
        Assert.Equal("blido.firstOrDefault", JsMethodNames.FirstOrDefault);
    }

    [Fact]
    public void GetVersion_EqualsBlidoDotGetVersion()
    {
        Assert.Equal("blido.getVersion", JsMethodNames.GetVersion);
    }

    [Fact]
    public void ToArray_EqualsBlidoDotToArray()
    {
        Assert.Equal("blido.toArray", JsMethodNames.ToArray);
    }

    [Fact]
    public void First_EqualsBlidoDotFirst()
    {
        Assert.Equal("blido.first", JsMethodNames.First);
    }

    [Fact]
    public void Find_EqualsBlidoDotFind()
    {
        Assert.Equal("blido.find", JsMethodNames.Find);
    }

    [Fact]
    public void Last_EqualsBlidoDotLast()
    {
        Assert.Equal("blido.last", JsMethodNames.Last);
    }

    [Fact]
    public void LastOrDefault_EqualsBlidoDotLastOrDefault()
    {
        Assert.Equal("blido.lastOrDefault", JsMethodNames.LastOrDefault);
    }

    [Fact]
    public void Single_EqualsBlidoDotSingle()
    {
        Assert.Equal("blido.single", JsMethodNames.Single);
    }

    [Fact]
    public void SingleOrDefault_EqualsBlidoDotSingleOrDefault()
    {
        Assert.Equal("blido.singleOrDefault", JsMethodNames.SingleOrDefault);
    }

    [Fact]
    public void Count_EqualsBlidoDotCount()
    {
        Assert.Equal("blido.count", JsMethodNames.Count);
    }

    [Fact]
    public void Any_EqualsBlidoDotAny()
    {
        Assert.Equal("blido.any", JsMethodNames.Any);
    }

    [Fact]
    public void All_EqualsBlidoDotAll()
    {
        Assert.Equal("blido.all", JsMethodNames.All);
    }

    [Fact]
    public void Sum_EqualsBlidoDotSum()
    {
        Assert.Equal("blido.sum", JsMethodNames.Sum);
    }

    [Fact]
    public void Average_EqualsBlidoDotAverage()
    {
        Assert.Equal("blido.average", JsMethodNames.Average);
    }

    [Fact]
    public void Min_EqualsBlidoDotMin()
    {
        Assert.Equal("blido.min", JsMethodNames.Min);
    }

    [Fact]
    public void Max_EqualsBlidoDotMax()
    {
        Assert.Equal("blido.max", JsMethodNames.Max);
    }

    [Fact]
    public void Insert_EqualsBlidoDotInsert()
    {
        Assert.Equal("blido.insert", JsMethodNames.Insert);
    }

    [Fact]
    public void Update_EqualsBlidoDotUpdate()
    {
        Assert.Equal("blido.update", JsMethodNames.Update);
    }

    [Fact]
    public void Delete_EqualsBlidoDotDelete()
    {
        Assert.Equal("blido.delete", JsMethodNames.Delete);
    }

    [Fact]
    public void MaterializerMethodNames_ContainAllIObjectStoreMethodNames()
    {
        // Arrange
        var methodInfos = typeof(IObjectStore<string>).GetMethods();
        var methodNames = methodInfos.Select(m => m.Name).ToList();

        // Act
        var materializerMethodNames = JsMethodNames.MaterializerMethodNames.Keys.ToList();

        // Assert
        foreach (string methodName in methodNames)
        {
            Assert.Contains(methodName, materializerMethodNames);
        }
    }
}