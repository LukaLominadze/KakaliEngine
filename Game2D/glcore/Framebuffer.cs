using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game2D.core;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Game2D.glcore
{
    public class Framebuffer
    {
        public int RendererID;
        public int ColorAttachment;
        public int DepthAttachment; 
        private int width, height; 
        public Framebuffer(int width, int height)
        {
            this.width = width;
            this.height = height;

            Log.GLCall(() => GL.CreateFramebuffers(1, out RendererID));
            Log.GLCall(() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, RendererID));

            Log.GLCall(() => GL.CreateTextures(TextureTarget.Texture2D, 1, out ColorAttachment));
            Log.GLCall(() => GL.BindTexture(TextureTarget.Texture2D, ColorAttachment));

            Log.GLCall(() => GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16, width, height,
                          0, PixelFormat.Rgba, PixelType.UnsignedByte, (nint)null));

            Log.GLCall(() => GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                             (int)TextureMinFilter.Nearest));
            Log.GLCall(() => GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                             (int)TextureMagFilter.Nearest));

            Log.GLCall(() => GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                             FramebufferAttachment.ColorAttachment0,
                             TextureTarget.Texture2D, ColorAttachment, 0));

            Log.GLCall(() => GL.CreateTextures(TextureTarget.Texture2D, 1, out DepthAttachment));
            Log.GLCall(() => GL.BindTexture(TextureTarget.Texture2D, DepthAttachment));
            Log.GLCall(() => GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8,
                             width, height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, (nint)null));

            Log.GLCall(() => GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                             FramebufferAttachment.DepthStencilAttachment,
                             TextureTarget.Texture2D, DepthAttachment, 0));

            Log.GLCall(() =>
            {
                if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                {
                    throw new InvalidOperationException("Framebuffer incomplete!");
                }
            }
            );

            Log.GLCall(() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0));
        }

        public void Bind()
        {
            Log.GLCall(() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, RendererID));
        }

        public void Unbind()
        {
            Log.GLCall(() => GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0));
        }
    }
}
