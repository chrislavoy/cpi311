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
    public class Lab06 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model model;

        Transform cameraTransform;
        Camera camera;
        
        Transform objectTransform;
        Rigidbody rigidbody;
        SphereCollider sphereCollider;

        BoxCollider boxCollider;

        public Lab06()
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
            model = Content.Load<Model>("Models/Sphere");
            foreach(ModelMesh mesh in model.Meshes)
                foreach(BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;

            objectTransform = new Transform();
            rigidbody = new Rigidbody();
            rigidbody.Transform = objectTransform;
            rigidbody.Mass = 1;
            //rigidbody.Acceleration = Vector3.Down * 9.81f;
            rigidbody.Velocity = new Vector3(0, 5, 0);
            sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1;
            sphereCollider.Transform = objectTransform;

            boxCollider = new BoxCollider();
            boxCollider.Size = 10;
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
            //if (objectTransform.LocalPosition.Y < 0 && rigidbody.Velocity.Y < 0)
            //    rigidbody.Impulse = -new Vector3(0,rigidbody.Velocity.Y,0) * 2.1f * rigidbody.Mass;
            rigidbody.Update();
            Vector3 normal;
            if(boxCollider.Collides(sphereCollider, out normal))
            {
                rigidbody.Velocity += Vector3.Dot(normal, rigidbody.Velocity) * -2 * normal;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            model.Draw(objectTransform.World, camera.View, camera.Projection);
            //spriteBatch.Begin();

            //spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
