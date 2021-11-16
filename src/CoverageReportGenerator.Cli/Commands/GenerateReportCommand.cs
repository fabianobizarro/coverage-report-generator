using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportGenerator;
using ReportGenerator.DotnetCorePluginLoader;
using CoverageReportGenerator.Cli.Utils;

namespace GenerateReportGenerator.Cli.Commands;


[Command("generate")]
internal class GenerateReportCommand : ICommand
{
    [CommandOption("project", 'p', Description = "Caminho do projeto a ser executado os testes", IsRequired = true)]
    public string? InputPath { get; init; }

    [CommandOption("output", 'o', Description = "asdasds")]
    public string? OutputPath { get; init; }

    public string CompleteInputPath =>
        InputPath.Pipe(path => path ?? throw new ArgumentNullException(nameof(InputPath)))
                 .Pipe(path => Path.GetFullPath(path));

    public string CompleteOutputPath =>
        OutputPath.Pipe(path => string.IsNullOrWhiteSpace(path) ? Path.GetTempPath() : path)
                  .Pipe(path => Path.GetFullPath(path));

    public ValueTask ExecuteAsync(IConsole console)
    {
        console.Output.WriteLine($"InputPath: {CompleteInputPath}");
        console.Output.WriteLine($"OutputPath: {CompleteOutputPath}");

        var assemblyLoader = new DotNetCoreAssemblyLoader();

        var assembly = assemblyLoader.Load(InputPath);

        console.Output.WriteLine(assembly.FullName);

        return default;
    }
}
