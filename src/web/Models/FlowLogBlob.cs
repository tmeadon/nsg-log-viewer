namespace NsgLogViewer.Models;

public record FlowLogBlob
{
    public Nsg Nsg { get; set; } = new Nsg();
    public DateTime StartTimeUTC { get; set; }
    public string MacAddress { get; set; } = "";
    public DateTimeOffset? LastModifiedUTC { get; set; }
}