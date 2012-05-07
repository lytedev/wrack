using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WrackEngine;

namespace WrackEngine.Gui
{
    public class AdvancedElement : Element
    {
        public TextAlign Alignment { get; set; }
        public Rectangle AlignmentBounds { get; set; }
        public bool AcceptsMouseSelect { get; set; }

        public AdvancedElement() : this("default") { }
        public AdvancedElement(string textureName)
            : base(textureName)
        {
            Alignment = TextAlign.MiddleCenter;
            AlignmentBounds = new Rectangle(0, 0, 0, 0);
            AcceptsMouseSelect = true;
        }

        public override Vector2 GetDrawPosition()
        {
            Rectangle ab = AlignmentBounds;
            if (ab.X == 0 && ab.Y == 0 && ab.Width == 0 && ab.Height == 0 && Parent != null)
            {
                ab = Graphics.Window.ClientBounds;
                ab.X = 0;
                ab.Y = 0;
            }
            Vector2 scale = Scale;
            if (!IgnoreCamera) scale *= Graphics.CurrentCamera.Scale;
            Vector2 pos = Graphics.GetAlignedPosition(Vector2.Zero, Size * scale, ab, Alignment) + Position;
            if (!IgnoreCamera) pos -= Graphics.CurrentCamera.Position;
            if (Parent != null) pos += Parent.GetDrawPosition();
            return pos;
        }
        
        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Draw(gameTime);
            }

            if (!DrawMe) return;
            Rectangle ab = AlignmentBounds;
            if (ab.X == 0 && ab.Y == 0 && ab.Width == 0 && ab.Height == 0)
            {
                ab = Graphics.Window.ClientBounds;
                ab.X = 0;
                ab.Y = 0;
            }

            Vector2 scale = Scale;
            if (!IgnoreCamera) scale *= Graphics.CurrentCamera.Scale;

            UpdateAnimation(gameTime);
            base.Draw(GetDrawPosition(), scale, Origin, Overlay, 0, Effects, 0);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2Rectangle cr = new Vector2Rectangle(GetDrawPosition(), Size * Scale);
            Selected = Utilities.PointIsInRect(Wrack.Input.MousePosition, cr.GetRect());
            base.Update(gameTime);
        }

        new public virtual AdvancedElement DeepClone()
        {
            AdvancedElement s = new AdvancedElement();
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
            s.Alignment = Alignment;
            s.AlignmentBounds = AlignmentBounds;
            s.AcceptsMouseSelect = AcceptsMouseSelect;
            return s;
        }
    }
}
