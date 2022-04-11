using NsgLogViewer.Models;
using NsgLogViewer.Services;
using NsgLogViewer.UnitTests.Fakes;
using NsgLogViewer.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace NsgLogViewer.UnitTests.Services;

public class BrowserFileLoaderTests
{
    private FlowLogFileParserFake flowLogFileParserFake;
    private IEnumerable<Flow> dummyFlows;
    private static readonly int dummyBrowserFileHashCode = 123;

    public BrowserFileLoaderTests()
    {
        dummyFlows = DummyFlowGenerator.Generate(10, dummyBrowserFileHashCode);
        flowLogFileParserFake = BuildFlowLogFileParserFake(dummyFlows);
    }

    private FlowLogFileParserFake BuildFlowLogFileParserFake(IEnumerable<Flow> dummyFlows)
    {
        return new FlowLogFileParserFake()
        {
            Flows = dummyFlows
        };
    }

    [Fact]
    public async Task LoadBrowserFilesThrowsIfNotJsonAsync()
    {
        var browserFile = new BrowserFileFake()
        {
            Name = "test.txt",
            ContentType = "text/plain",
            LastModified = DateTimeOffset.Now,
            Size = 123,
            HashCode = dummyBrowserFileHashCode
        };

        var browserFileLoader = new BrowserFileLoader(browserFile, flowLogFileParserFake);

        await Assert.ThrowsAsync<BrowserFileLoaderException>(() => browserFileLoader.LoadAsync());
    }

    [Fact]
    public async Task LoadBrowserFilesThrowsIfItContainsNoRecords()
    {
        var dummyFlowLogFile = DummyFlowLogFileBuilder.Build(new List<Flow>());
        var browserFile = BuildBrowserFileFake("test.json", "application/json", DateTimeOffset.Now, 123, dummyFlowLogFile);
        var browserFileLoader = new BrowserFileLoader(browserFile, flowLogFileParserFake);
    
        await Assert.ThrowsAsync<BrowserFileLoaderException>(() => browserFileLoader.LoadAsync());
    }

    [Fact]
    public async Task LoadBrowserFilesCallsFlowParserCorrectly()
    {
        var dummyFlowLogFile = DummyFlowLogFileBuilder.Build(dummyFlows);
        var browserFile = BuildBrowserFileFake("test.json", "application/json", DateTimeOffset.Now, 123, dummyFlowLogFile);
        var browserFileLoader = new BrowserFileLoader(browserFile, flowLogFileParserFake);

        await browserFileLoader.LoadAsync();

        Assert.True(flowLogFileParserFake.WasCalled);
        Assert.Equal<FlowLogFile>(dummyFlowLogFile, flowLogFileParserFake.FlowLogFileCalledWith, new FlowLogFileComparer());
        Assert.Equal(dummyBrowserFileHashCode, flowLogFileParserFake.BrowserFileHashCodeCalledWith);
    }

    [Fact]
    public async Task LoadBrowserFileAsyncReturnsCorrectFlows()
    {
        var dummyFlowLogFile = DummyFlowLogFileBuilder.Build(dummyFlows);
        var browserFile = BuildBrowserFileFake("test.json", "application/json", DateTimeOffset.Now, 123, dummyFlowLogFile);
        var browserFileLoader = new BrowserFileLoader(browserFile, flowLogFileParserFake);

        var result = await browserFileLoader.LoadAsync();

        Assert.Equal<Flow>(dummyFlows, result, new FlowComparer());
    }

    private BrowserFileFake BuildBrowserFileFake(string name, string contentType, DateTimeOffset lastModified,
        long size, FlowLogFile dummyFlowLogFile)
    {
        return new BrowserFileFake()
        {
            Name = name,
            ContentType = contentType,
            LastModified = lastModified,
            Size = size,
            FileContents = JsonSerializer.Serialize(dummyFlowLogFile, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            HashCode = dummyBrowserFileHashCode
        };
    }
}