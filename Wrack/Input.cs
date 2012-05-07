using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WrackEngine
{
    public class Input
    {
        #region Static
        public const string NULL_STRING = "(NULL)";

        public const int LeftMouse = -1;
        public const int RightMouse = -2;
        public const int MiddleMouse = -4;
        public const int MiscMouse1 = -8;
        public const int MiscMouse2 = -16;

        public static string GetKeyName(Keys k)
        {
            return k.ToString();
        }

        public static Keys GetKeyCode(string name)
        {
            for (int i = 0; i < 500; i++)
            {
                if (GetKeyName((Keys)i).ToLower().Trim() == name.ToLower().Trim())
                {
                    return (Keys)i;
                }
            }
            return (Keys)0;
        }

        public static string RependArgs(string[] str, int offset, string delimiter = " ", int length = 0)
        {
            string s = "";
            if (str.Length - offset < 1) return ""; 
            if (length == 0) length = str.Length;
            for (int i = offset; i < offset + length && i < str.Length; i++)
            {
                s += delimiter + str[i];
            }
            return s.Substring(delimiter.Length);
        }

        public static string GetArgParsedString(string str, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string s = "";
                if (args[i] == null) s = NULL_STRING;
                else s = args[i].ToString();
                str = str.Replace("{" + i + "}", s);
            }

            return str;
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

        public static char KeyToChar(Keys k, bool shift, bool alt, bool ctrl)
        {
            int kid = (int)k;
            if (kid <= 57 && kid >= 48)
            {
                if (shift)
                {
                    switch (kid - 48)
                    {
                        case 1:
                            return '!';
                        case 2:
                            return '@';
                        case 3:
                            return '#';
                        case 4:
                            return '$';
                        case 5:
                            return '%';
                        case 6:
                            return '^';
                        case 7:
                            return '&';
                        case 8:
                            return '*';
                        case 9:
                            return '(';
                        case 0:
                            return ')';
                    }
                }
                return (char)kid;
            }
            if (kid <= 90 && kid >= 65)
            {
                if (!Console.CapsLock)
                {
                    if (!shift)
                    {
                        return (char)(kid + 32);
                    }
                    return (char)kid;
                }
                else
                {
                    if (!shift)
                    {
                        return (char)(kid);
                    }
                    return (char)(kid + 32);
                }
            }
            else
            {
                switch (k)
                {
                    case Keys.NumPad0:
                        return '0';
                    case Keys.NumPad1:
                        return '1';
                    case Keys.NumPad2:
                        return '2';
                    case Keys.NumPad3:
                        return '3';
                    case Keys.NumPad4:
                        return '4';
                    case Keys.NumPad5:
                        return '5';
                    case Keys.NumPad6:
                        return '6';
                    case Keys.NumPad7:
                        return '7';
                    case Keys.NumPad8:
                        return '8';
                    case Keys.NumPad9:
                        return '9';
                    case Keys.Space:
                        return ' ';
                    case Keys.OemMinus:
                        if (shift) return '_'; else return '-';
                    case Keys.OemPlus:
                        if (shift) return '+'; else return '=';
                    case Keys.OemTilde:
                        if (shift) return '~'; else return '`';
                    case Keys.OemBackslash:
                        if (shift) return '|'; else return '\\';
                    case Keys.OemOpenBrackets:
                        if (shift) return '{'; else return '[';
                    case Keys.OemCloseBrackets:
                        if (shift) return '}'; else return ']';
                    case Keys.OemQuotes:
                        if (shift) return '"'; else return '\'';
                    case Keys.OemSemicolon:
                        if (shift) return ':'; else return ';';
                    case Keys.OemComma:
                        if (shift) return '<'; else return ',';
                    case Keys.OemPeriod:
                        if (shift) return '>'; else return '.';
                    case Keys.OemQuestion:
                        if (shift) return '?'; else return '/';
                    case Keys.Multiply:
                        return '*';
                    case Keys.Divide:
                        return '/';
                    case Keys.Subtract:
                        return '-';
                    case Keys.Add:
                        return '+';
                    case Keys.Decimal:
                        return '.';
                    case Keys.Home:
                        return (char)2;
                    case Keys.End:
                        return (char)4;
                    case Keys.Back:
                        return (char)8;
                    case Keys.Delete:
                        return (char)127;
                    case Keys.Tab:
                        return (char)9;
                    case Keys.Escape:
                        return (char)27;
                    case Keys.Enter:
                        return (char)13;
                    case Keys.Up:
                        return (char)17;
                    case Keys.Down:
                        return (char)18;
                    case Keys.Left:
                        return (char)19;
                    case Keys.Right:
                        return (char)20;
                    case Keys.LeftShift:
                    case Keys.LeftAlt:
                    case Keys.LeftControl:
                    case Keys.RightShift:
                    case Keys.RightAlt:
                    case Keys.RightControl: // Pre-Handled for text-input in this function
                        return char.MinValue;
                }
            }

            return char.MinValue;
        }
        #endregion

        public Dictionary<string, InputBind> Binds = new Dictionary<string, InputBind>();
        public KeyboardState Keyboard = new KeyboardState();
        public KeyboardState OldKeyboard = new KeyboardState();
        public MouseState Mouse = new MouseState();
        public MouseState OldMouse = new MouseState();

        public int KeyChangedAgo = 0;

        public Input()
        {
            Binds = new Dictionary<string, InputBind>();
        }

        public void AddBind(string name, Keys key)
        {
            int[] inputs = new int[1] { (int)key };
            Binds.Add(name, new InputBind(inputs));
        }

        public int UpdateTextField(ref string textField, char c, ref int cp)
        {
            if (c == 8)
            {
                if (cp > 0 && textField.Length > 0)
                {
                    textField = textField.Remove(cp - 1, 1);
                    cp--;
                }
                return 0;
            }
            else if (c == 127)
            {
                if (cp <= textField.Length - 1) textField = textField.Remove(cp, 1);
                return 0;
            }
            else if (c == 2)
            {
                cp = 0;
                return 0;
            }
            else if (c == 4)
            {
                cp = textField.Length;
                return 0;
            }

            textField = textField.Insert(cp, new string(c, 1));
            return 1;
        }

        public void AddBind(string name, params int[] inputs)
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
            Binds.Add(name, new InputBind(inputs));
        }

        public void AddBind(string name, params Keys[] inputs)
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
            Binds.Add(name, new InputBind(inputs));
        }

        public void AddBind(string name, InputBind i)
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
            Binds.Add(name, new InputBind(i.IDs));
        }

        public InputBind GetBind(string name)
        {
            if (Binds.ContainsKey(name))
            {
                return Binds[name];
            }
            return null;
        }

        public void DeleteBind(string name)
        {
            if (Binds.ContainsKey(name))
            {
                Binds.Remove(name);
                return;
            }
        }

        public void ClearBinds()
        {
            Binds.Clear();
        }

        public string GetTextInput()
        {
            bool shift = Down(Keys.LeftShift) || Down(Keys.RightShift);
            bool alt = Down(Keys.LeftAlt) || Down(Keys.RightAlt);
            bool control = Down(Keys.LeftControl) || Down(Keys.RightControl);

            Keys[] keys = Keyboard.GetPressedKeys();
            Keys[] oldKeys = OldKeyboard.GetPressedKeys();
            for (int i = 0; i < oldKeys.Length; i++)
            {
                for (int j = 0; j < keys.Length; j++)
                {
                    if (keys[j] == oldKeys[i])
                    {
                        keys[j] = Keys.None;
                    }
                }
            }
            string input = "";
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == Keys.None) continue;
                char c = KeyToChar(keys[i], shift, alt, control);
                if (c == char.MinValue) continue;
                input += c;
            }

            if (KeyChangedAgo >= 500 && keys.Length > 0)
            {
                char c = KeyToChar(Keyboard.GetPressedKeys()[0], shift, alt, control);
                if ((c >= 32 && c <= 127) || c == 8 || (c >= 17 && c <= 20)) input += c;
            }
            return input;
        }

        public void Update(GameTime gameTime)
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

            if (Keyboard.GetPressedKeys().Length != OldKeyboard.GetPressedKeys().Length)
            {
                KeyChangedAgo = 0;
            }
            else
            {
                KeyChangedAgo += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        public bool Pressed(Keys[] keys)
        {
            if (keys == null) return false;
            foreach (Keys k in keys)
            {
                if (Pressed((int)k))
                    return true;
            }

            return false;
        }

        public bool Pressed(Keys key)
        {
            if (Pressed((int)key))
                return true;

            return false;
        }

        public bool Pressed(int[] ids)
        {
            if (ids == null) return false;
            foreach (int i in ids)
            {
                if (Pressed(i))
                    return true;
            }
            return false;
        }

        public bool Pressed(InputBind input)
        {
            foreach (int i in input.IDs)
            {
                if (Pressed(i))
                    return true;
            }
            return false;
        }

        public bool Pressed(string name)
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

        public bool Pressed(int id)
        {
            switch (id)
            {
                case LeftMouse:
                    return OldMouse.LeftButton == ButtonState.Released && Mouse.LeftButton == ButtonState.Pressed;
                case RightMouse:
                    return OldMouse.RightButton == ButtonState.Released && Mouse.RightButton == ButtonState.Pressed;
                case MiddleMouse:
                    return OldMouse.MiddleButton == ButtonState.Released && Mouse.MiddleButton == ButtonState.Pressed;
                case MiscMouse1:
                    return OldMouse.XButton1 == ButtonState.Released && Mouse.XButton1 == ButtonState.Pressed;
                case MiscMouse2:
                    return OldMouse.XButton2 == ButtonState.Released && Mouse.XButton1 == ButtonState.Pressed;
                default:
                    return (!OldKeyboard.IsKeyDown((Keys)id)) && (Keyboard.IsKeyDown((Keys)id));
            }
        }

        public bool Down(Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (Down((int)k))
                    return true;
            }

            return false;
        }

        public bool Down(Keys key)
        {
            if (Down((int)key))
                return true;

            return false;
        }

        public bool Down(int[] ids)
        {
            foreach (int i in ids)
            {
                if (Down(i))
                    return true;
            }
            return false;
        }

        public bool Down(InputBind input)
        {
            foreach (int i in input.IDs)
            {
                if (Down(i))
                    return true;
            }
            return false;
        }

        public bool Down(string name)
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

        public bool Down(int id)
        {
            switch (id)
            {
                case LeftMouse:
                    return Mouse.LeftButton == ButtonState.Pressed;
                case RightMouse:
                    return Mouse.RightButton == ButtonState.Pressed;
                case MiddleMouse:
                    return Mouse.MiddleButton == ButtonState.Pressed;
                case MiscMouse1:
                    return Mouse.XButton1 == ButtonState.Pressed;
                case MiscMouse2:
                    return Mouse.XButton2 == ButtonState.Pressed;
                default:
                    return Keyboard.IsKeyDown((Keys)id);
            }
        }

        public bool Up(Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (Up((int)k))
                    return true;
            }

            return false;
        }

        public bool Up(Keys key)
        {
            if (Up((int)key))
                return true;

            return false;
        }

        public bool Up(int[] ids)
        {
            foreach (int i in ids)
            {
                if (Up(i))
                    return true;
            }
            return false;
        }

        public bool Up(InputBind input)
        {
            foreach (int i in input.IDs)
            {
                if (Up(i))
                    return true;
            }
            return false;
        }

        public bool Up(string name)
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

        public bool Up(int id)
        {
            switch (id)
            {
                case LeftMouse:
                    return Mouse.LeftButton == ButtonState.Released;
                case RightMouse:
                    return Mouse.RightButton == ButtonState.Released;
                case MiddleMouse:
                    return Mouse.MiddleButton == ButtonState.Released;
                case MiscMouse1:
                    return Mouse.XButton1 == ButtonState.Released;
                case MiscMouse2:
                    return Mouse.XButton2 == ButtonState.Released;
                default:
                    return (!Keyboard.IsKeyDown((Keys)id));
            }
        }

        public float ScrollChange
        {
            get
            {
                return OldMouse.ScrollWheelValue - Mouse.ScrollWheelValue;
            }
        }

        public Vector2 MousePosition
        {
            get
            {
                return new Vector2(Mouse.X, Mouse.Y);
            }
        }

        public Vector2 OldMousePosition
        {
            get
            {
                return new Vector2(OldMouse.X, OldMouse.Y);
            }
        }
    }

    public class InputBind
    {
        public const int CONTROL_MODIFIER = 0x1000;
        public const int ALT_MODIFIER = 0x2000;
        public const int SHIFT_MODIFIER = 0x4000;

        public int[] IDs { get; set; }

        public InputBind()
        {
            IDs = new int[] { 0 };
        }

        public InputBind(params int[] ids)
        {
            IDs = new int[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                IDs[i] = ids[i];
            }
        }

        public InputBind(params Keys[] keys)
        {
            IDs = new int[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                IDs[i] = (int)(keys[i]);
            }
        }
    }
}
