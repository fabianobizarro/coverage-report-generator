using CliFx;
using GenerateReportGenerator.Cli.Commands;

//Console.WriteLine("Hello, World!");

await new CliApplicationBuilder()
    .AddCommand<HelloCommand>()
    .AddCommand<GenerateReportCommand>()
    .Build()
    .RunAsync();