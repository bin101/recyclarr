using System.IO.Abstractions;

namespace Recyclarr.TrashLib;

public interface IConfigurationFinder
{
    IReadOnlyCollection<IFileInfo> GetConfigFiles(IReadOnlyCollection<IFileInfo>? configs = null);
}
