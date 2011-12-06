using Microsoft.Xna.Framework;

using FarseerPhysics.Dynamics;

namespace Wrack
{
    public static class Core
    {
        private const float DEFAULT_METERS_PER_PIXEL = 1f / 32f;
        private const int  DEFAULT_MAX_PARTICLES = 100;

        private static Game game;
        private static GraphicsDeviceManager graphicsDeviceManager;

        public static Game Game { get { return game; } }
        public static World World { get; set; }
        public static GraphicsDeviceManager GraphicsDeviceManager { get { return graphicsDeviceManager; } }
        public static float MetersPerPixel { get; set; }
        public static float PixelsPerMeter { get; set; }
        public static bool UsingFarseerPhysics { get; set; }
        public static Vector2 Gravity { get; set; }

        public static void Prepare(Game g, string contentDirectory)
        {
            game = g;
            graphicsDeviceManager = new GraphicsDeviceManager(g);
            g.Content.RootDirectory = contentDirectory;
        }

        public static void Initialize(Vector2 gravity, float metersPerPixel, bool useFarseerPhysics)
        {
            UsingFarseerPhysics = useFarseerPhysics;
            MetersPerPixel = metersPerPixel;
            PixelsPerMeter = 1f / MetersPerPixel;
            Gravity = gravity;

            Graphics.Initialize();
            Input.Initialize();
            Console.Initialize();
            if (UsingFarseerPhysics)
            {
                World = new World(gravity);
            }
        }

        public static void Update(GameTime gameTime)
        {
            if (World != null)
            {
                World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            Input.Update(gameTime);
        }
    }
}
