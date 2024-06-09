﻿using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using MonoGame.Source.States.Components;
using MonoGame.Source.Systems.Animation;
using MonoGame.Source.Systems.Components.SpriteRenderer;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components.Animator;

public class AnimatorComponent : EntityComponent
{
    public IAnimationBundle AnimationBundle { get; set; }
    private AnimatorComponentNetworkController networkController = new();
    public AnimationStateMachine StateMachine { get; set; }

    public AnimatorComponent(IGameEntity entity, IAnimationBundle animation)
    {
        Entity = entity;
        AnimationBundle = animation;
        networkController.InitializeListeners(this);
        StateMachine = new AnimationStateMachine(AnimationBundle)
        {
            OnSpriteChanged = (CurrentTextureX, CurrentTextureY) =>
        {
            Entity.GetFirstComponent<SpriteRendererComponent>()?.UpdateTexture(AnimationBundle.SpriteSheet, new Rectangle(CurrentTextureX * AnimationBundle.SizeX, CurrentTextureY * AnimationBundle.SizeY, AnimationBundle.SizeX, AnimationBundle.SizeY));
        }
        };
    }

    public string GetCurrentStateId()
    {
        return StateMachine.CurrentState.Animation.Id;
    }

    public void SetState(string animationId)
    {
        networkController.SendStateUpdate(this);
        StateMachine.SetState(animationId);
    }

    public override void Initialize()
    {
        base.Initialize();
        if (!Entity.ContainsComponent<SpriteRendererComponent>())
        {
            throw new Exception("AnimatorComponent requires a SpriteRendererComponent to be present on the entity.");
        }
    }

    public override void Update(GameTime gameTime)
    {
        StateMachine.Update(gameTime);
    }

    public override Type GetComponentStateType()
    {
        return typeof(AnimatorComponentState);
    }
}
