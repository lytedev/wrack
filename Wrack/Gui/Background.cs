using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrackEngine.Gui
{
    public class Background : Element
    {
        public int Frame { get; set; }
        public Vector2 Padding { get; set; }

        public Background(Element parent) : this(parent, "default3x3") { Size = new Vector2(1, 3); }
        public Background(Element parent, string textureName)
            : base(textureName)
        {
            Frame = 0;
            Padding = Vector2.Zero;
            parent.AddChild(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (Parent.Selected) Frame = 1;
            else Frame = 0;
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Draw(gameTime);
            }

            if (!DrawMe && Parent != null) return;

            int ysize = (int)Size.Y;
            int frameOffset = Frame * ysize;
            Rectangle src = new Rectangle(0, frameOffset, Texture.Width, ysize);
            Vector2Rectangle dest = new Vector2Rectangle(GetDrawPosition() + Padding, (Parent.Size * Parent.Scale) - (Padding * 2));
            Graphics.TileTexture(Texture, src, dest.GetRect(), Scale, Parent.Origin, Overlay, 0);
        }

        new public virtual Background DeepClone()
        {
            Background s = new Background(Parent);
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
            s.Parent = Parent;
            s.Children = new List<Element>();
            for (int i = 0; i < Children.Count; i++)
            {
                s.Children.Add(Children[i].DeepClone());
            }
            s.DrawMe = DrawMe;
            s.Selected = Selected;
            s.Disabled = Disabled;
            s.Frame = Frame;
            s.Padding = Padding;
            return s;
        }
    }
}
