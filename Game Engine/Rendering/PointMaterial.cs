using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class PointMaterial : Material
    {
        // Colors
        public Color Ambient { get; set; }
        public Color Diffuse { get; set; }
        public Color Specular { get; set; }
        public float Shininess { get; set; }
        public Vector2 Offset { get; set; }
        public Vector2 Tiling { get; set; }
        public Texture2D Texture { get; set; }

        public PointMaterial(ContentManager content, Texture2D texture)
            : base(content.Load<Effect>("Effects/PointEffect"))        {
            Ambient = Color.Gray;
            Diffuse = Color.Gray;
            Specular = Color.Gray;
            Shininess = 20;
            Offset = Vector2.Zero;
            Tiling = Vector2.One;
            Texture = texture;
        }

        public override void Apply(Matrix world)
        {
            base.Apply(world);
            Effect.CurrentTechnique = Effect.Techniques[0];
            Effect.Parameters["World"].SetValue(world);
            Effect.Parameters["View"].SetValue(Camera.Current.View);
            Effect.Parameters["Projection"].SetValue(Camera.Current.Projection);
            Effect.Parameters["DiffuseColor"].SetValue(Diffuse.ToVector4() * Light.Current.Diffuse.ToVector4());
            Effect.Parameters["PointTexture"].SetValue(Texture);
            Effect.Parameters["Offset"].SetValue(Offset);
            Effect.Parameters["Tiling"].SetValue(Tiling);
            Effect.CurrentTechnique.Passes[0].Apply();

        }
    }
}

