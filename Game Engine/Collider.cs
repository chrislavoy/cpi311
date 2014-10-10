using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class Collider : Component
    {
        public virtual bool Collides (Collider collider, out Vector3 normal)
        {
            normal = Vector3.Zero;
            return false;
        }
    }
}
