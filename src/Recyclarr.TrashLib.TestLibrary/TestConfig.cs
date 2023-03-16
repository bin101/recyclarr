using JetBrains.Annotations;

namespace Recyclarr.TrashLib.TestLibrary;

[UsedImplicitly]
public class TestConfig : ServiceConfiguration
{
    public override SupportedServices ServiceType => SupportedServices.Sonarr;
}
