using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxisEngine.UserInput
{
    /// <summary>
    /// Manages the input from the user
    /// </summary>
    public class InputManager : WorldObject
    {
        /// <summary>
        /// Axis bindings (a positive and negative button)
        /// </summary>
        private Dictionary<string, Tuple<Keys, Keys>> AxisBindings;

        /// <summary>
        /// A collection of bindings using the keyboard
        /// </summary>
        private Dictionary<string, Keys> Bindings_Keyboard;

        /// <summary>
        /// A collection of bindings using the mouse
        /// </summary>
        private Dictionary<string, MouseButtons> Bindings_Mouse;

        /// <summary>
        /// The last frame's keyboard's button presses
        /// </summary>
        private KeyboardState LastState_Keyboard;

        /// <summary>
        /// The last frame's mouse's state
        /// </summary>
        private MouseState LastState_Mouse;

        /// <summary>
        /// The state of the keyboard's button presses
        /// </summary>
        private KeyboardState State_Keyboard;

        /// <summary>
        /// The state of the Mouse
        /// </summary>
        private MouseState State_Mouse;

        /// <summary>
        /// Instantiates a new InputManager
        /// </summary>
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

        /// <summary>
        /// The change in the scroll value from the last frame
        /// </summary>
        public int MouseDeltaScroll { get; private set; }

        /// <summary>
        /// The change in X in the last update
        /// </summary>
        public int MouseDeltaX { get; private set; }

        /// <summary>
        /// the change in Y in the last update
        /// </summary>
        public int MouseDeltaY { get; private set; }

        /// <summary>
        /// The position of the mouse
        /// </summary>
        public Point MousePosition { get; private set; }

        /// <summary>
        /// The scroll input of the mouse
        /// </summary>
        public int MouseScroll { get; private set; }

        /// <summary>
        /// The X input of the mouse
        /// </summary>
        public int MouseX { get; private set; }

        /// <summary>
        /// The Y input of the mouse
        /// </summary>
        public int MouseY { get; private set; }

        /// <summary>
        /// adds an input axis
        /// </summary>
        /// <param name="name">the name of the axis</param>
        /// <param name="positiveKey">the positive key</param>
        /// <param name="negativeKey">the negative key</param>
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

        /// <summary>
        /// Adds a key binding
        /// </summary>
        /// <param name="name">the name of the binding</param>
        /// <param name="key">the key that is pushed</param>
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

        /// <summary>
        /// adds a mouse binding
        /// </summary>
        /// <param name="name">the name of the bnding</param>
        /// <param name="mouseButton">the mouse button to be pushed</param>
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

        /// <summary>
        /// removes all bindings
        /// </summary>
        public void ClearBindings()
        {
            Bindings_Keyboard.Clear();
            Bindings_Mouse.Clear();
        }

        /// <summary>
        /// gets the value of the axis [-1, 0, or 1]
        /// </summary>
        /// <param name="name">the name of the axis</param>
        /// <returns>-1, 0, or 1 depending on the state of the axis</returns>
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

        /// <summary>
        /// gets the state of a binding
        /// </summary>
        /// <param name="name">the name of the binding</param>
        /// <returns>whether or not the binding is pressed</returns>
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

        /// <summary>
        /// gets the binding on its first frame
        /// </summary>
        /// <param name="name">the name of the binding</param>
        /// <returns>returns true on the first frame that the binding is pressed</returns>
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

        /// <summary>
        /// gets the binding when it is released
        /// </summary>
        /// <param name="name">the name of the binding</param>
        /// <returns>returns true on the first frame that the binding is released</returns>
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

        /// <summary>
        /// removes a binding if it exists
        /// </summary>
        /// <param name="name">the name of the binding to remove</param>
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

        /// <summary>
        /// refreshes the input states for the current frame
        /// </summary>
        /// <param name="t">the time since the last update</param>
        public override void UpdateThis(GameTime t)
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

            base.UpdateThis(t);
        }
    }
}