using Game2D.glcore;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Game2D.game.components
{
    public class SpriteRenderer : Component
    {
        private Texture? texture;
        public Color4? Color;

        public SpriteRenderer() { }
        public SpriteRenderer(string filePath) { }

        public override void Init(Transform transform, params object[] args)
        {
            base.Init(transform, args);
            if (args.Length == 2)
            {
                texture = new Texture((string)args[0]);
                Color = (Color4)args[1];
            }
            else if (args.Length == 1)
            {
                texture = new Texture((string)args[0]);
                Color = Color4.White;
            }
            else
            {
                Color = Color4.White;
            }
        }

        public override void OnRender(FrameEventArgs args)
        {
            Renderer2D.DrawRotatedQuad(Transform.Position, Transform.Scale, Transform.Rotation, (Color4)Color, texture);
        }
    }
}
