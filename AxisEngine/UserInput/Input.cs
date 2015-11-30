using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine.UserInput
{
    public class InputManager : WorldObject
    {
        private Dictionary<string, Tuple<Keys, Keys>> AxisBindings;

        private Dictionary<string, Keys> Bindings_Keyboard;

        private Dictionary<string, MouseButtons> Bindings_Mouse;

        private KeyboardState LastState_Keyboard;

        private MouseState LastState_Mouse;

        private KeyboardState State_Keyboard;

        private MouseState State_Mouse;

        public InputManager()
        {
            // get first states
            State_Keyboard = LastState_Keyboard = Keyboard.GetState();
            State_Mouse = LastState_Mouse = Mouse.GetState();

            // set the first bindings
            Bindings_Keyboard = new Dictionary<string, Keys>();
            Bindings_Mouse = new Dictionary<string, MouseButtons>();
            AxisBindings = new Dictionary<string, Tuple<Keys, Keys>>();

            // set IUpdatable
            Enabled = true;
            UpdateOrder = 0;
        }

        public int MouseDeltaScroll { get; private set; }

        public int MouseDeltaX { get; private set; }

        public int MouseDeltaY { get; private set; }

        public Point MousePosition { get; private set; }

        public int MouseScroll { get; private set; }

        public int MouseX { get; private set; }

        public int MouseY { get; private set; }

        public void AddAxis(string name, Keys positiveKey, Keys negativeKey)
        {
            if (!Bindings_Keyboard.Values.Contains(positiveKey) && !Bindings_Keyboard.Values.Contains(negativeKey))
            {
                if (!AxisBindings.Keys.Contains(name))
                {
                    AxisBindings[name] = new Tuple<Keys, Keys>(positiveKey, negativeKey);
                }
                else
                {
                    throw new Exception("An axis with this name already exists");
                }
            }
            else
            {
                throw new Exception("One of the keys in this axis has been used in a binding.");
            }
        }

        public void AddBinding(string name, Keys key)
        {
            if (!Bindings_Keyboard.Values.Contains(key) && !AxisBindings.Values.Any(tuple => (tuple.Item1 == key || tuple.Item2 == key)))
            {
                if (!Bindings_Keyboard.Keys.Contains(name) && !Bindings_Mouse.Keys.Contains(name))
                {
                    Bindings_Keyboard[name] = key;
                }
                else
                {
                    throw new Exception("The binding " + name + " already exists.");
                }
            }
            else
            {
                throw new Exception("The key: " + key.ToString() + " already exists as a binding.");
            }
        }

        public void AddBinding(string name, MouseButtons mouseButton)
        {
            if (!Bindings_Mouse.Values.Contains(mouseButton))
            {
                if (!Bindings_Mouse.Keys.Contains(name) && !Bindings_Keyboard.Keys.Contains(name))
                {
                    Bindings_Mouse[name] = mouseButton;
                }
                else
                {
                    throw new Exception("The binding " + name + " already exists.");
                }
            }
            else
            {
                throw new Exception("The button: " + mouseButton.ToString() + " already exists as a binding.");
            }
        }

        public void ClearBindings()
        {
            Bindings_Keyboard.Clear();
            Bindings_Mouse.Clear();
        }

        public int GetAxis(string name)
        {
            if (Enabled)
            {
                if (AxisBindings.Keys.Contains(name))
                {
                    int axis = 0;
                    if (State_Keyboard.IsKeyDown(AxisBindings[name].Item1))
                        axis += 1;
                    if (State_Keyboard.IsKeyDown(AxisBindings[name].Item2))
                        axis -= 1;
                    return axis;
                }
                else
                {
                    throw new ArgumentException("Could not find the binding.", name);
                }
            }
            else return 0;
        }

        public bool GetBinding(string name)
        {
            if (Enabled)
            {
                if (Bindings_Keyboard.Keys.Contains(name))
                {
                    return State_Keyboard.IsKeyDown(Bindings_Keyboard[name]);
                }
                else if (Bindings_Mouse.Keys.Contains(name))
                {
                    switch (Bindings_Mouse[name])
                    {
                        case MouseButtons.LeftButton:
                            return State_Mouse.LeftButton == ButtonState.Pressed;

                        case MouseButtons.RightButton:
                            return State_Mouse.RightButton == ButtonState.Pressed;

                        case MouseButtons.MiddleButton:
                            return State_Mouse.MiddleButton == ButtonState.Pressed;

                        case MouseButtons.XButton1:
                            return State_Mouse.XButton1 == ButtonState.Pressed;

                        case MouseButtons.XButton2:
                            return State_Mouse.XButton2 == ButtonState.Pressed;

                        default:
                            throw new Exception("Could not find the mouse button.");
                    }
                }
                else
                {
                    throw new ArgumentException("Could not find the binding.", name);
                }
            }
            else return false;
        }

        public bool GetBindingDown(string name)
        {
            if (Enabled)
            {
                if (Bindings_Keyboard.Keys.Contains(name))
                {
                    return State_Keyboard.IsKeyDown(Bindings_Keyboard[name]) && LastState_Keyboard.IsKeyUp(Bindings_Keyboard[name]);
                }
                else if (Bindings_Mouse.Keys.Contains(name))
                {
                    switch (Bindings_Mouse[name])
                    {
                        case MouseButtons.LeftButton:
                            return State_Mouse.LeftButton == ButtonState.Pressed && LastState_Mouse.LeftButton == ButtonState.Released;

                        case MouseButtons.RightButton:
                            return State_Mouse.RightButton == ButtonState.Pressed && LastState_Mouse.RightButton == ButtonState.Released;

                        case MouseButtons.MiddleButton:
                            return State_Mouse.MiddleButton == ButtonState.Pressed && LastState_Mouse.MiddleButton == ButtonState.Released;

                        case MouseButtons.XButton1:
                            return State_Mouse.XButton1 == ButtonState.Pressed && LastState_Mouse.XButton1 == ButtonState.Released;

                        case MouseButtons.XButton2:
                            return State_Mouse.XButton2 == ButtonState.Pressed && LastState_Mouse.XButton2 == ButtonState.Released;

                        default:
                            throw new Exception("Could not find the mouse button.");
                    }
                }
                else
                {
                    throw new ArgumentException("Could not find the binding.", name);
                }
            }
            else return false;
        }

        public bool GetBindingUp(string name)
        {
            if (Enabled)
            {
                if (Bindings_Keyboard.Keys.Contains(name))
                {
                    return State_Keyboard.IsKeyUp(Bindings_Keyboard[name]) && LastState_Keyboard.IsKeyDown(Bindings_Keyboard[name]);
                }
                else if (Bindings_Mouse.Keys.Contains(name))
                {
                    switch (Bindings_Mouse[name])
                    {
                        case MouseButtons.LeftButton:
                            return State_Mouse.LeftButton == ButtonState.Released && LastState_Mouse.LeftButton == ButtonState.Pressed;

                        case MouseButtons.RightButton:
                            return State_Mouse.RightButton == ButtonState.Released && LastState_Mouse.RightButton == ButtonState.Pressed;

                        case MouseButtons.MiddleButton:
                            return State_Mouse.MiddleButton == ButtonState.Released && LastState_Mouse.MiddleButton == ButtonState.Pressed;

                        case MouseButtons.XButton1:
                            return State_Mouse.XButton1 == ButtonState.Released && LastState_Mouse.XButton1 == ButtonState.Pressed;

                        case MouseButtons.XButton2:
                            return State_Mouse.XButton2 == ButtonState.Released && LastState_Mouse.XButton2 == ButtonState.Pressed;

                        default:
                            throw new Exception("Could not find the mouse button.");
                    }
                }
                else
                {
                    throw new ArgumentException("Could not find the binding.", name);
                }
            }
            else return false;
        }

        public void RemoveBinding(string name)
        {
            if (Bindings_Keyboard.Keys.Contains(name))
            {
                Bindings_Keyboard.Remove(name);
            }
            else if (Bindings_Mouse.Keys.Contains(name))
            {
                Bindings_Mouse.Remove(name);
            }
        }

        protected override void UpdateThis(GameTime t)
        {
            // update the states
            LastState_Keyboard = State_Keyboard;
            State_Keyboard = Keyboard.GetState();
            LastState_Mouse = State_Mouse;
            State_Mouse = Mouse.GetState();

            // set the mouse values
            MouseX = State_Mouse.X;
            MouseY = State_Mouse.Y;
            MousePosition = State_Mouse.Position;
            MouseDeltaX = State_Mouse.X - LastState_Mouse.X;
            MouseDeltaY = State_Mouse.Y - LastState_Mouse.Y;
            MouseScroll = State_Mouse.ScrollWheelValue;
            MouseDeltaScroll = State_Mouse.ScrollWheelValue - LastState_Mouse.ScrollWheelValue;
        }
    }
}