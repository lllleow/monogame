
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using MonoGame.Source.Systems.Scripts;

namespace MonoGame;

public class HotbarUserInterfaceComponent : ContainerUserInterfaceComponent
{
    List<TileSlotComponent> Tiles;
    public HotbarUserInterfaceComponent(Vector2 localPosition) : base(localPosition, null)
    {
        Tiles = TileRegistry.Tiles.Keys.Select(tile => new TileSlotComponent("tile_slot", tile: TileRegistry.GetTile(tile), localPosition: new Vector2(0, 0))).ToList();

        SetChild(new DirectionalListUserInterfaceComponent(
            name: "list",
            spacing: 2,
            localPosition: new Vector2(0, 0),
            direction: ListDirection.Horizontal,
            children: Tiles.Cast<IUserInterfaceComponent>().ToList()
        ));
    }

    public override void Initialize(IUserInterfaceComponent parent)
    {
        base.Initialize(parent);
        Tiles.ForEach(tile => tile.OnClick = (component) => SetSelected(component as TileSlotComponent));
        SetSelected(Tiles[0]);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        int currentScrollValue = currentMouseState.ScrollWheelValue;
        int previousScrollValue = previousMouseState.ScrollWheelValue;

        if (currentScrollValue > previousScrollValue)
        {
            int currentIndex = Tiles.FindIndex(tile => tile.IsSelected);
            int nextIndex = (currentIndex + 1) % Tiles.Count;
            SetSelected(Tiles[nextIndex]);
        }
        else if (currentScrollValue < previousScrollValue)
        {
            int currentIndex = Tiles.FindIndex(tile => tile.IsSelected);
            int previousIndex = (currentIndex - 1 + Tiles.Count) % Tiles.Count;
            SetSelected(Tiles[previousIndex]);
        }

        previousMouseState = currentMouseState;
    }

    public void SetSelected(TileSlotComponent component)
    {
        foreach (TileSlotComponent tile in Tiles)
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
