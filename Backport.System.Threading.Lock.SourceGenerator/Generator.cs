using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Backport.System.Threading.Lock.SourceGenerator;

[Generator]
internal sealed class Generator : IIncrementalGenerator
{
    private static readonly Assembly Assembly = typeof(Generator).Assembly;

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