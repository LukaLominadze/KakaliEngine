using OpenTK.Windowing.Common;

namespace Game2D.layers
{
    public class Layer
    {
        public virtual void OnAttach() { }
        public virtual void OnUpdate(FrameEventArgs args) { }
        public virtual void OnRender(FrameEventArgs args) { }
        public virtual void OnResize(ResizeEventArgs args) { }
        public virtual void OnDetach() { }
    }
}
