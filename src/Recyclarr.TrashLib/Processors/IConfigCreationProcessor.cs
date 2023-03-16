namespace Recyclarr.TrashLib;

public interface IConfigCreationProcessor
{
    Task Process(string? configFilePath);
}
