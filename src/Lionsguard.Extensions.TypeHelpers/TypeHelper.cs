using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class TypeHelper
    {
        public static IEnumerable<Type> GetTypesWithCustomAttributes<TType, TAttribute>(bool isAbstract = false, bool isInterface = false)
            where TAttribute : Attribute
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes()
                    .Where(t => t.IsAbstract == isAbstract
                        && t.IsInterface == isInterface
                        && (t.GetTypeInfo().IsSubclassOf(typeof(TType)) || typeof(TType).GetTypeInfo().IsAssignableFrom(t))
                        && t.GetTypeInfo().GetCustomAttributes<TAttribute>().Any()
                        ));
            }
            return types;
        }

        public static T ChangeType<T>(object value)
        {
            try
            {
                if (value == null)
                    return default;

                if (typeof(T).IsEnum)
                {
                    try
                    {
                        return (T)Enum.Parse(typeof(T), value.ToString());
                    }
                    catch (Exception)
                    {
                    }
                    return default;
                }
                if (typeof(T) == typeof(Guid))
                {
                    Guid.TryParse(value.ToString(), out var guid);
                    return (T)(object)guid;
                }
                if (typeof(T) == typeof(DateTime))
                {
                    if (long.TryParse(value.ToString(), out var ticks))
                    {
                        return (T)(object)new DateTime(ticks);
                    }
                    else
                    {
                        DateTime.TryParse(value.ToString(), out var dt);
                        return (T)(object)dt;
                    }
                }

                if (value != null && typeof(T).IsSubclassOrAssignableFrom(value.GetType()))
                    return (T)value;

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }
    }
}
