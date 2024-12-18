using System.ComponentModel.DataAnnotations;

namespace SoftThorn.SourceGenerators.Tests
{
    [GenerateDto]
    public enum Color
    {
        [Display(Name = "None", Order = 0)]
        Default = 0,

        [Display(Order = 1)]
        Red = 1,

        [Display(Order = 3)]
        Blue = 2,

        [Display(Order = 2)]
        Green = 3,
    }
}
