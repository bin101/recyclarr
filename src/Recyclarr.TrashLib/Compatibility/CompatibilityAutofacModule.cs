using Autofac;

namespace Recyclarr.TrashLib;

public class CompatibilityAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        // Sonarr
        builder.RegisterType<SonarrCapabilityEnforcer>();
        builder.RegisterType<SonarrCapabilityChecker>().As<ISonarrCapabilityChecker>()
            .InstancePerLifetimeScope();

        // Radarr
        builder.RegisterType<RadarrCapabilityChecker>().InstancePerLifetimeScope();
    }
}
