using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Diagnostics;
using CoverageReportGenerator.Core;

namespace CoverageReportGenerator.Cli.Commands;

[Command("generate")]
internal class GenerateReportCommand : ICommand
{
    private readonly PathHelper _path = new();

    [CommandOption("project", 'p', Description = "Caminho do projeto a ser executado os testes", IsRequired = true)]
    public string? InputPath { get; init; }

    [CommandOption("output", 'o', Description = "asdasds")] //todo: colocar descricao
    public string? OutputPath { get; init; }

    [CommandOption("verbose", 'b')]
    public bool Verbose { get; init; } = false;

    [CommandOption("dark")]
    public bool DarkMode { get; init; } = false;

    [CommandOption("open-folder")]
    public bool OpenFolder { get; init; } = false;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        if (Verbose)
        {
            console.Output.WriteLine($"\tInputPath: {_path.CompleteInputPath(InputPath!)}");
            console.Output.WriteLine($"\tOutputPath: {_path.CompleteOutputPath(OutputPath!)}");
        }

        var tempPath = _path.GetTempPath();

        console.Output.WriteLine("Running unit tests.");
        console.Output.WriteLine($"OpenCover report will be stored at ");
        console.Output.WriteLine($"\t{tempPath}.opencover.xml.");

        await RunTestsAsync(tempPath);

        console.Output.WriteLine($"Generating report to output: {_path.CompleteOutputPath(OutputPath!)}.");
        await GenerateReport(tempPath);

        if (OpenFolder)
        {
            Process.Start("explorer", Path.Combine(_path.CompleteOutputPath(OutputPath!)));
        }
    }

    private async ValueTask RunTestsAsync(string tempPath)
    {
        using var process =
            new ProcessBuilder("dotnet")
                .WithArgument("test")
                .WithArgument("-c Release")
                .WithArgument("/p:CollectCoverage=true")
                .WithArgument("/p:CoverletOutputFormat=opencover")
                .WithArgument($"/p:CoverletOutput={tempPath}")
                .WithArgument(_path.CompleteInputPath(InputPath!))
                .ConfigStartInfo(p => p.RedirectStandardOutput = Verbose == false)
                .Build();

        process.Start();

        await process.WaitForExitAsync();
    }

    private async ValueTask GenerateReport(string tempPath)
    {
        var reportType = DarkMode ? "HtmlInline_AzurePipelines_Dark" : "Html";

        using var process =
            new ProcessBuilder("reportgenerator")
                .WithArgument($"-reports:{tempPath}.opencover.xml")
                .WithArgument($"-targetdir:{_path.CompleteOutputPath(OutputPath!)}")
                .WithArgument($"-reporttypes:{reportType}")
                .ConfigStartInfo(p => p.RedirectStandardOutput = Verbose == false)
                .Build();

        process.Start();

        await process.WaitForExitAsync();
    }
}
