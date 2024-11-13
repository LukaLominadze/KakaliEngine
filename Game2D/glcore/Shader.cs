using Game2D.core;
using OpenTK.Graphics.OpenGL4;

namespace Game2D.glcore
{
    internal class Shader
    {
        private int vertexShader;
        private int fragmentShader;
        public int Program { get; private set; }

        public Shader(string filePath)
        {
            vertexShader = Log.GLCall(() => GL.CreateShader(ShaderType.VertexShader));
            fragmentShader = Log.GLCall(() => GL.CreateShader(ShaderType.FragmentShader));

            string[] shaderSources = ShaderUtility.ParseShader(filePath);

            Log.DebugLog(shaderSources[0]);
            Log.DebugLog("------------------------------------------------");
            Log.DebugLog(shaderSources[1]);

            Log.GLCall(() => GL.ShaderSource(vertexShader, shaderSources[0]));
            Log.GLCall(() => GL.ShaderSource(fragmentShader, shaderSources[1]));

            Log.GLCall(() => GL.CompileShader(vertexShader));
            Log.GLCall(() => GL.CompileShader(fragmentShader));

            Program = Log.GLCall(GL.CreateProgram);
        }

        public void Bind()
        {
            Log.GLCall(() => GL.AttachShader(Program, vertexShader));
            Log.GLCall(() => GL.AttachShader(Program, fragmentShader));
        }

        public void Unbind()
        {
            Log.GLCall(() => GL.DetachShader(Program, vertexShader));
            Log.GLCall(() => GL.DetachShader(Program, fragmentShader));
        }

        public void Use()
        {
            Log.GLCall(() => GL.LinkProgram(Program));
            Log.GLCall(() => GL.UseProgram(Program));
        }

        public void Delete()
        {
            Unbind();

            Log.GLCall(() => GL.DeleteProgram(Program));
        }

        public void SetUniform4(string uniform, float v0, float v1, float v2, float v3)
        {
            Log.GLCall(() => GL.Uniform4(GetUniformLocation(uniform), v0, v1, v2, v3));
        }

        public int GetUniformLocation(string uniform)
        {
            return Log.GLCall(() => GL.GetUniformLocation(Program, uniform));
        }
    }

    internal static class ShaderUtility
    {
        public static string[] ParseShader(string filePath)
        {
            string[] shaderSources = { "", "" };

            const int NONE = -1, VERTEX = 0, FRAGMENT = 1;
            int shaderType = NONE;

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("type"))
                    {
                        if (line.Contains("vertex"))
                        {
                            shaderType = VERTEX;
                        }
                        else if (line.Contains("fragment"))
                        {
                            shaderType = FRAGMENT;
                        }
                    }
                    shaderSources[shaderType] += (line + "\n");
                }
            }
            return shaderSources;
        }
    }
}
