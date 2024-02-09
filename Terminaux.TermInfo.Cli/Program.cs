using Generator.Commands;
using Spectre.Console.Cli;

namespace Generator
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp<GenerateCommand>();
            app.Configure(config =>
            {
                config.AddCommand<GenerateCommand>("generate");
                config.AddCommand<InspectCommand>("inspect");
            });

            return app.Run(args);
        }
    }
}
