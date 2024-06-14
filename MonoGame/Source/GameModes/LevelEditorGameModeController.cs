using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common.Messages.Player;
using MonoGame.Source.Multiplayer;

namespace MonoGame.Source.GameModes;

public class LevelEditorGameModeController : GameModeController
{
    public Vector2 CameraPosition { get; set; } = new(0, 0);

    private List<Keys> lastKeys = new();

    public override void Initialize()
    {
        ClientNetworkEventManager.Subscribe<SetLevelEditorCameraPositionNetworkMessage>(message =>
        {
            if (message.UUID == Globals.World.GetLocalPlayer()?.UUID)
            {
                CameraPosition = message.Position;
            }
        });
    }

    public override void Update()
    {
        Globals.Camera.Follow(CameraPosition);

        var state = Keyboard.GetState();
        List<Keys> keys = [];

        if (state.IsKeyDown(Keys.W))
        {
            keys.Add(Keys.W);
        }

        if (state.IsKeyDown(Keys.A))
        {
            keys.Add(Keys.A);
        }

        if (state.IsKeyDown(Keys.S))
        {
            keys.Add(Keys.S);
        }

        if (state.IsKeyDown(Keys.D))
        {
            keys.Add(Keys.D);
        }

        bool keysChanged = keys.Count != lastKeys.Count || keys.Except(lastKeys).Any();
        if (!keysChanged)
        {
            return;
        }

        lastKeys = keys;
        var commonsKeys = keys.Select(x => (MonoGame_Common.Enums.Keys)x).ToList();

        NetworkClient.SendMessage(new KeyClickedNetworkMessage()
        {
            UUID = Globals.World.GetLocalPlayer()?.UUID ?? "",
            Keys = commonsKeys
        });
    }
}
