namespace Recyclarr.Cli;

public interface ILogJanitor
{
    void DeleteOldestLogFiles(int numberOfNewestToKeep);
}
