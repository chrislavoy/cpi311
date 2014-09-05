using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine
{
    public static class InputManager
    {
        private static KeyboardState PreviousKeyboardState { get; set; }
        private static KeyboardState CurrentKeyboardState { get; set; }
        private static MouseState PreviousMouseState { get; set; }
        private static MouseState CurrentMouseState { get; set; }

        public static void Initialize()
        {
            PreviousKeyboardState = CurrentKeyboardState =
                Keyboard.GetState();
            PreviousMouseState = CurrentMouseState =
                Mouse.GetState();
        }
        
        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public static bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) &&
                    PreviousKeyboardState.IsKeyUp(key);
        }
    }
}
