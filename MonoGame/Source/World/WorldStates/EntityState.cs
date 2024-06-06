using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class EntityState : INetSerializable
{
    public Vector2 Position { get; set; }

    public EntityState()
    {
    }

    public EntityState(IGameEntity entity)
    {
        Position = entity.Position;
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Position.X);
        writer.Put(Position.Y);
    }

    public void Deserialize(NetDataReader reader)
    {
        Position = new Vector2(reader.GetFloat(), reader.GetFloat());
    }
}
