#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using CPI311.GameEngine;
#endregion

namespace Class_Work
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Sprite sprite;
        Random random = new Random();

        Vector2 center = new Vector2(300,300);
        float radius = 150;
        float angle = 0;
        float speed = 5;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InputManager.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Arial");
            sprite = new Sprite(Content.Load<Texture2D>("Textures/Square"));
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            InputManager.Update();
            Time.Update(gameTime);
            angle += speed * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Up))
                radius += Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.Down))
                radius -= Time.ElapsedGameTime * 10;
            sprite.Position = center + new Vector2(
                (float)((radius + 10* Math.Cos(angle * 5)) * Math.Cos(angle)),
                (float)((radius + 10* Math.Cos(angle * 5)) * Math.Sin(angle)));
            sprite.Color = Color.Lerp(Color.Red, Color.Blue, (float)(Math.Cos(angle) + 1) / 2);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            sprite.Draw(spriteBatch);
            spriteBatch.DrawString(font,
                "Smaller method", new Vector2(200, 200), Color.Black);
            spriteBatch.DrawString(font,
                    "Hello World!",
                    new Vector2(50, 50),
                    Color.Black,
                    MathHelper.PiOver2,
                    Vector2.Zero,
                    Vector2.One,
                    SpriteEffects.FlipHorizontally,
                    0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
