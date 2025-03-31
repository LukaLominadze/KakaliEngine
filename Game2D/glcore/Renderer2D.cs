using Game2D.core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Game2D.glcore
{
    public static class Renderer2D
    {
        private static Renderer2DData s_data = new Renderer2DData();
        public static Renderer2DData Data { get => s_data; }

        public static void Initialize()
        {
            Log.DebugLog("Initializing Renderer...");

            Log.GLCall(() => GL.Enable(EnableCap.Blend));
            Log.GLCall(() => GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha));

            Log.DebugLog("Enabled blending");

            Log.GLCall(() => GL.Enable(EnableCap.DepthTest));
            Log.GLCall(() => GL.DepthFunc(DepthFunction.Less));

            Log.DebugLog("Enabled Depth Testing");

            s_data.QuadVertexArray = new VertexArray();
            VertexBuffer vbo = new VertexBuffer(Renderer2DData.MaxVertices * sizeof(float),
                                                s_data.Vertices, BufferUsageHint.DynamicDraw);
            VertexBufferLayout layout = new VertexBufferLayout();
            layout.Push<float>(3); // Position
            layout.Push<float>(4); // Color
            layout.Push<float>(2); // TexCoords
            layout.Push<float>(1); // Texture slot
            s_data.QuadVertexArray.AddBuffer(vbo, layout);

            IndexBuffer ibo = new IndexBuffer(Renderer2DData.MaxIndicies * sizeof(uint), s_data.Indicies,
                                              BufferUsageHint.DynamicDraw);
            s_data.QuadVertexArray.AddIndexBuffer(ibo);

            Log.DebugLog("Created Quad Vertex Array");

            s_data.DefaultShader = new Shader("../../../res/shaders/default.glsl");
            s_data.DefaultShader.Bind();
            s_data.DefaultShader.Use();

            Log.DebugLog("Created Shader");

            s_data.Textures[s_data.TextureSlotIndex] = new Texture("../../../res/textures/flat.png");
            s_data.Textures[s_data.TextureSlotIndex++].Bind(0);

            int[] textureUnits = new int[16];
            for (int i = 0; i < textureUnits.Length; i++)
                textureUnits[i] = i; // Texture slots from 0 to 15

            int location = Log.GLCall(() => s_data.DefaultShader.GetUniformLocation("u_Textures"));
            Log.GLCall(() => GL.Uniform1(location, textureUnits.Length, textureUnits));

            Log.DebugLog("Initialized Renderer!");

        }

        public static void SetClearColor(Color4 color)
        {
            Log.GLCall(() => GL.ClearColor(color.R, color.G, color.B, color.A));
        }

        public static void Clear()
        {
            Log.GLCall(() => GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        }

        public static void DrawQuad(Vector3 position, Vector2 size, Color4 color, Texture? texture = null)
        {
            FlushIfExceededMaxVertexCount(QuadVertex.FullElementCount);

            int texSlot = SetTextureAndGetSlot(texture);

            Vector2 halfSize = new Vector2(size.X / 2.0f, size.Y / 2.0f);
            float[] quad =
            {
                position.X - halfSize.X, position.Y - halfSize.Y, position.Z, color.R, color.G, color.B, color.A, 0.0f, 0.0f, texSlot,
                position.X - halfSize.X, position.Y + halfSize.Y, position.Z, color.R, color.G, color.B, color.A, 0.0f, 1.0f, texSlot,
                position.X + halfSize.X, position.Y + halfSize.Y, position.Z, color.R, color.G, color.B, color.A, 1.0f, 1.0f, texSlot,
                position.X + halfSize.X, position.Y - halfSize.Y, position.Z, color.R, color.G, color.B, color.A, 1.0f, 0.0f, texSlot
            };
            int vertexIndex = s_data.CurrentVertex / QuadVertex.VertexElementCount;
            int[] indicies =
            {
                vertexIndex + 0, vertexIndex + 1, vertexIndex + 2,
                vertexIndex + 2, vertexIndex + 3, vertexIndex + 0
            };
            Array.Copy(quad, 0, s_data.Vertices, s_data.CurrentVertex, quad.Length);
            Array.Copy(indicies, 0, s_data.Indicies, s_data.CurrentIndex, indicies.Length);
            s_data.CurrentVertex += quad.Length;
            s_data.CurrentIndex += indicies.Length;
        }

        public static void DrawRotatedQuad(Vector3 position, Vector2 size, float rotation, Color4 color, Texture? texture = null)
        {
            FlushIfExceededMaxVertexCount(QuadVertex.FullElementCount);

            int texSlot = SetTextureAndGetSlot(texture);

            float angleRadians = MathHelper.DegreesToRadians(rotation);
            Quaternion rotationMatrix = Quaternion.FromAxisAngle(Vector3.UnitZ, angleRadians);

            Vector2 halfSize = new Vector2(size.X / 2.0f, size.Y / 2.0f);

            Vector3 bottomLeft = new Vector3(-halfSize.X, -halfSize.Y, 0.0f);
            Vector3 topLeft = new Vector3(-halfSize.X, halfSize.Y, 0.0f);
            Vector3 topRight = new Vector3(halfSize.X, halfSize.Y, 0.0f);
            Vector3 bottomRight = new Vector3(halfSize.X, -halfSize.Y, 0.0f);

            topLeft = position + Vector3.Transform(topLeft, rotationMatrix);
            topRight = position + Vector3.Transform(topRight, rotationMatrix);
            bottomLeft = position + Vector3.Transform(bottomLeft, rotationMatrix);
            bottomRight = position + Vector3.Transform(bottomRight, rotationMatrix);

            float[] quad =
            {
                bottomLeft.X, bottomLeft.Y, position.Z, color.R, color.G, color.B, color.A, 0.0f, 0.0f, texSlot,
                topLeft.X, topLeft.Y, position.Z, color.R, color.G, color.B, color.A, 0.0f, 1.0f, texSlot,
                topRight.X, topRight.Y, position.Z, color.R, color.G, color.B, color.A, 1.0f, 1.0f, texSlot,
                bottomRight.X, bottomRight.Y, position.Z, color.R, color.G, color.B, color.A, 1.0f, 0.0f, texSlot
            };
            int vertexIndex = s_data.CurrentVertex / QuadVertex.VertexElementCount;
            int[] indicies =
            {
                vertexIndex + 0, vertexIndex + 1, vertexIndex + 2,
                vertexIndex + 2, vertexIndex + 3, vertexIndex + 0
            };
            Array.Copy(quad, 0, s_data.Vertices, s_data.CurrentVertex, quad.Length);
            Array.Copy(indicies, 0, s_data.Indicies, s_data.CurrentIndex, indicies.Length);
            s_data.CurrentVertex += quad.Length;
            s_data.CurrentIndex += indicies.Length;
        }

        public static void BeginScene()
        {
            s_data.QuadVertexArray.Bind();
        }

        public static void EndScene()
        {
            Flush();
        }

        private static void Flush()
        {
            s_data.QuadVertexArray.SetVertexBufferData(s_data.CurrentVertex * sizeof(float),
                                              s_data.Vertices, BufferUsageHint.DynamicDraw);
            s_data.QuadVertexArray.SetIndexBufferData(s_data.CurrentIndex * sizeof(uint),
                                              s_data.Indicies, BufferUsageHint.DynamicDraw);

            for (int i = 0; i < s_data.TextureSlotIndex; i++)
            {
                 s_data.Textures[i].Bind(i);
            }

            Log.GLCall(() => GL.DrawElements(PrimitiveType.Triangles, s_data.CurrentIndex, DrawElementsType.UnsignedInt, 0));

            for (int i = 0; i < s_data.CurrentVertex; i++)
            {
                s_data.Vertices[i] = 0;
            }
            for (int i = 1; i < s_data.TextureSlotIndex; i++)
            {
                s_data.Textures[i] = null;
            }
            s_data.CurrentVertex = 0;
            s_data.CurrentIndex = 0;
            s_data.TextureSlotIndex = 1;
        }

        public static void FlushIfExceededMaxVertexCount(int count)
        {
            if (s_data.ExceededMaxVertexCount(count))
            {
                Flush();
            }
        }

        public static void Deinitialize()
        {
            s_data.QuadVertexArray.Delete();
            s_data.DefaultShader.Delete();
        }

        private static int SetTextureAndGetSlot(Texture? texture)
        {
            int texSlot = 0;
            if (texture != null)
            {
                if (s_data.Textures.Length > 16)
                {
                    Flush();
                }
                texSlot = s_data.TextureSlotIndex;
                s_data.Textures[s_data.TextureSlotIndex] = texture;
                texture.Bind(s_data.TextureSlotIndex++);
            }
            return texSlot;
        }
    }

    public class Renderer2DData
    {
        public const int MaxQuads = 10000;
        public const int MaxVertices = MaxQuads * QuadVertex.FullElementCount;
        public const int MaxIndicies = MaxQuads * 6;

        public float[] Vertices = new float[MaxVertices];
        public uint[] Indicies = new uint[MaxIndicies];

        public int CurrentVertex = 0;
        public int CurrentIndex = 0;

        public VertexArray QuadVertexArray;
        public Shader DefaultShader;

        public Texture[] Textures = new Texture[16];
        public int TextureSlotIndex = 0;

        public uint QuadIndexCount = 0;
        public QuadVertex QuadVertexBufferBase;

        public Renderer2DData() { }

        public bool ExceededMaxVertexCount(int elementCount)
        {
            if (CurrentVertex + elementCount > MaxVertices)
            {
                return true;
            }
            return false;
        }
    }

    public struct QuadVertex
    {
        public const int VertexCount = 4;
        public const int VertexElementCount = 10;
        public const int FullElementCount = VertexCount * VertexElementCount;
        public const int IndexCount = 6;
    }
}
