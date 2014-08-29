using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Class_Work
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Source { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Layer { get; set; }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Position = Vector2.Zero;
            Source = new Rectangle(0, 0, texture.Width, texture.Height);
            Color = Color.White;
            Rotation = 0;
            Origin = Vector2.Zero;
            Scale = Vector2.One;
            Effects = SpriteEffects.None;
            Layer = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Source, Color,
                Rotation, Origin, Scale, Effects, Layer);
        }
    }
}
