namespace Recyclarr.TrashLib;

public class NoConfigurationFilesException : Exception
{
    public NoConfigurationFilesException()
        : base("No configuration YAML files found")
    {
    }
}
