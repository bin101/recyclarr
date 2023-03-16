using Recyclarr.Cli.TestLibrary;
using Recyclarr.TrashLib.TestLibrary;

namespace Recyclarr.TrashLib.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class CustomFormatTransactionPhaseTest : IntegrationFixture
{
    [Test]
    public void Add_new_cf()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            NewCf.Data("one", "cf1")
        };

        var serviceData = Array.Empty<CustomFormatData>();

        var cache = new CustomFormatCache();

        var config = new TestConfig();

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            NewCustomFormats =
            {
                NewCf.Data("one", "cf1")
            }
        });
    }

    [Test]
    public void Update_cf_by_matching_name()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            new CustomFormatData
            {
                Name = "one",
                TrashId = "cf1",
                // Only set the below value to make it different from the service CF
                IncludeCustomFormatWhenRenaming = true
            }
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "one"}
        };

        var cache = new CustomFormatCache();

        var config = new TestConfig();

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            UpdatedCustomFormats =
            {
                new CustomFormatData
                {
                    Name = "one",
                    IncludeCustomFormatWhenRenaming = true
                }
            }
        });
    }

    [Test]
    public void Update_cf_by_matching_id_different_names()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            new CustomFormatData
            {
                Name = "different1",
                TrashId = "cf1",
                // Only set the below value to make it different from the service CF
                IncludeCustomFormatWhenRenaming = true
            }
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "different2", Id = 2}
        };

        var cache = new CustomFormatCache
        {
            TrashIdMappings = new[]
            {
                new TrashIdMapping("cf1", "", 2)
            }
        };

        var config = new TestConfig();

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            UpdatedCustomFormats =
            {
                new CustomFormatData
                {
                    Name = "different1",
                    Id = 2,
                    IncludeCustomFormatWhenRenaming = true
                }
            }
        });
    }

    [Test]
    public void Update_cf_by_matching_id_same_names()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            new CustomFormatData
            {
                Name = "different1",
                TrashId = "cf1",
                // Only set the below value to make it different from the service CF
                IncludeCustomFormatWhenRenaming = true
            }
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "different1", Id = 2}
        };

        var cache = new CustomFormatCache();

        var config = new TestConfig();

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            UpdatedCustomFormats =
            {
                new CustomFormatData
                {
                    Name = "different1",
                    Id = 2,
                    IncludeCustomFormatWhenRenaming = true
                }
            }
        });
    }

    [Test]
    public void Conflicting_cf_when_new_cf_has_name_of_existing()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            NewCf.Data("one", "cf1")
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "one", Id = 2},
            new CustomFormatData {Name = "two", Id = 1}
        };

        var cache = new CustomFormatCache();

        var config = new TestConfig
        {
            ReplaceExistingCustomFormats = false
        };

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            ConflictingCustomFormats =
            {
                new ConflictingCustomFormat(guideCfs[0], 2)
            }
        });
    }

    [Test]
    public void Conflicting_cf_when_cached_cf_has_name_of_existing()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            NewCf.Data("one", "cf1")
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "one", Id = 2},
            new CustomFormatData {Name = "two", Id = 1}
        };

        var cache = new CustomFormatCache
        {
            TrashIdMappings = new[]
            {
                new TrashIdMapping("cf1", "one", 1)
            }
        };

        var config = new TestConfig
        {
            ReplaceExistingCustomFormats = false
        };

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            ConflictingCustomFormats =
            {
                new ConflictingCustomFormat(guideCfs[0], 2)
            }
        });
    }

    [Test]
    public void Updated_cf_with_matching_name_and_id()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            new CustomFormatData
            {
                Name = "one",
                TrashId = "cf1",
                // Only set the below value to make it different from the service CF
                IncludeCustomFormatWhenRenaming = true
            }
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "two", Id = 2},
            new CustomFormatData {Name = "one", Id = 1}
        };

        var cache = new CustomFormatCache
        {
            TrashIdMappings = new[]
            {
                new TrashIdMapping("cf1", "one", 1)
            }
        };

        var config = new TestConfig
        {
            ReplaceExistingCustomFormats = false
        };

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            UpdatedCustomFormats =
            {
                NewCf.Data("one", "cf1", 1) with {IncludeCustomFormatWhenRenaming = true}
            }
        });
    }

    [Test]
    public void Unchanged_cfs_with_replace()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            NewCf.Data("one", "cf1")
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "one", Id = 1}
        };

        var cache = new CustomFormatCache();

        var config = new TestConfig();

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            UnchangedCustomFormats = {guideCfs[0]}
        });
    }

    [Test]
    public void Unchanged_cfs_without_replace()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            NewCf.Data("one", "cf1")
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "one", Id = 1}
        };

        var cache = new CustomFormatCache
        {
            TrashIdMappings = new[]
            {
                new TrashIdMapping("cf1", "one", 1)
            }
        };

        var config = new TestConfig
        {
            ReplaceExistingCustomFormats = false
        };

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            UnchangedCustomFormats = {guideCfs[0]}
        });
    }

    [Test]
    public void Deleted_cfs()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = Array.Empty<CustomFormatData>();

        var serviceData = new[]
        {
            new CustomFormatData {Name = "two", Id = 2}
        };

        var cache = new CustomFormatCache
        {
            TrashIdMappings = new[]
            {
                new TrashIdMapping("cf2", "two", 2)
            }
        };

        var config = new TestConfig();

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.Should().BeEquivalentTo(new CustomFormatTransactionData
        {
            DeletedCustomFormats =
            {
                new TrashIdMapping("cf2", "two", 2)
            }
        });
    }

    [Test]
    public void Do_not_delete_cfs_in_config()
    {
        var sut = Resolve<CustomFormatTransactionPhase>();

        var guideCfs = new[]
        {
            NewCf.Data("two", "cf2", 2)
        };

        var serviceData = new[]
        {
            new CustomFormatData {Name = "two", Id = 2}
        };

        var cache = new CustomFormatCache
        {
            TrashIdMappings = new[]
            {
                new TrashIdMapping("cf2", "two", 2)
            }
        };

        var config = new TestConfig();

        var result = sut.Execute(config, guideCfs, serviceData, cache);

        result.DeletedCustomFormats.Should().BeEmpty();
    }
}
