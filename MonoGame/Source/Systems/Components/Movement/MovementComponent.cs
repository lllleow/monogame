using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Multiplayer;
using MonoGame_Common.Messages.Player;

namespace MonoGame.Source.Systems.Components.Movement;

public class MovementComponent : EntityComponent
{
    private bool sentEmptyKeysMessage;
    public Vector2 Speed { get; set; } = new(1, 1);

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

        var commonsKeys = keys.Select(x => (MonoGame_Common.Enums.Keys)x).ToList();

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