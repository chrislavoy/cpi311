using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
{
    public class Renderer : Component
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Material Material { get; set; }

        public Renderer(GraphicsDevice graphics)
        {
            GraphicsDevice = graphics;
        }

        public virtual void Draw()
        {
            
        }
    }
}
