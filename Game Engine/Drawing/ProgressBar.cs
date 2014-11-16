using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
{
    public class ProgressBar:Sprite
    {
        public float Value { get; set; }
        public Color Overlay { get; set; }
        public float Speed { get; set; }

        public ProgressBar(Texture2D texture, float value = 1, float speed = 0)
            : base(texture)
        {
            Speed = speed;
            Value = value;
            Overlay = Color.DarkGreen;
        }

        public override void Update()
        {
            base.Update();
            Value = MathHelper.Clamp(Value + Speed * Time.ElapsedGameTime, 0, 1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, Position, new Rectangle(Source.X, Source.Y, (int)(Source.Width * Value), Source.Height), Color, Rotation, Origin, Scale, Effect, Depth);
        }
    }
}
