namespace Blido.Core.Helpers;

public class EntityWithMultipleIds
{
    public int Id { get; set; }
    // ReSharper disable InconsistentNaming
    public int ID { get; set; }
    public int iD { get; set; }
    public int id { get; set; }
    // ReSharper restore InconsistentNaming
}