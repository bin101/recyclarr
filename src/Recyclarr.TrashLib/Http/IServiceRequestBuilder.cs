using Flurl.Http;

namespace Recyclarr.TrashLib;

public interface IServiceRequestBuilder
{
    IFlurlRequest Request(IServiceConfiguration config, params object[] path);
}
