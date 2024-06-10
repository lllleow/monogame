using MonoGame_Common.Attributes;
using System.Reflection;

namespace MonoGame_Common.Util;

public class MessageRegistry
{
    public static MessageRegistry Instance { get; } = new();
    private readonly Dictionary<int, Type> idToTypeMap = [];
    private readonly Dictionary<Type, int> typeToIdMap = [];

    public MessageRegistry()
    {
        RegisterMessages();
    }

    private void RegisterMessages()
    {
        var messageTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(NetworkMessageAttribute), false).Length > 0);

        foreach (var type in messageTypes)
        {
            var attribute = type.GetCustomAttribute<NetworkMessageAttribute>();
            idToTypeMap[attribute.Id] = type;
            typeToIdMap[type] = attribute.Id;
        }
    }

    public int GetIdByType(Type type)
    {
        return typeToIdMap[type];
    }

    public Type GetTypeById(int id)
    {
        return idToTypeMap[id];
    }
}