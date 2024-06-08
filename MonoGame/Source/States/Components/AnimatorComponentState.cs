using LiteNetLib.Utils;

namespace MonoGame.Source.States.Components;

public class AnimatorComponentState : ComponentState
{
    public int TextureX { get; set; }
    public int TextureY { get; set; }
    public int CurrentTime { get; set; }

    public AnimatorComponentState()
    {
        TextureX = 0;
        TextureY = 0;
        CurrentTime = 0;
    }

    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(TextureX);
        writer.Put(TextureY);
        writer.Put(CurrentTime);
    }

    public override void Deserialize(NetDataReader reader)
    {
        TextureX = reader.GetInt();
        TextureY = reader.GetInt();
        CurrentTime = reader.GetInt();
    }
}
