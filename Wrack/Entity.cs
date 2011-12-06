using System.Collections.Generic;

using Microsoft.Xna.Framework;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;

namespace Wrack
{
    public class Entity : Body
    {
        public Sprite Sprite { get; set; }

        public virtual void Default()
        {
            Sprite = new Sprite();
        }

        public virtual void MakeStatic()
        {
            IsStatic = true;
            Awake = false;
            BodyType = BodyType.Static;
            Enabled = false;
        }

        public virtual void MakeDynamic()
        {
            Enabled = true;
            BodyType = BodyType.Dynamic;
            Awake = true;
        }

        public virtual void MakeBullet()
        {
            Enabled = true;
            BodyType = BodyType.Dynamic;
            Awake = true;
            IsBullet = true;
        }

        public Entity()
            : base(Core.World)
        {
            Default();
        }
        
        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public virtual void Draw()
        {
            Sprite.Position = Position;
            Sprite.Rotation = Rotation;
            Sprite.Draw();
        }

        public virtual void CreateFixtureFromSize()
        {
            Sprite.Origin = new Vector2(Sprite.Size.X / 2, Sprite.Size.Y / 2);

            Vector2 size = Sprite.Size * (Sprite.Scale);

            float hx = (size.X / 2f);
            float hy = (size.Y / 2f);

            Vertices v = new Vertices(new Vector2[] {
                    new Vector2(-hx, -hy),
                    new Vector2(hx, -hy),
                    new Vector2(hx, hy),
                    new Vector2(-hx, hy)
                });

            Vector2 scale = new Vector2(Core.MetersPerPixel, Core.MetersPerPixel);
            v.Scale(ref scale);
            CreateFixture(new PolygonShape(v, 1));
        }

        public virtual bool CreateFixtureFromSprite()
        {
            float hx = (Sprite.Size.X / 2f);
            float hy = (Sprite.Size.Y / 2f);

            Sprite.Origin = new Vector2(hx, hy);

            uint[] data = new uint[Sprite.Texture.Width * Sprite.Texture.Height];
            Sprite.Texture.GetData(data);
            Vertices verts = PolygonTools.CreatePolygon(data, Sprite.Texture.Width, false);
            Vector2 s = new Vector2(Core.MetersPerPixel, Core.MetersPerPixel);
            verts.Scale(ref s);
            //if (verts.CheckPolygon())
            //{
            List<Vertices> v = FarseerPhysics.Common.Decomposition.BayazitDecomposer.ConvexPartition(verts);
            List<Fixture> compound = FixtureFactory.AttachCompoundPolygon(v, 1.0f, this);
            // CreateFixture(new PolygonShape(verts, 1.0f));
            //}
            //else
            //{
            //Console.WriteLine("Invalid sprite for conversion to fixture!");
            //return false;
            //}

            // Sprite.Origin = verts.GetCentroid() * Graphics.TILE_SIZE;

            return true;
        }

        public virtual void CreateFixtureFromRadius(float radius)
        {
            float hx = (Sprite.Size.X / 2f);
            float hy = (Sprite.Size.Y / 2f);

            Sprite.Origin = new Vector2(hx, hy);

            CircleShape c = new CircleShape(radius, 1);
            c.Position += new Vector2(radius, radius);
            CreateFixture(c);
        }

        public virtual void Destroy()
        {
            try
            {
                if (!IsDisposed)
                {
                    Core.World.RemoveBody(this);
                    IsDisposed = true;
                }
            }
            catch
            {

            }
        }

        new public virtual Entity DeepClone()
        {
            Entity e = new Entity();
            e.Sprite = Sprite.DeepClone();
            return e;
        }
    }
}
