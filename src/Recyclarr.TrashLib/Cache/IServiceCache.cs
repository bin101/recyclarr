namespace Recyclarr.TrashLib;

public interface IServiceCache
{
    T? Load<T>(IServiceConfiguration config) where T : class;
    void Save<T>(T obj, IServiceConfiguration config) where T : class;
}
