using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common.Messages.Player;
using MonoGame.Source.Multiplayer;
using System.Linq;

namespace MonoGame.Source.Systems.Components.Movement;

public class MovementComponent : EntityComponent
{
    public Vector2 Speed { get; set; } = new(1, 1);
    private bool sentEmptyKeysMessage = false;

    public override void Update(GameTime gameTime)
    {
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

        List<MonoGame_Common.Enums.Keys> commonsKeys = keys.Select(x => (MonoGame_Common.Enums.Keys)x).ToList();

        if (keys.Count > 0)
        {
            sentEmptyKeysMessage = false;
            NetworkClient.SendMessage(new KeyClickedNetworkMessage(Entity.UUID, commonsKeys));
        }
        else if (!sentEmptyKeysMessage)
        {
            NetworkClient.SendMessage(new KeyClickedNetworkMessage(Entity.UUID, commonsKeys));
            sentEmptyKeysMessage = true;
        }
    }
}
