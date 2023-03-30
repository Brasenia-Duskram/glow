using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Glow.glow_library{
    internal class GlowSettings{
        // ======================================================================================================
        // SAVE PATHS
        public static string glow_df = @"C:\Users\" + SystemInformation.UserName + @"\AppData\Roaming\TürkaySoftware\Glow";
        public static string glow_sf = glow_df + @"\GlowSettings.ini";
        // ======================================================================================================
        // GLOW SETTINGS SAVE CLASS
        public class GlowSettingsSave{
            [DllImport("kernel32.dll")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            public GlowSettingsSave(string file_path) { save_file_path = file_path; }
            private string save_file_path = glow_sf;
            private string default_save_process { get; set; }
            public string GlowReadSettings(string episode, string setting_name){
                default_save_process = default_save_process ?? string.Empty;
                StringBuilder str_builder = new StringBuilder(256);
                GetPrivateProfileString(episode, setting_name, default_save_process, str_builder, 255, save_file_path);
                return str_builder.ToString();
            }
            public long GlowWriteSettings(string episode, string setting_name, string value){
                return WritePrivateProfileString(episode, setting_name, value, save_file_path);
            }
        }
    }
}