namespace Recyclarr.Cli;

public class RequiredMigrationException : Exception
{
    public RequiredMigrationException()
        : base("Some REQUIRED migrations did not pass")
    {
        throw new NotImplementedException();
    }
}
