using System.Collections.Generic;

using Microsoft.Xna.Framework;

using FarseerPhysics.Dynamics;

namespace Wrack
{
    public class Particle : Entity
    {
        public static int MaxParticles { get; set; }
        public static List<Particle> Particles { get; set; }

        public static void ClearParticles()
        {
            Particle[] p = Particles.ToArray();
            for (int i = 0; i < p.Length; i++)
            {
                p[i].Destroy();
            }
        }

        public float Life { get; set; }

        public override void Default()
        {
            base.Default();
            Life = 1000;
        }

        public Particle()
            : base()
        {
            Particles.Add(this);

            while (Particles.Count > MaxParticles)
            {
                Particles[0].Destroy();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Life -= gameTime.ElapsedGameTime.Milliseconds;

            if (Life <= 0)
            {
                Destroy();
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            try { Particles.Remove(this); }
            catch { }
        }
    }
}
