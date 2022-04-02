namespace NsgLogViewer.Models;

public class LoadedFile
{
    public string FileName { get; set; } = "";
    public bool IsLoading { get; set; }
    public long Size { get; set; }
    public int BrowserFileHashCode { get; set; }
}