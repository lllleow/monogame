using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class ContainerUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public string BackgroundImage { get; set; }
    public UserInterfaceBackgroundImageMode BackgroundImageMode { get; set; } = UserInterfaceBackgroundImageMode.Cover;

    public ContainerUserInterfaceComponent(Vector2 localPosition, IUserInterfaceComponent child) : base("container", localPosition, child)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Enabled) return;
        if (BackgroundImage != null)
        {
            var backgroundImage = Globals.ContentManager.Load<Texture2D>(BackgroundImage);
            if (BackgroundImageMode == UserInterfaceBackgroundImageMode.Cover)
            {
                var position = GetPositionRelativeToParent();
                var size = GetPreferredSize();
                spriteBatch.Draw(backgroundImage, position, null, Color.White * Opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }
            else if (BackgroundImageMode == UserInterfaceBackgroundImageMode.Tile)
            {
                var position = GetPositionRelativeToParent();
                var size = GetPreferredSize();

                int borderSize = 16;
                int minSize = borderSize * 2;

                if (size.X < minSize || size.Y < minSize)
                {
                    borderSize = (int)(Math.Min(size.X, size.Y) / 2);
                }

                int centerWidth = (int)Math.Max(0, size.X - (2 * borderSize));
                int centerHeight = (int)Math.Max(0, size.Y - (2 * borderSize));

                spriteBatch.Draw(
                    backgroundImage,
                    new Rectangle((int)position.X, (int)position.Y, borderSize, borderSize),
                    new Rectangle(0, 0, borderSize, borderSize),
                    Color.White
                );

                spriteBatch.Draw(
                    backgroundImage,
                    new Rectangle((int)(position.X + size.X - borderSize), (int)position.Y, borderSize, borderSize),
                    new Rectangle(backgroundImage.Width - borderSize, 0, borderSize, borderSize),
                    Color.White
                );

                spriteBatch.Draw(
                    backgroundImage,
                    new Rectangle((int)position.X, (int)(position.Y + size.Y - borderSize), borderSize, borderSize),
                    new Rectangle(0, backgroundImage.Height - borderSize, borderSize, borderSize),
                    Color.White
                );

                spriteBatch.Draw(
                    backgroundImage,
                    new Rectangle((int)(position.X + size.X - borderSize), (int)(position.Y + size.Y - borderSize), borderSize, borderSize),
                    new Rectangle(backgroundImage.Width - borderSize, backgroundImage.Height - borderSize, borderSize, borderSize),
                    Color.White * Opacity
                );

                if (centerWidth > 0)
                {
                    spriteBatch.Draw(
                        backgroundImage,
                        new Rectangle((int)(position.X + borderSize), (int)position.Y, centerWidth, borderSize),
                        new Rectangle(borderSize, 0, backgroundImage.Width - (2 * borderSize), borderSize),
                        Color.White
                    );

                    spriteBatch.Draw(
                        backgroundImage,
                        new Rectangle((int)(position.X + borderSize), (int)(position.Y + size.Y - borderSize), centerWidth, borderSize),
                        new Rectangle(borderSize, backgroundImage.Height - borderSize, backgroundImage.Width - (2 * borderSize), borderSize),
                        Color.White * Opacity
                    );
                }

                if (centerHeight > 0)
                {
                    spriteBatch.Draw(
                        backgroundImage,
                        new Rectangle((int)position.X, (int)(position.Y + borderSize), borderSize, centerHeight),
                        new Rectangle(0, borderSize, borderSize, backgroundImage.Height - (2 * borderSize)),
                        Color.White
                    );

                    spriteBatch.Draw(
                        backgroundImage,
                        new Rectangle((int)(position.X + size.X - borderSize), (int)(position.Y + borderSize), borderSize, centerHeight),
                        new Rectangle(backgroundImage.Width - borderSize, borderSize, borderSize, backgroundImage.Height - (2 * borderSize)),
                        Color.White * Opacity
                    );
                }

                if (centerWidth > 0 && centerHeight > 0)
                {
                    spriteBatch.Draw(
                        backgroundImage,
                        new Rectangle((int)(position.X + borderSize), (int)(position.Y + borderSize), centerWidth, centerHeight),
                        new Rectangle(borderSize, borderSize, backgroundImage.Width - (2 * borderSize), backgroundImage.Height - (2 * borderSize)),
                        Color.White * Opacity
                    );
                }
            }
        }

        base.Draw(spriteBatch);
    }
}