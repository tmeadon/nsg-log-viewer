namespace NsgLogViewer.Models;

public class FlowTuple
{
    public string Id { get; init; } = "";
    public IPAddress SourceAddress { get; init; } = IPAddress.None;
    public int SourcePort { get; init; }
    public IPAddress DestinationAddress { get; init; } = IPAddress.None;
    public int DestinationPort { get; init; }
    public string Options { get; init; } = "";

    public static FlowTuple FromString(string flowTuple)
    {
        var elements = flowTuple.Split(',');

        return new FlowTuple
        {
            Id = elements[0],
            SourceAddress = IPAddress.Parse(elements[1]),
            DestinationAddress = IPAddress.Parse(elements[2]),
            SourcePort = int.Parse(elements[3]),
            DestinationPort = int.Parse(elements[4]),
            Options = string.Join(",", elements.Skip(5))
        };
    }
}