namespace Recyclarr.TrashLib;

public interface IServiceProcessor
{
    Task Process(ISyncSettings settings, IServiceConfiguration config);
}
