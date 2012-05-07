using Microsoft.Xna.Framework;

namespace WrackEngine
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Velocity { get; set; }

        public Camera()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * delta;
        }
    }
}
