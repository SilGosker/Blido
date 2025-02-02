namespace BlazorIndexedOrm.Core.Transaction;

public class JsMethodNameConstantsTests
{
    [Fact]
    public void RootObjectName_EqualsBlazorIndexedOrmDot()
    {
        Assert.Equal("blazorIndexedOrm.", JsMethodNameConstants.RootObjectName);
    }

    [Fact]
    public void FirstOrDefault_EqualsBlazorIndexedOrmDotFirstOrDefault()
    {
        Assert.Equal("blazorIndexedOrm.firstOrDefault", JsMethodNameConstants.FirstOrDefault);
    }

    [Fact]
    public void GetVersion_EqualsBlazorIndexedOrmDotGetVersion()
    {
        Assert.Equal("blazorIndexedOrm.getVersion", JsMethodNameConstants.GetVersion);
    }
}