namespace Recyclarr.TrashLib;

public class NoHomeDirectoryException : Exception
{
    public NoHomeDirectoryException(string msg)
        : base(msg)
    {
    }
}
