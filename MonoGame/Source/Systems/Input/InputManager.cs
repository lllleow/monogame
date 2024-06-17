using System;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;

namespace MonoGame;

public class InputManager
{
    private KeyboardState currentKeyboardState;
    private KeyboardState previousKeyboardState;
    private MouseState currentMouseState;
    private MouseState previousMouseState;

    public void Update()
    {
        previousKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();

        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();

        foreach (Keys key in Enum.GetValues(typeof(Keys)))
        {
            if (currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key))
            {
                InputEventManager.RaiseEvent(new InputEvent { EventType = InputEventType.KeyDown, Key = key });
            }

            if (!currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key))
            {
                InputEventManager.RaiseEvent(new InputEvent { EventType = InputEventType.KeyUp, Key = key });
            }
        }

        int scrollDelta = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
        if (scrollDelta != 0)
        {
            InputEventManager.RaiseEvent(new InputEvent { EventType = InputEventType.MouseScrolled, ScrollDelta = scrollDelta });
        }

        CheckMouseButton(MouseButton.Left, currentMouseState.LeftButton, previousMouseState.LeftButton);
        CheckMouseButton(MouseButton.Right, currentMouseState.RightButton, previousMouseState.RightButton);
        CheckMouseButton(MouseButton.Middle, currentMouseState.MiddleButton, previousMouseState.MiddleButton);
    }

    private void CheckMouseButton(MouseButton button, ButtonState currentState, ButtonState previousState)
    {
        if (currentState == ButtonState.Pressed && previousState == ButtonState.Released)
        {
            InputEventManager.RaiseEvent(new InputEvent { EventType = InputEventType.MouseButtonDown, Button = button });
        }

        if (currentState == ButtonState.Released && previousState == ButtonState.Pressed)
        {
            InputEventManager.RaiseEvent(new InputEvent { EventType = InputEventType.MouseButtonUp, Button = button });
        }
    }
}