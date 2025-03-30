using Game2D.game.components;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Game2D.game.scenes
{
    internal class SandboxScene : Scene
    {
        private float rotationSpeed = 10.0f;

        public override void OnAttach()
        {
            base.OnAttach();

            GameObject rect = Instantiate(new Vector3(0f, 0f, -1.0f), new Vector2(0.5f, 3.4f));
            rect.AddComponent<SpriteRenderer>().Color = Color4.MediumPurple;
            rect.AddComponent<RotationScript>(rotationSpeed);

            GameObject luigi = Instantiate(new Vector3(0f, 0f, -0.9f), new Vector2(0.8f, 0.8f));
            luigi.AddComponent<SpriteRenderer>("../../../res/textures/luigi.png");
            luigi.AddComponent<RotationScript>(rotationSpeed / 2.0f);
        }
    }
}
