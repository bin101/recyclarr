namespace Recyclarr.Cli;

public interface IBaseCommandSetupTask
{
    void OnStart();
    void OnFinish();
}
