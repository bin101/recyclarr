using System.IO.Abstractions;

namespace Recyclarr.TrashLib;

public interface IRepoUpdater
{
    IDirectoryInfo RepoPath { get; }
    Task UpdateRepo();
}
