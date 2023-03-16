using System.Collections.ObjectModel;
using Recyclarr.Common;

namespace Recyclarr.TrashLib;

public interface ICustomFormatPipelinePhases
{
    CustomFormatConfigPhase ConfigPhase { get; }
    CustomFormatApiFetchPhase ApiFetchPhase { get; }
    CustomFormatTransactionPhase TransactionPhase { get; }
    CustomFormatPreviewPhase PreviewPhase { get; }
    CustomFormatApiPersistencePhase ApiPersistencePhase { get; }
}

public record CustomFormatTransactionData
{
    public Collection<TrashIdMapping> DeletedCustomFormats { get; } = new();
    public Collection<CustomFormatData> NewCustomFormats { get; } = new();
    public Collection<CustomFormatData> UpdatedCustomFormats { get; } = new();
    public Collection<ConflictingCustomFormat> ConflictingCustomFormats { get; } = new();
    public Collection<CustomFormatData> UnchangedCustomFormats { get; } = new();
}

public class CustomFormatSyncPipeline : ISyncPipeline
{
    private readonly ILogger _log;
    private readonly ICachePersister _cachePersister;
    private readonly ICustomFormatPipelinePhases _phases;

    public CustomFormatSyncPipeline(
        ILogger log,
        ICachePersister cachePersister,
        ICustomFormatPipelinePhases phases)
    {
        _log = log;
        _cachePersister = cachePersister;
        _phases = phases;
    }

    public async Task Execute(ISyncSettings settings, IServiceConfiguration config)
    {
        var cache = _cachePersister.Load(config);

        var guideCfs = _phases.ConfigPhase.Execute(config);
        if (guideCfs.IsEmpty())
        {
            _log.Debug("No custom formats to process");
            return;
        }

        var serviceData = await _phases.ApiFetchPhase.Execute(config);

        cache = cache.RemoveStale(serviceData);

        var transactions = _phases.TransactionPhase.Execute(config, guideCfs, serviceData, cache);

        _phases.PreviewPhase.Execute(transactions);

        if (settings.Preview)
        {
            return;
        }

        // todo: Update the ProcessedCustomFormat objects' FormatId after API calls
        await _phases.ApiPersistencePhase.Execute(config, transactions);

        _cachePersister.Save(config, cache.Update(guideCfs));
    }
}
