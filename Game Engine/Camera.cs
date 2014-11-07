using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
{
    /// <summary>
    /// Represents a camera to manage viewing information
    /// Should be attached to a Transform
    /// --- Incomplete --- 
    /// </summary>
    public class Camera : Component
    {
        public static Camera Current { get; set; }

        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Viewport Viewport
        {
            get
            {
                return new Viewport((int)(ScreenManager.Width * Position.X),
                            (int)(ScreenManager.Height * Position.Y),
                            (int)(ScreenManager.Width * Size.X),
                            (int)(ScreenManager.Height * Size.Y));
            }
        }

        public Camera()
        {
            FieldOfView = MathHelper.PiOver2;
            AspectRatio = ScreenManager.DefaultViewport.AspectRatio;
            NearPlane = 0.1f;
            FarPlane = 100f;
            Transform = null;
            Position = Vector2.Zero;
            Size = Vector2.One;
        }

        /// <summary>
        /// Computes a viewing matrix using the transform
        /// this camera is attached to. If there is no parent,
        /// it returns the identity matrix.
        /// </summary>
        public Matrix View
        {
            get
            {
                if (Transform == null)
                    return Matrix.Identity;
                return Matrix.CreateLookAt(Transform.Position,
                        Transform.Position + Transform.Forward,
                        Transform.Up);
            }
        }

        /// <summary>
        /// Computes the Projection matrix using properties
        /// of this camera
        /// </summary>
        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(
                    FieldOfView, AspectRatio, NearPlane, FarPlane);
            }
        }

        public Ray ScreenPointToWorldRay(Vector2 position)
        {
            Vector3 start = Viewport.Unproject(
                new Vector3(position, 0), Projection, View, Matrix.Identity);
            Vector3 end = Viewport.Unproject(
                new Vector3(position, 1), Projection, View, Matrix.Identity);
            return new Ray(start, end - start);
        }
    }
}
  