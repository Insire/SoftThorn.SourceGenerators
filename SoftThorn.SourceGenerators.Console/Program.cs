using System;

namespace SoftThorn.SourceGenerators.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");
        }
    }

    public interface IEnumService<TEnum> : IEnumService
    {
    }

    public sealed class ConsoleEnumService : EnumBaseService, IEnumService<ConsoleEnum>
    {
        public override IEnumerable<EnumDto> GetDtos()
        {
            yield return new EnumDto()
            {
                DisplayName = "",
                Id = (int)ConsoleEnum.Default,
                Order = 0,
            };
        }
    }
}
