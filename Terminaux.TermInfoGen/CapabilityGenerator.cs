//
// Terminaux  Copyright (C) 2023-2024  Aptivi
//
// This file is part of Terminaux
//
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Terminaux.TermInfoGen
{
    [Generator]
    public class CapabilityGenerator : IIncrementalGenerator
    {
        private string capabilityContent = "";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Get the color data content
            var asm = typeof(CapabilityGenerator).Assembly;
            var stream = asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.Caps");
            using var reader = new StreamReader(stream);
            capabilityContent = reader.ReadToEnd();

            // Read all the terminal capabilities
            var capabilities = ReadCapabilities();

            // Generate all the capabilities
            GenerateCapabilities(capabilities, context);
            GenerateDescriptions(capabilities, context);
        }

        private void GenerateCapabilities(Capabilities capabilities, IncrementalGeneratorInitializationContext context)
        {
            string header =
                $$"""
                //
                // Terminaux  Copyright (C) 2023-2024  Aptivi
                //
                // This file is part of Terminaux
                //
                // Terminaux is free software: you can redistribute it and/or modify
                // it under the terms of the GNU General Public License as published by
                // the Free Software Foundation, either version 3 of the License, or
                // (at your option) any later version.
                //
                // Terminaux is distributed in the hope that it will be useful,
                // but WITHOUT ANY WARRANTY, without even the implied warranty of
                // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
                // GNU General Public License for more details.
                //
                // You should have received a copy of the GNU General Public License
                // along with this program.  If not, see <https://www.gnu.org/licenses/>.
                //
                
                // <auto-generated/>
                
                namespace Terminaux.Base.TermInfo
                {
                    /// <summary>
                    /// Represents known terminal information capabilities.
                    /// </summary>
                    public static class TermInfoCaps
                    {
                
                """;
            string footer =
                $$"""
                    }
                }
                """;
            string enumFooter =
                $$"""
                        }

                """;
            var builder = new StringBuilder(header);

            // Make an enumeration in this class for each type
            string boolHeader =
                $$$"""
                        /// <summary>
                        /// Represents known boolean terminfo capabilities.
                        /// </summary>
                        public enum Boolean
                        {
                """;
            builder.AppendLine(boolHeader);
            foreach (var cap in capabilities.Booleans)
            {
                string prop =
                    $$"""
                                /// <summary>
                                /// {{cap.Description}}
                                /// </summary>
                                {{cap.Name}} = {{cap.Index}},
                    """;
                builder.AppendLine(prop);
            }
            builder.AppendLine(enumFooter);

            // The integer TermInfo variables
            string numHeader =
                $$$"""
                        /// <summary>
                        /// Represents known numeric terminfo capabilities.
                        /// </summary>
                        public enum Num
                        {
                """;
            builder.AppendLine(numHeader);
            foreach (var cap in capabilities.Nums)
            {
                string prop =
                    $$"""
                                /// <summary>
                                /// {{cap.Description}}
                                /// </summary>
                                {{cap.Name}} = {{cap.Index}},
                    """;
                builder.AppendLine(prop);
            }
            builder.AppendLine(enumFooter);

            // The string TermInfo variables
            string stringHeader =
                $$$"""
                        /// <summary>
                        /// Represents known string terminfo capabilities.
                        /// </summary>
                        public enum String
                        {
                """;
            builder.AppendLine(stringHeader);
            foreach (var cap in capabilities.Strings)
            {
                string prop =
                    $$"""
                                /// <summary>
                                /// {{cap.Description}}
                                /// </summary>
                                {{cap.Name}} = {{cap.Index}},
                    """;
                builder.AppendLine(prop);
            }
            builder.AppendLine(enumFooter);

            // End the file
            builder.AppendLine(footer);
            context.RegisterPostInitializationOutput(ctx =>
            {
                ctx.AddSource("TermInfoCaps.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
            });
        }

        private void GenerateDescriptions(Capabilities capabilities, IncrementalGeneratorInitializationContext context)
        {
            string header =
                $$"""
                //
                // Terminaux  Copyright (C) 2023-2024  Aptivi
                //
                // This file is part of Terminaux
                //
                // Terminaux is free software: you can redistribute it and/or modify
                // it under the terms of the GNU General Public License as published by
                // the Free Software Foundation, either version 3 of the License, or
                // (at your option) any later version.
                //
                // Terminaux is distributed in the hope that it will be useful,
                // but WITHOUT ANY WARRANTY, without even the implied warranty of
                // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
                // GNU General Public License for more details.
                //
                // You should have received a copy of the GNU General Public License
                // along with this program.  If not, see <https://www.gnu.org/licenses/>.
                //
                
                // <auto-generated/>
                #nullable enable

                using Terminaux.Base.TermInfo.Parsing;

                namespace Terminaux.Base.TermInfo
                {
                    public sealed partial class TermInfoDesc
                    {
                
                """;
            string footer =
                $$"""
                    }
                }
                """;
            var builder = new StringBuilder(header);

            // Iterate through all the capabilities, starting with the boolean TermInfo variables
            foreach (var cap in capabilities.Booleans)
            {
                string prop =
                    $$"""
                            /// <summary>
                            /// {{cap.Description}}
                            /// </summary>
                            public TermInfoValueDesc<bool?>? {{cap.Name}} =>
                                GetBoolean(TermInfoCaps.Boolean.{{cap.Name}});

                    """;
                builder.AppendLine(prop);
            }

            // The integer TermInfo variables
            foreach (var cap in capabilities.Nums)
            {
                string prop =
                    $$"""
                            /// <summary>
                            /// {{cap.Description}}
                            /// </summary>
                            public TermInfoValueDesc<int?>? {{cap.Name}} =>
                                GetNum(TermInfoCaps.Num.{{cap.Name}});

                    """;
                builder.AppendLine(prop);
            }

            // The string TermInfo variables
            foreach (var cap in capabilities.Strings)
            {
                string prop =
                    $$"""
                            /// <summary>
                            /// {{cap.Description}}
                            /// </summary>
                            public TermInfoValueDesc<string?>? {{cap.Name}} =>
                                GetString(TermInfoCaps.String.{{cap.Name}});

                    """;
                builder.AppendLine(prop);
            }

            // End the file
            builder.AppendLine(footer);
            context.RegisterPostInitializationOutput(ctx =>
            {
                ctx.AddSource("TermInfoDesc.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
            });
        }

        private Capabilities ReadCapabilities()
        {
            // Get the capabilities from NCurses
            var result = new List<Capability>();
            var caps = capabilityContent.Replace("\r", "");
            var lines = caps.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var indices = new Dictionary<CapabilityType, int>
            {
                { CapabilityType.Bool, 0 },
                { CapabilityType.Num, 0 },
                { CapabilityType.String, 0 },
            };

            foreach (var line in lines)
            {
                // Ignore all the comments
                if (line.StartsWith("#"))
                    continue;

                // Split the tabs to get values
                var columns = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var name = columns[0].Trim();
                var description = columns[7].Trim();

                // Figure out the capability type
                var type = columns[2] switch
                {
                    "bool" => CapabilityType.Bool,
                    "num" => CapabilityType.Num,
                    "str" => CapabilityType.String,
                    _ => throw new InvalidOperationException($"Unknown caps type '{columns[2]}' ({line})"),
                };

                // Figure out the type name
                var typeName = type switch
                {
                    CapabilityType.Bool => "bool",
                    CapabilityType.Num => "num",
                    CapabilityType.String => "string",
                    _ => throw new NotSupportedException(),
                };

                // Figure out the prefix by type
                var prefix = type switch
                {
                    CapabilityType.Bool => "indicates",
                    CapabilityType.Num => "is",
                    CapabilityType.String => "is the",
                    _ => throw new NotSupportedException(),
                };

                // Add a capability
                result.Add(
                    new Capability
                    {
                        Name = name.Pascalize(),
                        Variable = name,
                        Type = type,
                        Index = indices[type],
                        Description = $"The {name} [{columns[0]}, {columns[1]}] {typeName} capability {prefix} {columns[7].Humanize(LetterCasing.LowerCase)}.",
                    }
                );

                // Increase an index
                indices[type]++;
            }

            var capabilities = new Capabilities(result);
            return capabilities;
        }
    }
}
