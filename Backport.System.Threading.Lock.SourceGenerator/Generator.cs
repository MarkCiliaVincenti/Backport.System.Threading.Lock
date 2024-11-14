using Microsoft.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;

namespace Backport.System.Threading.Lock.SourceGenerator;

[Generator]
internal sealed class Generator : IIncrementalGenerator
{
    private static readonly Assembly Assembly = typeof(Generator).Assembly;

    public void Initialize(IncrementalGeneratorInitializationContext context)
        => context.RegisterPostInitializationOutput(Generate);

    private static void Generate(IncrementalGeneratorPostInitializationContext context)
    {
        foreach (var resource in Assembly.GetManifestResourceNames())
        {
            Generate(context, resource);
        }
    }

    private static void Generate(IncrementalGeneratorPostInitializationContext context, string resourceName)
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