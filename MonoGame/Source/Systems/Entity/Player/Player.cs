using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Movement;
using MonoGame.Source.Systems.Components.SpriteRenderer;
using MonoGame.Source.Systems.Scripts;
using MonoGame_Common.Enums;

namespace MonoGame.Source.Systems.Entity.Player;

public class Player : GameEntity
{
    private readonly AnimatorComponent animator;
    private readonly SpriteRendererComponent spriteRenderer;

    public Player(string uuid, Vector2 position)
    {
        Position = position;
        UUID = uuid;

        animator = new AnimatorComponent(this, AnimationBundleRegistry.GetAnimationBundle("base.player"));
        spriteRenderer = new SpriteRendererComponent();

        NetworkController.InitializeListeners(this);

        AddComponent(spriteRenderer);
        AddComponent(animator);
        AddComponent(new CollisionComponent(CollisionMode.CollisionMask));
        AddComponent(new MovementComponent());
    }

    public string SelectedTile { get; set; } = "base.grass";
    public PlayerNetworkController NetworkController { get; set; } = new();

    public void SetSelectedTile(string selectedTileId)
    {
        SelectedTile = selectedTileId;
    }

    public bool IsLocalPlayer()
    {
        return this == Globals.World.GetLocalPlayer();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}