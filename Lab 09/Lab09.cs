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
    public class Lab09 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AStarSearch search;
        List<Vector3> path;

        Random random = new Random();

        Camera camera;

        Model ground;
        Model cube;
        Model sphere;

        public Lab09()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(graphics);

            search = new AStarSearch(100, 100);
            
            foreach(AStarNode node in search.Nodes)
                if(random.NextDouble() < 0.2)
                search.Nodes[random.Next(100), random.Next(100)].Passable = false;
            search.Start = search.Nodes[0, 0]; search.Start.Passable = true;
            search.End = search.Nodes[99, 99]; search.End.Passable = true;
            search.Search();
            path = new List<Vector3>();
            AStarNode current = search.End;
            while(current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cube = Content.Load<Model>("Models/Box");
            //ground = Content.Load<Model>("Models/Box");
            sphere = Content.Load<Model>("Models/Sphere");

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.One * 50;
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
            if(InputManager.IsKeyPressed(Keys.Space))
            {
                while(!(search.Start = search.Nodes[random.Next(search.Cols), random.Next(search.Rows)]).Passable);
                while(!(search.End = search.Nodes[random.Next(search.Cols), random.Next(search.Rows)]).Passable);
                search.Search();
                path.Clear();
                AStarNode current = search.End;
                while (current != null)
                {
                    path.Insert(0, current.Position);
                    current = current.Parent;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix view = camera.View;
            Matrix projection = camera.Projection;

            (cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.DarkBlue.ToVector3();
            cube.Draw(Matrix.CreateScale(100, 0.1f, 100) * Matrix.CreateTranslation(50, -5, 50), view, projection);
            (cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.DarkRed.ToVector3();
            foreach (AStarNode node in search.Nodes)
                if (!node.Passable)
                    cube.Draw(Matrix.CreateScale(0.5f,0.05f,0.5f)*Matrix.CreateTranslation(node.Position), view, projection);
            (sphere.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.WhiteSmoke.ToVector3();
            foreach(Vector3 position in path)
                sphere.Draw(Matrix.CreateScale(0.1f,0.1f,0.1f)*Matrix.CreateTranslation(position), view, projection);
            //spriteBatch.Begin();

            //spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
