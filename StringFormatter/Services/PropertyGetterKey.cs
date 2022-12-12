namespace StringFormatter.Services
{
    internal class PropertyGetterKey
    {
        internal Type? Type { get; set; }
        internal string? PropertyName { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PropertyGetterKey key &&
                   EqualityComparer<Type>.Default.Equals(Type, key.Type) &&
                   PropertyName == key.PropertyName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, PropertyName);
        }
    }
}
