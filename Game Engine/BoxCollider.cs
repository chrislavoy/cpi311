using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class BoxCollider : Collider
    {
        public float Size { get; set; }

        private Vector3[] normals = 
        { Vector3.Up, Vector3.Down, // top/down
            // Be careful about these normals
         Vector3.Right, Vector3.Left,
        Vector3.Backward, Vector3.Forward};

        private Vector3[] vertices = 
        {
            new Vector3(-1,-1,1),
            new Vector3(1,-1,1),
            new Vector3(1,-1,-1),
            new Vector3(-1,-1,-1),

            new Vector3(-1,1,1),
            new Vector3(1,1,1),
            new Vector3(1,1,-1),
            new Vector3(-1,1,-1),
        };

        private int[] indices = 
        {
            0,1,2,  0,2,3, // Down
            4,6,5,  4,7,6, // Up
            // Lab 6: Add four more faces
        };

        public override bool Collides(Collider collider, out Vector3 outNormal)
        {
            if (collider is SphereCollider)
            {
                SphereCollider c = collider as SphereCollider;
                for (int i = 0; i < 2 /* Lab 6: 6 */; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {

                        Vector3 x = vertices[indices[i * 6 + j * 3]]*Size;
                        Vector3 y = vertices[indices[i * 6 + j * 3 + 1]]*Size;
                        Vector3 z = vertices[indices[i * 6 + j * 3 + 2]]*Size;
                        Vector3 normal = normals[i];
                        float d = Math.Abs(Vector3.Dot(c.Transform.Position-x, normal));
                        // Check for closeness to plane
                        if(d < c.Radius)
                        {
                            Vector3 pointOnPlane = c.Transform.Position -
                                normal * d;
                            float area1 = Vector3.Dot(Vector3.Cross(y - x, pointOnPlane - x), normal);
                            float area2 = Vector3.Dot(Vector3.Cross(z - y, pointOnPlane - y), normal);
                            float area3 = Vector3.Dot(Vector3.Cross(x - z, pointOnPlane - z), normal);
                            if (!(area1 < 0 || area2 < 0 || area3 < 0))
                            {
                                outNormal = normal;
                                return true;
                            }
                        }

                    }
                }
            }
            return base.Collides(collider, out outNormal);
        }
    }
}
