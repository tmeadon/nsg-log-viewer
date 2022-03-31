namespace NsgLogViewer.Services;

public static class FlowParser
{
    public static IEnumerable<Flow> Parse(FlowLogFile flowLogFile)
    {
        var flows = new List<Flow>();

        foreach (var record in flowLogFile.Records)
        {
            foreach (var perRuleFlow in record.Properties.Flows)
            {
                foreach (var perMacFlow in perRuleFlow.Flows)
                {
                    foreach (var flowTuple in perMacFlow.FlowTuples)
                    {
                        var thisFlowTuple = FlowTuple.FromString(flowTuple);

                        var flow = new Flow
                        {
                            Time = record.Time,
                            MacAddress = record.MacAddress,
                            ResourceId = record.ResourceId,
                            SourceAddress = thisFlowTuple.SourceAddress,
                            SourcePort = thisFlowTuple.SourcePort,
                            DestinationAddress = thisFlowTuple.DestinationAddress,
                            DestinationPort = thisFlowTuple.DestinationPort,
                            Options = thisFlowTuple.Options
                        };

                        flows.Add(flow);
                    }
                }
            }
        }

        return flows;
    }
}