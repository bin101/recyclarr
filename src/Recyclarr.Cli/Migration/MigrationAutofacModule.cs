using Autofac;

namespace Recyclarr.Cli;

public class MigrationAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterType<MigrationExecutor>().As<IMigrationExecutor>();

        // Migration Steps
        builder.RegisterTypes
            (
                typeof(MigrateTrashYml),
                typeof(MigrateTrashUpdaterAppDataDir)
            )
            .As<IMigrationStep>();
    }
}
