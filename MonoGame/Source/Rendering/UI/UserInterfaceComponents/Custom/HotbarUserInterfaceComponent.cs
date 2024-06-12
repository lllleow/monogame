using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Systems.Scripts;
using System.Collections.Generic;
using System.Linq;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents.Custom;

public class HotbarUserInterfaceComponent : ContainerUserInterfaceComponent
{
    private readonly List<TileSlotComponent> tiles;

    public HotbarUserInterfaceComponent(Vector2 localPosition) : base(localPosition, null)
    {
        tiles = TileRegistry.Tiles.Keys
            .Select(tile => new TileSlotComponent("tile_slot", TileRegistry.GetTile(tile), new Vector2(0, 0))).ToList();

        SetChild(new DirectionalListUserInterfaceComponent(
            "list",
            spacing: 2,
            localPosition: new Vector2(0, 0),
            direction: ListDirection.Horizontal,
            children: tiles.Cast<IUserInterfaceComponent>().ToList()
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
            tile.LocalPosition = new Vector2(0, 1);
        }

        component.IsSelected = true;
        component.LocalPosition = new Vector2(0, 0);

        var selectedTileId = component.Tile.Id;
        Globals.World.GetLocalPlayer()?.SetSelectedTile(selectedTileId);
    }
}