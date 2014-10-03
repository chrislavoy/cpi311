using System;
using System.Collections.Generic;
using System.Linq;
using CPI311.GameEngine;

namespace CPI311.Assignments
{
    public class AstralBody : Transform
    {
        public float RotationSpeed { get; set; }

        public AstralBody(float rotationSpeed = 0)
            : base()
        {
            RotationSpeed = rotationSpeed;
        }
    }
}
