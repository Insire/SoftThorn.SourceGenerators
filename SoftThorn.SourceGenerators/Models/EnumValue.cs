namespace SoftThorn.SourceGenerators
{
    public readonly struct EnumValue
    {
        public readonly string DisplayName;
        public readonly string Order;
        public readonly string Value;

        public EnumValue(string displayName,string value, string order)
        {
            DisplayName = displayName;
            Value = value;
            Order = order;
        }
    }
}
