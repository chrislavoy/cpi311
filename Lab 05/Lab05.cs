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

namespace CPI311.Labs
{
    public class Lab05 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model model;
        Transform parentTransform;
        Transform childTransform;
        Transform cameraTransform;
        Texture2D texture;
        Camera camera;
        Effect effect;

        public Lab05()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Let's load our model
            model = Content.Load<Model>("Models/Torus");
            // Ask our model to do "default" lighting"
            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();
            this.effect = Content.Load<Effect>("Effects/SimpleShading");
            parentTransform = new Transform();
            childTransform = new Transform();
            childTransform.Parent = parentTransform;
            childTransform.LocalPosition = Vector3.Right * 10;
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;
            texture = Content.Load<Texture2D>("Textures/Square");
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();

            // Keep rotating my child object
            childTransform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            // Scale the parent if Shift+Up/Down is pressed
            if (InputManager.IsKeyDown(Keys.LeftShift))
            {
                if (InputManager.IsKeyDown(Keys.Up))
                    parentTransform.LocalScale += Vector3.One * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Down))
                    parentTransform.LocalScale -= Vector3.One * Time.ElapsedGameTime;
            }
            else if (InputManager.IsKeyDown(Keys.LeftControl))
            {
                if (InputManager.IsKeyDown(Keys.Right))
                    parentTransform.Rotate(Vector3.Up, Time.ElapsedGameTime * 5);
                if (InputManager.IsKeyDown(Keys.Left))
                    parentTransform.Rotate(Vector3.Down, Time.ElapsedGameTime * 5);
                if (InputManager.IsKeyDown(Keys.Up))
                    parentTransform.Rotate(Vector3.Right, Time.ElapsedGameTime * 5);
                if (InputManager.IsKeyDown(Keys.Down))
                    parentTransform.Rotate(Vector3.Left, Time.ElapsedGameTime * 5);
            }
            // Otherwise, move the parent with respect to its axes
            else
            {
                if (InputManager.IsKeyDown(Keys.Right))
                    parentTransform.LocalPosition += parentTransform.Right * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Left))
                    parentTransform.LocalPosition += parentTransform.Left * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Up))
                    parentTransform.LocalPosition += parentTransform.Up * Time.ElapsedGameTime;
                if (InputManager.IsKeyDown(Keys.Down))
                    parentTransform.LocalPosition += parentTransform.Down * Time.ElapsedGameTime;
            }

            // Control the camera
            if (InputManager.IsKeyDown(Keys.W)) // move forward
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.S)) // move backwars
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.A)) // rotate left
                cameraTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D)) // rotate right
                cameraTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Q)) // look up
                cameraTransform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.E)) // look down
                cameraTransform.Rotate(Vector3.Left, Time.ElapsedGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            Matrix view = camera.View;
            Matrix projection = camera.Projection;
            //model.Draw(parentTransform.World, view, projection);
            model.Draw(childTransform.World, view, projection);

            effect.CurrentTechnique = effect.Techniques[1];
            effect.Parameters["World"].SetValue(parentTransform.World);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 + Vector3.Right * 5);
            effect.Parameters["CameraPosition"].SetValue(cameraTransform.Position);
            effect.Parameters["Shininess"].SetValue(20f);
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.5f, 0, 0));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0, 0, 0.5f));
            effect.Parameters["DiffuseTexture"].SetValue(texture);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        GraphicsDevice.Indices = part.IndexBuffer;
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, part.VertexOffset, 0,
                            part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
            }
            spriteBatch.Begin();
            // Any 2D stuff goes here!
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
