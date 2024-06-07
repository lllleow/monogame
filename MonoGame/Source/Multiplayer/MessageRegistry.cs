using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoGame;

public class MessageRegistry
{
    public static MessageRegistry Instance { get; } = new();
    private Dictionary<int, Type> idToTypeMap = new Dictionary<int, Type>();
    private Dictionary<Type, int> typeToIdMap = new Dictionary<Type, int>();

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

    public int GetIdByType(Type type) => typeToIdMap[type];
    public Type GetTypeById(int id) => idToTypeMap[id];
}