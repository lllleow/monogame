using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public abstract class LevelEditorTool
{
    public string Name { get; set; } = "Tool";
    public bool Enabled { get; set; } = false;
    public abstract void Initialize();
    public abstract void Update();
    public abstract void Draw(SpriteBatch spriteBatch);
    public List<ToolConfiguration> ToolConfigurations { get; set; } = [];

    public T GetToolConfiguration<T>()
    where T : ToolConfiguration
    {
        return ToolConfigurations.OfType<T>().FirstOrDefault();
    }

    public bool HasToolConfiguration<T>()
    where T : ToolConfiguration
    {
        return ToolConfigurations.OfType<T>().Any();
    }

    public void RemoveToolConfiguration<T>()
    where T : ToolConfiguration
    {
        _ = ToolConfigurations.RemoveAll(x => x.GetType() == typeof(T));
    }

    public void AddToolConfiguration<T>(T toolConfiguration)
    where T : ToolConfiguration
    {
        toolConfiguration.Enabled = true;
        ToolConfigurations.Add(toolConfiguration);
    }
}
