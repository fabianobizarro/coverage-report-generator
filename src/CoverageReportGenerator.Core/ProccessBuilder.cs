using System.Diagnostics;

namespace CoverageReportGenerator.Core
{
    public class ProcessBuilder
    {
        private readonly List<Action<Process>> _modifiers = new();
        private readonly List<string> _arguments = new();

        public ProcessBuilder(string processName)
        {
            WithName(processName);
        }

        private void AddModifier(Action<Process> modifier)
            => _modifiers.Add(modifier);

        public ProcessBuilder WithName(string processName)
        {
            _modifiers.Add(p => p.StartInfo.FileName = processName);

            return this;
        }

        public ProcessBuilder WithArgument(string argument)
        {
            _arguments.Add(argument);

            return this;
        }

        public ProcessBuilder WithArgument(string key, string value)
            => WithArgument($"{key} {value}");

        public ProcessBuilder Config(Action<Process> config)
        {
            _modifiers.Add(config);

            return this;
        }

        public ProcessBuilder ConfigStartInfo(Action<ProcessStartInfo> config)
        {
            _modifiers.Add(p => config(p.StartInfo));

            return this;
        }

        public Process Build()
        {
            AddModifier(p => p.StartInfo.Arguments = string.Join(" ", _arguments));
            var process = new Process();

            foreach (var modifier in _modifiers)
            {
                modifier(process);
            }

            return process;
        }
    }
}
