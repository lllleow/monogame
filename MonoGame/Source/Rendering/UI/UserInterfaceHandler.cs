using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class UserInterfaceHandler
{
    public List<IUserInterface> UserInterfaces { get; set; } = new List<IUserInterface>();
    public float ScaleFactor { get; set; } = 6f;
    public Matrix Transform { get; set; } = Matrix.CreateScale(6f, 6f, 1f);

    public void Initialize()
    {
        UserInterfaces.Add(new LevelEditorUserInterface());
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
