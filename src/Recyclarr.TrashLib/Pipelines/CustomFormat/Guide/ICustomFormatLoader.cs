using System.IO.Abstractions;

namespace Recyclarr.TrashLib;

public interface ICustomFormatLoader
{
    ICollection<CustomFormatData> LoadAllCustomFormatsAtPaths(
        IEnumerable<IDirectoryInfo> jsonPaths,
        IFileInfo collectionOfCustomFormats);
}
