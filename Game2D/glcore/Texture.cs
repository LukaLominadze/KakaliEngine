using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using Game2D.core;

namespace Game2D.glcore
{
    public class Texture
    {
        public int RendererID { get; private set; }

        public Texture(string filePath)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);

            RendererID = Log.GLCall(GL.GenTexture);
            Log.GLCall(() => GL.BindTexture(TextureTarget.Texture2D, RendererID));

            ImageResult texture = ImageResult.FromStream(File.OpenRead(filePath), ColorComponents.RedGreenBlueAlpha);
            Log.GLCall(() => GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16, texture.Width, texture.Height,
                          0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.Data));

            Log.GLCall(() => GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat));
            Log.GLCall(() => GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat));
            Log.GLCall(() => GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest));
            Log.GLCall(() => GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest));

            Unbind();
        }

        public void Bind(int slot = 0)
        {
            Log.GLCall(() => GL.ActiveTexture(TextureUnit.Texture0 + slot));
            Log.GLCall(() => GL.BindTexture(TextureTarget.Texture2D, RendererID));
        }

        public void Unbind()
        {
            Log.GLCall(() => GL.BindTexture(TextureTarget.Texture2D, 0));
        }

        public void Delete()
        {
            Log.GLCall(() => GL.DeleteTexture(RendererID));
        }
    }
}
