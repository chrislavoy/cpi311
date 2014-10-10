using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    public class Rigidbody : Component, IUpdateable
    {
        public Vector3 Velocity { get; set; }
        public float Mass { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 Impulse { get; set; }

        public void Update()
        {
            Transform.LocalPosition += 
                Velocity * Time.ElapsedGameTime;
            Velocity += Acceleration * Time.ElapsedGameTime;
            Velocity += Impulse / Mass;
            Impulse = Vector3.Zero;
        }
    }
}
