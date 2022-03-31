namespace NsgLogViewer.Models;

public class Flow
{
    public DateTime Time { get; init; }
    public string MacAddress { get; init; } = "";
    public string ResourceId { get; init; } = "";
    public IPAddress SourceAddress { get; init; } = IPAddress.None;
    public int SourcePort { get; init; }
    public IPAddress DestinationAddress { get; init; } = IPAddress.None;
    public int DestinationPort { get; init; }
    public string Options { get; init; } = "";
}