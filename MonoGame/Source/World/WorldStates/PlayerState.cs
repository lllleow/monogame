using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Entity.PlayerNamespace;

namespace MonoGame.Source.WorldNamespace.WorldStates;

public class PlayerState : INetSerializable
{
    public string UUID { get; set; }
    public Vector2? Position { get; set; }
    public string SelectedTile { get; set; }

    public PlayerState()
    {
    }

    public PlayerState(string uuid)
    {
        UUID = uuid;
        SelectedTile = null;
        Position = Globals.SpawnPosition;
    }

    public PlayerState(Player player)
    {
        SelectedTile = player.SelectedTile;
        Position = player.Position;
        UUID = player.UUID;
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(UUID);
        writer.Put(Position?.X ?? 0);
        writer.Put(Position?.Y ?? 0);
        writer.Put(SelectedTile);
    }

    public void Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        Position = new Vector2(reader.GetFloat(), reader.GetFloat());
        SelectedTile = reader.GetString();
    }
}
