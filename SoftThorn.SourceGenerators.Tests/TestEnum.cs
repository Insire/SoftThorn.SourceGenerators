using System.ComponentModel.DataAnnotations;

namespace SoftThorn.SourceGenerators.Tests
{
    [GenerateDto]
    public enum TestEnum
    {
        [Display(Name = "None", Order = 1)]
        Default = 2,

        [Display(Name = "One", Order = 2)]
        FirstValue = 3,

        [Display(Name = "Two", Order = 3)]
        SecondValue = 4,
    }
}
