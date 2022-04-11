using NsgLogViewer.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace NsgLogViewer.UnitTests.Helpers;

public static class DummyFlowGenerator
{
    private static Random random = new Random();

    public static IEnumerable<Flow> Generate(int count, int dummyBrowserFileHashCode)
    {
        var flows = new List<Flow>();

        for (int i = 0; i < count; i++)
        {
            flows.Add(Generate(dummyBrowserFileHashCode));
        }

        return flows;
    }

    private static Flow Generate(int dummyBrowserFileHashCode)
    {
        return new Flow
        {
            Time = DateTime.Now,
            MacAddress = "0d4f3a0d8b9c",
            NsgName = "nsg-name",
            RuleName = "rule-name",
            SourceAddress = GenerateRandomIpAddress(),
            SourcePort = random.Next(),
            DestinationAddress = GenerateRandomIpAddress(),
            DestinationPort = random.Next(),
            Options = "1,a,s,123",
            BrowserFileHashCode = dummyBrowserFileHashCode
        };
    }

    private static string GenerateRandomIpAddress()
    {
        var data = new byte[4];
        new Random().NextBytes(data);
        return new IPAddress(data).ToString();
    }
}