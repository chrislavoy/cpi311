#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace CPI311.Labs
{
    /// <summary>
    /// Lab 01 is an exercise in C# programming. For this lab, you will
    /// create a data type for fraction calculation, and use XNA
    /// to write sample results to screen
    /// </summary>
    public class Lab01 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Fraction a = new Fraction(2, 81);
        Fraction b = new Fraction(8, -27);

        public Lab01()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(); // Setup the device for 2D drawing
            spriteBatch.DrawString(font, a + " + " + b + " = " + (a + b), new Vector2(50,50), Color.Black);
            spriteBatch.DrawString(font, a + " - " + b + " = " + (a - b), new Vector2(50, 100), Color.Black);
            spriteBatch.DrawString(font, a + " * " + b + " = " + (a * b), new Vector2(50, 150), Color.Black);
            spriteBatch.DrawString(font, a + " / " + b + " = " + (a / b), new Vector2(50, 200), Color.Black);
            spriteBatch.End(); // Indicate that we are done with SpriteBatch
            base.Draw(gameTime);
        }
    }
}
