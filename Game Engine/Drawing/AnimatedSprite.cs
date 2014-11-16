using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
{
    public class AnimatedSprite : Sprite
    {
        public int Clips { get; set; }
        public int Clip { get; set; }
        public int Frames { get; set; }
        public float Frame { get; set; }
        public float Speed { get; set; }

        public AnimatedSprite(Texture2D texture, int frames = 1, int clips = 1)
            : base(texture)
        {
            Frames = frames;
            Width = Texture.Width / Frames;
            Clips = clips;
            Height = Texture.Height / Clips;
            Clip = 0;
            Frame = 0;
            Speed = 20;
        }

        public override void Update()
        {
            base.Update();
            Frame = (Frame + Speed * Time.ElapsedGameTime) % Frames;
            Source = new Rectangle(Width * (int)Frame, Height * Clip, Width, Height);
        }
    }
}
