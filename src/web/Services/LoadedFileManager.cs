using Microsoft.AspNetCore.Components.Forms;

namespace NsgLogViewer.Services;

public class LoadedFileManager
{
    public List<LoadedFile> LoadedFiles { get; } = new();

    public void StartLoad(IBrowserFile browserFile)
    {
        LoadedFiles.Add(new LoadedFile {
            FileName = browserFile.Name,
            IsLoading = true,
            Size = browserFile.Size,
            BrowserFileHashCode = browserFile.GetHashCode()
        });
    }
    
    public void FinishLoad(int fileHashCode, IEnumerable<Flow> fileFlows)
    {
        var loadedFile = LoadedFiles.FirstOrDefault(x => x.BrowserFileHashCode == fileHashCode);

        if (loadedFile != null)
        {
            var sortedFlows = fileFlows.OrderBy(f => f.Time);
            loadedFile.FirstFlowTime = sortedFlows.First().Time;
            loadedFile.LastFlowTime = sortedFlows.Last().Time;
            loadedFile.IsLoading = false;
            loadedFile.FlowCount = sortedFlows.Count();
        }
    }

    public void RemoveLoadedFile(int fileHashCode)
    {
        LoadedFiles.RemoveAll(x => x.BrowserFileHashCode == fileHashCode);
    }

    public void HandleLoadError(int fileHashCode, string errorMessage)
    {
        var loadedFile = LoadedFiles.FirstOrDefault(x => x.BrowserFileHashCode == fileHashCode);

        if (loadedFile != null)
        {
            loadedFile.IsLoading = false;
            loadedFile.LoadError = errorMessage;
        }
    }
}