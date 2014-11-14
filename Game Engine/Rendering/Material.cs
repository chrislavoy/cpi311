using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
{
    public class Material
    {
        public Effect Effect { get; protected set; }

        public Material(Effect effect)
        {
            Effect = effect;
        }

        public virtual void Apply(Matrix world)
        {

        }
    }
}
