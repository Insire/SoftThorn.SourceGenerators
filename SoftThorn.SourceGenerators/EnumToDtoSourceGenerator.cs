using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace SoftThorn.SourceGenerators
{
    [Generator]
    public class EnumToDtoSourceGenerator : IIncrementalGenerator
    {
        private const string EnumExtensionsAttribute = "SoftThorn.SourceGenerators.GenerateDtoAttribute";
        private const string DisplayAttribute = "System.ComponentModel.DataAnnotations.DisplayAttribute";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
                "EnumExtensionsAttribute.g.cs", SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8)));

            context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
                "EnumExtensionsDto.g.cs", SourceText.From(SourceGenerationHelper.Dto, Encoding.UTF8)));

            context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
                "EnumExtensionsBaseInterface.g.cs", SourceText.From(SourceGenerationHelper.BaseInterface, Encoding.UTF8)));

            context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
                "EnumExtensionsBaseService.g.cs", SourceText.From(SourceGenerationHelper.BaseService, Encoding.UTF8)));

            context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
                "EnumExtensionsGenericService.g.cs", SourceText.From(SourceGenerationHelper.GenericInterface, Encoding.UTF8)));

            var enumDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null)!;

            IncrementalValueProvider<(Compilation, ImmutableArray<EnumDeclarationSyntax>)> compilationAndEnums
                = context.CompilationProvider.Combine(enumDeclarations.Collect());

            context.RegisterSourceOutput(compilationAndEnums,
                static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
            => node is EnumDeclarationSyntax m && m.AttributeLists.Count > 0;

        private static EnumDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            // we know the node is a EnumDeclarationSyntax thanks to IsSyntaxTargetForGeneration
            var enumDeclarationSyntax = (EnumDeclarationSyntax)context.Node;

            // loop through all the attributes on the method
            foreach (var attributeListSyntax in enumDeclarationSyntax.AttributeLists)
            {
                foreach (var attributeSyntax in attributeListSyntax.Attributes)
                {
                    if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                    {
                        // weird, we couldn't get the symbol, ignore it
                        continue;
                    }

                    var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                    var fullName = attributeContainingTypeSymbol.ToDisplayString();

                    // Is the attribute the [EnumExtensions] attribute?
                    if (fullName == EnumExtensionsAttribute)
                    {
                        // return the enum
                        return enumDeclarationSyntax;
                    }
                }
            }

            // we didn't find the attribute we were looking for
            return null;
        }

        private static void Execute(Compilation compilation, ImmutableArray<EnumDeclarationSyntax> enums, SourceProductionContext context)
        {
            if (enums.IsDefaultOrEmpty)
            {
                // nothing to do yet
                return;
            }

            // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
            var distinctEnums = enums.Distinct();

            // Convert each EnumDeclarationSyntax to an EnumToGenerate
            var enumsToGenerate = GetTypesToGenerate(compilation, distinctEnums, context.CancellationToken);

            // If there were errors in the EnumDeclarationSyntax, we won't create an
            // EnumToGenerate for it, so make sure we have something to generate
            if (enumsToGenerate.Count > 0)
            {
                // generate the source code and add it to the output
                var result = SourceGenerationHelper.GenerateEnumServicClass(enumsToGenerate);
                context.AddSource("EnumExtensions.g.cs", SourceText.From(result, Encoding.UTF8));
            }
        }

        private static List<EnumToGenerate> GetTypesToGenerate(Compilation compilation, IEnumerable<EnumDeclarationSyntax> enums, CancellationToken ct)
        {
            var enumsToGenerate = new List<EnumToGenerate>();
            var enumAttribute = compilation.GetTypeByMetadataName(EnumExtensionsAttribute);
            if (enumAttribute == null)
            {
                // nothing to do if this type isn't available
                return enumsToGenerate;
            }

            foreach (var enumDeclarationSyntax in enums)
            {
                // stop if we're asked to
                ct.ThrowIfCancellationRequested();

                var semanticModel = compilation.GetSemanticModel(enumDeclarationSyntax.SyntaxTree);
                if (semanticModel.GetDeclaredSymbol(enumDeclarationSyntax) is not INamedTypeSymbol enumSymbol)
                {
                    // something went wrong
                    continue;
                }

                var enumName = enumSymbol.ToString();
                var enumMembers = enumSymbol.GetMembers();
                var members = new List<EnumValue>(enumMembers.Length);

                foreach (var member in enumMembers)
                {
                    if (member is IFieldSymbol field && field.ConstantValue is not null)
                    {
                        var attributes = field.GetAttributes();
                        if (attributes.Length == 0)
                        {
                            members.Add(new EnumValue(member.Name, member.Name, "0"));
                        }
                        else
                        {
                            var attribute = attributes[0];
                            if(attribute.AttributeClass?.ToDisplayString() == DisplayAttribute)
                            {
                                var dictionary = attribute.NamedArguments.ToDictionary(p => p.Key, p => p.Value);

                                var name = member.Name;
                                if(dictionary.TryGetValue("Name", out  var typedConstant ))
                                {
                                    name = typedConstant.ToCSharpString();
                                }

                                var order = "0";
                                if (dictionary.TryGetValue("Order", out  typedConstant))
                                {
                                    order = typedConstant.ToCSharpString();
                                }

                                members.Add(new EnumValue(name!, member.Name, order));
                            }
                        }
                    }
                }

                enumsToGenerate.Add(new EnumToGenerate(enumName, enumSymbol.Name, members));
            }

            return enumsToGenerate;
        }
    }
}
