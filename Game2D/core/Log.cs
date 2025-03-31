using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Game2D.core
{
    public static class Log
    {
        [Conditional("DEBUG")]
        [Conditional("RELEASE")]
        public static void DebugLog<T>(T message)
        {
            Console.WriteLine(message);
        }
#if !(DISTRIBUTION)
        public static T GLCall<T>(Func<T> function,
                                 [CallerMemberName] string funcName = "",
                                 [CallerFilePath] string filePath = "",
                                 [CallerLineNumber] int line = 0)
        {
            T value = function.Invoke();
            GLLogCall(funcName, filePath, line);
            return value;
        }

        public static void GLCall(Action function,
                                 [CallerMemberName] string funcName = "",
                                 [CallerFilePath] string filePath = "",
                                 [CallerLineNumber] int line = 0)
        {
            function.Invoke();
            GLLogCall(funcName, filePath, line);
        }

        private static void GLLogCall(string funcName, string filePath, int line)
        {
            ErrorCode error;
            while ((error = GL.GetError()) != ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error ({error}): {funcName}, {filePath}, {line}");
            }
        }
#else
        public static T GLCall<T>(Func<T> function)
        {
            return function.Invoke();
        }

        public static void GLCall(Action function)
        {
            function.Invoke();
        }
#endif
    }
}
