using System.IO.Abstractions;
using System.IO.Abstractions.Extensions;
using Recyclarr.Cli.TestLibrary;
using Recyclarr.Common;
using Recyclarr.Common.TestLibrary;

namespace Recyclarr.TrashLib.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class QualitySizeGuideParserTest : IntegrationFixture
{
    [Test]
    public void Get_valid_data()
    {
        var qualityDir = Fs.CurrentDirectory().SubDir("json");
        Fs.AddSameFileFromEmbeddedResource(qualityDir.File("quality_size.json"), GetType());
        qualityDir.Refresh();

        var sut = Resolve<QualitySizeGuideParser>();

        var result = sut.GetQualities(new[] {qualityDir});

        result.Should().BeEquivalentTo(new[]
        {
            new QualitySizeData
            {
                Type = "series",
                Qualities = new[]
                {
                    new QualitySizeItem("quality1", 1, 2, 3),
                    new QualitySizeItem("quality2", 4.1m, 5.1m, 6.1m)
                }
            }
        });
    }

    [Test]
    public void Invalid_data_gets_skipped()
    {
        var qualityDir = Fs.CurrentDirectory().SubDir("json");
        Fs.AddSameFileFromEmbeddedResource(qualityDir.File("invalid_quality_size.json"), GetType());
        qualityDir.Refresh();

        var sut = Resolve<QualitySizeGuideParser>();

        var result = sut.GetQualities(new[] {qualityDir});

        result.Should().BeEmpty();
    }
}
