using Castle.Core.Internal;
using Spectre.Console;

namespace Recyclarr.TrashLib;

public class TagPreviewPhase
{
    private readonly IAnsiConsole _console;

    public TagPreviewPhase(IAnsiConsole console)
    {
        _console = console;
    }

    public void Execute(IReadOnlyList<string> tagsToCreate)
    {
        if (tagsToCreate.IsNullOrEmpty())
        {
            _console.WriteLine();
            _console.MarkupLine("[green]No tags to create[/]");
            _console.WriteLine();
            return;
        }

        var table = new Table {Border = TableBorder.Simple};
        table.AddColumn("[olive]Tags To Create[/]");

        foreach (var tag in tagsToCreate)
        {
            table.AddRow(tag);
        }

        _console.Write(table);
    }
}
