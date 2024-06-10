using LiteNetLib.Utils;

namespace MonoGame_Common.States;

public class PlayerState : EntityState
{
    public PlayerState()
    {
    }

    public PlayerState(string uuid)
    {
        UUID = uuid;
        SelectedTile = null;
    }

    public string? SelectedTile { get; set; }

    // public PlayerState(Player player) : base(player)
    // {
    //     SelectedTile = player.SelectedTile;
    //     Position = player.Position;
    // }
    public override void Serialize(NetDataWriter writer)
    {
        base.Serialize(writer);
        writer.Put(SelectedTile);
    }

    public override void Deserialize(NetDataReader reader)
    {
        base.Deserialize(reader);
        SelectedTile = reader.GetString();
    }
}