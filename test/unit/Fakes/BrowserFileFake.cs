using Microsoft.AspNetCore.Components.Forms;
using System;
using System.IO;
using System.Threading;

namespace NsgLogViewer.UnitTests.Fakes;

public class BrowserFileFake : IBrowserFile
{
    public string Name { get; set; } = "";
    public string ContentType { get; set; } = "";
    public DateTimeOffset LastModified { get; set; }
    public long Size { get; set; }
    public string FileContents { get; set; } = "";
    public int HashCode { get; set; }
    
    public Stream OpenReadStream(long maxAllowedSize, CancellationToken cancellationToken)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(FileContents);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    public override int GetHashCode()
    {
        return HashCode;
    }
}