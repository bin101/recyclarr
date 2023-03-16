namespace Recyclarr.TrashLib;

public class RadarrCapabilityChecker : ServiceCapabilityChecker<RadarrCapabilities>
{
    public RadarrCapabilityChecker(IServiceInformation info)
        : base(info)
    {
    }

    protected override RadarrCapabilities BuildCapabilitiesObject(Version? version)
    {
        return new RadarrCapabilities(version);
    }
}
