using OpenTK.Mathematics;

namespace Game2D.game
{
    public class Transform
    {
        public Vector3 Position = Vector3.Zero;
        public Vector2 Scale = Vector2.One;
        public float Rotation = 0.0f;

        public Transform() { }
        public Transform(Vector3 position, float rotation = 0.0f)
        {
            Position = position;
            Scale = Vector2.One;
            Rotation = rotation;
        }
        public Transform(Vector3 position, Vector2 scale, float rotation = 0.0f)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }
    }
}
