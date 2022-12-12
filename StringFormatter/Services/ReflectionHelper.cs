using System.Collections.Concurrent;
using System.Linq.Expressions;
using StringFormatter.Services;

namespace StringFormatter
{
    internal class ReflectionHelper
    {
        private readonly ConcurrentDictionary<PropertyGetterKey, Func<object, object>> propertyGetters;

        internal ReflectionHelper()
        {
            propertyGetters = new ConcurrentDictionary<PropertyGetterKey, Func<object, object>>();
        }

        public object? GetPropertyValue(object entity, string propertyName)
        {
            Func<object, object>? getter;

            var key = new PropertyGetterKey { Type = entity.GetType(), PropertyName = propertyName };

            if (propertyGetters.ContainsKey(key))
            {
                getter = propertyGetters[key];
            }
            else
            {
                getter = CreateGetter(entity, propertyName);
                if (getter is not null)
                {
                    propertyGetters.TryAdd(key, getter);
                }
                else
                {
                    return null;
                }
            }

            return getter(entity);
        }

        private static Func<object, object>? CreateGetter(object entity, string propertyName)
        {
            // instance => (object)((User)instance).firstName
            ParameterExpression param = Expression.Parameter(typeof(object), "instance");
            Expression<Func<object, object>> getterExpression;
            try
            {
                Expression paramAsEntityType = Expression.TypeAs(param, entity.GetType());
                Expression body = Expression.PropertyOrField(paramAsEntityType, propertyName);
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
