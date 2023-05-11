using System.Diagnostics.CodeAnalysis;
using Serilog.Events;

namespace Recyclarr.TrashLib.TestLibrary;

public class MockLoggerFactory : ILoggerFactory
{
    [SuppressMessage("Performance", "CA1822:Mark members as static",
        Justification = "Implements interface method; cannot be static")]
    public ILogger Create()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Is(LogEventLevel.Verbose)
            .WriteTo.TestCorrelator()
            .WriteTo.Console()
            .CreateLogger();
    }
}
