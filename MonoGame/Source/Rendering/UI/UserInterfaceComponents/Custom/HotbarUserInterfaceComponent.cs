using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame_Common.Systems.Scripts;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents.Custom;

public class HotbarUserInterfaceComponent : ContainerUserInterfaceComponent
{
    private readonly List<TileSlotComponent> tiles = new();

    public Action<string> OnTileSelected { get; set; } = (tile) => { };

    private int slotCount = 8;
    public HotbarUserInterfaceComponent(Vector2 localPosition, Action<string> onTileSelected) : base(localPosition, null)
    {
        BackgroundImage = "textures/ui_background";
        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile;
        OnTileSelected = onTileSelected;

        // tiles = TileRegistry.Tiles.Keys
        //     .Select(tile => new TileSlotComponent("tile_slot", TileRegistry.GetTile(tile), new Vector2(0, 0))).ToList();
        for (int i = 0; i < slotCount; i++)
        {
            tiles.Add(new TileSlotComponent("tile_slot", null, new Vector2(0, 0)));
        }

        SetChild(new PaddingUserInterfaceComponent(
                4,
                4,
                4,
                4,
                child: new DirectionalListUserInterfaceComponent(
                    "list",
                    spacing: 2,
                    localPosition: new Vector2(0, 0),
                    direction: ListDirection.Horizontal,
                    children: tiles.Cast<IUserInterfaceComponent>().ToList()
             )
        ));
    }

    public override void Initialize(IUserInterfaceComponent parent)
    {
        base.Initialize(parent);
        tiles.ForEach(tile => tile.OnClick = component => SetSelected(component as TileSlotComponent));
        SetSelected(tiles[0]);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        var currentScrollValue = CurrentMouseState.ScrollWheelValue;
        var previousScrollValue = PreviousMouseState.ScrollWheelValue;

        if (currentScrollValue > previousScrollValue)
        {
            var currentIndex = tiles.FindIndex(tile => tile.IsSelected);
            var nextIndex = (currentIndex + 1) % tiles.Count;
            SetSelected(tiles[nextIndex]);
        }
        else if (currentScrollValue < previousScrollValue)
        {
            var currentIndex = tiles.FindIndex(tile => tile.IsSelected);
            var previousIndex = (currentIndex - 1 + tiles.Count) % tiles.Count;
            SetSelected(tiles[previousIndex]);
        }

        PreviousMouseState = CurrentMouseState;
    }

    public void SetSelected(TileSlotComponent component)
    {
        foreach (var tile in tiles)
        {
            tile.IsSelected = false;
        }

        component.IsSelected = true;

        var selectedTile = component.Tile;
        if (selectedTile != null)
        {
            OnTileSelected?.Invoke(selectedTile.Id);
        }
    }
}