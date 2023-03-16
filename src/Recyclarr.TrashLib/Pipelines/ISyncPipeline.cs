namespace Recyclarr.TrashLib;

public interface ISyncPipeline
{
    public Task Execute(ISyncSettings settings, IServiceConfiguration config);
}
