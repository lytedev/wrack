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

        public static void Prepare(Game g, string contentDirectory)
        {
            game = g;
            graphicsDeviceManager = new GraphicsDeviceManager(g);
            g.Content.RootDirectory = contentDirectory;
        }

        public static void Initialize()
        {
            Graphics.Initialize();
            Input.Initialize();
            Console.Initialize();
            Core.World = new World(Vector2.Zero);
            Particle.Particles = new System.Collections.Generic.List<Particle>();
            if (Particle.MaxParticles == 0)
            {
                Particle.MaxParticles = DEFAULT_MAX_PARTICLES;
            }
            if (MetersPerPixel == 0)
            {
                MetersPerPixel = DEFAULT_METERS_PER_PIXEL;
            }
            PixelsPerMeter = 1f / MetersPerPixel;
        }

        public static void Initialize(Vector2 gravity)
        {
            Graphics.Initialize();
            Input.Initialize();
            Console.Initialize();
            Core.World = new World(gravity);
            Particle.Particles = new System.Collections.Generic.List<Particle>();
            if (Particle.MaxParticles == 0)
            {
                Particle.MaxParticles = DEFAULT_MAX_PARTICLES;
            }
            if (MetersPerPixel == 0)
            {
                MetersPerPixel = DEFAULT_METERS_PER_PIXEL;
            }
            PixelsPerMeter = 1f / MetersPerPixel;
        }

        public static void Initialize(float metersPerPixel)
        {
            Graphics.Initialize();
            Input.Initialize();
            Console.Initialize();
            World = new World(Vector2.Zero);
            MetersPerPixel = metersPerPixel;
            Particle.Particles = new System.Collections.Generic.List<Particle>();
            if (Particle.MaxParticles == 0)
            {
                Particle.MaxParticles = DEFAULT_MAX_PARTICLES;
            }
            PixelsPerMeter = 1f / MetersPerPixel;
        }

        public static void Initialize(Vector2 gravity, float metersPerPixel)
        {
            Graphics.Initialize();
            Input.Initialize();
            Console.Initialize();
            World = new World(gravity);
            MetersPerPixel = metersPerPixel;
            Particle.Particles = new System.Collections.Generic.List<Particle>();

            if (Particle.MaxParticles == 0)
            {
                Particle.MaxParticles = DEFAULT_MAX_PARTICLES;
            }
            PixelsPerMeter = 1f / MetersPerPixel;
        }

        public static void Update(GameTime gameTime)
        {
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
