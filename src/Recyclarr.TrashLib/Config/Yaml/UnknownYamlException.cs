namespace Recyclarr.TrashLib;

public class UnknownYamlException : Exception
{
    public UnknownYamlException(string msg)
        : base(msg)
    {
    }
}
