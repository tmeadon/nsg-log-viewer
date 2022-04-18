using NsgLogViewer.Models;
using NsgLogViewer.Services;
using System;
using Xunit;

namespace NsgLogViewer.UnitTests.Services;

public class BlobNameParserTests
{
    private FlowLogBlob dummyFlowLogBlob;

    private string dummyBlobName;

    private BlobNameParser parser;

    public BlobNameParserTests()
    {
        dummyFlowLogBlob = new FlowLogBlob
        {
            StartTimeUTC = new DateTime(2020, 1, 1, 1, 0, 0),
            MacAddress = "abc123",
            Nsg = new Nsg
            {
                SubscriptionId = Guid.NewGuid(),
                ResourceGroupName = "dummyResourceGroupName",
                Name = "dummyName"
            }
        };
        dummyBlobName = BuildDummyBlobName();
        parser = new BlobNameParser(dummyBlobName);
    }

    private string BuildDummyBlobName()
    {
        return "resourceId=/SUBSCRIPTIONS/" + dummyFlowLogBlob.Nsg.SubscriptionId + "/RESOURCEGROUPS/" + dummyFlowLogBlob.Nsg.ResourceGroupName + 
            "/PROVIDERS/MICROSOFT.NETWORK/NETWORKSECURITYGROUPS/" + dummyFlowLogBlob.Nsg.Name + "/y=" + dummyFlowLogBlob.StartTimeUTC.Year + "/m=" + 
            dummyFlowLogBlob.StartTimeUTC.Month + "/d=" + dummyFlowLogBlob.StartTimeUTC.Day + "/h=" + dummyFlowLogBlob.StartTimeUTC.Hour + "/m=0" +
            "/macAddress=" + dummyFlowLogBlob.MacAddress + "/PT1H.json";
    }

    [Fact]
    public void ParseReturnsCorrectFlowLog()
    {
        var flowLogBlob = parser.Parse();
        Assert.Equal(dummyFlowLogBlob, flowLogBlob);
    }
}