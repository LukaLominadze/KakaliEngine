using Game2D.core;
using OpenTK.Graphics.OpenGL4;

namespace Game2D.glcore
{
    internal class VertexBuffer
    {
        public int RendererID { get; private set; }

        public VertexBuffer(int size, float[] data, BufferUsageHint hint)
        {
            RendererID = Log.GLCall(GL.GenBuffer);
            Log.GLCall(() => GL.BindBuffer(BufferTarget.ArrayBuffer, RendererID));
            Log.GLCall(() => GL.BufferData(BufferTarget.ArrayBuffer, size, data, hint));
        }

        public void SetData(int size, float[] data, BufferUsageHint hint)
        {
            Log.GLCall(() => GL.BufferData(BufferTarget.ArrayBuffer, size, data, hint));
        }

        public void Bind()
        {
            Log.GLCall(() => GL.BindBuffer(BufferTarget.ArrayBuffer, RendererID));
        }

        public void Unbind()
        {
            Log.GLCall(() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0));
        }

        public void Delete()
        {
            Log.GLCall(() => GL.DeleteBuffer(RendererID));
        }
    }
}
