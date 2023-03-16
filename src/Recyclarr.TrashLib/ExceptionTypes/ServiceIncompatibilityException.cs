namespace Recyclarr.TrashLib;

public class ServiceIncompatibilityException : Exception
{
    public ServiceIncompatibilityException(string msg)
        : base(msg)
    {
    }
}
