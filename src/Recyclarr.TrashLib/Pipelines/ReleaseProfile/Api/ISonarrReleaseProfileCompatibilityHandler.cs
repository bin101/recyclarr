using Newtonsoft.Json.Linq;

namespace Recyclarr.TrashLib;

public interface ISonarrReleaseProfileCompatibilityHandler
{
    Task<object> CompatibleReleaseProfileForSending(
        IServiceConfiguration config,
        SonarrReleaseProfile profile);

    SonarrReleaseProfile CompatibleReleaseProfileForReceiving(JObject profile);
}
