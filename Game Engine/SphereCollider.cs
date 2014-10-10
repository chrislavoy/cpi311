using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class SphereCollider : Collider
    {
        public float Radius { get; set; }

        public override bool Collides(Collider collider, out Vector3 normal)
        {
            if(collider is SphereCollider)
            {
                SphereCollider c = collider as SphereCollider;
                if ((Transform.Position - c.Transform.Position).Length() <
                    Radius + c.Radius)
                {
                    normal = Vector3.Normalize(Transform.Position - c.Transform.Position);
                    return true;
                }
            }
            return base.Collides(collider, out normal);
        }
    }
}
