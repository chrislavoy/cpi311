using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
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
        public SpriteEffects Effect { get; set; }
        public float Depth { get; set; }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Position = Vector2.Zero;
            Source = new Rectangle(0, 0,
                        Texture.Width,
                        Texture.Height);
            Color = Color.White;
            Rotation = 0;
            Origin = new Vector2(Texture.Width / 2,
                        Texture.Height / 2);
            Scale = Vector2.One;
            Effect = SpriteEffects.None;
            Depth = 0;
        }

        public virtual void Update() { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Source, Color,
                Rotation, Origin, Scale, Effect, Depth);
        }

    }
}
