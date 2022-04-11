using System;
using System.Collections.Generic;
using NsgLogViewer.Models;
using NsgLogViewer.Services;

namespace NsgLogViewer.UnitTests.Fakes;

public class FlowLogFileParserFake : FlowLogFileParser
{
    public IEnumerable<Flow> Flows { get; set; } = new List<Flow>();
    public bool WasCalled { get; set; } = false;
    public FlowLogFile FlowLogFileCalledWith { get; set; } = new();
    public int BrowserFileHashCodeCalledWith { get; set; }

    public override IEnumerable<Flow> Parse(FlowLogFile flowLogFile, int? browserFileHashCode)
    {
        WasCalled = true;
        FlowLogFileCalledWith = flowLogFile;
        BrowserFileHashCodeCalledWith = browserFileHashCode ?? throw new ArgumentNullException(nameof(browserFileHashCode));
        return Flows;
    }
}