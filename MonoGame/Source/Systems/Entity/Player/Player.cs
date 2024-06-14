using Microsoft.Xna.Framework;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Systems.Scripts;
using MonoGame.Source.GameModes;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Movement;
using MonoGame.Source.Systems.Components.SpriteRenderer;

namespace MonoGame.Source.Systems.Entity.Player;

public class Player : GameEntity
{
    private readonly AnimatorComponent animator;
    private readonly SpriteRendererComponent spriteRenderer;
    public GameMode GameMode { get; set; } = GameMode.Survival;

    public Player(string uuid, Vector2 position)
    {
        Position = position;
        UUID = uuid;

        animator = new AnimatorComponent(this, AnimationBundleRegistry.GetAnimationBundle("base.player"));
        spriteRenderer = new SpriteRendererComponent();

        NetworkController.InitializeListeners(this);

        ClientNetworkEventManager.Subscribe<SetGameModeNetworkMessage>(message =>
        {
            if (UUID == message.UUID)
            {
                GameMode = message.GameMode;
            }
        });

        AddComponent(spriteRenderer);
        AddComponent(animator);
        AddComponent(new CollisionComponent(CollisionMode.CollisionMask));
        AddComponent(new MovementComponent());

        SetGameMode(GameMode.LevelEditor);
    }
    public PlayerNetworkController NetworkController { get; set; } = new();

    public void SetGameMode(GameMode gameMode)
    {
        NetworkClient.SendMessage(new ChangeGameModeNetworkMessage()
        {
            UUID = UUID,
            DesiredGameMode = gameMode
        });
    }

    public bool IsLocalPlayer()
    {
        return this == Globals.World.GetLocalPlayer();
    }
}