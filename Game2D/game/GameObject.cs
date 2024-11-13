using Game2D.game.components;
using Game2D.layers;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Game2D.game
{
    public class GameObject : Layer
    {
        public Transform Transform;
        private List<Component> components;

        public GameObject(Vector3? position = null, Vector2? scale = null, float rotation = 0.0f)
        {
            if (position == null || scale == null)
            {
                position = Vector3.Zero;
                scale = Vector2.One;
            }
            Transform = new Transform((Vector3)position, (Vector2)scale, rotation);
            components = new List<Component>();
        }

        public T AddComponent<T>(params object[]? parameters) where T : Component
        {
            foreach (Component component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    return null;
                }
            }
            components.Add((T)Activator.CreateInstance(typeof(T)));
            components.Last().Init(Transform, parameters);
            return (T)components.Last();
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in components)
            {
                if (component.GetType() == typeof(T))
                {
                    return (T)component;
                }
            }
            return null;
        }

        public override void OnAttach()
        {
            foreach (Component component in components)
            {
                component.OnAttach();
            }
        }

        public override void OnUpdate(FrameEventArgs args)
        {
            foreach (Component component in components)
            {
                component.OnUpdate(args);
            }
        }

        public override void OnRender(FrameEventArgs args)
        {
            foreach (Component component in components)
            {
                component.OnRender(args);
            }
        }

        public override void OnDetach()
        {
            foreach (Component component in components)
            {
                component.OnDetach();
            }
        }
    }
}
