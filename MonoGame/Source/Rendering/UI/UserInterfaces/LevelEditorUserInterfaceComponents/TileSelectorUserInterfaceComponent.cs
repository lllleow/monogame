﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame_Common.Systems.Scripts;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using MonoGame.Source;
using MonoGame.Source.GameModes;

namespace MonoGame;

public class TileSelectorUserInterfaceComponent : ContainerUserInterfaceComponent
{
    private readonly List<TileSlotComponent> tiles;
    public TileSelectorUserInterfaceComponent() : base(new Vector2(0, 0), null)
    {
        BackgroundImage = "textures/ui_background";
        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile;
        tiles = TileRegistry.Tiles.Keys.Select(tile => new TileSlotComponent("tile_slot", TileRegistry.GetTile(tile), new Vector2(0, 0))).ToList();

        SetChild(new PaddingUserInterfaceComponent(
                4,
                4,
                4,
                4,
                child: new GridUserInterfaceComponent(
                            "grid",
                            8,
                            8,
                            new Vector2(2, 2),
                            localPosition: new Vector2(0, 0),
                            children: tiles.Cast<IUserInterfaceComponent>().ToList()
                        )
        ));
    }
}
