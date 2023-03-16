using Autofac;
using Autofac.Extras.AggregateService;

namespace Recyclarr.TrashLib;

public class QualityProfileAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<QualityProfileService>().As<IQualityProfileService>();

        builder.RegisterAggregateService<IQualityProfilePipelinePhases>();
        builder.RegisterType<QualityProfileConfigPhase>();
        builder.RegisterType<QualityProfileApiFetchPhase>();
        builder.RegisterType<QualityProfileTransactionPhase>();
        builder.RegisterType<QualityProfilePreviewPhase>();
        builder.RegisterType<QualityProfileApiPersistencePhase>();
    }
}
