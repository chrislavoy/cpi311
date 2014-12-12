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

namespace CPI311.Examples
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        GameObject terrain, player;
        BoxCollider boxCollider;
        PointMaterial material;

        Vector3 targetPosition;

        Camera camera;
        Light light;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ScreenManager.Initialize(graphics);
            InputManager.Initialize();
            Time.Initialize();
            base.Initialize();

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.Position = new Vector3(0, 10, 50);
            light = new Light();
            light.Transform = camera.Transform;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Arial");

            terrain = new GameObject();
            terrain.Transform.LocalScale = new Vector3(50, 0.1f, 50);
            ModelRenderer renderer = terrain.Add<ModelRenderer>();
            renderer.Model = Content.Load<Model>("Models/Box");
            renderer.Material = material = new PointMaterial(Content, Content.Load<Texture2D>("Textures/Cross Hair"));
            boxCollider = terrain.Add<BoxCollider>();

            player = new GameObject();
            renderer = player.Add<ModelRenderer>();
            renderer.Model = Content.Load<Model>("Models/Sphere");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (InputManager.IsKeyDown(Keys.W))
                camera.Transform.LocalPosition += camera.Transform.Forward * Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.S))
                camera.Transform.LocalPosition += camera.Transform.Backward * Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.A))
                camera.Transform.LocalPosition += camera.Transform.Left * Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.D))
                camera.Transform.LocalPosition += camera.Transform.Right * Time.ElapsedGameTime * 10;

            if ((targetPosition - player.Transform.Position).LengthSquared() > Time.ElapsedGameTime * 10)
                player.Transform.LocalPosition += Vector3.Normalize(targetPosition - player.Transform.Position) * Time.ElapsedGameTime * 10;
            else
                player.Transform.LocalPosition = targetPosition;

            //
            {
                Ray ray = camera.ScreenPointToWorldRay(InputManager.GetMousePosition());
                float? p;
                if ((p = boxCollider.Intersects(ray)) != null)
                {
                    Vector3 position = ray.Position + (float)p * ray.Direction;
                    if (InputManager.IsMousePressed(1))
                        targetPosition = position;
                    material.Offset = new Vector2(position.X + 50, position.Z + 50) / 100 - Vector2.One *0.5f;
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            Camera.Current = camera;
            Light.Current = light;
            terrain.Draw();
            player.Draw();

            base.Draw(gameTime);
        }
    }
}
