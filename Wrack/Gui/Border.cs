using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WrackEngine.Gui
{
    public class Border : Element
    {
        public bool Pattern { get; set; }
        public int Frame { get; set; }
        public Vector2 Padding { get; set; }

        public Border(Element parent) : this (parent, "defaulth3x3") { }
        public Border(Element parent, string textureName)
            : base(textureName)
        {
            Pattern = true; // If not pattern, scale
            Frame = 0;
            Padding = Vector2.Zero;
            parent.AddChild(this);
            // TODO: Integrate Padding
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

            float diff = Size.X / 3;
            float sxdiff = diff * Scale.X;
            float sydiff = diff * Scale.Y;
            int frameOffset = (int)(Frame * (diff * 3));

            Vector2 sdiff = new Vector2(sxdiff, sydiff);
            Vector2 scale = Parent.Scale * Scale;

            Vector2 pos = Utilities.FloorVector(GetDrawPosition());
            Vector2 es = Parent.Size * Parent.Scale;

            int xi = (int)(sxdiff);
            int yi = (int)(sydiff);

            Vector2 midSize = new Vector2((int)(es.X - (sxdiff * 2)), (int)(es.Y - (sydiff * 2)));
            Vector2 usmidSize = new Vector2((int)(es.X - (diff * 2)), (int)(es.Y - (diff * 2)));
            int xpx = (int)(midSize.X);
            int ypx = (int)(midSize.Y);
            int usxpx = (int)(usmidSize.X);
            int usypx = (int)(usmidSize.Y);

            Vector2Rectangle te = new Vector2Rectangle(diff, frameOffset, diff, diff);
            Vector2Rectangle le = new Vector2Rectangle(0, diff + frameOffset, diff, diff);
            Vector2Rectangle be = new Vector2Rectangle(diff, (diff * 2) + frameOffset, diff, diff);
            Vector2Rectangle re = new Vector2Rectangle((diff * 2), diff + frameOffset, diff, diff);
            Vector2Rectangle mid = new Vector2Rectangle(diff, diff + frameOffset, diff, diff);

            if (!Pattern)
            {
                float btsc = (Parent.Size.X - (diff * 2)) / (diff) / Scale.X;
                Vector2 btscale = new Vector2(btsc, 1);

                float lrsc = (Parent.Size.Y - (diff * 2)) / (diff) / Scale.Y;
                Vector2 lrscale = new Vector2(1, lrsc);

                // Scaled
                Graphics.Draw(Texture,
                    pos + new Vector2(diff, diff),
                    Scale * (lrscale * btscale),
                    Parent.Origin,
                    mid.GetRect(),
                    Overlay,
                    Parent.Rotation,
                    Effects,
                    0);

                Graphics.Draw(Texture,
                    pos + te.Position,
                    Scale * btscale,
                    Parent.Origin,
                    te.GetRect(),
                    Overlay,
                    Parent.Rotation,
                    Effects,
                    0);

                // TODO: Fix
                Graphics.Draw(Texture,
                    pos + (es * Vector2.UnitY) + (new Vector2((diff / 2), -diff) * Scale),
                    Scale * btscale,
                    Parent.Origin,
                    be.GetRect(),
                    Color.Red,
                    Parent.Rotation,
                    Effects,
                    0);

                Graphics.Draw(Texture,
                    pos + le.Position,
                    Scale * lrscale,
                    Parent.Origin,
                    le.GetRect(),
                    Overlay,
                    Parent.Rotation,
                    Effects,
                    0);

                // TODO: Fix
                Graphics.Draw(Texture,
                    pos + (es * Vector2.UnitX) + (new Vector2(-diff, (diff / 2)) * Scale),
                    Scale * lrscale,
                    Parent.Origin,
                    re.GetRect(),
                    Color.Red,
                    Parent.Rotation,
                    Effects,
                    0);
            }
            else
            {
                Vector2 os = new Vector2(sxdiff, sydiff);
                Vector2Rectangle dest = new Vector2Rectangle(GetDrawPosition() + os, (Parent.Size) - (os * 2));
                Graphics.TileTexture(Texture,
                    mid.GetRect(),
                    dest.GetRect(),
                    Scale,
                    Parent.Origin,
                    Overlay,
                    Parent.Rotation);

                os = new Vector2(0, sydiff);
                dest = new Vector2Rectangle(GetDrawPosition() + os, (Parent.Size) - (os * 2));
                dest.Size.X = sxdiff;
                Graphics.TileTexture(Texture,
                    le.GetRect(),
                    dest.GetRect(),
                    Scale,
                    Parent.Origin,
                    Overlay,
                    Parent.Rotation);

                os = new Vector2(Parent.Size.X - sxdiff, sydiff);
                dest = new Vector2Rectangle(GetDrawPosition() + os, (Parent.Size) - (os * 2));
                dest.Size.X = sxdiff;
                Graphics.TileTexture(Texture,
                    re.GetRect(),
                    dest.GetRect(),
                    Scale,
                    Parent.Origin,
                    Overlay,
                    Parent.Rotation);

                os = new Vector2(sxdiff, 0);
                dest = new Vector2Rectangle(GetDrawPosition() + os, (Parent.Size) - (os * 2));
                dest.Size.Y = sydiff;
                Graphics.TileTexture(Texture,
                    te.GetRect(),
                    dest.GetRect(),
                    Scale,
                    Parent.Origin,
                    Overlay,
                    Parent.Rotation);

                os = new Vector2(sxdiff, Parent.Size.Y - sydiff);
                dest = new Vector2Rectangle(GetDrawPosition() + os, (Parent.Size) - (os * 2));
                dest.Size.Y = sydiff;
                Graphics.TileTexture(Texture,
                    be.GetRect(),
                    dest.GetRect(),
                    Scale,
                    Parent.Origin,
                    Overlay,
                    Parent.Rotation);
            }

            Vector2Rectangle tlc = new Vector2Rectangle(0, frameOffset, diff, diff);
            Vector2Rectangle trc = new Vector2Rectangle((diff * 2), frameOffset, diff, diff);
            Vector2Rectangle blc = new Vector2Rectangle(0, (diff * 2) + frameOffset, diff, diff);
            Vector2Rectangle brc = new Vector2Rectangle((diff * 2), (diff * 2) + frameOffset, diff, diff);

            Graphics.Draw(Texture,
                pos,
                Scale,
                Parent.Origin,
                tlc.GetRect(),
                Overlay,
                Parent.Rotation,
                Effects,
                0);

            Graphics.Draw(Texture,
                pos + (es * Vector2.UnitX) - (new Vector2(diff, 0) * Scale),
                Scale,
                Parent.Origin,
                trc.GetRect(),
                Overlay,
                Parent.Rotation,
                Effects,
                0);

            Graphics.Draw(Texture,
                pos + (es * Vector2.UnitY) - (new Vector2(0, diff) * Scale),
                Scale,
                Parent.Origin,
                blc.GetRect(),
                Overlay,
                Parent.Rotation,
                Effects,
                0);

            Graphics.Draw(Texture,
                pos + es - (new Vector2(diff, diff) * Scale),
                Scale,
                Parent.Origin,
                brc.GetRect(),
                Overlay,
                Parent.Rotation,
                Effects,
                0);

            Vector2Rectangle vrect = new Vector2Rectangle(pos, es);
            /*Rectangle rect = vrect.GetRect();
            SpriteFont font = Graphics.DefaultFont;
            string str = "HEY!";
            Vector2 tpos = Graphics.GetAlignedPosition(font, str, rect, TextAlign.MiddleCenter);
            Graphics.DrawStringWithOutline(Graphics.DefaultFont, "HEY!", tpos, Color.White, Color.Black, 1);*/

            // Graphics.DrawRectangleOutline(vrect, Color.Red);
        }

        new public virtual Border DeepClone()
        {
            Border s = new Border(Parent);
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
            s.Pattern = Pattern;
            s.Frame = Frame;
            s.Padding = Padding;
            return s;
        }
    }
}
