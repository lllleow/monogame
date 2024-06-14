using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class LevelEditorToolBarUserInterfaceComponent : ContainerUserInterfaceComponent
{
    public List<LevelEditorTool> Tools { get; set; } = new();
    public LevelEditorTool SelectedTool { get; set; } = null;
    public (int PosX, int PosY) CursorPosition { get; set; } = (0, 0);
    public string SelectedTile { get; set; } = "base.grass";

    public LevelEditorToolBarUserInterfaceComponent() : base(new Vector2(0, 0), null)
    {
        Tools.Add(new PlaceLevelEditorTool());

        List<UserInterfaceComponent> toolComponents = Tools.Select(tool =>
            (UserInterfaceComponent)new LabelUserInterfaceComponent(tool.Name, new Vector2(0, 0))
        ).ToList();

        for (int i = 0; i < toolComponents.Count; i++)
        {
            LevelEditorTool tool = Tools[i];
            if (tool == null) continue;
            toolComponents[i].OnClick = component => SetSelectedTool(tool);
        }

        foreach (var tool in Tools)
        {
            tool.Initialize();
        }

        SetSelectedTool(Tools[0]);

        SetChild(new DirectionalListUserInterfaceComponent(
            "list",
            spacing: 2,
            localPosition: new Vector2(0, 0),
            direction: ListDirection.Vertical,
            children: toolComponents.Cast<IUserInterfaceComponent>().ToList()
        ));
    }

    public void SetSelectedTool(LevelEditorTool tool)
    {
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
}
