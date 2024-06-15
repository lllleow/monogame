using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        Tools.Add(new PlaceLevelEditorTool());
        Tools.Add(new EraserLevelEditorTool());
        // Tools.Add(new SelectLevelEditorTool());

        toolComponents = Tools.Select(tool =>
            (UserInterfaceComponent)new ButtonUserInterfaceComponent(tool.Name, component => SetSelectedTool(component, tool))
            {
                SizeOverride = new Vector2(40, 15)
            }
        ).Cast<IUserInterfaceComponent>().ToList();

        foreach (var tool in Tools)
        {
            tool.Initialize();
        }

        Build();
    }

    public void Build()
    {
        List<IUserInterfaceComponent> toolConfigurations = [];
        if (SelectedTool != null)
        {
            toolConfigurations = SelectedTool?.ToolConfigurations.Select(configuration =>
            (UserInterfaceComponent)new ButtonUserInterfaceComponent(configuration.Name, component => SetSelectedToolConfiguration(component, configuration))
            {
                IsClicked = configuration.Enabled,
                SizeOverride = new Vector2(50, 15)
            }).Cast<IUserInterfaceComponent>().ToList();
        }

        SetChild(new DirectionalListUserInterfaceComponent(
                "list",
                spacing: 2,
                localPosition: new Vector2(0, 0),
                direction: ListDirection.Horizontal,
                children: [
                    new ContainerUserInterfaceComponent(
                        new Vector2(0, 0),
                        new PaddingUserInterfaceComponent(
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
                        )
                    )
                    {
                        BackgroundImage = "textures/ui_background",
                        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile
                    },
                    new ContainerUserInterfaceComponent(
                        new Vector2(0, 0),
                        new PaddingUserInterfaceComponent(
                            4,
                            4,
                            4,
                            4,
                            child: new DirectionalListUserInterfaceComponent(
                                "list",
                                spacing: 2,
                                localPosition: new Vector2(0, 0),
                                direction: ListDirection.Vertical,
                                children: toolConfigurations
                            )
                        )
                    )
                    {
                        Enabled = SelectedTool != null,
                        BackgroundImage = "textures/ui_background",
                        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile
                    },
                ]
            ));
    }

    public void SetSelectedToolConfiguration(IUserInterfaceComponent component, ToolConfiguration configuration)
    {
        configuration.Enabled = !configuration.Enabled;
        ((ButtonUserInterfaceComponent)component).IsClicked = configuration.Enabled;
    }

    public void SetSelectedTool(IUserInterfaceComponent component, LevelEditorTool tool)
    {
        toolComponents.ForEach(component => ((ButtonUserInterfaceComponent)component).IsClicked = false);
        if (tool != SelectedTool)
        {
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
        else
        {
            tool.Enabled = false;
            SelectedTool = null;
        }

        Build();
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
