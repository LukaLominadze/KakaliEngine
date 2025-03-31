using Game2D.game.components;
using Game2D.glcore;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using Game2D.core;

namespace Game2D.game.scenes
{
    internal class SandboxScene : Scene
    {
        private float rotationSpeed = 10.0f;
        private int current = 0;
        private Framebuffer? framebuffer;

        public override void OnAttach()
        {
            base.OnAttach();

            GameObject rect = Instantiate(new Vector3(0f, 0f, -1.0f), new Vector2(0.5f, 3.4f));
            rect.AddComponent<SpriteRenderer>().Color = Color4.MediumPurple;
            rect.AddComponent<RotationScript>(rotationSpeed);

            GameObject luigi = Instantiate(new Vector3(0f, 0f, -0.9f), new Vector2(0.8f, 0.8f));
            luigi.AddComponent<SpriteRenderer>("../../../res/textures/luigi.png");
            luigi.AddComponent<RotationScript>(rotationSpeed / 2.0f);

            framebuffer = new Framebuffer((int)Game2D.core.Game.WindowData.ScreenWidth, (int)Game2D.core.Game.WindowData.ScreenHeight);
        }

        public override void OnRender(FrameEventArgs args)
        {
            base.OnRender(args);
            framebuffer!.Bind();
            Renderer2D.BeginScene();
            Renderer2D.Clear();
            for (int i = 0; i < 52; i++)
            {
                Renderer2D.DrawQuad(new Vector3(i * 0.23f, 0f, -0.11f), new Vector2(0.2f, 1.0f), new Color4(255, 255, 255, 255));
            }
            int j = 5;
            for (int i = 0; i < 51; i++)
            {
                if (j != 2 && j != 6 )
                {
                    Renderer2D.DrawQuad(new Vector3((i * 0.23f) + 0.115f, 0.15f, -0.1f), new Vector2(0.2f, 0.7f), Color4.Black);
                }
                if (++j > 6)
                {
                    j = 0;
                }
            }
            Renderer2D.EndScene();
            framebuffer.Unbind();
        }

        public override void OnImGuiRender()
        {
            base.OnImGuiRender();

            ImGui.Begin("Test");
            string[] devices = { "MyDevice", "MyMidi", "SomeOther" };
            ImGui.Combo("Some Label", ref current, devices, devices.Length);
            ImGui.Image(framebuffer!.ColorAttachment,
                        new System.Numerics.Vector2(Game.WindowData.ScreenWidth, Game.WindowData.ScreenHeight),
                        new System.Numerics.Vector2(0, 1), new System.Numerics.Vector2(1, 0));
            ImGui.End();
            
        }
    }
}
