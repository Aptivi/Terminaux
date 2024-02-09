// Ported from https://github.com/xo/terminfo/blob/master/gen.go

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Humanizer;
using Scriban;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.IO;
using Terminaux.TermInfo;
using SpectreEnvironment = Spectre.IO.Environment;

namespace Generator.Commands
{
    public sealed class GenerateCommand : AsyncCommand<GenerateCommand.Settings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly IEnvironment _environment;

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<OUTPUT>")]
            public string Output { get; set; }

            [CommandOption("-i|--input <PATH>")]
            public string Input { get; set; }
        }

        public GenerateCommand()
        {
            _fileSystem = new FileSystem();
            _environment = new SpectreEnvironment();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var templates = new FilePath[]
            {
                $"Templates/{nameof(TermInfoCaps)}.template",
                $"Templates/{nameof(TermInfoDesc)}.Generated.template",
            };

            // Read all capabilities
            var capabilities = await ReadCapabilities(settings);

            // Ensure the output directory exist
            var output = new DirectoryPath(settings.Output);
            if (!_fileSystem.Directory.Exists(settings.Output))
            {
                _fileSystem.Directory.Create(settings.Output);
            }

            // Render all templates
            foreach (var templatePath in templates)
            {
                var outputFilename = templatePath.GetFilename().ChangeExtension(".cs");
                AnsiConsole.MarkupLine($"Generating [yellow]{outputFilename}[/]...");

                // Parse the Scriban template.
                var template = Template.Parse(File.ReadAllText(templatePath.FullPath));

                // Render the template with the model.
                var result = template.Render(new { Capabilities = capabilities });

                // Write output to file
                var file = output.CombineWithFilePath(outputFilename);
                File.WriteAllText(file.FullPath, result);
            }

            return 0;
        }

        private async Task<Capabilities> ReadCapabilities(Settings settings)
        {
            var result = new List<Capability>();
            var caps = await ReadCapabilitiesString(settings);

            AnsiConsole.MarkupLine($"Parsing [yellow]{CapsPath}[/]...");

            var lines = caps.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var indices = new Dictionary<CapabilityType, int>
            {
                { CapabilityType.Bool, 0 },
                { CapabilityType.Num, 0 },
                { CapabilityType.String, 0 },
            };

            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }

                var columns = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var name = columns[0].Trim();
                var description = columns[7].Trim();

                var type = columns[2] switch
                {
                    "bool" => CapabilityType.Bool,
                    "num" => CapabilityType.Num,
                    "str" => CapabilityType.String,
                    _ => throw new InvalidOperationException($"Unknown caps type '{columns[2]}' ({line})"),
                };

                var typeName = type switch
                {
                    CapabilityType.Bool => "bool",
                    CapabilityType.Num => "num",
                    CapabilityType.String => "string",
                    _ => throw new NotSupportedException(),
                };

                var prefix = type switch
                {
                    CapabilityType.Bool => "indicates",
                    CapabilityType.Num => "is",
                    CapabilityType.String => "is the",
                    _ => throw new NotSupportedException(),
                };

                result.Add(new Capability
                {
                    Name = name.Pascalize(),
                    Variable = name,
                    Type = type,
                    Index = indices[type],
                    Description = $"The {name} [{columns[0]}, {columns[1]}] {typeName} capability {prefix} {columns[7].Humanize(LetterCasing.LowerCase)}.",
                });

                indices[type] += 1;
            }

            var capabilities = new Capabilities(result);
            return capabilities;
        }

        private async Task<string> ReadCapabilitiesString(Settings settings)
        {
            using (var gzip = new GZipStream(await FetchSourcePackage(settings), CompressionMode.Decompress))
            {
                using var reader = SharpCompress.Readers.Tar.TarReader.Open(gzip);
                while (reader.MoveToNextEntry())
                {
                    if (reader.Entry.Key == CapsPath)
                    {
                        var output = new MemoryStream();
                        reader.WriteEntryTo(output);

                        output.Seek(0, SeekOrigin.Begin);
                        var streamReader = new StreamReader(output);
                        return streamReader.ReadToEnd();
                    }
                }
            }

            throw new InvalidOperationException("Could not find 'ncurses-6.4/include/Caps'");
        }

        private const string NCursesUrl = "https://ftp.gnu.org/pub/gnu/ncurses/ncurses-6.4.tar.gz";
        private const string NCursesFilename = "ncurses-6.4.tar.gz";
        private const string CapsPath = "ncurses-6.4/include/Caps";

        private async Task<Stream> FetchSourcePackage(Settings settings)
        {
            var input = string.IsNullOrEmpty(settings.Input)
                ? _environment.WorkingDirectory
                : new DirectoryPath(settings.Input);

            var file = _fileSystem.File.Retrieve(input.CombineWithFilePath(NCursesFilename));
            if (!file.Exists)
            {
                await AnsiConsole.Status()
                    .StartAsync($"Downloading [yellow]{NCursesUrl}[/]...", async ctx =>
                    {
                        using var http = new HttpClient();
                        using var httpStream = await http.GetStreamAsync(NCursesUrl);
                        using var outStream = file.OpenWrite();

                        await httpStream.CopyToAsync(outStream);
                    });
            }

            return file.OpenRead();
        }
    }
}
