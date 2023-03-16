using System.Collections.ObjectModel;

namespace Recyclarr.TrashLib;

public class QualitySizeTransactionPhase
{
    private readonly ILogger _log;

    public QualitySizeTransactionPhase(ILogger log)
    {
        _log = log;
    }

    public Collection<ServiceQualityDefinitionItem> Execute(
        IEnumerable<QualitySizeItem> guideQuality,
        IList<ServiceQualityDefinitionItem> serverQuality)
    {
        var newQuality = new Collection<ServiceQualityDefinitionItem>();
        foreach (var qualityData in guideQuality)
        {
            var serverEntry = serverQuality.FirstOrDefault(q => q.Quality?.Name == qualityData.Quality);
            if (serverEntry == null)
            {
                _log.Warning("Server lacks quality definition for {Quality}; it will be skipped", qualityData.Quality);
                continue;
            }

            if (!QualityIsDifferent(serverEntry, qualityData))
            {
                continue;
            }

            // Not using the original list again, so it's OK to modify the definition ref type objects in-place.
            serverEntry.MinSize = qualityData.MinForApi;
            serverEntry.MaxSize = qualityData.MaxForApi;
            serverEntry.PreferredSize = qualityData.PreferredForApi;
            newQuality.Add(serverEntry);

            _log.Debug("Setting Quality " +
                "[Name: {Name}] [Source: {Source}] [Min: {Min}] [Max: {Max}] [Preferred: {Preferred}]",
                serverEntry.Quality?.Name, serverEntry.Quality?.Source, serverEntry.MinSize, serverEntry.MaxSize,
                serverEntry.PreferredSize);
        }

        return newQuality;
    }

    private static bool QualityIsDifferent(ServiceQualityDefinitionItem a, QualitySizeItem b)
    {
        return b.IsMinDifferent(a.MinSize) || b.IsMaxDifferent(a.MaxSize) ||
            a.PreferredSize is not null && b.IsPreferredDifferent(a.PreferredSize);
    }
}
