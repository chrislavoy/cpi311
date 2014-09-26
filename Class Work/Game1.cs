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
        Effect effect;
        Transform parentTransform;
        Transform torusTransform;
        Transform cameraTransform;
        Camera camera;
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
            this.effect = Content.Load<Effect>("Effects/SimpleShading");
            torusTransform = new Transform();
            parentTransform = new Transform();
            parentTransform.Parent = torusTransform;
            parentTransform.LocalPosition = Vector3.Right * 10;

            camera = new Camera();
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera.Transform = cameraTransform;
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
            if(InputManager.IsKeyDown(Keys.Z))
                parentTransform.Rotate(Vector3.Right, 0.05f);
            if (InputManager.IsKeyDown(Keys.LeftShift))
            {
                if(InputManager.IsKeyDown(Keys.Up))
                    torusTransform.LocalScale += Vector3.One * 0.1f;
                if(InputManager.IsKeyDown(Keys.Down))
                    torusTransform.LocalScale -= Vector3.One * 0.1f;
            }
            else
            {
                if(InputManager.IsKeyDown(Keys.Right))
                    torusTransform.LocalPosition += torusTransform.Right * 0.1f;
                if(InputManager.IsKeyDown(Keys.Left))
                    torusTransform.LocalPosition += torusTransform.Left * 0.1f;
                if(InputManager.IsKeyDown(Keys.Up))
                    torusTransform.LocalPosition += torusTransform.Up * 0.1f;
                if(InputManager.IsKeyDown(Keys.Down))
                    torusTransform.LocalPosition += torusTransform.Down * 0.1f;
            }

            if (InputManager.IsKeyDown(Keys.Insert))
                torusTransform.Rotate(Vector3.Up, 0.05f);
            if (InputManager.IsKeyDown(Keys.Delete))
                torusTransform.Rotate(Vector3.Up, -0.05f);
            if (InputManager.IsKeyDown(Keys.PageUp))
                torusTransform.Rotate(Vector3.Right, +0.05f);
            if (InputManager.IsKeyDown(Keys.PageDown))
                torusTransform.Rotate(Vector3.Right, -0.05f);
            if (InputManager.IsKeyDown(Keys.Home))
                torusTransform.Rotate(Vector3.Forward, 0.05f);
            if (InputManager.IsKeyDown(Keys.End))
                torusTransform.Rotate(Vector3.Forward, -0.05f);

            if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition += cameraTransform.Forward * 0.1f;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition += cameraTransform.Backward * 0.1f;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.Rotate(Vector3.Up, 0.05f);
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.Rotate(Vector3.Up, -0.05f);
            if (InputManager.IsKeyDown(Keys.Q))
                cameraTransform.Rotate(Vector3.Right, 0.05f);
            if (InputManager.IsKeyDown(Keys.E))
                cameraTransform.Rotate(Vector3.Right, -0.05f);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            torus.Draw(torusTransform.World, camera.View, camera.Projection);
            //torus.Draw(parentTransform.World, camera.View, camera.Projection);

            effect.CurrentTechnique = effect.Techniques[0];
            effect.Parameters["World"].SetValue(parentTransform.World);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["LightPosition"].SetValue(Vector3.Backward*10);
            effect.Parameters["CameraPosition"].SetValue(cameraTransform.LocalPosition);
            effect.Parameters["Shininess"].SetValue(20f);
            effect.Parameters["AmbientColor"].SetValue(Vector3.Zero);
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f,0,0));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0,0,0.5f));
            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach(ModelMesh mesh in torus.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        GraphicsDevice.Indices = part.IndexBuffer;
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            part.VertexOffset,
                            0,
                            part.NumVertices,
                            part.StartIndex,
                            part.PrimitiveCount);
                    }
            }

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
