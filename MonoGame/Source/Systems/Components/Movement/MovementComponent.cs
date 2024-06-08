using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Systems.Components;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Components.PixelBounds;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Util.Enum;

namespace MonoGame;

public class MovementComponent : EntityComponent
{
    public Vector2 Speed { get; set; } = new(1, 1);
    private bool sentEmptyKeysMessage = false;

    public override void Update(GameTime gameTime)
    {
        var state = Keyboard.GetState();
        List<Keys> keys = new();

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

        if (keys.Count > 0)
        {
            sentEmptyKeysMessage = false;
            NetworkClient.SendMessage(new KeyClickedNetworkMessage(Entity.UUID, keys));
        }
        else if (!sentEmptyKeysMessage)
        {
            NetworkClient.SendMessage(new KeyClickedNetworkMessage(Entity.UUID, keys));
            sentEmptyKeysMessage = true;
        }
    }
}
