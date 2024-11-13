using Game2D.core;
using Game2D.layers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game2D.glcore
{
    internal class Camera3D : Layer
    {
        private float screenWidth;
        private float screenHeight;
        const float SPEED = 5f;
        const float SENSITIVITY = 20f;

        public Vector3 position;

        Vector3 up = Vector3.UnitY;
        Vector3 forward = -Vector3.UnitZ;
        Vector3 right = Vector3.UnitX;

        private float pitch;
        private float yaw = -90f;

        public Camera3D(Vector3 position)
        {
            screenWidth = Game.WindowData.ScreenWidth;
            screenHeight = Game.WindowData.ScreenHeight;
            this.position = position;
        }

        public override void OnResize(ResizeEventArgs args)
        {
            screenWidth = args.Width;
            screenHeight = args.Height;
        }

        public override void OnUpdate(FrameEventArgs args)
        {
            if (Input.IsMousePressed(MouseButton.Button1))
            {
                Game.WindowData.SetCursorState(CursorState.Grabbed);
            }
            else if (Input.IsKeyPressed(Keys.Escape))
            {
                Game.WindowData.SetCursorState(CursorState.Normal);
            }

            if (Input.IsKeyDown(Keys.W))
            {
                position += forward * SPEED * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.S))
            {
                position += -forward * SPEED * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.A))
            {
                position += -right * SPEED * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.D))
            {
                position += right * SPEED * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.Space))
            {
                position += up * SPEED * (float)args.Time;
            }
            if (Input.IsKeyDown(Keys.LeftShift))
            {
                position += -up * SPEED * (float)args.Time;
            }

            if (Game.WindowData.GetCursorState() == CursorState.Normal)
            {
                return;
            }

            yaw += Input.MouseDelta.X * SENSITIVITY * (float)args.Time;
            pitch -= Input.MouseDelta.Y * SENSITIVITY * (float)args.Time;

            UpdateVectors();
        }

        public override void OnRender(FrameEventArgs args)
        {
            UpdateView(Renderer2D.Data.DefaultShader.Program);
        }

        private void UpdateView(int shaderProgram)
        {
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = Matrix4.LookAt(position, position + forward, up);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), screenWidth / screenHeight, 0.1f, 100.0f);

            int modelLocation = GL.GetUniformLocation(shaderProgram, "model");
            int viewLocation = GL.GetUniformLocation(shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(shaderProgram, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);
        }

        private void UpdateVectors()
        {
            if (pitch > 89.0f)
            {
                pitch = 89.0f;
            }
            if (pitch < -89.0f)
            {
                pitch = -89.0f;
            }

            forward.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            forward.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            forward.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

            forward = Vector3.Normalize(forward);

            right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, forward));
        }
    }
}
