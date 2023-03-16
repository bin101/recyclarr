using System.Text;
using Recyclarr.Common;
using Spectre.Console;

namespace Recyclarr.TrashLib;

public class ReleaseProfileDataLister
{
    private readonly IAnsiConsole _console;
    private readonly IReleaseProfileGuideService _guide;

    public ReleaseProfileDataLister(IAnsiConsole console, IReleaseProfileGuideService guide)
    {
        _console = console;
        _guide = guide;
    }

    public void ListReleaseProfiles()
    {
        _console.WriteLine("\nList of Release Profiles in the TRaSH Guides:\n");

        var profilesFromGuide = _guide.GetReleaseProfileData();
        foreach (var profile in profilesFromGuide)
        {
            _console.WriteLine($"          - {profile.TrashId} # {profile.Name}");
        }

        _console.WriteLine(
            "\nThe above Release Profiles are in YAML format and ready to be copied & pasted under the `trash_ids:` property.");
    }

    private static bool HasIdentifiableTerms(ReleaseProfileData profile)
    {
        static bool HasTrashIds(IEnumerable<TermData> terms)
        {
            return terms.Any(x => !string.IsNullOrEmpty(x.TrashId));
        }

        return
            HasTrashIds(profile.Ignored) ||
            HasTrashIds(profile.Required) ||
            HasTrashIds(profile.Preferred.SelectMany(x => x.Terms));
    }

    public void ListTerms(string releaseProfileId)
    {
        var profile = _guide.GetReleaseProfileData()
            .FirstOrDefault(x => x.TrashId.EqualsIgnoreCase(releaseProfileId));

        if (profile is null)
        {
            throw new ArgumentException("No release profile found with that Trash ID");
        }

        if (!HasIdentifiableTerms(profile))
        {
            throw new ArgumentException(
                "This release profile has no terms that can be filtered " +
                "(terms must have Trash IDs assigned in order to be filtered)");
        }

        _console.WriteLine();
        _console.WriteLine($"List of Terms for the '{profile.Name}' Release Profile that may be filtered:\n");

        PrintTerms(profile.Required, "Required");
        PrintTerms(profile.Ignored, "Ignored");
        PrintTerms(profile.Preferred.SelectMany(x => x.Terms), "Preferred");

        _console.WriteLine(
            "The above Term Filters are in YAML format and ready to be copied & pasted under the `include:` or `exclude:` filter properties.");
    }

    private void PrintTerms(IEnumerable<TermData> terms, string category)
    {
        var filteredTerms = terms.Where(x => x.TrashId.Any()).ToList();
        if (!filteredTerms.Any())
        {
            return;
        }

        _console.WriteLine($"{category} Terms:\n");
        foreach (var term in filteredTerms)
        {
            var line = new StringBuilder($"            - {term.TrashId}");
            if (term.Name.Any())
            {
                line.Append($" # {term.Name}");
            }

            _console.WriteLine(line.ToString());
        }

        _console.WriteLine();
    }
}
