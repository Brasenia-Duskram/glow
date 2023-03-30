using System;
using System.Runtime.InteropServices;

namespace Glow.glow_library{
    internal class GlowThemes{
        // TITLE BAR SETTINGS DWM API
        // ======================================================================================================
        [DllImport("DwmApi")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
    }
}