using System.ComponentModel.DataAnnotations;

namespace SoftThorn.SourceGenerators.Tests
{
    /// <summary>
    /// class level xml comment
    /// </summary>
    [GenerateDto]
    public enum Color
    {
        //
        [Display(Name = "None", Order = 0)]
        Default = 0,

        // code comment
        [Display(Order = 1)]
        Red = 1,

        /// <summary>
        /// member xml comment
        /// </summary>
        [Display(Order = 3)]
        Blue = 2,

        [Display(Order = 2)]
        Green = 3,

        //
    }
}
