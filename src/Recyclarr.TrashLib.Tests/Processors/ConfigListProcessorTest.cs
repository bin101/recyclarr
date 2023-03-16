using Recyclarr.TestLibrary;

namespace Recyclarr.TrashLib.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class ConfigListProcessorTest
{
    [Test]
    [InlineAutoMockData(ConfigListCategory.Templates)]
    public async Task List_templates_invokes_correct_lister(
        ConfigListCategory category,
        [Frozen(Matching.ImplementedInterfaces)] StubAutofacIndex<ConfigListCategory, IConfigLister> configListers,
        IConfigLister lister,
        ConfigListProcessor sut)
    {
        configListers.Add(category, lister);

        await sut.Process(category);

        await lister.Received().List();
    }
}
