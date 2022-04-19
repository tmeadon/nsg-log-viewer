using System.Text.RegularExpressions;

namespace NsgLogViewer.Services;

public class BlobNameParser
{
    private string blobName;
    private string nsgPattern = "\\/SUBSCRIPTIONS\\/([^\\/]+)\\/RESOURCEGROUPS\\/([^\\/]+)\\/PROVIDERS\\/MICROSOFT.NETWORK\\/NETWORKSECURITYGROUPS\\/([^\\/]+)";
    private string startTimePattern = "y=(\\d+)\\/m=(\\d+)\\/d=(\\d+)\\/h=(\\d+)\\/m=(\\d+)";
    private string macAddressPattern = "macAddress=([^\\/]+)";

    public BlobNameParser(string blobName)
    {
        this.blobName = blobName;
    }

    public FlowLogBlob Parse()
    {
        return new FlowLogBlob()
        {
            Nsg = ExtractNsg(),
            StartTimeUTC = ExtractStartTime(),
            MacAddress = ExtractMacAddress()
        };
    }

    private Nsg ExtractNsg()
    {
        var regex = new Regex(nsgPattern, RegexOptions.IgnoreCase);
        var match = regex.Match(blobName);
        return match.Success ? GetNsgFromMatch(match) : throw new BlobNameParseException(blobName);
    }

    private Nsg GetNsgFromMatch(Match match)
    {
        return new Nsg()
        {
            SubscriptionId = Guid.Parse(match.Groups[1].Value),
            ResourceGroupName = match.Groups[2].Value,
            Name = match.Groups[3].Value
        };
    }

    private DateTime ExtractStartTime()
    {
        var regex = new Regex(startTimePattern, RegexOptions.IgnoreCase);
        var match = regex.Match(blobName);
        return match.Success ? GetStartTimeFromMatch(match) : throw new BlobNameParseException(blobName);
    }

    private DateTime GetStartTimeFromMatch(Match match)
    {
        return new DateTime(
            int.Parse(match.Groups[1].Value),
            int.Parse(match.Groups[2].Value),
            int.Parse(match.Groups[3].Value),
            int.Parse(match.Groups[4].Value),
            int.Parse(match.Groups[5].Value),
            0
        );
    }

    private string ExtractMacAddress()
    {
        var regex = new Regex(macAddressPattern, RegexOptions.IgnoreCase);
        var match = regex.Match(blobName);
        return match.Success ? match.Groups[1].Value : throw new BlobNameParseException(blobName);
    }
}

public class BlobNameParseException : Exception
{
    public BlobNameParseException(string blobName) : base($"Could not parse blob name: {blobName}")
    {
    }
}