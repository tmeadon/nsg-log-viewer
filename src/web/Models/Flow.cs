namespace NsgLogViewer.Models;

public class Flow
{
    public DateTime Time { get; init; }
    public string MacAddress { get; init; } = "";
    public string NsgName { get; init; } = "";
    public string RuleName { get; init; } = "";
    public string SourceAddress { get; init; } = "";
    public int SourcePort { get; init; }
    public string DestinationAddress { get; init; } = "";
    public int DestinationPort { get; init; }
    public string Options { get; init; } = "";
}