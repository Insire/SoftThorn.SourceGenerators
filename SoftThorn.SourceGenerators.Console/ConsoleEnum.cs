namespace SoftThorn.SourceGenerators.Console
{
    [GenerateDto]
    public enum ConsoleEnum
    {
        [Display(Name = "None", Order = 0)]
        Default,

        [Display(Name = "One", Order = 1)]
        FirstValue,

        [Display(Name = "Two", Order = 2)]
        SecondValue,
    }
}
