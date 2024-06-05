using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaces;

namespace MonoGame;

public class UserInterfaceHandler
{
    public List<IUserInterface> UserInterfaces { get; set; } = new List<IUserInterface>();
    public float ScaleFactor { get; set; } = 3f;
    public Matrix Transform { get; set; } = Matrix.CreateScale(3f, 3f, 1f);
    public Vector2 UIScreenSize { get; set; } = new Vector2(1280, 720);

    public void Initialize()
    {
        UserInterfaces.Add(new LevelEditorUserInterface());

        Vector2 transformed = Vector2.Transform(new Vector2(Globals.graphicsDevice.GraphicsDevice.Viewport.Width, Globals.graphicsDevice.GraphicsDevice.Viewport.Height), Matrix.Invert(Transform));
        UIScreenSize = new Vector2(transformed.X, transformed.Y);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (IUserInterface userInterface in UserInterfaces)
        {
            if (userInterface.Visible)
            {
                userInterface.Draw(spriteBatch);
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (IUserInterface userInterface in UserInterfaces)
        {
            userInterface.Update(gameTime);
        }
    }

    public IUserInterface GetUserInterface(string name)
    {
        return UserInterfaces.Find(ui => ui.Name == name);
    }

    public void AddUserInterface(IUserInterface userInterface)
    {
        UserInterfaces.Add(userInterface);
    }

    public void RemoveUserInterface(IUserInterface userInterface)
    {
        UserInterfaces.Remove(userInterface);
    }

    public Matrix GetUITransform()
    {
        return Transform;
    }
}
