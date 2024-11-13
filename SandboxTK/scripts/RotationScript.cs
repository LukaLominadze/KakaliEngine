using OpenTK.Windowing.Common;

namespace Game2D.game.components
{
    internal class RotationScript : Script
    {
        private float rotationSpeed = 0.0f;

        public override void Init(Transform transform, params object[]? args)
        {
            base.Init(transform, args);
            rotationSpeed = (float)args[0];
        }

        public override void OnUpdate(FrameEventArgs args)
        {
            Transform.Rotation += rotationSpeed * (float)args.Time;
        }
    }
}
