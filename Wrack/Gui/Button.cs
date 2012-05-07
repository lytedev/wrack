using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WrackEngine;

namespace WrackEngine.Gui
{
    public class Button : AdvancedElement
    {
        public string ExecutionScript { get; set; }
        public Background Background { get; set; }
        public Border Border { get; set; }
        public Text Content { get; set; }

        public Button(Element parent) : this(parent, "default") { }
        public Button(Element parent, string textureName)
            : base(textureName)
        {
            ExecutionScript = "";
            Size = new Vector2(100, 30);
            DrawMe = false;

            Background = new Background(this);
            Border = new Border(this);
            Content = new Text(this);

            //Background.Overlay = new Color(40, 40, 40, 255);
            //Border.Overlay = new Color(80, 80, 80, 255);
            
            Content.Content = "Button";

            parent.AddChild(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (AcceptsMouseSelect && Selected && (Wrack.Input.Pressed(Input.LeftMouse)))
            {
                Wrack.ScriptEngine.Interpreter.SetParameter("sender", this);
                Wrack.ScriptEngine.Interpreter.Run(ExecutionScript);
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        new public virtual Button DeepClone()
        {
        /*public string ExecutionScript { get; set; }
        public Background Background { get; set; }
        public Border Border { get; set; }
        public Text Content { get; set; }*/
            Button s = new Button(Parent);
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
            s.DrawMe = DrawMe;
            s.Selected = Selected;
            s.Disabled = Disabled;
            s.Alignment = Alignment;
            s.AlignmentBounds = AlignmentBounds;
            s.AcceptsMouseSelect = AcceptsMouseSelect;
            s.ExecutionScript = ExecutionScript;
            s.Children = new List<Element>();
            for (int i = 0; i < Children.Count; i++)
            {
                Element e = Children[i];
                if (e == Border || e == Background || e == Content) continue;
                s.Children.Add(e.DeepClone());
            }
            s.Border = Border.DeepClone();
            s.Background = Background.DeepClone();
            s.Content = Content.DeepClone();
            s.AddChild(s.Background);
            s.AddChild(s.Border);
            s.AddChild(s.Content);
            return s;
        }
    }
}
