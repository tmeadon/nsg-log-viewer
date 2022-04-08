using NsgLogViewer.Models;
using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NsgLogViewer.UnitTests.Helpers;

public static class DummyFlowLogFileBuilder
{
    public static FlowLogFile Build(IEnumerable<Flow> flows)
    {
        var flowLogFile = new FlowLogFile
        {
            Records = new List<FlowLogFile.FlowLogRecord>()
        };

        foreach (var time in flows.Select(f => f.Time).Distinct())
        {
            foreach (var nsg in flows.Where(f => f.Time == time).Select(f => f.NsgName).Distinct())
            {
                foreach (var mac in flows.Where(f => f.Time == time && f.NsgName == nsg).Select(f => f.MacAddress).Distinct())
                {
                    var record = BuildFlowLogRecord(time, mac, nsg, flows);
                    flowLogFile.Records.Add(record);
                }
            }
        }

        return flowLogFile;
    }

    private static FlowLogFile.FlowLogRecord BuildFlowLogRecord(DateTime time, string mac, string nsg, IEnumerable<Flow> flows)
    {
        var record = new FlowLogFile.FlowLogRecord
        {
            Time = time,
            MacAddress = mac,
            ResourceId = GenerateDummyNsgResourceId(nsg),
            Properties = new FlowLogFile.FlowLogRecord.RecordProperties
            {
                Flows = new List<FlowLogFile.FlowLogRecord.RecordProperties.PerRuleFlows>()
            }
        };

        foreach (var rule in flows.Where(f => f.Time == time && f.NsgName == nsg && f.MacAddress == mac).Select(f => f.RuleName).Distinct())
        {
            var perRuleFlows = flows.Where(f => f.Time == time && f.NsgName == nsg && f.MacAddress == mac && f.RuleName == rule);
            record.Properties.Flows.Add(BuildPerRuleFlowRecord(perRuleFlows, rule));
        }

        return record;
    }

    private static string GenerateDummyNsgResourceId(string nsgName)
    {
        return $"/subscriptions/{Guid.NewGuid()}/resourceGroups/{Guid.NewGuid()}/providers/Microsoft.Network/networkSecurityGroups/{nsgName}";
    }

    private static FlowLogFile.FlowLogRecord.RecordProperties.PerRuleFlows BuildPerRuleFlowRecord(IEnumerable<Flow> flows, string rule)
    {
        var perRuleFlow = new FlowLogFile.FlowLogRecord.RecordProperties.PerRuleFlows
        {
            Rule = rule,
            Flows = new List<FlowLogFile.FlowLogRecord.RecordProperties.PerRuleFlows.PerMacFlows>()
        };

        foreach (var mac in flows.Select(f => f.MacAddress).Distinct())
        {
            var perMacFlow = new FlowLogFile.FlowLogRecord.RecordProperties.PerRuleFlows.PerMacFlows
            {
                Mac = mac,
                FlowTuples = new List<string>()
            };

            foreach (var flow in flows.Where(f => f.MacAddress == mac))
            {
                perMacFlow.FlowTuples.Add(BuildFlowString(flow));
            }

            perRuleFlow.Flows.Add(perMacFlow);
        }

        return perRuleFlow;
    }

    private static string BuildFlowString(Flow flow)
    {
        var flowString = new StringBuilder();

        flowString.Append(flow.MacAddress);
        flowString.Append(",");
        flowString.Append(flow.SourceAddress);
        flowString.Append(",");
        flowString.Append(flow.DestinationAddress);
        flowString.Append(",");
        flowString.Append(flow.SourcePort);
        flowString.Append(",");
        flowString.Append(flow.DestinationPort);
        flowString.Append(",");
        flowString.Append(flow.Options);

        return flowString.ToString();
    }
}

