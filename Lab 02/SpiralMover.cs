using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using CPI311.GameEngine;

namespace CPI311.Labs
{
    public class SpiralMover
    {
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; set; }
        public float Phase { get; set; }
        public float Radius { get; set; }
        public float Frequency { get; set; }
        public float Amplitude { get; set; }
        public float Speed { get; set; }

        public SpiralMover(Texture2D texture, Vector2 position, float radius = 150, float speed = 1, float frequency = 20, float amplitude = 10, float phase = 0)
        {
            Sprite = new Sprite(texture);
            Position = position;
            Radius = radius;
            Speed = speed;
            Frequency = frequency;
            Amplitude = amplitude;
            Phase = 0;
            Sprite.Position = Position + new Vector2(Radius, 0);
        }

        public void Update()
        {
            Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Phase += Speed * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Up))
                Radius += Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.Down))
                Radius -= Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.Right))
                Amplitude += Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Left))
                Amplitude -= Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.Right))
                Frequency += Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.Left))
                Frequency -= Time.ElapsedGameTime;
            Sprite.Position = Position + new Vector2(
                (float)((Radius + Amplitude * Math.Cos(Phase * Frequency)) * Math.Cos(Phase)),
                (float)((Radius + Amplitude * Math.Cos(Phase * Frequency)) * Math.Sin(Phase)));
            Sprite.Color = Color.Lerp(Color.Red, Color.Green, (float)(Math.Cos(Phase * Frequency) + 1) / 2);
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
