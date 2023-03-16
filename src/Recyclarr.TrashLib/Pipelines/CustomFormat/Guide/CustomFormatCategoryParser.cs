using System.IO.Abstractions;
using System.Text.RegularExpressions;
using Recyclarr.Common;

namespace Recyclarr.TrashLib;

public class CustomFormatCategoryParser : ICustomFormatCategoryParser
{
    private static readonly Regex TableRegex = new(@"^\s*\|(.*)\|\s*$");
    private static readonly Regex LinkRegex = new(@"^\[(.+?)\]\(#(.+?)\)$");

    public ICollection<CustomFormatCategoryItem> Parse(IFileInfo collectionOfCustomFormatsMdFile)
    {
        var columns = new List<List<string>>();

        using var md = collectionOfCustomFormatsMdFile.OpenText();
        while (!md.EndOfStream)
        {
            var rows = ParseTable(md);

            // Pivot the data so that we have lists of columns instead of lists of rows
            // Taken from: https://stackoverflow.com/a/39485441/157971
            columns.AddRange(rows
                .SelectMany(x => x.Select((value, index) => (value, index)))
                .GroupBy(x => x.index, x => x.value)
                .Select(x => x.ToList()));
        }

        return columns
            .GroupBy(x => x[0], x => x.Skip(1))
            .SelectMany(x => x.SelectMany(y => y).Distinct().Select(y => ParseLink(x.Key, y)))
            .NotNull()
            .ToList();
    }

    private static CustomFormatCategoryItem? ParseLink(string categoryName, string markdownLink)
    {
        var match = LinkRegex.Match(markdownLink);
        return match.Success
            ? new CustomFormatCategoryItem(categoryName, match.Groups[1].Value, match.Groups[2].Value)
            : null;
    }

    private static IEnumerable<List<string>> ParseTable(TextReader stream)
    {
        var tableRows = new List<List<string>>();

        while (true)
        {
            var line = stream.ReadLine();
            if (line is null)
            {
                break;
            }

            if (line.Any())
            {
                var fields = GetTableRow(line);
                if (fields.Any())
                {
                    tableRows.Add(fields);
                    continue;
                }
            }

            if (tableRows.Any())
            {
                break;
            }
        }

        return tableRows
            // Filter out the `|---|---|---|` part of the table between the heading & data rows.
            .Where(x => !Regex.IsMatch(x[0], @"^-+$"));
    }

    private static List<string> GetTableRow(string line)
    {
        var fields = new List<string>();

        var match = TableRegex.Match(line);
        if (match.Success)
        {
            var tableRow = match.Groups[1].Value;
            fields = tableRow.Split('|').Select(x => x.Trim()).ToList();
        }

        return fields;
    }
}
