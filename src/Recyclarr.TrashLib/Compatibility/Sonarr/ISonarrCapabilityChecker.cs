namespace Recyclarr.TrashLib;

public interface ISonarrCapabilityChecker
{
    Task<SonarrCapabilities?> GetCapabilities(IServiceConfiguration config);
}
