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
        var flows = DummyFlowGenerator.Generate(10, dummyBrowserFileHashCode);
        var flowLogFile = DummyFlowLogFileBuilder.Build(flows);
        var parser = new FlowLogFileParser();
        var result = parser.Parse(flowLogFile, dummyBrowserFileHashCode);
        Assert.Equal<Flow>(flows, result, new FlowComparer());
    }
}