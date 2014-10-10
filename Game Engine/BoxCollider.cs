using System;
using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    public class BoxCollider : Collider
    {
        public float Size { get; set; }

        private static Vector3[] normals = 
        { Vector3.Up, Vector3.Down, // top/down
          // Lab 6: Add four more normals
        };

        private static Vector3[] vertices = 
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

        private static int[] indices = 
        {
            0,1,2,  0,2,3, // Down
            4,6,5,  4,7,6, // Up
            // Lab 6: Add four more faces
        };

        public override bool Collides(Collider other, out Vector3 normal)
        {
            if (other is SphereCollider)
            {
                SphereCollider collider = other as SphereCollider;

                // For each face
                for (int i = 0; i < 2 /* Lab 6: 6 */; i++)
                {
                    // For each triangle in the face
                    for (int j = 0; j < 2; j++)
                    {
                        // Extract the vertices and normal for this triangle
                        int baseIndex = i * 6 + j * 3;
                        Vector3 a = vertices[indices[baseIndex]]*Size;
                        Vector3 b = vertices[indices[baseIndex + 1]] * Size;
                        Vector3 c = vertices[indices[baseIndex + 2]] * Size;
                        Vector3 n = normals[i];
                        // How far is the sphere center from the plane?
                        float d = Math.Abs(Vector3.Dot(collider.Transform.Position - a, n));
                        // Check for closeness to plane
                        if(d < collider.Radius)
                        {
                            // Close enough, find point in plane 
                            Vector3 pointOnPlane = collider.Transform.Position - n * d;
                            // Now, find areas of the three "inner" triangles
                            float area1 = Vector3.Dot(Vector3.Cross(b - a, pointOnPlane - a), n);
                            float area2 = Vector3.Dot(Vector3.Cross(c - b, pointOnPlane - b), n);
                            float area3 = Vector3.Dot(Vector3.Cross(a - c, pointOnPlane - c), n);
                            // No collision if either one is less than zero
                            if (!(area1 < 0 || area2 < 0 || area3 < 0))
                            {
                                normal = n;
                                return true;
                            }
                        }
                    }
                }
            }
            return base.Collides(other, out normal);
        }
    }
}
