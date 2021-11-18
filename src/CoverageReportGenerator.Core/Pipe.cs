namespace CoverageReportGenerator.Core
{
    public static class PipeMethods
    {
        public static TOut Pipe<Tin, TOut>(this Tin input, Func<Tin, TOut> fn) => fn(input);
    }
}
