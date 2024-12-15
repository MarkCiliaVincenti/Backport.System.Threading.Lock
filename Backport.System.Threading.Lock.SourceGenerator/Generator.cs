// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Backport.System.Threading.Lock.SourceGenerator;

/// <summary>
/// Source generator for Backport.System.Threading.Lock.
/// </summary>
[Generator]
internal sealed class Generator : IIncrementalGenerator
{
    private static readonly Assembly Assembly = typeof(Generator).Assembly;

    /// <summary>
    /// <inheritdoc cref="IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext)"/>
    /// </summary>
    /// <param name="context"><inheritdoc cref="IncrementalGeneratorInitializationContext"/></param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
        => context.RegisterSourceOutput(context.CompilationProvider, Generate);

    private static void Generate(SourceProductionContext context, Compilation compilation)
    {
        var isReferenced = compilation.ReferencedAssemblyNames
            .Any(x => x.Name == "Backport.System.Threading.Lock");

        if (isReferenced)
        {
            // If the runtime package is referenced, we shouldn't emit the code.
            return;
        }

        foreach (var resource in Assembly.GetManifestResourceNames())
        {
            Generate(context, resource);
        }
    }

    private static void Generate(SourceProductionContext context, string resourceName)
    {
        using var stream = Assembly.GetManifestResourceStream(resourceName);
        using var streamReader = new StreamReader(stream);
        var text = streamReader.ReadToEnd();

        var sb = new StringBuilder();
        sb.AppendLine("#define SOURCE_GENERATOR");
        sb.AppendLine(text);

        context.AddSource(resourceName, sb.ToString());
    }
}
