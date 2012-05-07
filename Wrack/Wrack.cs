using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WrackEngine
{
    public static class Wrack
    {
        private static Game game;
        private static Input input;
        private static Terminal terminal;

        public static bool DrawCursor { get; set; }
        public static AnimatedTexture2D Cursor { get; set; }
        public static Game Game { get { return game; } }
        public static Input Input { get { return input; } }
        public static Terminal Terminal { get { return terminal; } }
        public static ScriptEngine ScriptEngine { get { return Terminal.ScriptEngine; } }
        public static Keys[] FullscreenKey { get; set; }

        // public static bool LockMouseInScreen { get; set; }
        
        static int ticks = 0;
        static int frames = 0;
        static double time = 0;
        static float fps { get { return (float)(frames / time); } }
        static float tps { get { return (float)(ticks / time); } }

        public static float GetFPS() { return fps; }
        public static float GetTPS() { return tps; }

        public static void Prepare(Game g, string contentDirectory)
        {
            FullscreenKey = new Keys[] { Keys.F11 };
            Settings.Load();
            // LockMouseInScreen = false;
            game = g;
            game.Content.RootDirectory = contentDirectory;
            Graphics.Prepare();
            input = new Input();
            terminal = new Terminal(Input);
            Cursor = null;
            DrawCursor = true;
        }

        public static void Initialize()
        {
            Graphics.Initialize();
            Terminal.Initialize();
            Terminal.WriteLine("Loaded!");
            Terminal.ScrollPosition = 0;

            ScriptEngine.Interpreter.SetParameter("Game", Game);
            ScriptEngine.Interpreter.SetParameter("Input", Input);
            ScriptEngine.Interpreter.SetParameter("Terminal", Terminal);
        }

        public static void Update(GameTime gameTime)
        {
            Terminal.ScriptEngine.Interpreter.SetParameter("dt", gameTime.ElapsedGameTime.TotalSeconds);

            if (time > 2)
            {
                time = 0;
                frames = 0;
                ticks = 0;
            }
            ticks++;
            time += gameTime.ElapsedGameTime.TotalSeconds;
            /*if (LockMouseInScreen && Game.IsActive)
            {
                if (Input.MousePosition.X < 0) Mouse.SetPosition(0, (int)Input.MousePosition.Y);
                if (Input.MousePosition.X >= Graphics.Resolution.X) Mouse.SetPosition((int)Graphics.Resolution.X - 1, (int)Input.MousePosition.Y);
                if (Input.MousePosition.Y < 0) Mouse.SetPosition((int)Input.MousePosition.X, 0);
                if (Input.MousePosition.Y >= Graphics.Resolution.Y) Mouse.SetPosition((int)Input.MousePosition.X, (int)Graphics.Resolution.Y - 1);
            }*/
            Input.Update(gameTime);
            Terminal.Update(gameTime);
            if (Input.Pressed(FullscreenKey))
            {
                Graphics.ToggleFullscreen();
            }
        }

        public static void Draw(GameTime gameTime)
        {
            Graphics.CurrentCamera.Update(gameTime);
            Terminal.ScriptEngine.Interpreter.SetParameter("ddt", gameTime.ElapsedGameTime.TotalSeconds);

            frames++;
            Terminal.Draw(gameTime);
            if (DrawCursor && Cursor != null && Utilities.PointIsInRect(Input.MousePosition, Utilities.VectorsToRectangle(Vector2.Zero, Graphics.Resolution))) Cursor.Draw(Input.MousePosition);
        }
    }
}
