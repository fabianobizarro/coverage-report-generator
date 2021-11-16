using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateReportGenerator.Cli.Commands
{
    [Command("hello")]
    internal class HelloCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.Write("World");

            return default;
        }
    }
}
