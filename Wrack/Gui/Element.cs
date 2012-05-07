using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace WrackEngine.Gui
{
    public class Element : Sprite
    {
        public Element Parent { get; set; }
        public List<Element> Children { get; set; }
        public bool DrawMe { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }

        public Element() : this ("default") { }
        public Element(string textureName)
            : base(textureName)
        {
            IgnoreCamera = true;
            Children = new List<Element>();
            DrawMe = true;
            Selected = false;
            Disabled = false;
        }

        public virtual Vector2 GetDrawPosition()
        {
            Vector2 pos = Position;
            if (Parent != null) pos += Parent.GetDrawPosition();
            return pos;
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Draw(gameTime);
            }

            if (DrawMe) base.Draw(GetDrawPosition(), Scale * Parent.Scale, Origin, Overlay, 0, Effects, 0);
        }

        public override void Update(GameTime gameTime)
        {
            // if (Utilities.PointIsInRect(Wrack.Input.MousePosition, 

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        public void AddChild(Element e)
        {
            if (!Children.Contains(e)) Children.Add(e);
            if (e.Parent != null)
                if (e.Parent.Children.Contains(e))
                    e.Parent.Children.Remove(e);
            e.Parent = this;
        }

        new public virtual Element DeepClone()
        {
            Element s = new Element();
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
            return s;
        }
    }
}
