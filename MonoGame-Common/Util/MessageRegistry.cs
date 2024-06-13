using System.Reflection;
using MonoGame_Common.Attributes;

namespace MonoGame_Common.Util;

public class MessageRegistry
{
    private readonly Dictionary<int, Type> idToTypeMap = [];
    private readonly Dictionary<Type, int> typeToIdMap = [];

    public MessageRegistry()
    {
        RegisterMessages();
    }

    public static MessageRegistry Instance { get; } = new();

    private void RegisterMessages()
    {
        var messageTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(NetworkMessageAttribute), false).Length > 0);

        foreach (var type in messageTypes)
        {
            var attribute = type.GetCustomAttribute<NetworkMessageAttribute>();
            var id = attribute?.Id;
            if (id == null)
            {
                continue;
            }
            else
            {
                idToTypeMap[id.Value] = type;
                typeToIdMap[type] = id.Value;
            }
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