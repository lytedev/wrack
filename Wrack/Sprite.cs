using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrackEngine
{
    public class Sprite : AnimatedTexture2D
    {
        public bool IgnoreCamera { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }
        public Color Overlay { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Rotation { get; set; }

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

        public Sprite() : this("default") { }
        public Sprite(string textureName)
            : base(textureName)
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Origin = Vector2.Zero;
            Overlay = Color.White;
            Effects = SpriteEffects.None;
            Rotation = 0;
            TextureName = textureName;
            Size = new Vector2(Texture.Width, Texture.Height);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 pos = Position;
            Vector2 scale = Scale;
            if (!IgnoreCamera) pos -= Graphics.CurrentCamera.Position;
            if (!IgnoreCamera) scale *= Graphics.CurrentCamera.Scale;

            UpdateAnimation(gameTime);
            base.Draw(pos, scale, Origin, Overlay, 0, Effects, 0);
        }

        new public virtual Sprite DeepClone()
        {
            Sprite s = new Sprite();
            s.Position = Position;
            s.Size = Size;
            s.TextureName = TextureName;
            s.Scale = Scale;
            s.Rotation = Rotation;
            s.Origin = Origin;
            s.Overlay = Overlay;
            s.Effects = Effects;
            s.CurrentFrame = CurrentFrame;
            s.AnimationSetName = AnimationSetName;
            return s;
        }
    }
}
