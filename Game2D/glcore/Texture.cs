using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Game2D.glcore
{
    internal class Texture
    {
        public int RendererID { get; private set; }

        public Texture(string filePath)
        {
            StbImage.stbi_set_flip_vertically_on_load(1);

            RendererID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, RendererID);

            ImageResult texture = ImageResult.FromStream(File.OpenRead(filePath), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, texture.Width, texture.Height,
                          0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.Data);

            GL.TextureStorage2D(RendererID, 1, SizedInternalFormat.Rgba8, texture.Width, texture.Height);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            Unbind();
        }

        public void Bind(int slot = 0)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + slot);
            GL.BindTexture(TextureTarget.Texture2D, RendererID);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Delete()
        {
            GL.DeleteTexture(RendererID);
        }
    }
}
