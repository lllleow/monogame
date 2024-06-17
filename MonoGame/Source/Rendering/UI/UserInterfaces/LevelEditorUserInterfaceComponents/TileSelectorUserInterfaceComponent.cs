using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame_Common.Systems.Scripts;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class TileSelectorUserInterfaceComponent : ContainerUserInterfaceComponent
{
    private readonly List<TileSlotComponent> tiles = new();
    public SlotUserInterfaceComponentController SlotController { get; set; }
    private GridUserInterfaceComponent grid;

    public TileSelectorUserInterfaceComponent(SlotUserInterfaceComponentController slotController) : base(new Vector2(0, 0), null)
    {
        BackgroundImage = "textures/ui_background";
        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile;
        SlotController = slotController;

        foreach (TileSlotComponent tile in TileRegistry.Tiles.Keys.Select(tile => new TileSlotComponent(slotController, "tile_slot", TileRegistry.GetTile(tile), new Vector2(0, 0))).ToList())
        {
            tiles.Add(tile);
        }

        grid = new GridUserInterfaceComponent(
            8,
            new Vector2(2, 2),
            children: tiles.Cast<IUserInterfaceComponent>().ToList()
        );

        Enabled = false;
        Build();
    }

    public void Build()
    {
        SetChild(new PaddingUserInterfaceComponent(
                4,
                4,
                4,
                4,
                child: new ContainerUserInterfaceComponent(
                    new Vector2(0, 0),
                    new PaddingUserInterfaceComponent(
                            4,
                            4,
                            4,
                            4,
                            new DirectionalListUserInterfaceComponent(
                                "list",
                                spacing: 2,
                                localPosition: new Vector2(0, 0),
                                direction: Axis.Vertical,
                                children: [
                                    new StretchUserInterfaceComponent(Axis.Horizontal, new TextFieldUserInterfaceComponent(
                                        onTextChanged: (text) =>
                                        {
                                            tiles.Clear();
                                            TileRegistry.Tiles.Keys.ToList().ForEach(tile =>
                                            {
                                                if (text.Length == 0 || TileRegistry.Tiles[tile].Name.ToLower().Contains(text.ToLower()))
                                                {
                                                    tiles.Add(new TileSlotComponent(SlotController, "tile_slot", TileRegistry.GetTile(tile), new Vector2(0, 0)));
                                                }
                                            });
                                            grid.RemoveAllChildren();
                                            grid.AddManyChildren(tiles.Cast<IUserInterfaceComponent>().ToList());
                                        }
                                    )),
                                    new ScrollViewUserInterfaceComponent(
                                        new Vector2(-1, 75),
                                        grid
                                    )
                                ]
                            )
                        )
                )

                {
                    BackgroundImage = "textures/ui_background",
                    BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile
                }
        )
        );
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        SlotController.Draw(spriteBatch);
    }
}
