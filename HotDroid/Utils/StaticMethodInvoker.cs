using System.Reflection;

namespace HotDroid.Utils;

public static class StaticMethodInvoker
{
    public static object? InvokeStaticMethod(Type type, string methodName, params object[] parameters)
    {
        var parameterTypes = parameters?.Select(p => p?.GetType()).ToArray() ?? [];
        var method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, null, parameterTypes, null);
        
        if (method == null)
        {
            // Try to find method by name only if exact parameter match fails
            method = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == methodName);
        }
        
        return method?.Invoke(null, parameters);
    }
}