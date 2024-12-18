namespace SoftThorn.SourceGenerators
{
    public readonly struct EnumValue(string displayName, string value, string order)
    {
        public readonly string DisplayName = displayName;

        public readonly string Order = order;

        public readonly string Value = value;
    }
}
