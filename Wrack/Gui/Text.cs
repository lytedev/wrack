using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrackEngine.Gui
{
    public class Text : Element
    {
        public SpriteFont Font { get; set; }
        public string Content { get; set; }
        public Color ForeColor { get; set; }
        public Color SelectedForeColor { get; set; }
        public Color ShadowColor { get; set; }
        public Color OutlineColor { get; set; }
        public bool Outline { get; set; }
        public bool Shadow { get; set; }
        public float OutlineOffset { get; set; }
        public float ShadowOffset { get; set; }
        public TextAlign TextAlignment { get; set; }

        public Text(Element parent) : this (parent, "default") { }
        public Text(Element parent, string textureName)
            : base(textureName)
        {
            Font = Graphics.DefaultFont;
            Content = "Text";
            SelectedForeColor = Color.White;
            ForeColor = Color.LightGray;
            ShadowColor = Color.Black;
            OutlineColor = Color.Black;
            Outline = true;
            Shadow = false;
            OutlineOffset = 1;
            ShadowOffset = 1;
            TextAlignment = TextAlign.MiddleCenter;
            parent.AddChild(this);
        }

        public override Vector2 GetDrawPosition()
        {
            Rectangle ab = new Rectangle();
            if (Parent != null)
            {
                Vector2 ppos = Parent.GetDrawPosition();
                ab.X = (int)ppos.X;
                ab.Y = (int)ppos.Y;
                ab.Width = (int)(Parent.Size.X * Parent.Scale.X);
                ab.Height = (int)(Parent.Size.Y * Parent.Scale.Y);
            }
            else
            {
                ab = Graphics.Window.ClientBounds;
                ab.X = 0;
                ab.Y = 0;
            }
            Vector2 pos = Graphics.GetAlignedPosition(Font, Content, ab, TextAlignment, Scale) + Position;
            // if (Parent != null) pos += Parent.GetDrawPosition();
            return pos;
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Draw(gameTime);
            }

            if (!DrawMe) return;

            Color foreColor = ForeColor;
            if (Selected) foreColor = SelectedForeColor;
            if (Parent != null) if (Parent.Selected) foreColor = SelectedForeColor;

            if (Outline) Graphics.DrawStringWithOutline(Font, Content, Utilities.FloorVector(GetDrawPosition()), foreColor, OutlineColor, OutlineOffset, Scale);
            else if (Shadow) Graphics.DrawStringWithOutline(Font, Content, Utilities.FloorVector(GetDrawPosition()), foreColor, ShadowColor, ShadowOffset, Scale);
            else Graphics.DrawString(Font, Content, new Vector2Rectangle(Position, Size).GetRect(), TextAlignment, foreColor, Scale);
        }

        new public virtual Text DeepClone()
        {
            Text s = new Text(Parent);
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
            s.Font = Font;
            s.Content = Content;
            s.ForeColor = ForeColor;
            s.SelectedForeColor = SelectedForeColor;
            s.ShadowColor = ShadowColor;
            s.OutlineColor = OutlineColor;
            s.Outline = Outline;
            s.OutlineOffset = OutlineOffset;
            s.ShadowOffset = ShadowOffset;
            s.TextAlignment = TextAlignment;
            return s;
        }
    }
}
