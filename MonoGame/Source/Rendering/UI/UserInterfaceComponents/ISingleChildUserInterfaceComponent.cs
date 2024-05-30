using System;

namespace MonoGame;

public interface ISingleChildUserInterfaceComponent
{
    public IUserInterfaceComponent Child { get; set; }
    public void SetChild(IUserInterfaceComponent child);
    public void RemoveChild();
}
