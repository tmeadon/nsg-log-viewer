using NsgLogViewer.Models;
using NsgLogViewer.Services;
using System;
using Xunit;

namespace NsgLogViewer.UnitTests.Services;

public class BlobNameParserTests
{
    private FlowLogBlob dummyFlowLogBlob;
    private string goodBlobName;
    private string blobNameBadNsg;
    private string blobNameBadStartTime;
    private string blobNameBadMacAddress;

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
        goodBlobName = BuildGoodBlobName();
        blobNameBadNsg = BuildBadNsgBlobName();
        blobNameBadStartTime = BuildBadStartTimeBlobName();
        blobNameBadMacAddress = BuildBadMacAddressBlobName();
    }

    private string BuildGoodBlobName()
    {
        return "resourceId=/SUBSCRIPTIONS/" + dummyFlowLogBlob.Nsg.SubscriptionId + "/RESOURCEGROUPS/" + dummyFlowLogBlob.Nsg.ResourceGroupName + 
            "/PROVIDERS/MICROSOFT.NETWORK/NETWORKSECURITYGROUPS/" + dummyFlowLogBlob.Nsg.Name + "/y=" + dummyFlowLogBlob.StartTimeUTC.Year + "/m=" + 
            dummyFlowLogBlob.StartTimeUTC.Month + "/d=" + dummyFlowLogBlob.StartTimeUTC.Day + "/h=" + dummyFlowLogBlob.StartTimeUTC.Hour + "/m=0" +
            "/macAddress=" + dummyFlowLogBlob.MacAddress + "/PT1H.json";
    }

    private string BuildBadNsgBlobName()
    {
        return "resourceId=/SUBSCRIPTIONS/" + dummyFlowLogBlob.Nsg.SubscriptionId + "/PROVIDERS/MICROSOFT.NETWORK/NETWORKSECURITYGROUPS/" + 
            dummyFlowLogBlob.Nsg.Name + "/y=" + dummyFlowLogBlob.StartTimeUTC.Year + "/m=" + dummyFlowLogBlob.StartTimeUTC.Month + "/d=" + 
            dummyFlowLogBlob.StartTimeUTC.Day + "/h=" + dummyFlowLogBlob.StartTimeUTC.Hour + "/m=0" + "/macAddress=" + dummyFlowLogBlob.MacAddress + "/PT1H.json";
    }
    
    private string BuildBadStartTimeBlobName()
    {
        return "resourceId=/SUBSCRIPTIONS/" + dummyFlowLogBlob.Nsg.SubscriptionId + "/RESOURCEGROUPS/" + dummyFlowLogBlob.Nsg.ResourceGroupName + 
            "/PROVIDERS/MICROSOFT.NETWORK/NETWORKSECURITYGROUPS/" + dummyFlowLogBlob.Nsg.Name + "/y=" + dummyFlowLogBlob.StartTimeUTC.Year + "/m=" + 
            dummyFlowLogBlob.StartTimeUTC.Month + "/d=" + dummyFlowLogBlob.StartTimeUTC.Day + "/m=0" + "/macAddress=" + dummyFlowLogBlob.MacAddress + "/PT1H.json";
    }

    private string BuildBadMacAddressBlobName()
    {
        return "resourceId=/SUBSCRIPTIONS/" + dummyFlowLogBlob.Nsg.SubscriptionId + "/RESOURCEGROUPS/" + dummyFlowLogBlob.Nsg.ResourceGroupName + 
            "/PROVIDERS/MICROSOFT.NETWORK/NETWORKSECURITYGROUPS/" + dummyFlowLogBlob.Nsg.Name + "/y=" + dummyFlowLogBlob.StartTimeUTC.Year + "/m=" + 
            dummyFlowLogBlob.StartTimeUTC.Month + "/d=" + dummyFlowLogBlob.StartTimeUTC.Day + "/h=" + dummyFlowLogBlob.StartTimeUTC.Hour + "/m=0" +
            "/macAddress=/PT1H.json";
    }

    [Fact]
    public void ParseReturnsCorrectFlowLog()
    {
        var parser = new BlobNameParser(goodBlobName);
        var flowLogBlob = parser.Parse();
        Assert.Equal(dummyFlowLogBlob, flowLogBlob);
    }

    [Fact]
    public void ParseThrowsExceptionWhenNsgDetailsAreInvalid()
    {
        var parser = new BlobNameParser(blobNameBadNsg);
        Assert.Throws<BlobNameParseException>(() => parser.Parse());
    }

    [Fact]
    public void ParseThrowsExceptionWhenStartTimeIsInvalid()
    {
        var parser = new BlobNameParser(blobNameBadStartTime);
        Assert.Throws<BlobNameParseException>(() => parser.Parse());
    }

    [Fact]
    public void ParseThrowsExceptionWhenMacAddressIsInvalid()
    {
        var parser = new BlobNameParser(blobNameBadMacAddress);
        Assert.Throws<BlobNameParseException>(() => parser.Parse());
    }
}