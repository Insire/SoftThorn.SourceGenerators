namespace SoftThorn.SourceGenerators
{
    public readonly struct EnumToGenerate
    {
        /// <summary>
        /// Name including namespace
        /// </summary>
        public readonly string FullName;

        /// <summary>
        /// Only the TypeName
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Enum values
        /// </summary>
        public readonly List<EnumValue> Values;

        public EnumToGenerate(string fullName, string name, List<EnumValue> values)
        {
            FullName = fullName;
            Name = name;
            Values = values;
        }
    }
}
