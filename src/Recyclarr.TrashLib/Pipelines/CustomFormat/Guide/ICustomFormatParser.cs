namespace Recyclarr.TrashLib;

public interface ICustomFormatParser
{
    CustomFormatData ParseCustomFormatData(string guideData, string fileName);
}
