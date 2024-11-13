using Game2D.core;
using Game2D.layers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game2D.glcore
{
    internal class Camera2D : Layer
    {
        private float screenWidth;
        private float screenHeight;
        private float zoom = 1.0f;
        private float aspect = 0.0f;
        const float SPEED = 5f;
        const float SCROLLSPEED = 350f;

        public Vector3 position;

        Vector3 up = Vector3.UnitY;
        Vector3 forward = -Vector3.UnitZ;
        Vector3 right = Vector3.UnitX;

        public Camera2D(Vector3 position)
        {
            screenWidth = Game.WindowData.ScreenWidth;
            screenHeight = Game.WindowData.ScreenHeight;
            aspect = screenHeight / screenWidth;
            this.position = position;
        }

        public override void OnResize(ResizeEventArgs args)
        {
            screenWidth = args.Width;
            screenHeight = args.Height;
            aspect = screenHeight / screenWidth;
        }

        public override void OnUpdate(FrameEventArgs args)
        {
            if (Input.IsKeyDown(Keys.W))
            {
                position += up * SPEED * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.S))
            {
                position += -up * SPEED * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.A))
            {
                position += -right * SPEED * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                position += right * SPEED * (float)args.Time;
            }

            //position += forward * Input.MouseScrollDelta.Y * SCROLLSPEED * (float)args.Time;
            zoom -= Input.MouseScrollDelta.Y * SCROLLSPEED * (float)args.Time;
            zoom = MathHelper.Clamp(zoom, 0.1f, 10.0f);
        }

        public override void OnRender(FrameEventArgs args)
        {
            UpdateView(Renderer2D.Data.DefaultShader.Program);
        }

        private void UpdateView(int shaderProgram)
        {
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = Matrix4.LookAt(position, position + forward, up);

            float orthoHeight = 1 * zoom;
            float orthoWidth = 1 / aspect * zoom;
            Matrix4 projection = Matrix4.CreateOrthographic(orthoWidth, orthoHeight, 0.1f, 100.0f);

            int modelLocation = GL.GetUniformLocation(shaderProgram, "model");
            int viewLocation = GL.GetUniformLocation(shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(shaderProgram, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);
        }
    }
}
