﻿using Game2D.glcore;
using Game2D.layers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game2D.core
{
    public class Game : GameWindow
    {
        public static WindowData WindowData { get; private set; }

        private List<Layer> layers = new List<Layer>();

        public Game(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            WindowData = new WindowData(width, height, KeyboardState, MouseState, CursorState,
                () => { return CursorState; },
                (state) => CursorState = state);
            AddLayer(new Camera2D(new Vector3(0, 0, 0)));
        }

        public void AddLayer(Layer layer)
        {
            layers.Add(layer);
            layer.OnAttach();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Renderer2D.Initialize();
            Renderer2D.SetClearColor(new Color4(0.1f, 0.1f, 0.1f, 1.0f));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            WindowData.SetDimensions(e.Width, e.Height);

            foreach (Layer layer in layers)
            {
                layer.OnResize(e);
            }
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            Log.GLCall(() => GL.Viewport(0, 0, e.Width, e.Height));
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (Input.IsKeyPressed(Keys.F11))
            {
                WindowState = WindowState != WindowState.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;
            }

            foreach (Layer layer in layers)
            {
                layer.OnUpdate(args);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            Renderer2D.BeginScene();
            Renderer2D.Clear();

            foreach (Layer layer in layers)
            {
                layer.OnRender(args);
            }

            Renderer2D.EndScene();
            Context.SwapBuffers();
        }

        protected override void OnUnload()
        {
            foreach (Layer layer in layers)
            {
                layer.OnDetach();
            }

            Renderer2D.Deinitialize();

            base.OnUnload();
        }
    }

    public struct WindowData
    {
        public float ScreenWidth { get; private set; }
        public float ScreenHeight { get; private set; }
        public KeyboardState KeyboardState { get; private set; }
        public MouseState MouseState { get; private set; }

        public Func<CursorState> GetCursorState { get; private set; }
        public Action<CursorState> SetCursorState { get; private set; }

        public WindowData(float width, float height, KeyboardState keyboardState, MouseState mouseState,
            CursorState cursorState, Func<CursorState> stateGetter, Action<CursorState> stateSetter)
        {
            SetDimensions(width, height);
            KeyboardState = keyboardState;
            MouseState = mouseState;
            GetCursorState = stateGetter;
            SetCursorState = stateSetter;
        }

        public void SetDimensions(float width, float height)
        {
            ScreenWidth = width;
            ScreenHeight = height;
        }
    }
}