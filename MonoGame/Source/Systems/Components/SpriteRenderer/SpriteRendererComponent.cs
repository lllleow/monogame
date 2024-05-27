using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components;
using MonoGame.Source.Util.Loaders;

namespace MonoGame;

/// <summary>
/// Represents a component that renders a sprite on an entity.
/// </summary>
public class SpriteRendererComponent : EntityComponent
{
        /// <summary>
        /// The name to the sprite sheet.
        /// </summary>
        public string SpriteSheet;

        /// <summary>
        /// The rectangle that defines the portion of the sprite sheet to render.
        /// </summary>
        public Rectangle TextureRectangle = Rectangle.Empty;

        /// <summary>
        /// The scale of the rendered sprite.
        /// </summary>
        public Vector2 Scale = new Vector2(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteRendererComponent"/> class.
        /// </summary>
        public SpriteRendererComponent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteRendererComponent"/> class with the specified sprite sheet, texture coordinates, and texture size.
        /// </summary>
        /// <param name="spritesheet">The path to the sprite sheet.</param>
        /// <param name="textureCoordinates">The coordinates of the top-left corner of the texture on the sprite sheet.</param>
        /// <param name="textureSize">The size of the texture on the sprite sheet.</param>
        public SpriteRendererComponent(string spritesheet, Vector2 textureCoordinates, Vector2 textureSize)
        {
            SpriteSheet = spritesheet;
            TextureRectangle = new Rectangle((int)textureCoordinates.X, (int)textureCoordinates.Y, (int)textureSize.X, (int)textureSize.Y);
        }

        /// <summary>
        /// Updates the texture of the sprite renderer component.
        /// </summary>
        /// <param name="spritesheet">The path to the new sprite sheet.</param>
        /// <param name="textureRectangle">The new texture rectangle.</param>
        public void UpdateTexture(string spritesheet, Rectangle textureRectangle)
        {
            SpriteSheet = spritesheet;
            TextureRectangle = textureRectangle;
        }

        /// <summary>
        /// Draws the sprite using the specified sprite batch.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used for drawing.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SpriteSheet), Entity.Position, TextureRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }
