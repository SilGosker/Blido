namespace Blido.Core.Transaction;

public class JsMethodNameConstantsTests
{
    [Fact]
    public void RootObjectName_EqualsBlidoDot()
    {
        Assert.Equal("blazorIndexedOrm.", JsMethodNameConstants.RootObjectName);
    }

    [Fact]
    public void FirstOrDefault_EqualsBlidoDotFirstOrDefault()
    {
        Assert.Equal("blazorIndexedOrm.firstOrDefault", JsMethodNameConstants.FirstOrDefault);
    }

    [Fact]
    public void GetVersion_EqualsBlidoDotGetVersion()
    {
        Assert.Equal("blazorIndexedOrm.getVersion", JsMethodNameConstants.GetVersion);
    }
}