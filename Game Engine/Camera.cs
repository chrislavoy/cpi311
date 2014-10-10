using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    /// <summary>
    /// Represents a camera to manage viewing information
    /// Should be attached to a Transform
    /// --- Incomplete --- 
    /// </summary>
    public class Camera : Component
    {
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }

        public Camera()
        {
            FieldOfView = MathHelper.PiOver2;
            AspectRatio = 1;
            NearPlane = 0.1f;
            FarPlane = 100f;
            Transform = null;
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
    }
}
  