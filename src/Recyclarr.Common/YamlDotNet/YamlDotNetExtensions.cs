using JetBrains.Annotations;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NodeDeserializers;

// ReSharper disable UnusedMember.Global

namespace Recyclarr.Common;

public static class YamlDotNetExtensions
{
    public static T? DeserializeType<T>(this IDeserializer deserializer, string data)
        where T : class
    {
        var extractor = deserializer.Deserialize<RootExtractor<T>>(data);
        return extractor.RootObject;
    }

    public static DeserializerBuilder WithRequiredPropertyValidation(this DeserializerBuilder builder)
    {
        return builder
            .WithNodeDeserializer(inner => new ValidatingDeserializer(inner),
                s => s.InsteadOf<ObjectNodeDeserializer>());
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    private sealed class RootExtractor<T>
        where T : class
    {
        public T? RootObject { get; }
    }
}
