using Microsoft.Xna.Framework.Graphics;
namespace CPI311.GameEngine
{
    /// <summary>
    /// Interface for anything that can be updated
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Update this object
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Interface for anything that can draw in 3D
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Draw this 3D object
        /// </summary>
        void Draw();
    }

    /// <summary>
    /// Interface for anything that can draw in 2D
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draw this 2D object
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(SpriteBatch spriteBatch);
    }
}
