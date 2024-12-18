namespace SoftThorn.SourceGenerators.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var service = new ConsoleEnumService();
            foreach (var value in service.GetDtos())
            {
                System.Console.WriteLine(value);
            }

            System.Console.ReadKey();
        }
    }
}
