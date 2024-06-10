using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents.Custom;

namespace MonoGame.Source.Rendering.UI.UserInterfaces;

public class LevelEditorUserInterface : UserInterface
{
    public LevelEditorUserInterface()
    {
        Name = "level_editor_user_interface";
        AddComponent(
            new AlignmentUserInterfaceComponent(
                alignment: UserInterfaceAlignment.CenterDown,
                child: new PaddingUserInterfaceComponent(
                    0,
                    0,
                    0,
                    8,
                    new HotbarUserInterfaceComponent(
                        new Vector2(0, 0)
                    )
                )
            )
        );
    }
}