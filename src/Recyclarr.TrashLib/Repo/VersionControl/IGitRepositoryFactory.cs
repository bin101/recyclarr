namespace Recyclarr.TrashLib;

public interface IGitRepositoryFactory
{
    Task<IGitRepository> CreateAndCloneIfNeeded(Uri repoUrl, string repoPath, string branch);
}
