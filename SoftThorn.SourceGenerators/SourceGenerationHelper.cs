using System.Text;

namespace SoftThorn.SourceGenerators
{
    public static class SourceGenerationHelper
    {
        public const string Attribute = """

                                        namespace SoftThorn.SourceGenerators
                                        {
                                            [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
                                            public sealed class GenerateDtoAttribute : Attribute;
                                        }
                                        """;


        public const string Dto = """

                                  namespace SoftThorn.SourceGenerators
                                  {
                                      public readonly record struct EnumDto
                                      {
                                          required public int Id { get; init; }

                                          required public string DisplayName { get; init; }

                                          required public int Order { get; init; }
                                      }
                                  }
                                  """;

        public const string BaseInterface = """

                                            namespace SoftThorn.SourceGenerators
                                            {
                                                public interface IEnumService
                                                {
                                                    IEnumerable<EnumDto> GetDtos();
                                                }
                                            }
                                            """;

        public const string GenericInterface = """

                                               namespace SoftThorn.SourceGenerators
                                               {
                                                   public interface IEnumService<TEnum> : IEnumService;
                                               }
                                               """;

        public const string BaseService = """

                                          namespace SoftThorn.SourceGenerators
                                          {
                                              public abstract class EnumBaseService : IEnumService
                                              {
                                                  public abstract IEnumerable<EnumDto> GetDtos();
                                              }
                                          }
                                          """;

        public static string GenerateEnumServiceClass(List<EnumToGenerate> enumsToGenerate)
        {
            var sb = new StringBuilder();

            sb.Append(@"
namespace SoftThorn.SourceGenerators
{");
            foreach (var enumToGenerate in enumsToGenerate)
            {
                sb.Append(@"
    public sealed class ").Append(enumToGenerate.Name).Append(@"Service : EnumBaseService, IEnumService<").Append(enumToGenerate.FullName).Append(@">
    {").Append(@"
        public override IEnumerable<EnumDto> GetDtos()
        {");
                foreach (var member in enumToGenerate.Values)
                {
                    sb.Append(@"
            yield return new EnumDto()
            {
                Id = (int)").Append(enumToGenerate.FullName).Append('.').Append(member.Value).Append(@",
                DisplayName = ").Append(member.DisplayName).Append(',').Append(@"
                Order = ").Append(member.Order).Append(',').Append(@"
            };").AppendLine();
                }

                sb.Append(@"		}");
            }

            sb.Append(@"
    }
}");
            return sb.ToString();
        }
    }
}
