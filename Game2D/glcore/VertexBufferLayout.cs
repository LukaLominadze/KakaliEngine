using OpenTK.Graphics.OpenGL4;

namespace Game2D.glcore
{
    internal class VertexBufferLayout
    {
        private uint stride = 0;
        private List<VertexBufferLayoutElements> elements = new List<VertexBufferLayoutElements>();
        public uint Stride { get => stride; }
        public List<VertexBufferLayoutElements> Elements { get => elements; }

        public void Push<T>(uint count) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                elements.Add(new VertexBufferLayoutElements { Type = VertexAttribPointerType.Float, Count = count, Normalized = false });
                stride += count * VertexBufferLayoutElements.GetTypeSize(VertexAttribPointerType.Float);
            }
            else if (typeof(T) == typeof(uint))
            {
                elements.Add(new VertexBufferLayoutElements { Type = VertexAttribPointerType.UnsignedInt, Count = count, Normalized = false });
                stride += count * VertexBufferLayoutElements.GetTypeSize(VertexAttribPointerType.UnsignedInt);
            }
            else if (typeof(T) == typeof(int))
            {
                elements.Add(new VertexBufferLayoutElements { Type = VertexAttribPointerType.Int, Count = count, Normalized = false });
                stride += count * VertexBufferLayoutElements.GetTypeSize(VertexAttribPointerType.Int);
            }
            else if (typeof(T) == typeof(char))
            {
                elements.Add(new VertexBufferLayoutElements { Type = VertexAttribPointerType.UnsignedByte, Count = count, Normalized = false });
                stride += count * VertexBufferLayoutElements.GetTypeSize(VertexAttribPointerType.UnsignedByte);
            }
            else
            {
                Console.WriteLine("Incorrect type used on generic method VertexBufferLayout.Push<T>");
            }
        }

    }

    internal struct VertexBufferLayoutElements
    {
        public VertexAttribPointerType Type;
        public uint Count;
        public bool Normalized;

        public static uint GetTypeSize(VertexAttribPointerType type)
        {
            switch (type)
            {
                case VertexAttribPointerType.Float:
                    return sizeof(float);
                case VertexAttribPointerType.UnsignedInt:
                    return sizeof(uint);
                case VertexAttribPointerType.Int:
                    return sizeof(int);
                case VertexAttribPointerType.UnsignedByte:
                    return sizeof(byte);
                default:
                    Console.WriteLine("Wrong vertex attrib pointer type!");
                    return 0;
            }
        }
    };
}
