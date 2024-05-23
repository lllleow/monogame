using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace MonoGame;

public class Globals
{
    public static ContentManager contentManager { get; set; }
    public static SpriteBatch spriteBatch { get; set; }
    public static World world { get; set; }
    public static Microsoft.Xna.Framework.GraphicsDeviceManager graphicsDevice { get; set; }
}
