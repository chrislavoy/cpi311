using System;
using Microsoft.Xna.Framework;

namespace CPI311.GameEngine
{
    /// <summary>
    /// Static class to maintain timing information
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// Elapsed time since the last Update
        /// </summary>
        public static float ElapsedGameTime { get; set; }

        /// <summary>
        /// Total time since the game started
        /// </summary>
        public static TimeSpan TotalGameTime { get; set; }

        /// <summary>
        /// Initialize the timining information
        /// </summary>
        public static void Initialize()
        {
            ElapsedGameTime = 0;
            TotalGameTime = new TimeSpan(0);
        }

        /// <summary>
        /// Update the timing information.
        /// </summary>
        /// <param name="gameTime">GameTime instance from XNA</param>
        public static void Update(GameTime gameTime)
        {
            ElapsedGameTime =
                (float)gameTime.ElapsedGameTime.TotalSeconds;
            TotalGameTime = gameTime.TotalGameTime;
        }
    }
}
