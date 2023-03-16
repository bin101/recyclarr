using System.IO.Abstractions;

namespace Recyclarr.TrashLib;

internal record QualitySizePaths(
    IReadOnlyCollection<IDirectoryInfo> QualitySizeDirectories
);
