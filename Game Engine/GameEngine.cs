using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class GameEngine : Game
    {
        protected SpriteBatch SpriteBatch { get; set; }
        GraphicsDeviceManager graphics;

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            ScreenManager.Initialize(graphics);
            InputManager.Initialize();
            Time.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            Time.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            Render3D(gameTime);

            SpriteBatch.Begin();
            Render2D(gameTime);
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        protected virtual void Render3D(GameTime gameTime)
        {

        }

        protected virtual void Render2D(GameTime gameTime)
        {

        }
    }
}
