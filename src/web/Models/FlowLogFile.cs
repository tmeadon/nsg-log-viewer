namespace NsgLogViewer.Models;

public class FlowLogFile
{
    public List<FlowLogRecord> Records { get; init; } = new();

    public class FlowLogRecord
    {
        public DateTime Time { get; init; }
        public string MacAddress { get; init; } = "";
        public string ResourceId { get; init; } = "";
        public RecordProperties Properties { get; init; } = new();

        public class RecordProperties 
        {
            public List<PerRuleFlows> Flows { get; init; } = new();

            public class PerRuleFlows
            {
                public string Rule { get; init; } = ""; 
                public List<PerMacFlows> Flows { get; init; } = new();

                public class PerMacFlows
                {
                    public string Mac { get; init; } = "";
                    public List<string> FlowTuples { get; init; } = new List<string>();
                }
            }
        }
    }
}
