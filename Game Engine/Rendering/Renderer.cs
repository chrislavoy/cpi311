using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
{
    public class Renderer : Component, IRenderable
    {
        public Material Material { get; set; }
        public virtual void Draw()
        {
            
        }
    }
}
