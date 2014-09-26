using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CPI311.GameEngine
{
    /// <summary>
    /// Stores the information about a transformation. Also
    /// allows hierarchical organization of objects
    /// </summary>
    public class Transform
    {
        // Some private variables
        // There is a corresponding public property for each
        private Vector3 localScale;
        private Quaternion localRotation;
        private Vector3 localPosition;
        private Matrix world;
        private Transform parent;
        // A list containing the children of this transform
        private List<Transform> Children { get; set; }

        /// <summary>
        /// Updates the world matrix for this transform and all
        /// its children
        /// </summary>
        private void UpdateWorld()
        {
            world = Matrix.CreateScale(localScale) *
                Matrix.CreateFromQuaternion(localRotation) *
                Matrix.CreateTranslation(localPosition);
            if (parent != null)
                world = world * parent.World;
            foreach (Transform child in Children)
                child.UpdateWorld();
        }

        /// <summary>
        /// The parent transform for this instance. NULL if
        /// there is no parent
        /// </summary>
        public Transform Parent
        {
            get { return parent; }
            set
            {
                // Failsafe check - do not allow self-loops
                // But what about non-self loops?
                if (value == this) return;
                // If there is a parent right now, remove me from that list
                if (parent != null)
                    parent.Children.Remove(this);
                parent = value;
                // If I do have a parent, add me me to it
                if (parent != null)
                    parent.Children.Add(this);
                UpdateWorld();
            }
        }

        /// <summary>
        /// Local scale of this transformation
        /// </summary>
        public Vector3 LocalScale
        {
            get { return localScale; }
            set { localScale = value; UpdateWorld(); }
        }

        /// <summary>
        /// Local rotation of this transformation
        /// </summary>
        public Quaternion LocalRotation
        {
            get { return localRotation; }
            set { localRotation = value; UpdateWorld(); }
        }

        /// <summary>
        /// Local position of this transformation
        /// </summary>
        public Vector3 LocalPosition
        {
            get { return localPosition; }
            set { localPosition = value; UpdateWorld(); }
        }

        /// <summary>
        /// Global position of this transformation
        /// </summary>
        public Vector3 Position
        {
            get { return world.Translation; }
            set { LocalPosition = value - Parent.Position; }
        }

        /// <summary>
        /// Matrix representation of this transformation
        /// </summary>
        public Matrix World
        {
            get { return world; }
        }

        /// <summary>
        /// Creates an transform object with default parameters:
        /// position = (0,0,0), scale = (1,1,1), rotation = identity
        /// </summary>
        public Transform()
        {
            Children = new List<Transform>();
            parent = null;
            localScale = Vector3.One;
            localRotation = Quaternion.Identity;
            localPosition = Vector3.Zero;
            UpdateWorld();
        }

        /// <summary>
        /// Rotates the transformation about the local axis
        /// </summary>
        /// <param name="axis">Axis to rotate around</param>
        /// <param name="angle">Angle to rotate (in radians)</param>
        public void Rotate(Vector3 axis, float angle)
        {
            LocalRotation *= Quaternion.CreateFromAxisAngle(axis, angle);
        }

        /// <summary>
        /// Forward direction of this transformation
        /// </summary>
        public Vector3 Forward { get { return world.Forward; } }

        /// <summary>
        /// Backward direction of this transformation
        /// </summary>
        public Vector3 Backward { get { return world.Backward; } }

        /// <summary>
        /// Up direction of this transformation
        /// </summary>
        public Vector3 Up { get { return world.Up; } }

        /// <summary>
        /// Down direction of this transformation
        /// </summary>
        public Vector3 Down { get { return world.Down; } }

        /// <summary>
        /// Right direction of this transformation
        /// </summary>
        public Vector3 Right { get { return world.Right; } }

        /// <summary>
        /// Left direction of this transformation
        /// </summary>
        public Vector3 Left { get { return world.Left; } }
    }
}
