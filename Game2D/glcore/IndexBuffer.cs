using Game2D.core;
using OpenTK.Graphics.OpenGL4;

namespace Game2D.glcore
{
    internal class IndexBuffer
    {
        public int RendererID { get; private set; }

        public IndexBuffer(int size, uint[] data, BufferUsageHint hint)
        {
            RendererID = Log.GLCall(GL.GenBuffer);
            Log.GLCall(() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, RendererID));
            Log.GLCall(() => GL.BufferData(BufferTarget.ElementArrayBuffer, size, data, hint));
        }

        public void SetData(int size, uint[] data, BufferUsageHint hint)
        {
            Log.GLCall(() => GL.BufferData(BufferTarget.ElementArrayBuffer, size, data, hint));
        }

        public void Bind()
        {
            Log.GLCall(() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, RendererID));
        }

        public void Unbind()
        {
            Log.GLCall(() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0));
        }

        public void Delete()
        {
            Log.GLCall(() => GL.DeleteBuffer(RendererID));
        }
    }
}
