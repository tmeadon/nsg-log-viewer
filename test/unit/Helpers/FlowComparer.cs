using NsgLogViewer.Models;
using System;
using System.Collections.Generic;

namespace NsgLogViewer.UnitTests.Helpers;

public class FlowComparer : IEqualityComparer<Flow>
{
    public bool Equals(Flow? expected, Flow? actual)
    {
        return expected != null && actual != null &&
            expected.Time == actual.Time &&
            expected.MacAddress == actual.MacAddress &&
            expected.NsgName == actual.NsgName &&
            expected.RuleName == actual.RuleName &&
            expected.SourceAddress == actual.SourceAddress &&
            expected.SourcePort == actual.SourcePort &&
            expected.DestinationAddress == actual.DestinationAddress &&
            expected.DestinationPort == actual.DestinationPort &&
            expected.Options == actual.Options &&
            expected.BrowserFileHashCode == actual.BrowserFileHashCode;
    }

    public int GetHashCode(Flow flow)
    {
        return flow.GetHashCode();
    }
}