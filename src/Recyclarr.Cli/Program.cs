namespace Recyclarr.Cli;

internal static class Program
{
    public static int Main(params string[] args)
    {
        var app = RecyclarrCli.Initialize();
        return RecyclarrCli.Run(app, args);
    }
}
