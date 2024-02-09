using System.Diagnostics;
using Spectre.Console;
using Spectre.Console.Cli;
using Terminaux.TermInfo;

namespace Generator.Commands
{
    public sealed class InspectCommand : Command<InspectCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [CommandOption("-n|--name <NAME>")]
            public string Name { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var desc = LoadTermInfoDesc(settings);
            if (desc == null)
            {
                AnsiConsole.WriteLine("Could not find terminfo file");
                return -1;
            }

            var names = string.Join("[yellow],[/] ", desc.Names);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[yellow]Names:[/] {names}");
            AnsiConsole.MarkupLine($"[yellow]Extended caps:[/] {desc.Extended.Count}");
            AnsiConsole.WriteLine();

            if (desc.Extended.Count > 0)
            {
                RenderExtendedCapabilitiesTable(desc);
            }

            RenderCapabilitiesTable(desc);

            return 0;
        }

        private static void RenderCapabilitiesTable(TermInfoDesc desc)
        {
            var table = new Table();
            table.Expand();
            table.Title("[green]Default capabilities[/]");
            table.AddColumns("Name", "Type", "Value");

            foreach (int key in System.Enum.GetValues(typeof(TermInfoCaps.Num)))
            {
                var value = desc.GetNum((TermInfoCaps.Num)key);
                if (value != null)
                {
                    table.AddRow(
                        "[yellow]" + ((TermInfoCaps.Num)key).ToString() + "[/]",
                        "[grey]num[/]",
                        value.ToString().EscapeMarkup());
                }
            }

            foreach (int key in System.Enum.GetValues(typeof(TermInfoCaps.String)))
            {
                var value = desc.GetString((TermInfoCaps.String)key);
                if (value != null)
                {
                    if (key == 38 || key == 39)
                    {
                        if (!Debugger.IsAttached)
                        {
                            //Debugger.Launch();
                        }
                    }

                    value = value.Replace("\u001b", "ESC")
                        .Replace("\u000e", "")
                        .Replace("\u000f", "")
                        .Replace("\t", "\\t")
                        .Replace("\r", "\\r")
                        .Replace("\a", "\\a")
                        .Replace("\n", "\\n");
                    table.AddRow(
                        "[yellow]" + ((TermInfoCaps.String)key).ToString().EscapeMarkup() + "[/]",
                        "[grey]string[/]",
                        value.EscapeMarkup());
                }
            }

            foreach (int key in System.Enum.GetValues(typeof(TermInfoCaps.Boolean)))
            {
                var value = desc.GetBoolean((TermInfoCaps.Boolean)key);
                if (value != null)
                {
                    table.AddRow(
                        "[yellow]" + ((TermInfoCaps.Boolean)key).ToString() + "[/]",
                        "[grey]boolean[/]",
                        value.ToString().EscapeMarkup());
                }
            }

            AnsiConsole.Write(table);
        }

        private static void RenderExtendedCapabilitiesTable(TermInfoDesc desc)
        {
            var table = new Table();
            table.Expand();
            table.Title("[green]Extended capabilities[/]");
            table.AddColumns("Name", "Type", "Value");

            foreach (var key in desc.Extended.GetNames(TermInfoCapsKind.Num))
            {
                var value = desc.Extended.GetNum(key);
                if (value != null)
                {
                    table.AddRow(
                        "[yellow]" + key + "[/]",
                        "[grey]num[/]",
                        value.ToString().EscapeMarkup());
                }
            }

            foreach (var key in desc.Extended.GetNames(TermInfoCapsKind.String))
            {
                var value = desc.Extended.GetString(key);
                if (value != null)
                {
                    value = value.Replace("\u001b", "ESC")
                        .Replace("\u000e", "")
                        .Replace("\u000f", "")
                        .Replace("\t", "\\t")
                        .Replace("\r", "\\r")
                        .Replace("\a", "\\a")
                        .Replace("\n", "\\n");
                    table.AddRow(
                        "[yellow]" + key.EscapeMarkup() + "[/]",
                        "[grey]string[/]",
                        value.EscapeMarkup());
                }
            }

            foreach (var key in desc.Extended.GetNames(TermInfoCapsKind.Boolean))
            {
                var value = desc.Extended.GetBoolean(key);
                if (value != null)
                {
                    table.AddRow(
                        "[yellow]" + key.EscapeMarkup() + "[/]",
                        "[grey]boolean[/]",
                        value.ToString().EscapeMarkup());
                }
            }

            AnsiConsole.Write(table);
        }

        private static TermInfoDesc LoadTermInfoDesc(Settings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.Name))
            {
                return TermInfoDesc.Load(settings.Name);
            }

            return TermInfoDesc.Load();
        }
    }
}
