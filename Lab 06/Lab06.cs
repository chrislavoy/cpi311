#region Using Statements
using System;
using System.Threading;
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
        SpriteFont font;
        Model model;
        Random random;

        Transform cameraTransform;
        Camera camera;
        
        List<Rigidbody> rigidbodies;
        List<Collider> colliders;
        List<Transform> transforms;

        BoxCollider boxCollider;

        int lastSecondCollisions = 0;
        int numberCollisions = 0;
        bool haveThreadRunning = false;

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
            random = new Random();
            transforms = new List<Transform>();
            rigidbodies = new List<Rigidbody>();
            colliders = new List<Collider>();

            haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(
                new WaitCallback(CollisionReset));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Arial");
            model = Content.Load<Model>("Models/Sphere");
            foreach(ModelMesh mesh in model.Meshes)
                foreach(BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 20;
            camera = new Camera();
            camera.Transform = cameraTransform;

            AddSphere();
            transforms[0].LocalPosition = new Vector3(0, -9.5f, 0);
            rigidbodies[0].Velocity = new Vector3(1, 1, 0);

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

            if (InputManager.IsKeyPressed(Keys.Space))
            {
                AddSphere();
            }

            foreach (Rigidbody rigidbody in rigidbodies)
                rigidbody.Update();
            
            // On a new thread --
            // To fix: Assignment 4
            // 1. Binary spheres (fixed (see (A))
            // 2. Enery in the system increases (how to fix?)
            Vector3 normal;
            for (int i = 0; i < transforms.Count; i++)
            {
                if (boxCollider.Collides(colliders[i], out normal))
                {
                    numberCollisions++;
                    // Lab 7: include mass in equation
                    if(Vector3.Dot(normal, rigidbodies[i].Velocity) < 0)
                        rigidbodies[i].Impulse += Vector3.Dot(normal, rigidbodies[i].Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < transforms.Count; j++)
                {
                    if (colliders[i].Collides(colliders[j], out normal))
                    {
                        // Lab 7: include mass in equation
                        numberCollisions++;
                        // do resolution ONLY if they are colliding into one another
                        // if normal is from i to j
                        //dot(normal, vi) > 0 & dot(normal, vj) < 0) (A)
                        if (Vector3.Dot(normal, rigidbodies[i].Velocity) > 0 &&
                            Vector3.Dot(normal, rigidbodies[j].Velocity) < 0)
                            return;
                        Vector3 velocityNormal = Vector3.Dot(normal, rigidbodies[i].Velocity - rigidbodies[j].Velocity)
                                        * -2 * normal * rigidbodies[i].Mass * rigidbodies[j].Mass;
                        rigidbodies[i].Impulse += velocityNormal / 2;
                        rigidbodies[j].Impulse += -velocityNormal / 2;
                    }
                }
            }
            // on a new thread --
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            foreach(Transform transform in transforms)
            model.Draw(transform.World, camera.View, camera.Projection);
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Collision: " + lastSecondCollisions, Vector2.Zero, Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void AddSphere()
        {
            Transform transform = new Transform();
            transform.LocalScale *= random.Next(1, 10) * 0.25f;
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = transform;
            rigidbody.Mass = 1; // Lab 7: random mass
            //rigidbody.Acceleration = Vector3.Down * 9.81f;
            //rigidbody.Velocity = new Vector3((float)random.NextDouble() * 5, (float)random.NextDouble() * 5, (float)random.NextDouble() * 5);
            Vector3 direction = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity = direction * ((float)random.NextDouble()*5 + 5);
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 2.5f * transform.LocalScale.Y;
            sphereCollider.Transform = transform;

            transforms.Add(transform);
            colliders.Add(sphereCollider);
            rigidbodies.Add(rigidbody);
        }

        // Simple example of multi threading
        private void CollisionReset(Object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                numberCollisions = 0;
                System.Threading.Thread.Sleep(1000);
            }
        }
    }

 
}
