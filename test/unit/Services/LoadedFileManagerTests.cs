using NsgLogViewer.Models;
using NsgLogViewer.Services;
using NsgLogViewer.UnitTests.Fakes;
using NsgLogViewer.UnitTests.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NsgLogViewer.UnitTests.Services;

public class LoadedFileManagerTests
{
    private LoadedFileManager loadedFileManager;
    private BrowserFileFake browserFileFake;
    private IEnumerable<Flow> dummyFlows;

    public LoadedFileManagerTests()
    {
        loadedFileManager = new LoadedFileManager();
        browserFileFake = new BrowserFileFake()
        {
            Name = "test.json",
            Size = 123,
            ContentType = "application/json",
            HashCode = 100
        };
        dummyFlows = DummyFlowGenerator.Generate(10, browserFileFake.HashCode);
    }

    [Fact]
    public void StartLoadAddsNewLoadedFileWithCorrectName()
    {
        loadedFileManager.StartLoad(browserFileFake);

        Assert.Equal(browserFileFake.Name, loadedFileManager.LoadedFiles[0].FileName);
    }

    [Fact]
    public void StartLoadAddsNewLoadedFileWithCorrectLoadingFlag()
    {
        loadedFileManager.StartLoad(browserFileFake);
        
        Assert.True(loadedFileManager.LoadedFiles[0].IsLoading);
    }

    [Fact]
    public void StartLoadAddsNewLoadedFileWithCorrectSize()
    {
        loadedFileManager.StartLoad(browserFileFake);
        
        Assert.Equal(browserFileFake.Size, loadedFileManager.LoadedFiles[0].Size);
    }

    [Fact]
    public void StartLoadAddsNewLoadedFileWithCorrectHashCode()
    {
        loadedFileManager.StartLoad(browserFileFake);
        
        Assert.Equal(browserFileFake.HashCode, loadedFileManager.LoadedFiles[0].BrowserFileHashCode);
    }

    [Fact]
    public void FinishLoadSetsFirstFlowTimeCorrectly()
    {
        loadedFileManager.StartLoad(browserFileFake);
        loadedFileManager.FinishLoad(browserFileFake.HashCode, dummyFlows);
        var firstFlowTime = dummyFlows.MinBy(f => f.Time)?.Time;
        
        Assert.Equal(firstFlowTime, loadedFileManager.LoadedFiles[0].FirstFlowTime);
    }

    [Fact]
    public void FinishLoadSetsLastFlowTimeCorrectly()
    {
        loadedFileManager.StartLoad(browserFileFake);
        loadedFileManager.FinishLoad(browserFileFake.HashCode, dummyFlows);
        var lastFlowTime = dummyFlows.MaxBy(f => f.Time)?.Time;
        
        Assert.Equal(lastFlowTime, loadedFileManager.LoadedFiles[0].LastFlowTime);
    }

    [Fact]
    public void FinishLoadSetsFlowCountCorrectly()
    {
        loadedFileManager.StartLoad(browserFileFake);
        loadedFileManager.FinishLoad(browserFileFake.HashCode, dummyFlows);
        
        Assert.Equal(dummyFlows.Count(), loadedFileManager.LoadedFiles[0].FlowCount);
    }

    [Fact]
    public void FinishLoadSetsLoadingFlagCorrectly()
    {
        loadedFileManager.StartLoad(browserFileFake);
        loadedFileManager.FinishLoad(browserFileFake.HashCode, dummyFlows);
        
        Assert.False(loadedFileManager.LoadedFiles[0].IsLoading);
    }

    [Fact]
    public void FinishLoadDoesNotThrowIfFileNotFound()
    {
        loadedFileManager.FinishLoad(123123, dummyFlows);
    }

    [Fact]
    public void RemoveLoadedFileRemovesCorrectFileWhenCalledBeforeLoadFinished()
    {
        loadedFileManager.StartLoad(browserFileFake);
        loadedFileManager.RemoveLoadedFile(browserFileFake.HashCode);

        Assert.DoesNotContain(browserFileFake.HashCode, loadedFileManager.LoadedFiles.Select(f => f.BrowserFileHashCode));
    }

    [Fact]
    public void RemoveLoadedFileRemovesCorrectFileWhenCalledAfterLoadFinished()
    {
        loadedFileManager.StartLoad(browserFileFake);
        loadedFileManager.FinishLoad(browserFileFake.HashCode, dummyFlows);
        loadedFileManager.RemoveLoadedFile(browserFileFake.HashCode);

        Assert.DoesNotContain(browserFileFake.HashCode, loadedFileManager.LoadedFiles.Select(f => f.BrowserFileHashCode));
    }

    [Fact]
    public void RemoveLoadedFileDoesNotThrowIfFileNotFound()
    {
        loadedFileManager.RemoveLoadedFile(1253454);
    }

    [Fact]
    public void HandleLoadErrorCorrectSetsLoadingFlag()
    {
        var dummyError = "test load error";
        loadedFileManager.StartLoad(browserFileFake);
        loadedFileManager.HandleLoadError(browserFileFake.HashCode, dummyError);

        Assert.False(loadedFileManager.LoadedFiles[0].IsLoading);
    }

    [Fact]
    public void HandleLoadErrorCorrectlySetsErrorMessage()
    {
        var dummyError = "test load error";
        loadedFileManager.StartLoad(browserFileFake);
        loadedFileManager.HandleLoadError(browserFileFake.HashCode, dummyError);

        Assert.Equal(dummyError, loadedFileManager.LoadedFiles[0].LoadError);
    }

    [Fact]
    public void HandleLoadErrorDoesNotThrowIfFileNotFound()
    {
        var dummyError = "test load error";
        loadedFileManager.HandleLoadError(1253454, dummyError);
    }
}