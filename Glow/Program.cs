using System;
using System.IO;
using System.Windows.Forms;
using static Glow.GlowExternalModules;

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