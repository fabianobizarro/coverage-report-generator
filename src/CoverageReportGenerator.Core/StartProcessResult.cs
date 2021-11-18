namespace CoverageReportGenerator.Cli.Utils
{
    public record StartProcessResult(bool Success, Exception? Exception)
    {
        public static StartProcessResult Ok() => new(true, null);
        public static StartProcessResult Error(Exception ex) => new(false, ex);
    }
}
