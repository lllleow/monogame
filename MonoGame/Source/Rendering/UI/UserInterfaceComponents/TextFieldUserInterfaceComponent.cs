﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;
using System;

namespace MonoGame;

public class TextFieldUserInterfaceComponent : ContainerUserInterfaceComponent
{
    public string Text { get; set; } = string.Empty;
    public bool IsFocused { get; set; } = false;
    public LabelUserInterfaceComponent Label { get; set; }
    private KeyboardLayout keyboardLayout = new();
    private bool IsDeleting { get; set; } = false;
    public Action<string> OnTextChanged { get; set; } = (text) => { };

    public TextFieldUserInterfaceComponent(Action<string> onTextChanged) : base(new Vector2(0, 0), null)
    {
        BackgroundImage = "textures/ui_background";
        BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile;
        OnTextChanged = onTextChanged;

        Label = new LabelUserInterfaceComponent(
            " ",
            new Vector2(0, 0)
        );

        SetChild(new PaddingUserInterfaceComponent(
                4,
                4,
                4,
                4,
                child: Label
        ));
    }

    public override void Initialize(IUserInterfaceComponent parent)
    {
        base.Initialize(parent);
        InputEventManager.Subscribe(InputEventChannel.UI, priority: 1, handler: inputEvent =>
        {
            if (inputEvent.EventType == InputEventType.MouseButtonUp)
            {
                if (MouseIntersectsComponent())
                {
                    OnFocus();
                }
                else
                {
                    OnLoseFocus();
                }
            }

            if (inputEvent.EventType == InputEventType.KeyUp)
            {
                if (IsFocused && inputEvent.Key != null)
                {
                    inputEvent.Handled = true;
                    if (inputEvent.Key == Keys.Back)
                    {
                        IsDeleting = false;
                    }
                    else
                    {
                        char? key = keyboardLayout.ToChar(inputEvent.Key);
                        if (key != null)
                        {
                            Text += key;
                        }

                    }

                    UpdateText();
                }
            }

            if (inputEvent.EventType == InputEventType.KeyDown)
            {
                if (IsFocused && inputEvent.Key != null)
                {
                    inputEvent.Handled = true;
                    if (inputEvent.Key == Keys.Back)
                    {
                        IsDeleting = true;
                    }
                }
            }
        });
    }

    public void OnFocus()
    {
        IsFocused = true;
        UpdateText();
    }

    public void OnLoseFocus()
    {
        IsFocused = false;
        UpdateText();
    }

    public void UpdateText()
    {
        if (Text.Length > 0 && IsFocused)
        {
            Label.Text = Text + "_";
        }
        else if (Text.Length > 0)
        {
            Label.Text = Text;
        }
        else if (Text.Length == 0)
        {
            Label.Text = "_";
        }
        else
        {
            Label.Text = " ";
        }

        OnTextChanged?.Invoke(Text);
    }

    private int deleteCounter = 0;
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (IsDeleting && deleteCounter > 2)
        {
            deleteCounter = 0;
            if (Text.Length > 0)
            {
                Text = Text.Substring(0, Text.Length - 1);
                UpdateText();
            }
        }
        else
        {
            deleteCounter++;
        }
    }
}
