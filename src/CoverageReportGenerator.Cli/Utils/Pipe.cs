using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverageReportGenerator.Cli.Utils
{
    internal static class PipeMethods
    {
        public static TOut Pipe<Tin, TOut>(this Tin input, Func<Tin, TOut> fn) => fn(input);
    }
}
