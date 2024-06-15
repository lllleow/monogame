using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class LevelEditorToolBarUserInterfaceComponent : ContainerUserInterfaceComponent
{
    public List<LevelEditorTool> Tools { get; set; } = new();
    public LevelEditorTool SelectedTool { get; set; } = null;
    public (int PosX, int PosY) CursorPosition { get; set; } = (0, 0);
    public string SelectedTile { get; set; } = "base.grass";
    private List<IUserInterfaceComponent> toolComponents;

    public LevelEditorToolBarUserInterfaceComponent() : base(new Vector2(0, 0), null)
    {
        BackgroundImage = "textures/ui_background";
        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile;

        Tools.Add(new PlaceLevelEditorTool());
        Tools.Add(new EraserLevelEditorTool());
        Tools.Add(new SelectLevelEditorTool());

        toolComponents = Tools.Select(tool =>
            (UserInterfaceComponent)new ButtonUserInterfaceComponent(tool.Name, component => SetSelectedTool(component, tool))
            {
                SizeOverride = new Vector2(50, 15)
            }
        ).Cast<IUserInterfaceComponent>().ToList();

        foreach (var tool in Tools)
        {
            tool.Initialize();
        }

        SetSelectedTool(toolComponents[0], Tools[0]);

        SetChild(new PaddingUserInterfaceComponent(
            4,
            4,
            4,
            4,
            child: new DirectionalListUserInterfaceComponent(
                "list",
                spacing: 2,
                localPosition: new Vector2(0, 0),
                direction: ListDirection.Vertical,
                children: toolComponents
            )
        ));
    }

    public void SetSelectedTool(IUserInterfaceComponent component, LevelEditorTool tool)
    {
        toolComponents.ForEach(component => ((ButtonUserInterfaceComponent)component).IsClicked = false);
        ((ButtonUserInterfaceComponent)component).IsClicked = true;

        foreach (var _tool in Tools)
        {
            if (_tool == tool)
            {
                _tool.Enabled = true;
            }
            else
            {
                _tool.Enabled = false;
            }
        }

        SelectedTool = tool;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        foreach (var tool in Tools)
        {
            tool.Update();
            if (tool is TileManipulatorEditorTool tileManipulatorEditorTool)
            {
                tileManipulatorEditorTool.CursorPosition = CursorPosition;
                tileManipulatorEditorTool.SelectedTile = SelectedTile;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        foreach (var tool in Tools)
        {
            tool.Draw(spriteBatch);
        }
    }
}
