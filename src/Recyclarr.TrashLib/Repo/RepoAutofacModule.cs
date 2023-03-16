using Autofac;

namespace Recyclarr.TrashLib;

public class RepoAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterType<RepoUpdater>().As<IRepoUpdater>();
        builder.RegisterType<RepoMetadataBuilder>().As<IRepoMetadataBuilder>().InstancePerLifetimeScope();
    }
}
