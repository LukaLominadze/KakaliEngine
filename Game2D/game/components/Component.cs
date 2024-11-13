using Game2D.layers;

namespace Game2D.game.components
{
    public class Component : Layer
    {
        public Transform? Transform;

        public Component() { }

        public virtual void Init(Transform transform, params object[]? args)
        {
            Transform = transform;
        }
    }
}
