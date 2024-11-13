using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game2D.core
{
    internal static class Input
    {
        public static Vector2 MousePosition { get => Game.WindowData.MouseState.Position; }
        public static Vector2 MouseDelta { get => Game.WindowData.MouseState.Delta; }
        public static Vector2 MouseScrollDelta { get => Game.WindowData.MouseState.ScrollDelta; }

        public static bool IsKeyPressed(Keys key)
        {
            return Game.WindowData.KeyboardState.IsKeyPressed(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return Game.WindowData.KeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return Game.WindowData.KeyboardState.IsKeyReleased(key);
        }

        public static bool IsMousePressed(MouseButton button)
        {
            return Game.WindowData.MouseState.IsButtonDown(button);
        }

        public static bool IsMouseDown(MouseButton button)
        {
            return Game.WindowData.MouseState.IsButtonDown(button);
        }

        public static bool IsMouseReleased(MouseButton button)
        {
            return Game.WindowData.MouseState.IsButtonReleased(button);
        }
    }
}
