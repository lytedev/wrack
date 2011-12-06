using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wrack
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }
        public Color Overlay { get; set; }
        public SpriteEffects Effects { get; set; }
        public Dictionary<string, Animation> Animations { get; set; }
        public Animation CurrentAnimation { get; set; }
        public int CurrentFrame { get; set; }
        public int CurrentFrameTime { get; set; }
        public int LayerDepth { get; set; }
        public float Rotation { get; set; }
        public bool CameraBased { get; set; }

        public byte Opacity
        {
            get
            {
                return Overlay.A;
            }
            set
            {
                Overlay = new Color(Overlay.R, Overlay.G, Overlay.B, value);
            }
        }

        public virtual void Default()
        {
            Animations = new Dictionary<string, Animation>();
            Position = Vector2.Zero;
            Size = Vector2.One;
            Texture = Graphics.WhitePixel;
            Scale = Vector2.One;
            Origin = Vector2.Zero;
            Overlay = Color.White;
            Effects = SpriteEffects.None;
            Animations.Add("default", new Animation());
            CurrentAnimation = Animations["default"];
            Rotation = 0;
            LayerDepth = 0;
            CameraBased = true;
        }

        public Sprite()
        {
            Default();
        }

        public Sprite(Texture2D t)
        {
            Default();
            Texture = t;
            Size = new Vector2(t.Width, t.Height);
        }

        public Rectangle GetSourceRect()
        {
            int sheetWidth = Texture.Width / (int)Size.X;
            int sheetHeight = Texture.Height / (int)Size.Y;
            int y = (CurrentFrame / sheetWidth) * (int)Size.Y;
            int x = (CurrentFrame % sheetWidth) * (int)Size.X;
            return new Rectangle(x, y, (int)Size.X, (int)Size.Y);
        }

        public virtual void Draw()
        {
            Vector2 pos;
            Vector2 scale;

            pos = Position;
            scale = Scale;

            if (CameraBased)
            {
                pos /= Core.MetersPerPixel;
                pos *= Graphics.CurrentCamera.Scale;
                pos -= Graphics.CurrentCamera.Position;
                scale *= Graphics.CurrentCamera.Scale;
            }

            // Vector2 v = Size * scale;
            // Rectangle bounds = new Rectangle((int)(pos.X - ((Origin.X * Core.MetersPerPixel / Scale.X) * v.X)), (int)(pos.Y - ((Origin.Y * Core.MetersPerPixel / Scale.Y) * v.Y)), (int)v.X, (int)v.Y);
            // Rectangle bounds = new Rectangle((int)(pos.X), (int)(pos.Y), (int)v.X, (int)v.Y);
            // Rectangle window = new Rectangle(0, 0, (int)Graphics.Resolution.X + (int)Size.X, (int)Graphics.Resolution.Y + (int)Size.Y);
            // TODO: CLIPPING SUCKS
            if (/*bounds.Intersects(window)*/true)
            {
                Graphics.Draw(Texture,
                    pos,
                    scale,
                    Origin,
                    GetSourceRect(),
                    Overlay,
                    Rotation,
                    Effects,
                    LayerDepth);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            CurrentFrameTime += gameTime.ElapsedGameTime.Milliseconds;
            if (CurrentFrameTime >= CurrentAnimation.FrameTime)
            {
                if (CurrentAnimation.IsSwinging)
                {
                    CurrentFrame--;
                }
                else
                {
                    CurrentFrame++;
                }
                CurrentFrameTime = 0;

                if (CurrentFrame <= CurrentAnimation.StartingFrame)
                {
                    CurrentAnimation.IsSwinging = false;
                    CurrentFrame = CurrentAnimation.StartingFrame + 1;
                }
                else if (CurrentFrame > CurrentAnimation.Frames + CurrentAnimation.StartingFrame)
                {
                    if (CurrentAnimation.Swings)
                    {
                        CurrentAnimation.IsSwinging = true;
                    }
                    this.CurrentFrame = CurrentAnimation.StartingFrame + CurrentAnimation.Frames - 2;
                }
                if (CurrentFrame <= CurrentAnimation.StartingFrame)
                {
                    CurrentFrame = CurrentAnimation.StartingFrame;
                }
                else if (CurrentFrame > CurrentAnimation.StartingFrame + CurrentAnimation.Frames - 1)
                {
                    CurrentFrame = CurrentAnimation.StartingFrame + CurrentAnimation.Frames - 1;
                }
            }
        }

        public virtual Sprite DeepClone()
        {
            Sprite s = new Sprite();
            s.Position = Position;
            s.Size = Size;
            s.Texture = Texture;
            s.Scale = Scale;
            s.Rotation = Rotation;
            s.Origin = Origin;
            s.Overlay = Overlay;
            s.Effects = Effects;
            s.CurrentFrame = CurrentFrame;
            s.Animations.Clear();
            foreach (KeyValuePair<string, Animation> kvp in Animations)
            {
                s.Animations.Add(kvp.Key, kvp.Value.DeepClone());
                if (CurrentAnimation == kvp.Value)
                {
                    s.CurrentAnimation = s.Animations[kvp.Key];
                }
            }
            return s;
        }
    }
}
