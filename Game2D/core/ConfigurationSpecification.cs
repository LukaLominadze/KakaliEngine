using System.Runtime.InteropServices;

namespace Game2D.core
{
    public static class ConfigurationSpecification
    {
        [DllImport("kernel32.dll")]
        static extern nint GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(nint hWnd, int nCmdShow);

        public static void SetConsoleVisibility()
        {
#if DISTRIBUTION
                const int SW_SHOWORHIDE = 0; // Hide console
#else
            const int SW_SHOWORHIDE = 5; // Show console
#endif

            nint handle = GetConsoleWindow();
            ShowWindow(handle, SW_SHOWORHIDE);
        }
    }
}
