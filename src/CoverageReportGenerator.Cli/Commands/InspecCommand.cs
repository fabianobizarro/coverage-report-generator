using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using CoverageReportGenerator.Cli.Utils;
using CoverageReportGenerator.Core;
using System.Diagnostics;

namespace CoverageReportGenerator.Cli.Commands
{
    [Command("inspec")]
    internal class InspecCommand : ICommand
    {
        [CommandOption("verbose", 'v', IsRequired = false)]
        public bool Verbose { get; init; } = false;

        public async ValueTask ExecuteAsync(IConsole console)
        {
            // dotnet
            using var dotnet = CreateDotNetProcess();
            var dotnetResult = TryStartProcess(dotnet);
            await ValidateProcess("dotnet", console, dotnet, dotnetResult);
            CloseProcess(dotnet);

            // reportgenerator
            using var reportGenerator = CreateReportGeneratorProcess();
            var reportResult = TryStartProcess(reportGenerator);
            await ValidateProcess("reportgenerator", console, reportGenerator, reportResult);
            CloseProcess(reportGenerator);
        }

        private static Process CreateDotNetProcess()
        {
            return new ProcessBuilder("dotnet")
                .WithArgument("--version")
                .ConfigStartInfo(p => p.RedirectStandardOutput = true)
                .Build();
        }

        private static Process CreateReportGeneratorProcess()
        {
            return new ProcessBuilder("reportgenerator")
                .ConfigStartInfo(p => p.RedirectStandardOutput = true)
                .Build();
        }

        private static StartProcessResult TryStartProcess(in Process process)
        {
            try
            {
                var started = process.Start();

                return new StartProcessResult(started, null);
            }
            catch (Exception ex)
            {
                return StartProcessResult.Error(ex);
            }
        }

        private async Task ValidateProcess(
            string processName,
            IConsole console,
            Process process,
            StartProcessResult startProcessResult)
        {
            var (sucess, exception) = startProcessResult;

            if (sucess)
            {
                var output = await process.StandardOutput.ReadToEndAsync();

                console.Output.Console.ForegroundColor = ConsoleColor.Green;
                console.Output.WriteLine($"{processName} command found!");
                console.Output.Console.ResetColor();

                if (Verbose)
                    console.Output.WriteLine($"\tOutput: {output}");
            }
            else
            {
                console.Output.Console.ForegroundColor = ConsoleColor.Red;
                console.Output.WriteLine($"{processName} command not found!");
                console.Output.Console.ResetColor();

                if (Verbose && exception is not null)
                {
                    console.Output.WriteLine($"\t{exception?.Message}");
                }
            }
        }

        private static void CloseProcess(in Process process)
        {
            try
            {
                if (!process.HasExited)
                    process.WaitForExit();
            }
            finally
            {
                process.Close();
            }
        }
    }
}
