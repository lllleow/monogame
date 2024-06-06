using Microsoft.Xna.Framework;

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
                    paddingLeft: 0,
                    paddingRight: 0,
                    paddingTop: 0,
                    paddingBottom: 8,
                    child: new HotbarUserInterfaceComponent(
                        localPosition: new Vector2(0, 0)
                    )
                )
            )
        );
    }
}
