using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace CoverageReportGenerator.Cli.Commands;

[Command("hello")]
internal class HelloCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
    {
        console.Output.Write("World");

        return default;
    }
}
