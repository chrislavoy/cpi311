using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
