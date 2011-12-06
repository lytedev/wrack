using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wrack
{
    public static class Graphics
    {
        private static GraphicsDevice graphicsDevice;
        private static GameWindow gameWindow;
        private static SpriteBatch spriteBatch;
        private static SpriteFont defaultFont;

        public static Texture2D WhitePixel { get; set; }
        public static Color ClearColor { get; set; }
        public static SpriteFont DefaultFont { get { return defaultFont; } set { defaultFont = value; defaultFontSet = defaultFont != null; } }
        public static Camera CurrentCamera { get; set; }
        public static GraphicsDevice GraphicsDevice { get { return graphicsDevice; } }
        public static GameWindow Window { get { return gameWindow; } }
        public static Vector2 Resolution { get { return new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight); } }
        public static SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public static Dictionary<string, Camera> Cameras { get; set; }
        public static Dictionary<string, Texture2D> Textures { get; set; }
        public static Dictionary<string, SpriteFont> Fonts { get; set; }

        private static bool defaultFontSet = false;

        public static void Initialize()
        {
            graphicsDevice = Core.Game.GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameWindow = Core.Game.Window;

            WhitePixel = new Texture2D(GraphicsDevice, 1, 1);
            WhitePixel.SetData<Color>(new Color[] { new Color(255, 255, 255, 255) });

            Cameras = new Dictionary<string, Camera>();
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();

            CurrentCamera = new Camera();
            Cameras.Add("default", CurrentCamera);
            Textures.Add("default", WhitePixel);
            if (DefaultFont != null)
            {
                Fonts.Add("default", DefaultFont);
            }
        }

        public static Camera GetCamera(string name)
        {
            return Cameras[name];
        }

        public static void AddCamera(string name, Camera camera)
        {
            Cameras.Add(name, camera);
        }

        public static Texture2D GetTexture(string name)
        {
            return Textures[name];
        }

        public static void AddTexture(string name, Texture2D texture)
        {
            Textures.Add(name, texture);
        }

        public static SpriteFont GetFont(string name)
        {
            return Fonts[name];
        }

        public static void AddFont(string name, SpriteFont font)
        {
            Fonts.Add(name, font);
        }

        public static void Draw(Texture2D texture, Vector2 position, Vector2 scale, Vector2 origin, Rectangle sourceRect, Color overlay, float rotation, SpriteEffects effects, int layerDepth)
        {
            SpriteBatch.Draw(texture, position, sourceRect, overlay, rotation, origin, scale, effects, layerDepth);
        }

        public static void DrawString(string str)
        {
            if (defaultFontSet)
                DrawString(DefaultFont, str, Vector2.Zero, Color.White);
        }

        public static void DrawString(SpriteFont font, string str)
        {
            DrawString(font, str, Vector2.Zero, Color.White);
        }

        public static void DrawString(string str, Color color)
        {
            if (defaultFontSet)
                DrawString(DefaultFont, str, Vector2.Zero, color);
        }

        public static void DrawString(string str, Vector2 position)
        {
            if (defaultFontSet)
                DrawString(DefaultFont, str, position, Color.White);
        }

        public static void DrawString(SpriteFont font, string str, Color color)
        {
            DrawString(font, str, Vector2.Zero, color);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position)
        {
            DrawString(font, str, position, Color.White);
        }

        public static void DrawString(string str, Vector2 position, Color color)
        {
            if (defaultFontSet)
                DrawString(DefaultFont, str, position, color);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position, Color color)
        {
            SpriteBatch.DrawString(font, str, position, color);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position, TextAlign align, Color color)
        {
            DrawString(font, str, GetAlignedPosition(font, str, position, align), color);
        }

        public static void DrawString(SpriteFont font, string str, Rectangle bounds, TextAlign align, Color color)
        {
            DrawString(font, str, GetAlignedPosition(font, str, bounds, align), color);
        }

        public static Vector2 GetAlignedPosition(SpriteFont font, string str, Vector2 position, TextAlign align)
        {
            Vector2 drawSize = font.MeasureString(str);
            float x = position.X, y = position.Y;
            if ((int)align <= 3)
            {
                x = position.X - drawSize.X;
            }
            else if ((int)align <= 6)
            {
                x = position.X - (drawSize.X / 2);
            }
            else if ((int)align <= 9)
            {
                x = position.X;
            }
            if ((int)align % 3 == 0)
            {
                y = position.Y;
            }
            else if ((int)align % 3 == 1)
            {
                y = position.Y - drawSize.Y;
            }
            else if ((int)align % 3 == 2)
            {
                y = position.Y - (drawSize.Y / 2);
            }
            return new Vector2(x, y);
        }

        public static Vector2 GetAlignedPosition(SpriteFont font, string str, Rectangle bounds, TextAlign align)
        {
            Vector2 drawSize = font.MeasureString(str);
            float x = bounds.X, y = bounds.Y;
            if ((int)align <= 3)
            {
                x = bounds.X;
            }
            else if ((int)align <= 6)
            {
                x = bounds.X + ((bounds.Width / 2) - (drawSize.X / 2));
            }
            else if ((int)align <= 9)
            {
                x = bounds.X + bounds.Width - drawSize.X;
            }
            if ((int)align % 3 == 0)
            {
                y = bounds.Y + bounds.Height - drawSize.Y;
            }
            else if ((int)align % 3 == 1)
            {
                y = bounds.Y;
            }
            else if ((int)align % 3 == 2)
            {
                y = bounds.Y + ((bounds.Height / 2) - (drawSize.Y / 2));
            }
            return new Vector2(x, y);
        }

        public static void Clear()
        {
            GraphicsDevice.Clear(ClearColor);
        }

        public static void Begin()
        {
            Clear();
            SpriteBatch.Begin();
        }

        public static void End()
        {
            SpriteBatch.End();
        }
    }

    public enum TextAlign
    {
        TopLeft = 1,
        MiddleLeft,
        BottomLeft,
        TopCenter,
        MiddleCenter,
        BottomCenter,
        TopRight,
        MiddleRight,
        BottomRight
    }
}
