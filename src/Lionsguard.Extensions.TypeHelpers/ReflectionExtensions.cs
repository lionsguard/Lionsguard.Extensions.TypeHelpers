using System.Reflection;

namespace System
{
    public static class ReflectionExtensions
    {
        public static bool IsSubclassOrAssignableFrom<T>(this Type source)
        {
            return source.IsSubclassOrAssignableFrom(typeof(T));
        }

        public static bool IsSubclassOrAssignableFrom(this Type source, Type type)
        {
            var sourceInfo = source.GetTypeInfo();
            var typeInfo = type.GetTypeInfo();

            return sourceInfo.IsSubclassOf(type) || sourceInfo.IsAssignableFrom(typeInfo);
        }
    }
}
