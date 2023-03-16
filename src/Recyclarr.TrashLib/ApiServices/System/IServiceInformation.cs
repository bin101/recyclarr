namespace Recyclarr.TrashLib;

public interface IServiceInformation
{
    public Task<Version?> GetVersion(IServiceConfiguration config);
}
