using Game2D.layers;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Game2D.game.scenes
{
    public class Scene : Layer
    {
        protected List<GameObject> gameObjects = new List<GameObject>();

        public GameObject Instantiate(Vector3? position = null, Vector2? scale = null, float rotation = 0.0f)
        {
            gameObjects.Add(new GameObject(position, scale, rotation));
            return gameObjects.Last();
        }

        public override void OnAttach()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.OnAttach();
            }
        }

        public override void OnUpdate(FrameEventArgs args)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.OnUpdate(args);
            }
        }

        public override void OnRender(FrameEventArgs args)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.OnRender(args);
            }
        }

        public override void OnDetach()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.OnDetach();
            }
        }
    }
}
