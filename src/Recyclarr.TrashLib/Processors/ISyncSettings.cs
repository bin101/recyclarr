using System.IO.Abstractions;

namespace Recyclarr.TrashLib;

public interface ISyncSettings
{
    SupportedServices? Service { get; }
    IReadOnlyCollection<IFileInfo> Configs { get; }
    bool Preview { get; }
    IReadOnlyCollection<string>? Instances { get; }
}
