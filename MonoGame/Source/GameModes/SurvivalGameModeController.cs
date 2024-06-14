using Microsoft.Xna.Framework;

namespace MonoGame.Source.GameModes;
public class SurvivalGameModeController : GameModeController
{
    public override void Initialize()
    {
    }

    public override void Update()
    {
        Globals.Camera.Follow(Globals.World.GetLocalPlayer()?.Position ?? Vector2.Zero);
    }
}
