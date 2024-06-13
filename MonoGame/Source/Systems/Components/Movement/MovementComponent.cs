using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common.Messages.Player;
using MonoGame.Source.Multiplayer;
using System;

namespace MonoGame.Source.Systems.Components.Movement;

public class MovementComponent : EntityComponent
{
    private List<Keys> lastKeys = new();

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

        bool keysChanged = keys.Count != lastKeys.Count || keys.Except(lastKeys).Any();
        if (!keysChanged)
        {
            return;
        }

        lastKeys = keys;
        var commonsKeys = keys.Select(x => (MonoGame_Common.Enums.Keys)x).ToList();
        foreach (var key in keys)
        {
            Console.WriteLine(key);
        }
        NetworkClient.SendMessage(new KeyClickedNetworkMessage(Entity.UUID, commonsKeys));
    }
}