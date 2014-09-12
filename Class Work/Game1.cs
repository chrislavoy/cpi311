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
        Model torus;
        Matrix projection;
        Matrix view;
        Matrix world; // for the torus
        Vector3 torusPosition;
        Vector3 torusRotation;
        Vector2 projectionCenter = new Vector2(0f, 0f);
        Vector2 projectionSize = new Vector2(1, 1);
        Vector3 torusScale = Vector3.One;
        bool isOrderFlipped = false;
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
            torus = Content.Load<Model>("Models/Torus");
            foreach (ModelMesh mesh in torus.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.PiOver2, 1, 0.1f, 100f);
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

            if (InputManager.IsKeyDown(Keys.LeftShift) &&
                InputManager.IsKeyDown(Keys.Up))
                torusScale += Vector3.One * 0.1f;
            if (InputManager.IsKeyDown(Keys.LeftShift) &&
                InputManager.IsKeyDown(Keys.Down))
                torusScale -= Vector3.One * 0.1f;
            if (InputManager.IsKeyDown(Keys.Insert))
                torusRotation.X += 0.05f;
            if (InputManager.IsKeyDown(Keys.Delete))
                torusRotation.X -= 0.05f;
            if (InputManager.IsKeyDown(Keys.PageUp))
                torusRotation.Y += 0.05f;
            if (InputManager.IsKeyDown(Keys.PageDown))
                torusRotation.Y -= 0.05f;
            if (InputManager.IsKeyDown(Keys.Home))
                torusRotation.Z += 0.05f;
            if (InputManager.IsKeyDown(Keys.End))
                torusRotation.Z -= 0.05f;
            if (InputManager.IsKeyDown(Keys.Up))
                torusPosition.Y += 0.05f;
            if (InputManager.IsKeyDown(Keys.Down))
                torusPosition.Y -= 0.05f;

            if (InputManager.IsKeyDown(Keys.LeftShift))
            {
                if (InputManager.IsKeyDown(Keys.W))
                    projectionCenter.Y += 0.05f;
                if (InputManager.IsKeyDown(Keys.S))
                    projectionCenter.Y -= 0.05f;
            }
            else if (InputManager.IsKeyDown(Keys.LeftControl))
            {
                if (InputManager.IsKeyDown(Keys.A))
                    projectionSize.X += 0.05f;
                if (InputManager.IsKeyDown(Keys.D))
                    projectionSize.X -= 0.05f;
                projectionSize.Y = projectionSize.X * 2;
            }
            else

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            Vector2 topLeft = projectionCenter - projectionSize/2;
            Vector2 bottomRight = projectionCenter + projectionSize/2;
            projection = Matrix.CreatePerspectiveOffCenter(
                topLeft.X, bottomRight.X, topLeft.Y, bottomRight.Y, 0.1f, 100f);
            view = Matrix.CreateLookAt(Vector3.Backward * 10,
                Vector3.Zero, Vector3.Up);
            world = Matrix.CreateScale(torusScale) *
                Matrix.CreateFromYawPitchRoll(torusRotation.X,torusRotation.Y, torusRotation.Z) *
                Matrix.CreateTranslation(torusPosition);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            torus.Draw(world, view, projection);
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
