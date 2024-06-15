using System;

namespace MonoGame;

public abstract class ToolConfiguration
{
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = false;
}
