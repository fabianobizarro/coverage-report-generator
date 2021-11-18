namespace CoverageReportGenerator.Core
{
    public class PathHelper
    {
        private readonly Guid _guid = Guid.NewGuid();

        public const string BaseDirectory = "reportgenerator";

        public string CompleteInputPath(string input)
            => input.Pipe(path => !string.IsNullOrWhiteSpace(path) ? path : throw new ArgumentNullException(nameof(input)))
                    .Pipe(path => Path.GetFullPath(path));

        public string CompleteOutputPath(string output)
            => output.Pipe(path => string.IsNullOrWhiteSpace(path) ? GetTempPath() : path)
                     .Pipe(path => Path.GetFullPath(path));

        public string GetTempPath(string baseDirectory = BaseDirectory)
            => Path.Combine(Path.GetTempPath(), baseDirectory, _guid.ToString());
    }
}