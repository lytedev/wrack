using Microsoft.Xna.Framework;

namespace Wrack
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public Camera()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
        }
    }
}
