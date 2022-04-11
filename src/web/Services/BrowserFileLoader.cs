using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

namespace NsgLogViewer.Services;

public class BrowserFileLoader
{
    private static readonly long maxAllowedSize = 1024 * 1024 * 10; // 10 MB
    private IBrowserFile browserFile;
    private FlowLogFileParser flowLogFileParser;

    public BrowserFileLoader (IBrowserFile browserFile, FlowLogFileParser flowLogFileParser)
    {
        this.browserFile = browserFile;
        this.flowLogFileParser = flowLogFileParser;
    }

    public async Task<IEnumerable<Flow>> LoadAsync()
    {
        if (browserFile.ContentType != "application/json")
        {
            throw new BrowserFileLoaderException($"Browser file {browserFile.Name} has unsupported content type {browserFile.ContentType}");
        }

        var flowLogFile = await DeserializeFileAsync(browserFile);
        var flows = flowLogFileParser.Parse(flowLogFile, browserFile.GetHashCode());
        return flows;
    }

    private static async Task<FlowLogFile> DeserializeFileAsync(IBrowserFile browserFile)
    {
        var readStream = browserFile.OpenReadStream(maxAllowedSize: maxAllowedSize);

        var file = await JsonSerializer.DeserializeAsync<FlowLogFile>(readStream, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });

        if (file == null || file.Records.Count == 0)
        {
            throw new BrowserFileLoaderException($"Browser file {browserFile.Name} does not contain any NSG flow log records");
        }
        
        return file;
    }
}

public class BrowserFileLoaderException : Exception
{
    public BrowserFileLoaderException(string message) : base(message) { }
}