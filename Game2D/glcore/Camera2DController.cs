using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game2D.core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Game2D.glcore
{
    public class Camera2DController : Camera2D
    {
        const float SPEED = 5f;
        const float SCROLLSPEED = 350f;

        public Camera2DController(Vector3 position) : base(position) { }

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
    }
}
