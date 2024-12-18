namespace SoftThorn.SourceGenerators
{
    public readonly struct EnumToGenerate(string fullName, string name, List<EnumValue> values)
    {
        /// <summary>
        /// Name including namespace
        /// </summary>
        public readonly string FullName = fullName;

        /// <summary>
        /// Only the TypeName
        /// </summary>
        public readonly string Name = name;

        /// <summary>
        /// Enum values
        /// </summary>
        public readonly List<EnumValue> Values = values;
    }
}
