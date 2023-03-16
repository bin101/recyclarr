using Flurl.Http;

namespace Recyclarr.TrashLib;

public interface IFlurlClientFactory
{
    IFlurlClient BuildClient(Uri baseUrl);
}
