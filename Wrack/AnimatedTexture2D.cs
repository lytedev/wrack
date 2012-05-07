using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrackEngine
{
    [Serializable]
    public class AnimatedTexture2D
    {
        public string TextureName { get; set; }
        public Texture2D Texture { get { return Graphics.GetTexture(TextureName); } }
        public Vector2 Size { get; set; }
        public string AnimationSetName { get; set; }
        public Dictionary<string, Animation> Animations { get { return Graphics.GetAnimationSet(AnimationSetName); } }
        public Animation CurrentAnimation { get; set; }
        public int CurrentFrame { get; set; }
        public int CurrentFrameTime { get; set; }

        public AnimatedTexture2D() : this("default") { }
        public AnimatedTexture2D(string textureName)
        {
            Size = Vector2.One;
            CurrentAnimation = new Animation();
            TextureName = textureName;
            Size = new Vector2(Texture.Width, Texture.Height);
        }

        public Rectangle GetSourceRect()
        {
            if (Size.X == 0) Size = new Vector2(1, Size.Y);
            if (Size.Y == 0) Size = new Vector2(Size.X, 1);
            int sheetWidth = Texture.Width / (int)Size.X;
            int sheetHeight = Texture.Height / (int)Size.Y;
            int y = (CurrentFrame / sheetWidth) * (int)Size.Y;
            int x = (CurrentFrame % sheetWidth) * (int)Size.X;
            return new Rectangle(x, y, (int)Size.X, (int)Size.Y);
        }

        public Texture GetSubTexture(Rectangle bounds)
        {
            if (Texture == null) return null;
            Color[] c = new Color[bounds.Width * bounds.Height];
            Texture.GetData<Color>(0, bounds, c, 0, bounds.Width * bounds.Height);
            Texture2D t = new Texture2D(Graphics.GraphicsDevice, bounds.Width, bounds.Height);
            t.SetData<Color>(c);
            return t;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {
            Draw(Vector2.Zero, Vector2.One, Vector2.Zero, Color.White, 0, SpriteEffects.None, 0);
        }

        public virtual void UpdateAnimation(GameTime gameTime)
        {
            if (CurrentFrame < CurrentAnimation.StartingFrame || CurrentFrame > CurrentAnimation.Frames + CurrentAnimation.StartingFrame - 1)
            {
                CurrentFrame = CurrentAnimation.StartingFrame;
            }
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
                else if (CurrentFrame > CurrentAnimation.Frames + CurrentAnimation.StartingFrame - 1)
                {
                    if (CurrentAnimation.Swings)
                    {
                        CurrentAnimation.IsSwinging = true;
                        CurrentFrame = CurrentAnimation.StartingFrame + CurrentAnimation.Frames - 1;
                    }
                    else
                    {
                        CurrentFrame = CurrentAnimation.StartingFrame;
                    }
                }
                if (CurrentFrame <= CurrentAnimation.StartingFrame)
                {
                    CurrentFrame = CurrentAnimation.StartingFrame;
                }
            }
        }

        public virtual void Draw(Vector2 pos)
        {
            Draw(pos, Vector2.One, Vector2.Zero, Color.White, 0, SpriteEffects.None, 0);
        }

        public virtual void Draw(Vector2 pos, Vector2 scale)
        {
            Draw(pos, scale, Vector2.Zero, Color.White, 0, SpriteEffects.None, 0);
        }

        public virtual void Draw(Vector2 pos, Vector2 scale, Vector2 origin)
        {
            Draw(pos, scale, origin, Color.White, 0, SpriteEffects.None, 0);
        }

        public virtual void Draw(Vector2 pos, Vector2 scale, Vector2 origin, Color overlay)
        {
            Draw(pos, scale, origin, overlay, 0, SpriteEffects.None, 0);
        }

        public virtual void Draw(Vector2 pos, Vector2 scale, Vector2 origin, Color overlay, float rotation)
        {
            Draw(pos, scale, origin, overlay, rotation, SpriteEffects.None, 0);
        }

        public virtual void Draw(Vector2 pos, Vector2 scale, Vector2 origin, Color overlay, float rotation, SpriteEffects effects)
        {
            Draw(pos, scale, origin, overlay, rotation, effects, 0);
        }

        public virtual void Draw(Vector2 pos, Vector2 scale, Vector2 origin, Color overlay, float rotation, SpriteEffects effects, int layer)
        {
            Graphics.Draw(Texture,
                pos,
                scale,
                origin,
                GetSourceRect(),
                overlay,
                rotation,
                effects,
                layer);
        }

        public virtual AnimatedTexture2D DeepClone()
        {
            AnimatedTexture2D a = new AnimatedTexture2D();
            a.Size = Size;
            a.TextureName = TextureName;
            a.CurrentFrame = CurrentFrame;
            a.AnimationSetName = AnimationSetName;
            return a;
        }
    }
}
