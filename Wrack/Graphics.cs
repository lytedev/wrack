using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;

namespace WrackEngine
{
    public static class Graphics
    {
        private static GraphicsDevice graphicsDevice;
        private static GraphicsDeviceManager graphicsDeviceManager;
        private static GameWindow gameWindow;
        private static SpriteBatch spriteBatch;
        private static SpriteFont defaultFont;

        public static bool CanFullscreen { get; set; }
        public static int BaseResolutionHeight { get; set; }
        public static Texture2D WhitePixel { get; set; }
        public static Texture2D Default3x3Texture { get; set; }
        public static Texture2D DefaultHollow3x3Texture { get; set; }
        public static Color ClearColor { get; set; }
        public static SpriteFont DefaultFont { get { return defaultFont; } set { defaultFont = value; defaultFontSet = defaultFont != null; } }
        public static Camera CurrentCamera { get; set; }
        public static GraphicsDevice GraphicsDevice { get { return graphicsDevice; } }
        public static GraphicsDeviceManager GraphicsDeviceManager { get { return graphicsDeviceManager; } }
        public static GameWindow Window { get { return gameWindow; } }
        public static Vector2 Resolution { get { return new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight); } }
        public static Vector2 WindowedResolution { get; set; }
        public static SpriteBatch SpriteBatch { get { return spriteBatch; } }
        public static Dictionary<string, Camera> Cameras { get; set; }
        public static Dictionary<string, Texture2D> Textures { get; set; }
        public static Dictionary<string, SpriteFont> Fonts { get; set; }
        public static Dictionary<string, Effect> Effects { get; set; }
        public static Dictionary<string, Dictionary<string, Animation>> AnimationSets { get; set; }

        private static bool defaultFontSet = false;

        public static void Prepare()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(Wrack.Game);
            WindowedResolution = new Vector2(800, 480);
            BaseResolutionHeight = 0;
        }

        public static void Initialize()
        {
            CanFullscreen = true;
            graphicsDevice = graphicsDeviceManager.GraphicsDevice;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameWindow = Wrack.Game.Window;

            WhitePixel = new Texture2D(GraphicsDevice, 1, 1);
            WhitePixel.SetData<Color>(new Color[] { new Color(255, 255, 255, 255) });

            Default3x3Texture = new Texture2D(GraphicsDevice, 3, 3);
            Default3x3Texture.SetData<Color>(new Color[] { new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255) });

            DefaultHollow3x3Texture = new Texture2D(GraphicsDevice, 3, 3);
            DefaultHollow3x3Texture.SetData<Color>(new Color[] { new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), Color.Transparent, new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255), new Color(255, 255, 255, 255) });

            Cameras = new Dictionary<string, Camera>();
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
            Effects = new Dictionary<string, Effect>();
            AnimationSets = new Dictionary<string, Dictionary<string, Animation>>();

            CurrentCamera = new Camera();
            Cameras.Add("default", CurrentCamera);
            Textures.Add("default", WhitePixel);
            Textures.Add("default3x3", Default3x3Texture);
            Textures.Add("defaulth3x3", DefaultHollow3x3Texture);
            Textures.Add("null", null);
            Textures.Add(Input.NULL_STRING, GetTexture("null"));
            Fonts.Add("default", DefaultFont);
        }

        public static Dictionary<string, Animation> GetAnimationSet(string name)
        {
            if (AnimationSets.ContainsKey(name)) return AnimationSets[name];
            else return null;
        }

        public static void AddAnimationSet(string name, Dictionary<string, Animation> AnimationSet)
        {
            if (AnimationSets.ContainsKey(name)) AnimationSets[name] = AnimationSet;
            else AnimationSets.Add(name, AnimationSet);
        }

        public static Camera GetCamera(string name)
        {
            if (Cameras.ContainsKey(name)) return Cameras[name];
            else return null;
        }

        public static void AddCamera(string name, Camera camera)
        {
            if (Cameras.ContainsKey(name)) Cameras[name] = camera;
            else Cameras.Add(name, camera);
        }

        public static Texture2D GetTexture(string name)
        {
            if (Textures.ContainsKey(name)) return Textures[name];
            else
            {
                return WhitePixel;
            }
        }

        public static Texture2D GetSubTexture(Texture2D texture, Rectangle bounds)
        {
            if (texture == null) return null;
            Color[] c = new Color[bounds.Width * bounds.Height];
            texture.GetData<Color>(0, bounds, c, 0, bounds.Width * bounds.Height);
            Texture2D t = new Texture2D(Graphics.GraphicsDevice, bounds.Width, bounds.Height);
            t.SetData<Color>(c);
            return t;
        }

        public static void DrawBorder(Texture2D borderTexture, Rectangle destRect, Vector2 partSize, Vector2 scale, bool repeating)
        {
            // Corners
            int extCounter = 0;
            Texture2D left = GetSubTexture(borderTexture, Utilities.VectorsToRectangle(new Vector2(partSize.X * extCounter++, 0), partSize));
            Texture2D right = GetSubTexture(borderTexture, Utilities.VectorsToRectangle(new Vector2(partSize.X * extCounter++, 0), partSize));
            Texture2D top = GetSubTexture(borderTexture, Utilities.VectorsToRectangle(new Vector2(partSize.X * extCounter++, 0), partSize));
            Texture2D bottom = GetSubTexture(borderTexture, Utilities.VectorsToRectangle(new Vector2(partSize.X * extCounter++, 0), partSize));
            Texture2D topLeft = GetSubTexture(borderTexture, Utilities.VectorsToRectangle(new Vector2(partSize.X * extCounter++, 0), partSize));
            Texture2D topRight = GetSubTexture(borderTexture, Utilities.VectorsToRectangle(new Vector2(partSize.X * extCounter++, 0), partSize));
            Texture2D bottomLeft = GetSubTexture(borderTexture, Utilities.VectorsToRectangle(new Vector2(partSize.X * extCounter++, 0), partSize));
            Texture2D bottomRight = GetSubTexture(borderTexture, Utilities.VectorsToRectangle(new Vector2(partSize.X * extCounter++, 0), partSize));

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

        public static Effect GetEffect(string name)
        {
            return Effects[name];
        }

        public static void AddEffect(string name, Effect effect)
        {
            Effects.Add(name, effect);
        }

        public static void Draw(Texture2D texture, Vector2 position, Vector2 scale, Vector2 origin, Rectangle sourceRect, Color overlay, float rotation, SpriteEffects effects, int layerDepth)
        {
            SpriteBatch.Draw(texture, position, sourceRect, overlay, rotation, origin, scale, effects, layerDepth);
        }

        public static void Draw(Texture2D texture, Vector2 position, Vector2 scale, Vector2 origin, Color overlay, float rotation, SpriteEffects effects, int layerDepth)
        {
            SpriteBatch.Draw(texture, position, null, overlay, rotation, origin, scale, effects, layerDepth);
        }

        public static void TileTexture(Texture2D texture, Rectangle srcRect, Rectangle destRect, Vector2 scale, Vector2 origin, Color overlay, float rotation)
        {
            if (destRect.Width < 1 || destRect.Height < 1) return;

            Vector2 pos = new Vector2(destRect.X, destRect.Y);
            int xi = (int)(srcRect.Width * scale.X);
            int yi = (int)(srcRect.Height * scale.Y);
            int xpx = (int)(destRect.Width + (scale.X - 1));
            int ypx = (int)(destRect.Height + (scale.Y - 1));
            int axpx = xpx - xi;
            int aypx = ypx - yi;
            int sxleft = xpx % xi;
            int syleft = ypx % yi;
            float xos = ((((xpx - xi) / xi)) * (xi)) + xi;
            float yos = ((((ypx - yi) / yi)) * (yi)) + yi;
            bool cornerCap = false;
            Rectangle src = new Rectangle(srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);

            // string dbstr = "Position: " + pos + "\nInterval: " + xi + ", " + yi + "\nSize (Pixels): " + xpx + ", " + ypx + "\nScaled Remainder: " + sxleft + ", " + syleft;

            if (xpx < xi || ypx < yi)
            {
                if (xpx <= xi && ypx >= yi)
                {
                    for (float y = 0; y <= aypx; y += yi)
                    {
                        src = new Rectangle(srcRect.X, srcRect.Y, (int)(sxleft / scale.X), srcRect.Height);
                        Graphics.Draw(texture,
                            pos + new Vector2(0, y),
                            scale,
                            origin,
                            src,
                            overlay,
                            rotation,
                            SpriteEffects.None,
                            0);
                    }
                    src = new Rectangle(srcRect.X, srcRect.Y, (int)(sxleft / scale.X), (int)(syleft / scale.Y));
                    Graphics.Draw(texture,
                        pos + new Vector2(0, yos),
                        scale,
                        origin,
                        src,
                        overlay,
                        rotation,
                        SpriteEffects.None,
                        0);
                }
                else if (ypx <= yi && xpx >= xi)
                {
                    for (float x = 0; x <= axpx; x += xi)
                    {
                        src = new Rectangle(srcRect.X, srcRect.Y, srcRect.Width, (int)(syleft / scale.Y));
                        Graphics.Draw(texture,
                            pos + new Vector2(x, 0),
                            scale,
                            origin,
                            src,
                            overlay,
                            rotation,
                            SpriteEffects.None,
                            0);
                    }
                    src = new Rectangle(srcRect.X, srcRect.Y, (int)(sxleft / scale.X), (int)(syleft / scale.Y));
                    Graphics.Draw(texture,
                        pos + new Vector2(xos, 0),
                        scale,
                        origin,
                        src,
                        overlay,
                        rotation,
                        SpriteEffects.None,
                        0);
                }
                else
                {
                    src = new Rectangle(srcRect.X, srcRect.Y, (int)(sxleft / scale.X), (int)(syleft / scale.Y));
                    Graphics.Draw(texture,
                        pos,
                        scale,
                        origin,
                        src,
                        overlay,
                        rotation,
                        SpriteEffects.None,
                        0);
                }
                return;
            }

            for (float x = 0; x <= axpx; x += xi)
            {
                for (float y = 0; y <= aypx; y += yi)
                {
                    Graphics.Draw(texture,
                        pos + new Vector2(x, y),
                        scale,
                        origin,
                        src,
                        overlay,
                        rotation,
                        SpriteEffects.None,
                        0);
                }
            }
            
            src = new Rectangle(srcRect.X, srcRect.Y, (int)(sxleft / scale.X), srcRect.Height);
            for (float y = 0; y <= aypx; y += yi)
            {
                cornerCap = true;
                Graphics.Draw(texture,
                    pos + new Vector2(xos, y),
                    scale,
                    origin,
                    src,
                    overlay,
                    rotation,
                    SpriteEffects.None,
                    0);
            }

            src = new Rectangle(srcRect.X, srcRect.Y, srcRect.Width, (int)(syleft / scale.Y));
            for (float x = 0; x <= axpx; x += xi)
            {
                cornerCap = true;
                Graphics.Draw(texture,
                    pos + new Vector2(x, yos),
                    scale,
                    origin,
                    src,
                    overlay,
                    rotation,
                    SpriteEffects.None,
                    0);
            }

            if (cornerCap)
            {
                src = new Rectangle(srcRect.X, srcRect.Y, (int)(sxleft / scale.X), (int)(syleft / scale.Y));
                Graphics.Draw(texture,
                    pos + new Vector2(xos, yos),
                    scale,
                    origin,
                    src,
                    overlay,
                    rotation,
                    SpriteEffects.None,
                    0);
            }
        }

        public static void DrawString(string str)
        {
            if (defaultFontSet)
                DrawString(DefaultFont, str, Vector2.Zero, Vector2.One, Color.White);
        }

        public static void DrawString(SpriteFont font, string str)
        {
            DrawString(font, str, Vector2.Zero, Vector2.One, Color.White);
        }

        public static void DrawString(string str, Color color)
        {
            if (defaultFontSet)
                DrawString(DefaultFont, str, Vector2.Zero, Vector2.One, color);
        }

        public static void DrawString(string str, Vector2 position)
        {
            if (defaultFontSet)
                DrawString(DefaultFont, str, position, Vector2.One, Color.White);
        }

        public static void DrawString(SpriteFont font, string str, Color color)
        {
            DrawString(font, str, Vector2.Zero, Vector2.One, color);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position)
        {
            DrawString(font, str, position, Vector2.One, Color.White);
        }

        public static void DrawString(string str, Vector2 position, Color color)
        {
            if (defaultFontSet)
                DrawString(DefaultFont, str, position, Vector2.One, color);
        }

        public static void DrawString(string str, Vector2 position, Vector2 scale)
        {
            if (defaultFontSet)
                SpriteBatch.DrawString(DefaultFont, str, position, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position, Color color)
        {
            SpriteBatch.DrawString(font, str, position, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position, Color color, Vector2 scale)
        {
            SpriteBatch.DrawString(font, str, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position, Color color, Vector2 scale, Vector2 origin)
        {
            SpriteBatch.DrawString(font, str, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position, Vector2 scale, Color color)
        {
            SpriteBatch.DrawString(font, str, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position, TextAlign align, Color color)
        {
            DrawString(font, str, GetAlignedPosition(font, str, position, align), Vector2.One, color);
        }

        public static void DrawString(SpriteFont font, string str, Rectangle bounds, TextAlign align, Color color)
        {
            DrawString(font, str, GetAlignedPosition(font, str, bounds, align), Vector2.One, color);
        }

        public static void DrawString(SpriteFont font, string str, Rectangle bounds, TextAlign align, Color color, Vector2 scale)
        {
            DrawString(font, str, GetAlignedPosition(font, str, bounds, align), scale, color);
        }

        public static void DrawString(SpriteFont font, string str, Vector2 position, Vector2 size, TextAlign align, Color color, Vector2 scale)
        {
            DrawString(font, str, GetAlignedPosition(font, str, Utilities.VectorsToRectangle(position, size), align), scale, color);
        }

        public static void DrawStringWithOutline(SpriteFont font, string str, Vector2 position, Color color, Color backColor, float offset)
        {
            DrawString(font, str, position + new Vector2(offset, offset), backColor);
            DrawString(font, str, position + new Vector2(0, offset), backColor);
            DrawString(font, str, position + new Vector2(-offset, offset), backColor);
            DrawString(font, str, position + new Vector2(-offset, 0), backColor);
            DrawString(font, str, position + new Vector2(-offset, -offset), backColor);
            DrawString(font, str, position + new Vector2(0, -offset), backColor);
            DrawString(font, str, position + new Vector2(offset, -offset), backColor);
            DrawString(font, str, position + new Vector2(offset, 0), backColor);
            DrawString(font, str, position, color);
        }

        public static void DrawStringWithOutline(SpriteFont font, string str, Vector2 position, Color color, Color backColor, float offset, Vector2 scale)
        {
            DrawString(font, str, position + new Vector2(offset, offset), backColor, scale);
            DrawString(font, str, position + new Vector2(0, offset), backColor, scale);
            DrawString(font, str, position + new Vector2(-offset, offset), backColor, scale);
            DrawString(font, str, position + new Vector2(-offset, 0), backColor, scale);
            DrawString(font, str, position + new Vector2(-offset, -offset), backColor, scale);
            DrawString(font, str, position + new Vector2(0, -offset), backColor, scale);
            DrawString(font, str, position + new Vector2(offset, -offset), backColor, scale);
            DrawString(font, str, position + new Vector2(offset, 0), backColor, scale);
            DrawString(font, str, position, color, scale);
        }

        public static void DrawStringWithOutline(SpriteFont font, string str, Vector2 position, Color color, Color backColor, float offset, Vector2 scale, Vector2 origin)
        {
            DrawString(font, str, position + new Vector2(offset, offset), backColor, scale, origin);
            DrawString(font, str, position + new Vector2(0, offset), backColor, scale, origin);
            DrawString(font, str, position + new Vector2(-offset, offset), backColor, scale, origin);
            DrawString(font, str, position + new Vector2(-offset, 0), backColor, scale, origin);
            DrawString(font, str, position + new Vector2(-offset, -offset), backColor, scale, origin);
            DrawString(font, str, position + new Vector2(0, -offset), backColor, scale, origin);
            DrawString(font, str, position + new Vector2(offset, -offset), backColor, scale, origin);
            DrawString(font, str, position + new Vector2(offset, 0), backColor, scale, origin);
            DrawString(font, str, position, color, scale, origin);
        }

        public static void DrawStringWithShadow(SpriteFont font, string str, Vector2 position, Color color, Color backColor, Vector2 offset)
        {
            DrawString(font, str, position + offset, backColor);
            DrawString(font, str, position, color);
        }

        public static void DrawStringWithShadow(SpriteFont font, string str, Vector2 position, Color color, Color backColor, Vector2 offset, Vector2 scale)
        {
            DrawString(font, str, position + offset, backColor, scale);
            DrawString(font, str, position, color, scale);
        }

        public static Vector2 GetAlignedPosition(SpriteFont font, string str, Vector2 center, TextAlign align)
        {
            Vector2 drawSize = font.MeasureString(str);
            float x = center.X, y = center.Y;
            if ((int)align <= 3)
            {
                x = center.X - drawSize.X;
            }
            else if ((int)align <= 6)
            {
                x = center.X - (drawSize.X / 2);
            }
            else if ((int)align <= 9)
            {
                x = center.X;
            }
            if ((int)align % 3 == 0)
            {
                y = center.Y;
            }
            else if ((int)align % 3 == 1)
            {
                y = center.Y - drawSize.Y;
            }
            else if ((int)align % 3 == 2)
            {
                y = center.Y - (drawSize.Y / 2);
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

        public static Vector2 GetAlignedPosition(SpriteFont font, string str, Rectangle bounds, TextAlign align, Vector2 scale)
        {
            Vector2 drawSize = font.MeasureString(str) * scale;
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

        public static Vector2 GetAlignedPosition(Vector2 position, Vector2 size, Vector2 center, TextAlign align)
        {
            float x = center.X, y = center.Y;
            if ((int)align <= 3)
            {
                x = center.X - size.X;
            }
            else if ((int)align <= 6)
            {
                x = center.X - (size.X / 2);
            }
            else if ((int)align <= 9)
            {
                x = center.X;
            }
            if ((int)align % 3 == 0)
            {
                y = center.Y;
            }
            else if ((int)align % 3 == 1)
            {
                y = center.Y - size.Y;
            }
            else if ((int)align % 3 == 2)
            {
                y = center.Y - (size.Y / 2);
            }
            return new Vector2(x, y) + position;
        }

        public static Vector2 GetAlignedPosition(Vector2 position, Vector2 size, Rectangle bounds, TextAlign align)
        {
            /*

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
             
             */

            float x = bounds.X, y = bounds.Y;
            if ((int)align <= 3)
            {
                x = bounds.X;
            }
            else if ((int)align <= 6)
            {
                x = bounds.X + ((bounds.Width / 2) - (size.X / 2));
            }
            else if ((int)align <= 9)
            {
                x = bounds.X + bounds.Width - size.X;
            }
            if ((int)align % 3 == 0)
            {
                y = bounds.Y + bounds.Height - size.Y;
            }
            else if ((int)align % 3 == 1)
            {
                y = bounds.Y;
            }
            else if ((int)align % 3 == 2)
            {
                y = bounds.Y + ((bounds.Height / 2) - (size.Y / 2));
            }
            return new Vector2(x, y) + position;
        }

        public static void DrawLine(Vector2 p1, Vector2 p2, Color c, int layer = 0)
        {
            Vector2 diff = p1 - p2;
            float a = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
            float l = diff.Length();
            Vector2 pos = p1;
            Vector2 scale = new Vector2(l, 1);
            SpriteBatch.Draw(WhitePixel, pos, null, c, a, Vector2.Zero, scale, SpriteEffects.None, layer);
        }

        public static void DrawRectangle(Vector2 pos, Vector2 size, Color c, int layer = 0)
        {
            SpriteBatch.Draw(WhitePixel, pos, null, c, 0, Vector2.Zero, size, SpriteEffects.None, layer);
        }

        public static void DrawRectangleOutline(Vector2 pos, Vector2 size, Color c, int layer = 0)
        {
            DrawLine(new Vector2(pos.X, pos.Y), new Vector2(pos.X + size.X - 1, pos.Y), c, layer);
            DrawLine(new Vector2(pos.X + size.X, pos.Y), new Vector2(pos.X + size.X, pos.Y + size.Y - 1), c, layer);
            DrawLine(new Vector2(pos.X + size.X, pos.Y + size.Y), new Vector2(pos.X, pos.Y + size.Y), c, layer);
            DrawLine(new Vector2(pos.X, pos.Y + size.Y - 1), new Vector2(pos.X, pos.Y + 1), c, layer);
        }

        public static void DrawRectangle(Vector2Rectangle r, Color c, int layer = 0)
        {
            SpriteBatch.Draw(WhitePixel, r.Position, null, c, 0, Vector2.Zero, r.Size, SpriteEffects.None, layer);
        }

        public static void DrawRectangleOutline(Vector2Rectangle r, Color c, int layer = 0)
        {
            DrawLine(new Vector2(r.Position.X, r.Position.Y), new Vector2(r.Position.X + r.Size.X - 1, r.Position.Y), c, layer);
            DrawLine(new Vector2(r.Position.X + r.Size.X, r.Position.Y), new Vector2(r.Position.X + r.Size.X, r.Position.Y + r.Size.Y - 1), c, layer);
            DrawLine(new Vector2(r.Position.X + r.Size.X, r.Position.Y + r.Size.Y), new Vector2(r.Position.X, r.Position.Y + r.Size.Y), c, layer);
            DrawLine(new Vector2(r.Position.X, r.Position.Y + r.Size.Y - 1), new Vector2(r.Position.X, r.Position.Y + 1), c, layer);
        }

        public static void Clear()
        {
            GraphicsDevice.Clear(ClearColor);
        }

        public static void Begin(Effect e = null, SpriteSortMode ssm = SpriteSortMode.Deferred) { Begin(SamplerState.PointClamp, Vector2.One, e, ssm); }
        public static void Begin(Vector2 scale, Effect e = null, SpriteSortMode ssm = SpriteSortMode.Deferred) { Begin(SamplerState.PointClamp, scale, e, ssm); }
        public static void Begin(SamplerState ss, Vector2 scale, Effect e = null, SpriteSortMode ssm = SpriteSortMode.Deferred)
        {
            SpriteBatch.Begin(ssm, BlendState.AlphaBlend, ss,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, e,
                Matrix.CreateScale(new Vector3(scale.X, scale.Y, 1)));

            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        }

        public static void End()
        {
            SpriteBatch.End();
        }

        public static void SetResolution(Vector2 resolution)
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = (int)resolution.X;
            GraphicsDeviceManager.PreferredBackBufferHeight = (int)resolution.Y;

            GraphicsDeviceManager.ApplyChanges();
        }

        public static void SetResolutionAndScale() { SetResolutionAndScale(Resolution); }
        public static void SetResolutionAndScale(Vector2 resolution)
        {
            SetResolution(resolution);
            float resRatio = resolution.X / resolution.Y;
            if (BaseResolutionHeight != 0) CurrentCamera.Scale = resolution / new Vector2(resRatio * BaseResolutionHeight, BaseResolutionHeight);
            else CurrentCamera.Scale = Vector2.One;
        }

        public static void ToggleFullscreen()
        {
            if (CanFullscreen && !GraphicsDeviceManager.IsFullScreen) GoFullscreen();
            else if (GraphicsDeviceManager.IsFullScreen) GoWindowed();
        }

        public static void GoWindowed()
        {
            if (GraphicsDeviceManager.IsFullScreen) GraphicsDeviceManager.ToggleFullScreen();
            SetResolutionAndScale(WindowedResolution);

            GraphicsDeviceManager.ApplyChanges();
        }

        public static void GoFullscreen()
        {
            if (!CanFullscreen) return;
            Vector2 nRes = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            SetResolutionAndScale(nRes);
            if (!GraphicsDeviceManager.IsFullScreen) GraphicsDeviceManager.ToggleFullScreen();

            GraphicsDeviceManager.ApplyChanges();
        }
    }

    public enum TextAlign
    {
        TopLeft = 1,
        MiddleLeft = 2,
        BottomLeft = 3,
        TopCenter = 4,
        MiddleCenter = 5,
        BottomCenter = 6,
        TopRight = 7,
        MiddleRight = 8,
        BottomRight = 9
    }
}
