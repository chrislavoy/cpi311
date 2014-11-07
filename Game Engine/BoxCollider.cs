using System;
using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    public class BoxCollider : Collider
    {
        public float Size { get; set; }

        private static Vector3[] normals = 
        { Vector3.Up, Vector3.Down, // down/up
          Vector3.Right, Vector3.Left, // left/right
          Vector3.Forward, Vector3.Backward, // front/back
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
            5,4,7,  5,7,6, // Up
            4,0,3,  4,3,7, // Left
            1,5,6,  1,6,2, // Right
            4,5,1,  4,1,0, // Front
            3,2,6,  3,6,7, // Back
        };

        public override bool Collides(Collider other, out Vector3 normal)
        {
            if (other is SphereCollider)
            {
                SphereCollider collider = other as SphereCollider;
                normal = Vector3.Zero; // no collision
                bool isColliding = false;
                // For each face
                for (int i = 0; i < 6; i++)
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
                                normal += n;
                                j = 1; // skip second triangle, if necessary
                                if (i % 2 == 0) i += 1; // skip opposite side if necessary
                                isColliding = true;
                            }
                        }
                    }
                }
                normal.Normalize();
                return isColliding;
            }
            return base.Collides(other, out normal);
        }

        public override float? Intersects(Ray ray)
        {
            Matrix worldInverted = Matrix.Invert(Transform.World);
            ray.Position = Vector3.Transform(ray.Position, worldInverted);
            ray.Direction = Vector3.TransformNormal(ray.Direction, worldInverted);
            BoundingBox box = new BoundingBox(Vector3.One * -Size, Vector3.One * Size);
            return box.Intersects(ray);
        }
    }
}
