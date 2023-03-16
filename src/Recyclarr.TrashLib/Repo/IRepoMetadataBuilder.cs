using System.IO.Abstractions;

namespace Recyclarr.TrashLib;

public interface IRepoMetadataBuilder
{
    RepoMetadata GetMetadata();
    IReadOnlyList<IDirectoryInfo> ToDirectoryInfoList(IEnumerable<string> listOfDirectories);
    IDirectoryInfo DocsDirectory { get; }
}
