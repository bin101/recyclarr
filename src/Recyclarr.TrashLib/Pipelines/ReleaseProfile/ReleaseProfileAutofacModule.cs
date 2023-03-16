using Autofac;
using Autofac.Extras.AggregateService;
using Autofac.Extras.Ordering;

namespace Recyclarr.TrashLib;

public class ReleaseProfileAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<ReleaseProfileApiService>().As<IReleaseProfileApiService>();
        builder.RegisterType<ReleaseProfileFilterPipeline>().As<IReleaseProfileFilterPipeline>();
        builder.RegisterType<ReleaseProfileGuideParser>();
        builder.RegisterType<ReleaseProfileDataLister>();

        builder.RegisterType<SonarrReleaseProfileCompatibilityHandler>()
            .As<ISonarrReleaseProfileCompatibilityHandler>();

        builder.RegisterType<ReleaseProfileGuideService>().As<IReleaseProfileGuideService>().SingleInstance();

        builder.RegisterAggregateService<IReleaseProfilePipelinePhases>();
        builder.RegisterType<ReleaseProfileConfigPhase>();
        builder.RegisterType<ReleaseProfileApiFetchPhase>();
        builder.RegisterType<ReleaseProfileTransactionPhase>();
        builder.RegisterType<ReleaseProfilePreviewPhase>();
        builder.RegisterType<ReleaseProfileApiPersistencePhase>();

        // Release Profile Filters (ORDER MATTERS!)
        builder.RegisterTypes(
                typeof(IncludeExcludeFilter),
                typeof(StrictNegativeScoresFilter))
            .As<IReleaseProfileFilter>()
            .OrderByRegistration();
    }
}
