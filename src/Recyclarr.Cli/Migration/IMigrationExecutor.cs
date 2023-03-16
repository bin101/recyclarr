namespace Recyclarr.Cli;

public interface IMigrationExecutor
{
    void PerformAllMigrationSteps(bool withDiagnostics);
    void CheckNeededMigrations();
}
