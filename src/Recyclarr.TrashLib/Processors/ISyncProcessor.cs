namespace Recyclarr.TrashLib;

public interface ISyncProcessor
{
    Task<ExitStatus> ProcessConfigs(ISyncSettings settings);
}
