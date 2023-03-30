using System;
using System.IO;
using System.Windows.Forms;
using static Glow.glow_library.GlowSettings;
using static Glow.glow_library.GlowDisplaySettings;

namespace Glow{
    static class Program{
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main(){
            if (Environment.OSVersion.Version.Major >= 6){ SetProcessDPIAware(); }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!Directory.Exists(glow_df)){ Directory.CreateDirectory(glow_df); }
            Application.Run(new Glow());
        }
    }
}