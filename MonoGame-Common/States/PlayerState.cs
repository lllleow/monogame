using LiteNetLib.Utils;

namespace MonoGame_Common.States;

public class PlayerState : EntityState
{
    required public GameMode GameMode { get; set; }

    public PlayerState()
    {
    }

    public PlayerState(string uuid)
    {
        UUID = uuid;
        SelectedTile = null;
    }

    public string? SelectedTile { get; set; }

    public override void Serialize(NetDataWriter writer)
    {
        base.Serialize(writer);
        writer.Put((byte)GameMode);
        writer.Put(SelectedTile);
    }

    public override void Deserialize(NetDataReader reader)
    {
        base.Deserialize(reader);
        GameMode = (GameMode)reader.GetByte();
        SelectedTile = reader.GetString();
    }
}