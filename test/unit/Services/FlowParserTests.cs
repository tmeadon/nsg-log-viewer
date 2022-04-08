using NsgLogViewer.Models;
using NsgLogViewer.Services;
using NsgLogViewer.UnitTests.Helpers;
using System.Linq;
using Xunit;

namespace NsgLogViewer.UnitTests.Services;

public class FlowParserTests
{
    private readonly int dummyBrowserFileHashCode = 123;

    [Fact]
    public void FlowParserCorrectlyParsesFlowLogFile()
    {
        var flows = DummyFlowGenerator.GenerateDummyFlows(10, dummyBrowserFileHashCode);
        var flowLogFile = DummyFlowLogFileBuilder.Build(flows);
        var result = FlowParser.Parse(flowLogFile, dummyBrowserFileHashCode);
        Assert.Equal<Flow>(flows, result, new FlowComparer());
    }
}