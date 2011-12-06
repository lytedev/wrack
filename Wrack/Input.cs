using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wrack
{
    public class InputEventArgs
    {
        public GameTime GameTime = null;

        public InputEventArgs()
        {

        }

        public InputEventArgs(GameTime gameTime)
        {
            GameTime = gameTime;
        }
    }

    public class Input
    {
        public static Dictionary<string, Input> Binds = new Dictionary<string, Input>();
        public static KeyboardState Keyboard = new KeyboardState();
        public static KeyboardState OldKeyboard = new KeyboardState();
        public static MouseState Mouse = new MouseState();
        public static MouseState OldMouse = new MouseState();

        public static int LeftMouse = -1;
        public static int RightMouse = -2;
        public static int MiddleMouse = -3;
        public static int MiscMouse1 = -4;
        public static int MiscMouse2 = -5;

        public static void Initialize()
        {
            Binds = new Dictionary<string, Input>();
        }

        public static void AddBind(string name, int[] inputs)
        {
            if (inputs.Length > 8)
                inputs = new int[8] { inputs[0], inputs[1], inputs[2], inputs[3], inputs[4], inputs[5], inputs[6], inputs[7] };
            if (inputs.Length < 1)
            {
                return;
            }
            if (Binds.ContainsKey(name))
            {
                return;
            }
            Binds.Add(name, new Input(inputs));
        }

        public static void AddBind(string name, Keys[] inputs)
        {
            if (inputs.Length > 8)
                inputs = new Keys[8] { inputs[0], inputs[1], inputs[2], inputs[3], inputs[4], inputs[5], inputs[6], inputs[7] };
            if (inputs.Length < 1)
            {
                return;
            }
            if (Binds.ContainsKey(name))
            {
                return;
            }
            Binds.Add(name, new Input(inputs));
        }

        public static void AddBind(string name, Input i)
        {
            if (i.IDs.Length > 8)
                i.IDs = new int[8] { i.IDs[0], i.IDs[1], i.IDs[2], i.IDs[3], i.IDs[4], i.IDs[5], i.IDs[6], i.IDs[7] };
            if (i.IDs.Length < 1)
            {
                return;
            }
            if (Binds.ContainsKey(name))
            {
                return;
            }
            Binds.Add(name, new Input(i.IDs));
        }

        public static Input GetBind(string name)
        {
            if (Binds.ContainsKey(name))
            {
                return Binds[name];
            }
            return null;
        }

        public static void DeleteBind(string name)
        {
            if (Binds.ContainsKey(name))
            {
                Binds.Remove(name);
                return;
            }
        }

        public static void ClearBinds()
        {
            Binds.Clear();
        }

        public static bool CommandMatch(string command, string input, bool caseSensitive = false, bool includeSpaces = false, bool useSubstrings = false)
        {
            if (command == null || input == null || command == "" || input == "")
                return false;

            if (!caseSensitive)
            {
                command = command.ToLower();
                input = input.ToLower();
            }
            if (!includeSpaces)
            {
                command = command.Replace(" ", "");
                input = input.Replace(" ", "");
            }
            if (useSubstrings)
            {
                int min = Math.Min(command.Length, input.Length);
                command = command.Substring(0, min);
                input = input.Substring(0, min);
            }

            return command == input;
        }

        public static string[] ParseArguments(string command)
        {
            if (command == "")
                return new string[] { "" };

            List<string> arguments = new List<string>();
            {
                string[] argumentsQ = command.Split(new char[] { '"', '\'', '`' });
                for (int i = 0; i < argumentsQ.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        arguments.AddRange(argumentsQ[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    }
                    else
                    {
                        arguments.Add(argumentsQ[i]);
                    }
                }
            }

            for (int i = 0; i < arguments.Count; i++)
            {
                arguments[i] = arguments[i].Trim();
            }
            return arguments.ToArray();
        }

        public static void Update(GameTime gameTime)
        {
            try
            {
                OldKeyboard = Keyboard;
                Keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            }
            catch
            {

            }
            try
            {
                OldMouse = Mouse;
                Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
            }
            catch
            {

            }

            foreach (KeyValuePair<string, Input> b in Binds)
            {
                if (Pressed(b.Key) && b.Value.OnPress != null)
                    b.Value.OnPress.Invoke(b.Value, new InputEventArgs(gameTime));
                if (Down(b.Key) && b.Value.OnDown != null)
                    b.Value.OnDown.Invoke(b.Value, new InputEventArgs(gameTime));
                if (Up(b.Key) && b.Value.OnUp != null)
                    b.Value.OnUp.Invoke(b.Value, new InputEventArgs(gameTime));
            }
        }

        public static bool Pressed(Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (Pressed((int)k))
                    return true;
            }

            return false;
        }

        public static bool Pressed(Keys key)
        {
            if (Pressed((int)key))
                return true;

            return false;
        }

        public static bool Pressed(int[] ids)
        {
            foreach (int i in ids)
            {
                if (Pressed(i))
                    return true;
            }
            return false;
        }

        public static bool Pressed(Input input)
        {
            foreach (int i in input.IDs)
            {
                if (Pressed(i))
                    return true;
            }
            return false;
        }

        public static bool Pressed(string name)
        {
            if (!Binds.ContainsKey(name))
            {
                return false;
            }
            foreach (int i in Binds[name].IDs)
            {
                if (Pressed(i))
                    return true;
            }
            return false;
        }

        public static bool Pressed(int id)
        {
            switch (id)
            {
                case -1:
                    return OldMouse.LeftButton == ButtonState.Released && Mouse.LeftButton == ButtonState.Pressed;
                case -2:
                    return OldMouse.RightButton == ButtonState.Released && Mouse.RightButton == ButtonState.Pressed;
                case -3:
                    return OldMouse.MiddleButton == ButtonState.Released && Mouse.MiddleButton == ButtonState.Pressed;
                case -4:
                    return OldMouse.XButton1 == ButtonState.Released && Mouse.XButton1 == ButtonState.Pressed;
                case -5:
                    return OldMouse.XButton2 == ButtonState.Released && Mouse.XButton1 == ButtonState.Pressed;
                default:
                    return (!OldKeyboard.IsKeyDown((Keys)id)) && (Keyboard.IsKeyDown((Keys)id));
            }
        }

        public static bool Down(Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (Down((int)k))
                    return true;
            }

            return false;
        }

        public static bool Down(Keys key)
        {
            if (Down((int)key))
                return true;

            return false;
        }

        public static bool Down(int[] ids)
        {
            foreach (int i in ids)
            {
                if (Down(i))
                    return true;
            }
            return false;
        }

        public static bool Down(Input input)
        {
            foreach (int i in input.IDs)
            {
                if (Down(i))
                    return true;
            }
            return false;
        }

        public static bool Down(string name)
        {
            if (!Binds.ContainsKey(name))
            {
                return false;
            }
            foreach (int i in Binds[name].IDs)
            {
                if (Down(i))
                    return true;
            }
            return false;
        }

        public static bool Down(int id)
        {
            switch (id)
            {
                case -1:
                    return Mouse.LeftButton == ButtonState.Pressed;
                case -2:
                    return Mouse.RightButton == ButtonState.Pressed;
                case -3:
                    return Mouse.MiddleButton == ButtonState.Pressed;
                case -4:
                    return Mouse.XButton1 == ButtonState.Pressed;
                case -5:
                    return Mouse.XButton2 == ButtonState.Pressed;
                default:
                    return Keyboard.IsKeyDown((Keys)id);
            }
        }

        public static bool Up(Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (Up((int)k))
                    return true;
            }

            return false;
        }

        public static bool Up(Keys key)
        {
            if (Up((int)key))
                return true;

            return false;
        }

        public static bool Up(int[] ids)
        {
            foreach (int i in ids)
            {
                if (Up(i))
                    return true;
            }
            return false;
        }

        public static bool Up(Input input)
        {
            foreach (int i in input.IDs)
            {
                if (Up(i))
                    return true;
            }
            return false;
        }

        public static bool Up(string name)
        {
            if (!Binds.ContainsKey(name))
            {
                return false;
            }
            foreach (int i in Binds[name].IDs)
            {
                if (Up(i))
                    return true;
            }
            return false;
        }

        public static bool Up(int id)
        {
            switch (id)
            {
                case -1:
                    return Mouse.LeftButton == ButtonState.Released;
                case -2:
                    return Mouse.RightButton == ButtonState.Released;
                case -3:
                    return Mouse.MiddleButton == ButtonState.Released;
                case -4:
                    return Mouse.XButton1 == ButtonState.Released;
                case -5:
                    return Mouse.XButton2 == ButtonState.Released;
                default:
                    return (!Keyboard.IsKeyDown((Keys)id));
            }
        }

        public static float ScrollChange
        {
            get
            {
                return OldMouse.ScrollWheelValue - Mouse.ScrollWheelValue;
            }
        }

        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(Mouse.X, Mouse.Y);
            }
        }

        public static Vector2 OldMousePosition
        {
            get
            {
                return new Vector2(OldMouse.X, OldMouse.Y);
            }
        }

        public delegate void OnPressHandler(object sender, InputEventArgs e);
        public delegate void OnDownHandler(object sender, InputEventArgs e);
        public delegate void OnUpHandler(object sender, InputEventArgs e);

        public event OnPressHandler OnPress;
        public event OnDownHandler OnDown;
        public event OnUpHandler OnUp;

        public int[] IDs { get; set; }

        public Input()
        {
            IDs = new int[] { 0 };
        }

        public Input(params int[] ids)
        {
            IDs = new int[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                IDs[i] = ids[i];
            }
        }

        public Input(params Keys[] keys)
        {
            IDs = new int[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                IDs[i] = (int)(keys[i]);
            }
        }
    }
}
