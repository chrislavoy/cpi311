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

namespace CPI311.Assignments
{
    public class Assignment2 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<AstralBody> bodies;
        Transform cameraTransform;
        Camera camera;
        Model model;
        Transform sun, mercury, earth, moon;
        float systemSpeed = 1;

        public Assignment2()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            bodies = new List<AstralBody>();
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
            model = Content.Load<Model>("Models/Box");
            foreach (ModelMesh mesh in model.Meshes)
                foreach (BasicEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();
            AstralBody planet, orbit, solarSystem, earthOrbit;
            // Solar System
            solarSystem = new AstralBody(0);
            bodies.Add(solarSystem);
            // Sun
            sun = planet = new AstralBody(0.1f);
            planet.LocalScale *= 5;
            planet.Parent = solarSystem;
            bodies.Add(planet);
            // Mercury Orbit
            orbit = new AstralBody(0.3f);
            orbit.Parent = solarSystem;
            bodies.Add(orbit);
            // Mercury Planet
            mercury = planet = new AstralBody(-0.3f);
            planet.Parent = orbit;
            planet.LocalScale *= 2;
            planet.LocalPosition = new Vector3(20, 0, 0);
            bodies.Add(planet);
            // Earth Orbit
            earthOrbit = orbit = new AstralBody(0.1f);
            orbit.Parent = solarSystem;
            bodies.Add(orbit);
            // Earth Planet
            earth = planet = new AstralBody(0.1f);
            planet.Parent = orbit;
            planet.LocalScale *= 3;
            planet.LocalPosition = new Vector3(50, 0, 0);
            bodies.Add(planet);
            // Moon Orbit
            orbit = new AstralBody(0.2f);
            orbit.Parent = earthOrbit;
            orbit.LocalPosition = new Vector3(50, 0, 0);
            bodies.Add(orbit);
            // Moon Planet
            moon = planet = new AstralBody(0.2f);
            planet.Parent = orbit;
            planet.LocalPosition = new Vector3(15, 0, 0);
            bodies.Add(planet);

            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 50;
            camera = new Camera();
            camera.FarPlane = 1000;
            camera.FieldOfView = MathHelper.TwoPi / 3;
            camera.Transform = cameraTransform;
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
            if (InputManager.IsKeyDown(Keys.Q))
                systemSpeed += Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.E))
                systemSpeed -= Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.PageUp))
                camera.FieldOfView += Time.ElapsedGameTime ;
            if (InputManager.IsKeyDown(Keys.PageDown))
                camera.FieldOfView -= Time.ElapsedGameTime;
            foreach (AstralBody body in bodies)
                body.Rotate(Vector3.Up, body.RotationSpeed * Time.ElapsedGameTime * systemSpeed);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Matrix view = camera.View;
            Matrix projection = camera.Projection;
            model.Draw(sun.World, view, projection);
            model.Draw(mercury.World, view, projection);
            model.Draw(earth.World, view, projection);
            model.Draw(moon.World, view, projection);
            base.Draw(gameTime);
        }
    }
}
