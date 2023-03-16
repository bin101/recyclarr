using System.IO.Abstractions;

namespace Recyclarr.TrashLib;

public interface ICacheStoragePath
{
    IFileInfo CalculatePath(IServiceConfiguration config, string cacheObjectName);
}
