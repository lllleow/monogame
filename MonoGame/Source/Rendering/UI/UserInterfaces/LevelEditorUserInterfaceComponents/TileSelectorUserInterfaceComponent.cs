using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame_Common.Systems.Scripts;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using System.ComponentModel;
using MonoGame_Common.Systems.Tiles.Interfaces;

namespace MonoGame;

public class TileSelectorUserInterfaceComponent : ContainerUserInterfaceComponent
{
    private readonly List<TileSlotComponent> tiles = new();

    public TileSelectorUserInterfaceComponent(SlotUserInterfaceComponentController slotController) : base(new Vector2(0, 0), null)
    {
        BackgroundImage = "textures/ui_background";
        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile;

        for (int x = 0; x < 20; x++)
        {
            foreach (TileSlotComponent tile in TileRegistry.Tiles.Keys.Select(tile => new TileSlotComponent(slotController, "tile_slot", TileRegistry.GetTile(tile), new Vector2(0, 0))).ToList())
            {
                tiles.Add(tile);
            }
        }

        Enabled = false;

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
                            child: new ScrollViewUserInterfaceComponent(
                        new Vector2(-1, 75),
                        new GridUserInterfaceComponent(
                                8,
                                new Vector2(2, 2),
                                children: tiles.Cast<IUserInterfaceComponent>().ToList()
                            )
                        )
                    )
                )
                {
                    BackgroundImage = "textures/ui_background",
                    BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile
                }
        ));
    }
}
