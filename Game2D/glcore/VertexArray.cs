using Game2D.core;
using OpenTK.Graphics.OpenGL4;

namespace Game2D.glcore
{
    public class VertexArray
    {
        public int RendererID { get; private set; }

        private VertexBuffer vbo;
        private IndexBuffer ibo;

        public VertexArray()
        {
            RendererID = Log.GLCall(GL.GenVertexArray);

            Log.GLCall(() => GL.BindVertexArray(RendererID));
        }

        public void AddBuffer(VertexBuffer vbo, VertexBufferLayout layout)
        {
            Log.GLCall(() => GL.BindVertexArray(RendererID));
            this.vbo = vbo;
            List<VertexBufferLayoutElements> elements = layout.Elements;
            uint offset = 0;
            for (uint i = 0; i < elements.Count; i++)
            {
                VertexBufferLayoutElements element = elements[(int)i];
                Log.GLCall(() => GL.EnableVertexAttribArray(i));
                Log.GLCall(() => GL.VertexAttribPointer(i, (int)element.Count, element.Type, element.Normalized,
                    (int)layout.Stride, (int)offset));
                offset += element.Count * VertexBufferLayoutElements.GetTypeSize(element.Type);
            }
        }

        public void AddIndexBuffer(IndexBuffer ibo)
        {
            this.ibo = ibo;
        }

        public void SetVertexBufferData(int size, float[] data, BufferUsageHint hint)
        {
            vbo.Bind();
            vbo.SetData(size, data, hint);
        }

        public void SetIndexBufferData(int size, uint[] data, BufferUsageHint hint)
        {
            ibo.Bind();
            ibo.SetData(size, data, hint);
        }

        public void Bind()
        {
            Log.GLCall(() => GL.BindVertexArray(RendererID));
            ibo.Bind();
        }

        public void Unbind()
        {
            Log.GLCall(() => GL.BindVertexArray(0));
            ibo.Unbind();
        }

        public void Delete()
        {
            Log.GLCall(() => GL.DeleteVertexArray(RendererID));
            ibo.Delete();
        }
    }
}
