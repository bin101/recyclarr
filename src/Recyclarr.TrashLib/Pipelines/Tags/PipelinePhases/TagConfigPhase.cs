using System.Diagnostics.CodeAnalysis;
using Recyclarr.Common;

namespace Recyclarr.TrashLib;

public class TagConfigPhase
{
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification =
        "This non-static method establishes a pattern that will eventually become an interface")]
    public IList<string>? Execute(SonarrConfiguration config)
    {
        return config.ReleaseProfiles
            .SelectMany(x => x.Tags)
            .Distinct()
            .ToListOrNull();
    }
}
