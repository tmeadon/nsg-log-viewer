using NsgLogViewer.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace NsgLogViewer.UnitTests.Helpers;

public class FlowLogFileComparer : IEqualityComparer<FlowLogFile>
{
    public bool Equals(FlowLogFile? expected, FlowLogFile? actual)
    {
        if (expected == null && actual == null)
        {
            return true;
        }

        if (expected == null || actual == null)
        {
            return false;
        }

        var expectedStr = JsonSerializer.Serialize(expected);
        var actualStr = JsonSerializer.Serialize(actual);
        return expectedStr == actualStr;
    }

    public int GetHashCode(FlowLogFile obj)
    {
        return obj.GetHashCode();
    }
}