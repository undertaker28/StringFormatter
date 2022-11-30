using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace StringFormatter
{
    internal class ReflectionHelper
    {

        private class PropertyGetterKey
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
        private readonly ConcurrentDictionary<PropertyGetterKey, Func<object, object>> propertyGetters;
        internal ReflectionHelper()
        {
            propertyGetters = new ConcurrentDictionary<PropertyGetterKey, Func<object, object>>();
        }
        public object? GetPropertyValue(object entity, string propertyName)
        {
            Func<object, object> getter;

            var key = new PropertyGetterKey { Type = entity.GetType(), PropertyName = propertyName };

            if (propertyGetters.ContainsKey(key))
                getter = propertyGetters[key];
            else
            {
                getter = CreateGetter(entity, propertyName);
                if (getter != null)
                    propertyGetters.TryAdd(key, getter);
                else
                    return null;
            }

            return getter(entity);
        }

        private static Func<object, object>? CreateGetter(object entity, string propertyName)
        {
            var param = Expression.Parameter(typeof(object), "instance");
            Expression<Func<object, object>> getterExpression;
            try
            {
                Expression body = Expression.PropertyOrField(Expression.TypeAs(param, entity.GetType()), propertyName);
                Expression conversion = Expression.Convert(body, typeof(object));
                getterExpression = Expression.Lambda<Func<object, object>>(conversion, param);
            }
            catch
            {
                return null;
            }
            return getterExpression.Compile();
        }
    }
}
