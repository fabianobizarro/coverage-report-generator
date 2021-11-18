using CliFx;
using CoverageReportGenerator.Cli.Commands;

await new CliApplicationBuilder()
    .AddCommand<GenerateReportCommand>()
    .AddCommand<InspecCommand>()
    .Build()
    .RunAsync();