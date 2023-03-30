using System.Text;
using System.Runtime.InteropServices;

namespace Glow.glow_library{
    internal class GlowLangs{
        // ======================================================================================================
        // SAVE PATHS
        public static string glow_lang_folder = @"langs";                           // Main Path
        public static string glow_lang_tr = glow_lang_folder + @"\Turkish.ini";     // Turkish  | tr
        public static string glow_lang_en = glow_lang_folder + @"\English.ini";     // English  | en
        public static string glow_lang_zh = glow_lang_folder + @"\Chinese.ini";     // Chinese  | zh
        public static string glow_lang_hi = glow_lang_folder + @"\Hindi.ini";       // Hindi    | hi
        public static string glow_lang_es = glow_lang_folder + @"\Spanish.ini";     // Spanish  | es
        // ======================================================================================================
        // GLOW SETTINGS SAVE CLASS
        public class GlowGetLangs{
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            public GlowGetLangs(string file_path){ save_file_path = file_path; }
            private string save_file_path = glow_lang_folder;
            private string default_save_process{ get; set; }
            public string GlowReadLangs(string episode, string setting_name){
                default_save_process = default_save_process ?? string.Empty;
                StringBuilder str_builder = new StringBuilder(256);
                GetPrivateProfileString(episode, setting_name, default_save_process, str_builder, 255, save_file_path);
                return str_builder.ToString();
            }
        }
    }
}