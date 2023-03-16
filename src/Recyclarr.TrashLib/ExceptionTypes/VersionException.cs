using System.Runtime.Serialization;

namespace Recyclarr.TrashLib;

[Serializable]
public class VersionException : Exception
{
    public VersionException(string msg)
        : base(msg)
    {
    }

    protected VersionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
