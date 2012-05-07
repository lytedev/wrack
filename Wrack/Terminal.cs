using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WrackEngine
{
    public class Terminal
    {
        public const string NULL_STIRNG = "NULL";

        int scrollPosition = 0;
        int cursorPosition = 0;

        public int MaxMessages { get; set; }
        public int ScrollPosition
        {
            get
            {
                return scrollPosition;
            }
            set
            {
                scrollPosition = value;
                if (scrollPosition > Math.Min(Messages.Count, MaxMessages) - MaxLines) scrollPosition = Math.Min(Messages.Count, MaxMessages) - MaxLines;
                if (scrollPosition < 0) scrollPosition = 0;
            }
        }
        public int MaxLines
        {
            get
            {
                if (Font == null) return 0;
                else return ((int)((Size.Y - (Spacing.Y * 2)) / Font.LineSpacing)) - 1; // Minus 1 for input room!
            }
        }
        public bool Showing { get; set; }

        public Input Input { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 Spacing { get; set; }
        public Color BackgroundColor { get; set; }
        public Color DefaultMessageColor { get; set; }
        public Color MessageBackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public bool DrawBackground { get; set; }
        public TextAlign Alignment { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Keys[] TerminalKey { get; set; }
        public bool CanShow { get; set; }

        public ScriptEngine ScriptEngine { get; set; }

        public int CurrentCommandIndex { get; set; }
        public List<string> PreviousCommands { get; set; }
        public string Command { get; set; }

        public string CurrentCommand { get; set; }
        public bool ShowCursor { get; set; }
        public int CursorTime { get; set; }
        public int CursorBlinkTime { get; set; }
        public int CursorPosition
        {
            get
            {
                return cursorPosition;
            }
            set
            {
                cursorPosition = value;
                if (cursorPosition < 0) cursorPosition = 0;
                if (cursorPosition >= CurrentCommand.Length) cursorPosition = CurrentCommand.Length;
            }
        }

        public Dictionary<TerminalMessageType, Color> MessageColors = new Dictionary<TerminalMessageType, Color>();
        public Queue<TerminalMessage> Messages = new Queue<TerminalMessage>();

        public Terminal(Input input)
        {
            CanShow = true;
            Input = input;
            MaxMessages = 300;
            TerminalKey = new Keys[] { Keys.Tab, Keys.OemTilde };
            Spacing = new Vector2(10, 10);
            ScriptEngine = new ScriptEngine();
            ScriptEngine.Terminal = this;
            MaxMessages = 300;
            BackgroundColor = new Color(17, 17, 17);
            DefaultMessageColor = new Color(128, 128, 128);
            MessageBackgroundColor = Color.Black;
            BorderColor = new Color(34, 34, 34);
            DrawBackground = true;
            Alignment = TextAlign.TopLeft;
            Position = -Vector2.One;
            Size = new Vector2(640, 180);

            CurrentCommand = "";
            CursorPosition = 0;
            ShowCursor = true;
            CursorBlinkTime = 500;
            CursorTime = 0;

            PreviousCommands = new List<string>();

            MessageColors.Add(TerminalMessageType.Error, new Color(255, 40, 0, 255));
            MessageColors.Add(TerminalMessageType.Fatal, new Color(255, 20, 255, 255));
            MessageColors.Add(TerminalMessageType.Good, new Color(40, 255, 0, 255));
            MessageColors.Add(TerminalMessageType.Info, new Color(255, 255, 255, 255));
            MessageColors.Add(TerminalMessageType.Input, new Color(0, 150, 255, 255));
            MessageColors.Add(TerminalMessageType.Warning, new Color(255, 255, 0, 255));
        }

        public void Initialize()
        {
            if (Font == null)
                Font = Graphics.DefaultFont;

            ScriptEngine.Initialize();
            WriteLine("TERMINAL: JavaScript interpreter initialized.");
            WriteLine("TERMINAL: Type \"help\" and press enter for more commands.");
            WriteLine("TERMINAL: Scroll up and down with CTRL+UpArrow and CTRL+DownArrow.");
            Size = new Vector2(Graphics.Resolution.X + 2, Graphics.WindowedResolution.Y / 2);
        }

        public void TestMessageTypes()
        {
            WriteLine(TerminalMessageType.Error, "TERMINAL: Test error.");
            WriteLine(TerminalMessageType.Fatal, "TERMINAL: Test fatal.");
            WriteLine(TerminalMessageType.Good, "TERMINAL: Test good.");
            WriteLine(TerminalMessageType.Info, "TERMINAL: Test info.");
            WriteLine(TerminalMessageType.Input, "TERMINAL: Test input.");
            WriteLine(TerminalMessageType.Normal, "TERMINAL: Test normal.");
            WriteLine(TerminalMessageType.Warning, "TERMINAL: Test warning.");

            WriteLine(TerminalMessageType.Info, "TERMINAL: Test complete!");
        }

        public void Update(GameTime gameTime)
        {
            if (!Wrack.Game.IsActive) return;
            if (Input.Pressed(TerminalKey))
            {
                Input.OldKeyboard = Input.Keyboard;
                Showing = !Showing;
            } if (!Showing) { CurrentCommand = ""; CursorPosition = 0; return; }
            if (Showing && !CanShow) Showing = false;
            if (Input.Keyboard.GetPressedKeys().Length > 0 && Showing)
            {
                string ti = Input.GetTextInput();
                if (ti != "")
                {
                    bool control = Input.Down(Keys.LeftControl) || Input.Down(Keys.RightControl);
                    CursorTime = CursorBlinkTime;
                    ShowCursor = true;
                    int added = 0;
                    for (int i = 0; i < ti.Length; i++)
                    {
                        char c = ti[i];
                        if ((c == 'v' || c == 'V') && control)
                        {
                            if (System.Windows.Forms.Clipboard.ContainsText())
                            {
                                string str = System.Windows.Forms.Clipboard.GetText();
                                CurrentCommand = CurrentCommand.Insert(CursorPosition, str);
                                added += str.Length;
                            }
                            continue;
                        }
                        if (c == 13)
                        {
                            if (CurrentCommand.Trim() != "")
                            {
                                ProcessCommand(CurrentCommand);
                                if (PreviousCommands.Count > 0 && CurrentCommand != "")
                                {
                                    if (PreviousCommands[PreviousCommands.Count - 1] != CurrentCommand) PreviousCommands.Add(CurrentCommand);
                                }
                                else if (CurrentCommand != "")
                                {
                                    PreviousCommands.Add(CurrentCommand);
                                }
                                while (PreviousCommands.Count > 100) PreviousCommands.RemoveAt(0);
                                Command = "";
                                CursorPosition = 0;
                                CurrentCommandIndex = PreviousCommands.Count;
                                CheckCurrentCommand();
                            }
                        }
                        else if (c == 9)
                        {
                            ScriptEngine.AutoComplete(CurrentCommand);
                            continue;
                        }
                        else if (c == 27)
                        {
                            Command = "";
                            CurrentCommandIndex = PreviousCommands.Count;
                            added = 0;
                            CursorPosition = 0;
                            continue;
                        }
                        else if (c == 17)
                        {
                            if (control)
                            {
                                ScrollPosition -= MaxLines / 2;
                                continue;
                            }
                            int last = CurrentCommandIndex;
                            CurrentCommandIndex--;
                            if (CurrentCommandIndex != last) CheckCurrentCommand();
                            continue;
                        }
                        else if (c == 18)
                        {
                            if (control)
                            {
                                ScrollPosition += MaxLines / 2;
                                continue;
                            }
                            int last = CurrentCommandIndex;
                            CurrentCommandIndex++;
                            if (CurrentCommandIndex != last) CheckCurrentCommand();
                            continue;
                        }
                        else if (c == 19)
                        {
                            if (CursorPosition <= 0) continue;
                            char ch = CurrentCommand[CursorPosition - 1];
                            do
                            {
                                ch = CurrentCommand[CursorPosition - 1];
                                CursorPosition--;
                            } while (control &&
                                CursorPosition - 1 > -1 && (
                                (ch >= '0' && ch <= '9') ||
                                (ch >= 'a' && ch <= 'z') ||
                                (ch >= 'A' && ch <= 'Z')
                                ));
                            continue;
                        }
                        else if (c == 20)
                        {
                            if (CursorPosition >= CurrentCommand.Length - 1) continue;
                            char ch = CurrentCommand[CursorPosition];
                            do
                            {
                                ch = CurrentCommand[CursorPosition];
                                CursorPosition++;
                            } while (control &&
                                CursorPosition + 1 <= CurrentCommand.Length && (
                                (ch >= '0' && ch <= '9') ||
                                (ch >= 'a' && ch <= 'z') ||
                                (ch >= 'A' && ch <= 'Z')
                                ));
                            continue;
                        }
                        else
                        {
                            string cc = CurrentCommand;
                            int cp = CursorPosition;
                            added += Input.UpdateTextField(ref cc, c, ref cp);
                            CurrentCommand = cc;
                            CursorPosition = cp;
                        }
                    }
                    CursorPosition += added;
                }
                if (CurrentCommandIndex == PreviousCommands.Count) Command = CurrentCommand;
            }
        }

        public void CheckCurrentCommand()
        {
            if (CurrentCommandIndex < 0) CurrentCommandIndex = 0;
            else if (CurrentCommandIndex >= PreviousCommands.Count)
            {
                CurrentCommandIndex = PreviousCommands.Count;
                CurrentCommand = Command;
            }
            else
            {
                CurrentCommand = PreviousCommands[CurrentCommandIndex];
            }
            CursorPosition = CurrentCommand.Length;
        }

        public void ProcessCommand(string str)
        {
            try
            {
                ScriptEngine.CallCommand(str);
            }
            catch (Exception e)
            {
                WriteLine(TerminalMessageType.Error, e.Message.Replace('\n', ' ').Replace("\r", "").Trim());
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (!Showing) return;

            if (DrawBackground)
                DrawTerminalBackground();

            if (Font == null) return;

            Vector2 bPos = Position + Spacing;
            int j = 0;
            int maxBound = 0;
            int initOffset = 0;
            float offset = 1;

            lock (Messages)
            {
                for (int i = scrollPosition + initOffset; i < scrollPosition + MaxLines + maxBound && i < Messages.Count + maxBound; i++)
                {
                    TerminalMessage tm = Messages.ElementAt(i);
                    string msg = tm.Sender + ": " + tm.Message;
                    Vector2 pos = new Vector2(0, Font.LineSpacing * j) + bPos;
                    Graphics.DrawStringWithOutline(Font, msg, pos, GetMessageColor(tm.Type), MessageBackgroundColor, offset);
                    j++;
                }
            }

            bPos = Position + (Size * Vector2.UnitY) + (Spacing * new Vector2(1, -1)) - new Vector2(0, Font.LineSpacing);
            Graphics.DrawStringWithOutline(Font, CurrentCommand, bPos, GetMessageColor(TerminalMessageType.Input), MessageBackgroundColor, offset);

            CursorTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (CursorTime <= 0)
            {
                ShowCursor = !ShowCursor;
                CursorTime = CursorBlinkTime;
            }

            if (ShowCursor)
            {
                bPos += (Font.MeasureString(CurrentCommand.Substring(0, CursorPosition)) * Vector2.UnitX);
                Graphics.DrawLine(bPos, bPos + new Vector2(0, Font.LineSpacing), GetMessageColor(TerminalMessageType.Input));
            }
        }

        public void DrawTerminalBackground()
        {
            Graphics.DrawRectangle(Position, Size, BackgroundColor);
            Graphics.DrawRectangleOutline(Position, Size, BorderColor);
        }

        public void WriteLine()
        {
            WriteLine("");
        }

        public void WriteLine(string str)
        {
            if (str == null) return;
            WriteLine(new TerminalMessage(str));
        }

        public void WriteLine(object arg)
        {
            if (arg == null) WriteLine(NULL_STIRNG);
            else 
            {
                try
                {
                    WriteLine(arg.ToString());
                }
                catch (Exception e)
                {
                    WriteLine(TerminalMessageType.Error, "TERMINAL: {0}", e.Message);
                } 
            }
        }

        public void WriteLine(string str, params object[] args)
        {
            WriteLine(new TerminalMessage(Input.GetArgParsedString(str, args)));
        }

        public void WriteLine(TerminalMessageType type, string str, params object[] args)
        {
            TerminalMessage tm = new TerminalMessage(Input.GetArgParsedString(str, args));
            tm.Type = type;
            WriteLine(tm);
        }

        public void WriteLine(TerminalMessage tMessage)
        {
            tMessage.Message = tMessage.Message.Replace('\n', ' ');
            Messages.Enqueue(tMessage);

            while (Messages.Count >= MaxMessages && Messages.Count > 0)
            {
                Messages.Dequeue();
            }

            ScrollPosition++;
        }

        public Color GetMessageColor(TerminalMessageType type)
        {
            if (MessageColors.ContainsKey(type))
            {
                return MessageColors[type];
            }
            else
            {
                return DefaultMessageColor;
            }
        }
    }

    public struct TerminalMessage
    {
        public TerminalMessageType Type;
        public string Sender;
        public string Message;

        public TerminalMessage(string str)
        {
            if (str == null) str = "";
            str = str.Trim();
            Type = TerminalMessageType.Normal;
            string[] ss = str.Split(new char[] { ':' });
            if (ss.Length < 2)
            {
                Sender = "CORE";
                Message = str.Trim();
            }
            else
            {
                Sender = ss[0].Trim();
                Message = ss[1].Trim();
                for (int i = 2; i < ss.Length; i++)
                {
                    Message += ":" + ss[i];
                }
            }
        }
    }

    public enum TerminalMessageType
    {
        Normal = 0, // Normal Message
        Info, // Something particularly useful/interesting to the user, server info, initialization finished, etc.
        Good, // Something with a chance of failure that worked

        Input, // The input box color

        Warning, // Something that may have been intended did not go that way.
        Error, // An error. Something potentially crashed the program. 
        Fatal, // The program would definitely have crashed. Report immediately. 
    }
}
