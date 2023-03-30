using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Management;
using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
using System.Runtime.InteropServices;
using static Glow.glow_library.GlowSettings;
using static Glow.glow_library.GlowThemes;
using static Glow.glow_library.GlowLangs;
using static Glow.glow_library.GlowDisplaySettings;

namespace Glow{
    public partial class Glow : Form{
        public Glow(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // ======================================================================================================
        // GLOBAL INT / STRING
        public static int lang, theme;
        public static string lang_path = "";
        // LOCAL INT / STRING / BOOL
        int menu_btns = 1, menu_rp = 1;
        string wp_rotate, wp_resoulation;
        readonly string ts_website = "https://www.turkaysoftware.com";
        readonly string glow_github = "https://github.com/turkaysoftware/glow";
        bool loop_status = true;
        // ARCHITECTURAL
        // ======================================================================================================
        List<string> architectures = new List<string>(){ "64 Bit", "32 Bit", "ARM" };
        public static List<string> architectures_detail = new List<string>(){ "64 Bit (x64)", "32 Bit (x86)", "ARM64" };
        // ======================================================================================================
        // COLOR MODES
        List<Color> ui_colors = new List<Color>();
        List<Color> btn_colors = new List<Color>(){ Color.FromArgb(235, 235, 235), Color.WhiteSmoke, Color.FromArgb(37, 37, 43), Color.FromArgb(53, 53, 61) };
        static List<Color> header_colors = new List<Color>(){ Color.FromArgb(210, 210, 210), Color.FromArgb(53, 53, 61) };
        static List<Color> header_arrows = new List<Color>(){ Color.FromArgb(53, 53, 61), Color.FromArgb(210, 210, 210) };
        // ======================================================================================================
        // HEADER SETTINGS
        private class WhiteTheme : ToolStripProfessionalRenderer{
            public WhiteTheme() : base(new WhiteColors()){ }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e){ e.ArrowColor = header_arrows[0]; base.OnRenderArrow(e); }
        }
        private class WhiteColors : ProfessionalColorTable{
            public override Color MenuItemSelected{ get { return header_colors[0]; } }
            public override Color ToolStripDropDownBackground{ get { return header_colors[0]; } }
            public override Color ImageMarginGradientBegin{ get { return header_colors[0]; } }
            public override Color ImageMarginGradientEnd{ get { return header_colors[0]; } }
            public override Color ImageMarginGradientMiddle{ get { return header_colors[0]; } }
            public override Color MenuItemSelectedGradientBegin{ get { return header_colors[0]; } }
            public override Color MenuItemSelectedGradientEnd{ get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientBegin{ get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientMiddle{ get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientEnd{ get { return header_colors[0]; } }
            public override Color MenuItemBorder{ get { return header_colors[0]; } }
            public override Color CheckBackground{ get { return header_colors[0]; } }
            public override Color ButtonSelectedBorder{ get { return header_colors[0]; } }
            public override Color CheckSelectedBackground{ get { return header_colors[0]; } }
            public override Color CheckPressedBackground{ get { return header_colors[0]; } }
            public override Color MenuBorder{ get { return header_colors[0]; } }
        }
        private class DarkTheme : ToolStripProfessionalRenderer{
            public DarkTheme() : base(new DarkColors()){ }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e){ e.ArrowColor = header_arrows[1]; base.OnRenderArrow(e); }
        }
        private class DarkColors : ProfessionalColorTable{
            public override Color MenuItemSelected { get { return header_colors[1]; } }
            public override Color ToolStripDropDownBackground { get { return header_colors[1]; } }
            public override Color ImageMarginGradientBegin { get { return header_colors[1]; } }
            public override Color ImageMarginGradientEnd { get { return header_colors[1]; } }
            public override Color ImageMarginGradientMiddle { get { return header_colors[1]; } }
            public override Color MenuItemSelectedGradientBegin { get { return header_colors[1]; } }
            public override Color MenuItemSelectedGradientEnd { get { return header_colors[1]; } }
            public override Color MenuItemPressedGradientBegin { get { return header_colors[1]; } }
            public override Color MenuItemPressedGradientMiddle { get { return header_colors[1]; } }
            public override Color MenuItemPressedGradientEnd { get { return header_colors[1]; } }
            public override Color MenuItemBorder { get { return header_colors[1]; } }
            public override Color CheckBackground { get { return header_colors[1]; } }
            public override Color ButtonSelectedBorder { get { return header_colors[1]; } }
            public override Color CheckSelectedBackground { get { return header_colors[1]; } }
            public override Color CheckPressedBackground { get { return header_colors[1]; } }
            public override Color MenuBorder{ get { return header_colors[1]; } }
        }
        // ======================================================================================================
        // TOOLTIP SETTINGS
        private void MainToolTip_Draw(object sender, DrawToolTipEventArgs e){ e.DrawBackground(); e.DrawBorder(); e.DrawText(); }
        // ======================================================================================================
        // GLOW LOAD
        private void Glow_Load(object sender, EventArgs e){
            // PRELOAD SETTINGS
            Text = Application.ProductName + " " + Application.ProductVersion.Substring(0, 4) + " - " + architectures[0];
            KeyPreview = true; KeyDown += new KeyEventHandler(Glow_KeyUp);
            HeaderMenu.Cursor = Cursors.Hand;
            // GLOW LAUNCH PROCESS
            glow_load_check_langs();
            glow_load_langs_settings();
            glow_load_tasks();
        }
        // ======================================================================================================
        // GLOW CHECK LANGS FOLDER & FILES
        private void glow_load_check_langs(){
            try{
                if (Directory.Exists(glow_lang_folder)){
                    int get_langs_file = Directory.GetFiles(glow_lang_folder, "*.ini", SearchOption.AllDirectories).Length;
                    if (get_langs_file > 0){
                        // TR
                        if (!File.Exists(glow_lang_tr)){
                            türkçeToolStripMenuItem.Enabled = false;
                        }
                        // EN
                        if (!File.Exists(glow_lang_en)){
                            englishToolStripMenuItem.Enabled = false;
                        }
                        // ZH
                        if (!File.Exists(glow_lang_zh)){
                            çinceToolStripMenuItem.Enabled = false;
                        }
                        // HI
                        if (!File.Exists(glow_lang_hi)){
                            hintçeToolStripMenuItem.Enabled = false;
                        }
                        // ES
                        if (!File.Exists(glow_lang_es)){
                            ispanyolcaToolStripMenuItem.Enabled = false;
                        }
                    }else{
                        MessageBox.Show("No language files were found.\n\nThe program is closing.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Exit();
                    }
                }else{
                    MessageBox.Show("Langs folder not found.\n\nThe program is closing.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }catch (Exception){ }
        }
        // ======================================================================================================
        // GLOW LOAD LANGS SETTINGS
        private void GetGlowSetting(){
            GlowSettingsSave glow_theme_read = new GlowSettingsSave(glow_sf);
            string theme_mode = glow_theme_read.GlowReadSettings("Theme", "ThemeStatus");
            if (theme_mode == "light"){
                color_mode(1);
                açıkTemaToolStripMenuItem.Checked = true;
            }else if (theme_mode == "dark"){
                color_mode(2);
                koyuTemaToolStripMenuItem.Checked = true;
            }else{
                color_mode(1);
                açıkTemaToolStripMenuItem.Checked = true;
            }
            dgv_columns_add();
            GlowSettingsSave glow_lang_read = new GlowSettingsSave(glow_sf);
            string lang_mode = glow_lang_read.GlowReadSettings("Language", "LanguageStatus");
            switch (lang_mode){
                case "tr":
                    lang_engine("tr");
                    türkçeToolStripMenuItem.Checked = true;
                    break;
                case "en":
                    lang_engine("en");
                    englishToolStripMenuItem.Checked = true;
                    break;
                case "zh":
                    lang_engine("zh");
                    çinceToolStripMenuItem.Checked = true;
                    break;
                case "hi":
                    lang_engine("hi");
                    hintçeToolStripMenuItem.Checked = true;
                    break;
                case "es":
                    lang_engine("es");
                    ispanyolcaToolStripMenuItem.Checked = true;
                    break;
                default:
                    lang_engine("en");
                    englishToolStripMenuItem.Checked = true;
                    break;
            }
        }
        private void glow_load_langs_settings(){
            try{
                if (File.Exists(glow_sf)){
                    GetGlowSetting();
                }else{
                    // DETECT SYSTEM THEME
                    GlowSettingsSave glow_setting_save_theme = new GlowSettingsSave(glow_sf);
                    string get_system_theme = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "").ToString().Trim();
                    switch (get_system_theme){
                        case "1":
                            // Light
                            glow_setting_save_theme.GlowWriteSettings("Theme", "ThemeStatus", "light");
                            break;
                        case "0":
                            // Dark
                            glow_setting_save_theme.GlowWriteSettings("Theme", "ThemeStatus", "dark");
                            break;
                        default:
                            // Other
                            glow_setting_save_theme.GlowWriteSettings("Theme", "ThemeStatus", "light");
                            break;
                    }
                    // DETECT SYSTEM LANG
                    string culture_lang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.Trim();
                    GlowSettingsSave glow_lang_write = new GlowSettingsSave(glow_sf);
                    switch (culture_lang){
                        case "tr":
                            glow_lang_write.GlowWriteSettings("Language", "LanguageStatus", "tr");
                            break;
                        case "en":
                            glow_lang_write.GlowWriteSettings("Language", "LanguageStatus", "en");
                            break;
                        case "zh":
                            glow_lang_write.GlowWriteSettings("Language", "LanguageStatus", "zh");
                            break;
                        case "hi":
                            glow_lang_write.GlowWriteSettings("Language", "LanguageStatus", "hi");
                            break;
                        case "es":
                            glow_lang_write.GlowWriteSettings("Language", "LanguageStatus", "es");
                            break;
                        default:
                            glow_lang_write.GlowWriteSettings("Language", "LanguageStatus", "en");
                            break;
                    }
                    GetGlowSetting();
                }
            }catch (Exception){ }
        }
        // ======================================================================================================
        // GLOW TASK ALL PROCESS
        private void glow_load_tasks(){
            // START OS TASK
            Task task_os = new Task(os);
            task_os.Start();
            // START MOTHERBOARD TASK
            Task task_mb = new Task(mb);
            task_mb.Start();
            // START CPU TASK
            Task task_cpu = new Task(cpu);
            task_cpu.Start();
            // START RAM TASK
            Task task_ram = new Task(ram);
            task_ram.Start();
            // START GPU TASK
            Task task_gpu = new Task(gpu);
            task_gpu.Start();
            // START DISK TASK
            Task task_disk = new Task(disk);
            task_disk.Start();
            // START NETWORK TASK
            Task task_network = new Task(network);
            task_network.Start();
            // START SOUND TASK
            Task task_sound = new Task(sound);
            task_sound.Start();
            // START USB TASK
            Task task_usb = new Task(usb);
            task_usb.Start();
            // START BATTERY TASK
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){ battery_visible_off(); /*DESKTOP*/ }
            else{
                battery_visible_on(); /*LAPTOP*/
                Task task_battery = new Task(battery);
                task_battery.Start();
                Task task_laptop_bg = new Task(laptop_bg_process);
                task_laptop_bg.Start();
            }
            // START OSD TASK
            Task task_osd = new Task(osd);
            task_osd.Start();
            // START GS SERVICES TASK
            Task task_gs_services = new Task(gs_services);
            task_gs_services.Start();
            // OS ASYNC BG TASK
            Task task_os_bg = new Task(os_bg_process);
            task_os_bg.Start();
            // CPU ASYNC BG TASK
            Task cpu_usage_bg = new Task(cpu_bg_process);
            cpu_usage_bg.Start();
            // RAM ASYNC STARTER
            Task task_ram_bg = new Task(ram_bg_process);
            task_ram_bg.Start();
        }
        // ======================================================================================================
        // ALL DATAGRID SETTINGS
        private void dgv_columns_add(){
            // INSTALLED DRIVERS
            DataMainTable.Columns.Add("osd_1", "x");
            DataMainTable.Columns.Add("osd_2", "x");
            DataMainTable.Columns.Add("osd_3", "x");
            DataMainTable.Columns.Add("osd_4", "x");
            DataMainTable.Columns.Add("osd_5", "x");
            // SERVICES
            ServicesDataGrid.Columns.Add("ser_1", "x");
            ServicesDataGrid.Columns.Add("ser_2", "x");
            ServicesDataGrid.Columns.Add("ser_3", "x");
            ServicesDataGrid.Columns.Add("ser_4", "x");
            ServicesDataGrid.Columns.Add("ser_5", "x");
            settings_ui_widths();
        }
        private void settings_ui_widths(){
            int c1 = 180, c2 = 243, c3 = 180, c4 = 85, c5 = 85;
            // INSTALLED DRIVERS
            DataMainTable.Columns[0].Width = c1;
            DataMainTable.Columns[1].Width = c2;
            DataMainTable.Columns[2].Width = c3;
            DataMainTable.Columns[3].Width = c4;
            DataMainTable.Columns[4].Width = c5;
            DataMainTable.ClearSelection();
            // SERVICES
            ServicesDataGrid.Columns[0].Width = c1;
            ServicesDataGrid.Columns[1].Width = c2;
            ServicesDataGrid.Columns[2].Width = c3;
            ServicesDataGrid.Columns[3].Width = c4;
            ServicesDataGrid.Columns[4].Width = c5;
            ServicesDataGrid.ClearSelection();
            // PANEL WIDTH SETTINGS
            int global_width_1 = 785;
            int global_width_2 = 802;
            int global_width_3 = 801;
            // GW 1
            os_panel_1.Width = global_width_1;
            os_panel_2.Width = global_width_1;
            os_panel_3.Width = global_width_1;
            os_panel_4.Width = global_width_1;
            // GW 2
            mb_panel_1.Width = global_width_2;
            mb_panel_2.Width = global_width_2;
            cpu_panel_1.Width = global_width_2;
            cpu_panel_2.Width = global_width_2;
            ram_panel_1.Width = global_width_2;
            ram_panel_2.Width = global_width_2;
            gpu_panel_1.Width = global_width_2;
            gpu_panel_2.Width = global_width_2;
            disk_panel_1.Width = global_width_2;
            disk_panel_2.Width = global_width_2;
            // GW 3
            network_panel_1.Width = global_width_3;
            sound_panel_1.Width = global_width_3;
            usb_panel_1.Width = global_width_3;
            battery_panel_1.Width = global_width_3;
            osd_panel_1.Width = global_width_3;
            service_panel_1.Width = global_width_3;
        }
        // ======================================================================================================
        // OPERATING SYSTEM
        private void os(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_cs = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystemProduct");
            ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher search_desktop = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Desktop");
            try{
                // SYSTEM USER
                SystemUser_V.Text = SystemInformation.UserName;
            }catch (Exception){ }
            try{
                // PC NAME
                ComputerName_V.Text = SystemInformation.ComputerName;
            }catch (Exception){ }
            try{
                foreach (ManagementObject query_cs_rotate in search_cs.Get()){
                    // SYSTEM MODEL
                    SystemModel_V.Text = Convert.ToString(query_cs_rotate["Name"]);
                }
            }catch (Exception){ }
            foreach (ManagementObject query_os_rotate in search_os.Get()){
                try{
                    // REGISTERED USER
                    OS_SavedUser_V.Text = Convert.ToString(query_os_rotate["RegisteredUser"]);
                }catch (Exception){ }
                try{
                    // OS NAME
                    OSName_V.Text = Convert.ToString(query_os_rotate["Caption"]);
                }catch (Exception){ }
                try{
                    // OS MANUFACTURER
                    OSManufacturer_V.Text = Convert.ToString(query_os_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // OS VERSION
                    string os_version_display_version = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", "").ToString().Trim();
                    string os_version_release_id = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString().Trim();
                    if (os_version_display_version != string.Empty && os_version_release_id != string.Empty && os_version_display_version != "" && os_version_release_id != ""){
                        SystemVersion_V.Text = os_version_display_version + " - " + os_version_release_id;
                    }else{
                        SystemVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_1").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // OS BUILD NUMBER
                    object os_build_num = query_os_rotate["Version"];
                    OSBuild_V.Text = os_build_num.ToString();
                }catch (Exception){ }
                try{
                    // OS ARCHITECTURE
                    string system_bit = Convert.ToString(query_os_rotate["OSArchitecture"]).Replace("bit", "");
                    SystemArchitectural_V.Text = system_bit.Trim() + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_2").Trim())) + " - " + string.Format("(x{0})", system_bit.Trim());
                }catch (Exception){ }
                try{
                    // OS FAMILY
                    OSFamily_V.Text = new ComputerInfo().OSPlatform.Trim();
                }catch (Exception){ }
                try{
                    // OS SERIAL
                    OS_Serial_V.Text = Convert.ToString(query_os_rotate["SerialNumber"]);
                }catch (Exception){ }
                try{
                    // SYSTEM LANGUAGE
                    CultureInfo culture_info = CultureInfo.InstalledUICulture;
                    OS_Country_V.Text = culture_info.DisplayName.Trim();
                }catch (Exception){ }
                try{
                    // OS CHARACTER SET
                    object os_code_set = query_os_rotate["CodeSet"];
                    OS_CharacterSet_V.Text = os_code_set.ToString() + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_3").Trim()));
                }catch (Exception){ }
                try{
                    // OS ENCRYPTION BIT VALUE
                    OS_EncryptionType_V.Text = Convert.ToString(query_os_rotate["EncryptionLevel"]) + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_2").Trim()));
                }catch (Exception){ }
                try{
                    // WINDOWS DIRECTORY
                    object windows_dir = query_os_rotate["WindowsDirectory"];
                    SystemRootIndex_V.Text = windows_dir.ToString().Replace("WINDOWS", "Windows") + @"\";
                }catch (Exception){ }
                try{
                    // BUILD PARTITON
                    object system_yapi_partition = query_os_rotate["SystemDirectory"];
                    SystemBuildPart_V.Text = system_yapi_partition.ToString().Replace("WINDOWS", "Windows") + @"\";
                }catch (Exception){ }
                try{
                    // SYSTEM INSTALL DATE
                    string os_install_date = Convert.ToString(query_os_rotate["InstallDate"]);
                    string os_id_year = os_install_date.Substring(0, 4);
                    string os_id_month = os_install_date.Substring(4, 2);
                    string os_id_day = os_install_date.Substring(6, 2);
                    string os_id_hour = os_install_date.Substring(8, 2);
                    string os_id_minute = os_install_date.Substring(10, 2);
                    DateTime format_date = new DateTime(Convert.ToInt32(os_id_year), Convert.ToInt32(os_id_month), Convert.ToInt32(os_id_day));
                    DateTime now_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan remaining_day = now_date - format_date;
                    double last_day = remaining_day.TotalDays;
                    string os_id_date = os_id_day + "." + os_id_month + "." + os_id_year + " - " + os_id_hour + ":" + os_id_minute;
                    OS_Install_V.Text = os_id_date + " - ( " + last_day.ToString() + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_4").Trim())) + " )";
                }catch (Exception){ }
                try{
                    // OS LAST BOOT
                    string last_bt = Convert.ToString(query_os_rotate["LastBootUpTime"]);
                    string last_bt_year = last_bt.Substring(0, 4);
                    string last_bt_month = last_bt.Substring(4, 2);
                    string last_bt_day = last_bt.Substring(6, 2);
                    string last_bt_hour = last_bt.Substring(8, 2);
                    string last_bt_minute = last_bt.Substring(10, 2);
                    string last_bt_process = last_bt_day + "." + last_bt_month + "." + last_bt_year + " - " + last_bt_hour + ":" + last_bt_minute;
                    LastBootTime_V.Text = last_bt_process;
                }catch (Exception){ }
                try{
                    // PORTABLE OS STATUS
                    bool system_portable_status = Convert.ToBoolean(query_os_rotate["PortableOperatingSystem"]);
                    if (system_portable_status == true){
                        PortableOS_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_5").Trim()));
                    }else if (system_portable_status == false){
                        PortableOS_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_6").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // BOOT PARTITION
                    object boot_device = query_os_rotate["BootDevice"];
                    string boot_device_1 = Convert.ToString(boot_device).Replace(@"\Device\", "");
                    string boot_device_2 = boot_device_1.Replace("HarddiskVolume", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_7").Trim())) + " - ");
                    BootPartition_V.Text = boot_device_2.Trim();
                }catch (Exception){ }
                try{
                    // SYSTEM PARTITION
                    object system_device = query_os_rotate["SystemDevice"];
                    string system_device_1 = Convert.ToString(system_device).Replace(@"\Device\", "");
                    string system_device_2 = system_device_1.Replace("HarddiskVolume", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_8").Trim())) + " - ");
                    SystemPartition_V.Text = system_device_2.ToString();
                }catch (Exception){ }
            }
            try{
                // GET WALLPAPER
                foreach (ManagementObject query_desktop_rotate in search_desktop.Get()){
                    string get_wallpaper = Convert.ToString(query_desktop_rotate["Wallpaper"]);
                    if (get_wallpaper != "" && get_wallpaper != string.Empty){
                        wp_rotate = get_wallpaper;
                        if (File.Exists(get_wallpaper)){
                            // GET WALLPAPER RESOULATION
                            using (var wallpaper_res = new FileStream(get_wallpaper, FileMode.Open, FileAccess.Read, FileShare.Read)){
                                using (var wallpaper_res_x64 = Image.FromStream(wallpaper_res, false, false)){
                                    wp_resoulation = wallpaper_res_x64.Width + "x" + wallpaper_res_x64.Height;
                                }
                            }
                            // GET WALLPAPER SIZE
                            FileInfo wallpaper_size = new FileInfo(get_wallpaper);
                            double wallpaper_size_x64 = Convert.ToDouble(wallpaper_size.Length) / 1024;
                            if (wallpaper_size_x64 > 1024){
                                GPUWallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.00} MB", wallpaper_size_x64 / 1024);
                            }else if (wallpaper_size_x64 < 1024){
                                GPUWallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0} KB", wallpaper_size_x64);
                            }
                            MainToolTip.SetToolTip(GoWallpaperRotate, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_29").Trim())), wp_rotate));
                        }else{
                            GPUWallpaper_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_9").Trim()));
                            GoWallpaperRotate.Visible = false;
                        }
                    }
                }
                if (GPUWallpaper_V.Text == "N/A" || GPUWallpaper_V.Text.Trim() == "" || GPUWallpaper_V.Text == string.Empty){
                    GPUWallpaper_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_10").Trim()));
                    GoWallpaperRotate.Visible = false;
                }
            }catch (Exception){ }
        }
        private void os_bg_process(){
            try{
                // DESCRIPTIVE
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                do{
                    foreach (ManagementObject query_os_rotate in search_os.Get()){
                        // FREE VIRTUAL RAM
                        double free_sanal_ram = Convert.ToDouble(query_os_rotate["FreeVirtualMemory"]) / 1024 / 1024;
                        EmptyVirtualRam_V.Text = String.Format("{0:0.00} GB", free_sanal_ram);
                        // GET LAST BOOT
                        string last_boot = Convert.ToString(query_os_rotate["LastBootUpTime"]);
                        // Year - Month - Day - Hour - Minute - Second
                        int year = Convert.ToInt32(last_boot.Substring(0, 4));
                        int month = Convert.ToInt32(last_boot.Substring(4, 2));
                        int day = Convert.ToInt32(last_boot.Substring(6, 2));
                        int hour = Convert.ToInt32(last_boot.Substring(8, 2));
                        int minute = Convert.ToInt32(last_boot.Substring(10, 2));
                        int second = Convert.ToInt32(last_boot.Substring(12, 2));
                        // Convert DateTime
                        var boot_date_x64 = new DateTime(year, month, day, hour, minute, second);
                        // Current Date and Hour
                        var now_date = DateTime.Now;
                        // System Uptime
                        var system_uptime = now_date.Subtract(boot_date_x64);
                        string su_days = system_uptime.Days + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_11").Trim()));
                        string su_hours = system_uptime.Hours + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_12").Trim()));
                        string su_minutes = system_uptime.Minutes + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_13").Trim()));
                        string su_seconds = system_uptime.Seconds + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_14").Trim()));
                        string system_uptime_x64 = string.Format("{0}, {1}, {2}, {3}", su_days, su_hours, su_minutes, su_seconds);
                        SystemWorkTime_V.Text = system_uptime_x64;
                    }
                    // SYSTEM TIME
                    SystemTime_V.Text = DateTime.Now.ToString("dd.MM.yyyy - H:mm:ss");
                    // SYSTEM WORK TIME
                    try{
                        // MOUSE WHEEL SPEED
                        int mouse_wheel_speed = new Computer().Mouse.WheelScrollLines;
                        MouseWheelStatus_V.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_15").Trim())), mouse_wheel_speed);
                    }catch (Exception){ }
                    try{
                        // SCROLL LOCK STATUS
                        bool scroll_lock_status = new Computer().Keyboard.ScrollLock;
                        if (scroll_lock_status == true){
                            ScrollLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (scroll_lock_status == false){
                            ScrollLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    try{
                        // NUMLOCK STATUS
                        bool numlock_status = new Computer().Keyboard.NumLock;
                        if (numlock_status == true){
                            NumLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (numlock_status == false){
                            NumLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    try{
                        // CAPSLOCK STATUS
                        bool capslock_status = new Computer().Keyboard.CapsLock;
                        if (capslock_status == true){
                            CapsLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (capslock_status == false){
                            CapsLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        private void GoWallpaperRotate_Click(object sender, EventArgs e){
            try{ string wallpaper_start_path = Convert.ToString(wp_rotate).Trim(); Process.Start(wallpaper_start_path); }
            catch (Exception){
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_18").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // MB
        // ======================================================================================================
        private void mb(){
            ManagementObjectSearcher search_bb = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            ManagementObjectSearcher search_bios = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
            ManagementObjectSearcher search_md = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");
            foreach (ManagementObject query_bb_rotate in search_bb.Get()){
                try{
                    // MB NAME
                    MotherBoardName_V.Text = Convert.ToString(query_bb_rotate["Product"]);
                }catch (Exception){ }
                try{
                    // MB MAN
                    MotherBoardMan_V.Text = Convert.ToString(query_bb_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // MB SERIAL
                    MotherBoardSerial_V.Text = Convert.ToString(query_bb_rotate["SerialNumber"]);
                }catch (Exception){ }
                try{
                    // MB VERSION
                    MB_Model_V.Text = Convert.ToString(query_bb_rotate["Version"]);
                }catch (Exception){ }
            }
            foreach (ManagementObject query_bios_rotate in search_bios.Get()){
                try{
                    // BIOS MAN
                    BiosManufacturer_V.Text = Convert.ToString(query_bios_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // BIOS DATE
                    string bios_date = Convert.ToString(query_bios_rotate["ReleaseDate"]);
                    string b_date_render = bios_date.Substring(6, 2) + "." + bios_date.Substring(4, 2) + "." + bios_date.Substring(0, 4);
                    BiosDate_V.Text = b_date_render;
                }catch (Exception){ }
                try{
                    // BIOS VERSION
                    BiosVersion_V.Text = Convert.ToString(query_bios_rotate["Caption"]);
                }catch (Exception){ }
                try{
                    // SM BIOS VERSION
                    SmBiosVersion_V.Text = Convert.ToString(query_bios_rotate["Version"]);
                }catch (Exception){ }
                try{
                    // BIOS MAJOR MINOR
                    object bios_major = query_bios_rotate["SystemBiosMajorVersion"];
                    object bios_minor = query_bios_rotate["SystemBiosMinorVersion"];
                    BiosMajorMinor_V.Text = bios_major.ToString() + "." + bios_minor.ToString();
                }catch (Exception){ }
                try{
                    // SM-BIOS MAJOR MINOR
                    object sm_bios_major = query_bios_rotate["SMBIOSMajorVersion"];
                    object sm_bios_minor = query_bios_rotate["SMBIOSMinorVersion"];
                    SMBiosMajorMinor_V.Text = sm_bios_major.ToString() + "." + sm_bios_minor.ToString();
                }catch (Exception){ }
            }
            try{
                foreach (ManagementObject query_md_rotate in search_md.Get()){
                    // PRIMARY BUS TYPE
                    PrimaryBusType_V.Text = Convert.ToString(query_md_rotate["PrimaryBusType"]);
                }
            }catch (Exception){ }
        }
        // CPU
        // ======================================================================================================
        List<double> cpu_l_sizes = new List<double>();
        private void cpu(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_process = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject query_process_rotate in search_process.Get()){
                try{
                    // CPU NAME
                    CPUName_V.Text = Convert.ToString(query_process_rotate["Name"]).Trim();
                }catch (Exception){ }
                try{
                    // CPU MANUFACTURER
                    string cpu_man = Convert.ToString(query_process_rotate["Manufacturer"]);
                    string cpu_man_sv = "GenuineIntel";
                    bool cpu_man_search = cpu_man.Contains(cpu_man_sv);
                    if (cpu_man_search == true){
                        MB_Chipset_V.Text = "Intel";
                        CPUManufacturer_V.Text = "Intel Corporation";
                    }else{
                        MB_Chipset_V.Text = "AMD";
                        CPUManufacturer_V.Text = cpu_man;
                    }
                }catch (Exception){ }
                try{
                    // CPU ARCHITECTURE
                    int cpu_arch_num = Convert.ToInt32(query_process_rotate["Architecture"]);
                    string[] cpu_architectures = { "32 " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_1").Trim())) + " - (x86)", "MIPS", "ALPHA", "POWER PC", "ARM", "IA64", "64 " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_1").Trim())) + " - (x64)" };
                    if (cpu_arch_num == 0){
                        CPUArchitectural_V.Text = cpu_architectures[0];
                    }else if (cpu_arch_num == 1){
                        CPUArchitectural_V.Text = cpu_architectures[1];
                    }else if (cpu_arch_num == 2){
                        CPUArchitectural_V.Text = cpu_architectures[2];
                    }else if (cpu_arch_num == 3){
                        CPUArchitectural_V.Text = cpu_architectures[3];
                    }else if (cpu_arch_num == 5){
                        CPUArchitectural_V.Text = cpu_architectures[4];
                    }else if (cpu_arch_num == 6){
                        CPUArchitectural_V.Text = cpu_architectures[5];
                    }else if (cpu_arch_num == 9){
                        CPUArchitectural_V.Text = cpu_architectures[6];
                    }else{
                        CPUArchitectural_V.Text = cpu_arch_num.ToString();
                    }
                }catch (Exception){ }
                try{
                    // CPU NORMAL SPEED
                    double cpu_speed = Convert.ToDouble(query_process_rotate["CurrentClockSpeed"]);
                    if (cpu_speed > 1024){
                        CPUNormalSpeed_V.Text = String.Format("{0:0.00} GHz", cpu_speed / 1000);
                    }else{
                        CPUNormalSpeed_V.Text = cpu_speed.ToString() + " MHz";
                    }
                }catch (Exception){ }
                try{
                    // CPU DEFAULT SPEED
                    double cpu_max_speed = Convert.ToDouble(query_process_rotate["MaxClockSpeed"]);
                    if (cpu_max_speed > 1024){
                        CPUDefaultSpeed_V.Text = String.Format("{0:0.00} GHz", cpu_max_speed / 1000);
                    }else{
                        CPUDefaultSpeed_V.Text = cpu_max_speed.ToString() + " MHz";
                    }
                }catch (Exception){ }
                try{
                    // CPU CORES
                    CPUCoreCount_V.Text = Convert.ToString(query_process_rotate["NumberOfCores"]);
                }catch (Exception){ }
                try{
                    // CPU LOGICAL CORES
                    string thread_count = Convert.ToString(query_process_rotate["ThreadCount"]);
                    if (thread_count == String.Empty || thread_count == ""){
                        CPULogicalCore_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_2").Trim()));
                    }else{
                        CPULogicalCore_V.Text = thread_count;
                    }
                }catch (Exception){ }
                try{
                    // CPU SOCKET
                    CPUSocketDefinition_V.Text = Convert.ToString(query_process_rotate["SocketDesignation"]);
                }catch (Exception){ }
                try{
                    // CPU FAMILY
                    string cpu_description = Convert.ToString(query_process_rotate["Description"]);
                    string cpu_tanim = cpu_description.Replace("Family", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_3").Trim())));
                    string cpu_tanim_2 = cpu_tanim.Replace("Model", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_4").Trim())));
                    string cpu_tanim_3 = cpu_tanim_2.Replace("Stepping", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_5").Trim())));
                    string cpu_tanim_4 = cpu_tanim_3.Replace("64", " X64");
                    CPUFamily_V.Text = cpu_tanim_4;
                }catch (Exception){ }
                try{
                    // CPU VIRTUALIZATION
                    bool cpu_virtual_mod = Convert.ToBoolean(query_process_rotate["VirtualizationFirmwareEnabled"]);
                    if (cpu_virtual_mod == true){
                        CPUVirtualization_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_6").Trim()));
                    }else if (cpu_virtual_mod == false){
                        CPUVirtualization_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_7").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // CPU VIRTUALIZATION MONITOR EXTENSIONS
                    bool vm_monitor_ext = Convert.ToBoolean(query_process_rotate["VMMonitorModeExtensions"]);
                    if (vm_monitor_ext == true){
                        CPUVMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_8").Trim()));
                    }else if (vm_monitor_ext == false){
                        CPUVMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_9").Trim()));
                    }else{
                        CPUVMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_10").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // CPU SERIAL ID
                    CPUSerialName_V.Text = Convert.ToString(query_process_rotate["ProcessorId"]);
                }catch (Exception){ }
            }
            for (int l_level = 3; l_level <= 5; l_level++){
                ManagementObjectSearcher search_cm = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_CacheMemory WHERE Level = {l_level}");
                foreach (ManagementObject query_cm_rotate in search_cm.Get()){
                    cpu_l_sizes.Add(Convert.ToDouble(query_cm_rotate["MaxCacheSize"]));
                }
            }
            try{
                // L1 CACHE
                double l1_size = cpu_l_sizes[0];
                if (l1_size >= 1024){
                    CPU_L1_V.Text = (l1_size / 1024).ToString() + " MB";
                }else{
                    CPU_L1_V.Text = l1_size.ToString() + " KB";
                }
            }catch (Exception){ }
            try{
                // L2 CACHE
                double l2_size = cpu_l_sizes[1];
                if (l2_size >= 1024){
                    CPU_L2_V.Text = (l2_size / 1024).ToString() + " MB";
                }else{
                    CPU_L2_V.Text = l2_size.ToString() + " KB";
                }
            }catch (Exception){ }
            try{
                // L3 CACHE
                CPU_L3_V.Text = cpu_l_sizes[2] / 1024 + " MB";
            }catch (Exception){ }
            // WRITE ENGINE ENABLED
            bilgileriYazdırToolStripMenuItem.Enabled = true;
        }
        private void cpu_bg_process(){
            try{
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                CPUUsage_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_11").Trim()));
                do{
                    PerformanceCounter cpu_usage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    dynamic zero_value = cpu_usage.NextValue();
                    Thread.Sleep(1500);
                    dynamic last_value = cpu_usage.NextValue();
                    CPUUsage_V.Text = string.Format("%{0:0.0}", last_value);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        // RAM
        // ======================================================================================================
        List<string> ram_slot_count = new List<string>();
        List<string> ram_slot_list = new List<string>();
        List<string> ram_amount_list = new List<string>();
        List<string> ram_type_list = new List<string>();
        List<string> ram_frekans_list = new List<string>();
        List<string> ram_voltage_list = new List<string>();
        List<string> ram_form_factor = new List<string>();
        List<string> ram_serial_list = new List<string>();
        List<string> ram_manufacturer_list = new List<string>();
        List<string> ram_bank_label_list = new List<string>();
        List<string> ram_data_width_list = new List<string>();
        List<string> bellek_type_list = new List<string>();
        List<string> ram_part_number_list = new List<string>();
        private void ram(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher search_pm = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
            try{
                // TOTAL RAM
                ComputerInfo main_query = new ComputerInfo();
                ulong total_ram_x64_tick = ulong.Parse(main_query.TotalPhysicalMemory.ToString());
                double total_ram_isle = total_ram_x64_tick / (1024 * 1024);
                if (total_ram_isle > 1024){
                    if ((total_ram_isle / 1024) > 1024){
                        TotalRAM_V.Text = String.Format("{0:0.00} TB", total_ram_isle / 1024 / 1024);
                    }else{
                        TotalRAM_V.Text = String.Format("{0:0.00} GB", total_ram_isle / 1024);
                    }
                }else{
                    TotalRAM_V.Text = total_ram_isle.ToString() + " MB";
                }
            }catch (Exception){ }
            try{
                foreach (ManagementObject query_os_rotate in search_os.Get()){
                    // TOTAL VIRTUAL RAM
                    TotalVirtualRam_V.Text = String.Format("{0:0.00} GB", Convert.ToDouble(query_os_rotate["TotalVirtualMemorySize"]) / 1024 / 1024);
                }
            }catch (Exception){ }
            foreach (ManagementObject queryObj in search_pm.Get()){
                try{
                    // RAM AMOUNT
                    string ram_count = Convert.ToString(queryObj["BankLabel"]);
                    if (ram_count == "" || ram_count == string.Empty){
                        ram_slot_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_1").Trim())));
                    }else{
                        ram_slot_list.Add(ram_count);
                    }
                }catch (Exception){ }
                try{
                    // RAM SLOT COUNT
                    ram_slot_count.Add(Convert.ToString(queryObj["Capacity"]));
                    RamSlotStatus_V.Text = ram_slot_count.Count + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_2").Trim()));
                }catch (Exception){ }
                try{
                    // RAM CAPACITY
                    double ram_amount = Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024;
                    if (ram_amount > 1024){
                        ram_amount_list.Add(ram_amount / 1024 + " GB");
                    }else{
                        ram_amount_list.Add(ram_amount + " MB");
                    }
                    RamMik_V.Text = ram_amount_list[0];
                }catch (Exception){ }
                try{
                    // MEMORY TYPE
                    int sm_bios_memory_type = Convert.ToInt32(queryObj["SMBIOSMemoryType"]);
                    int memory_type = Convert.ToInt32(queryObj["MemoryType"]);
                    if (sm_bios_memory_type == 1 || memory_type == 1){
                        ram_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_3").Trim())));
                    }else if (sm_bios_memory_type == 2 || memory_type == 2){
                        ram_type_list.Add("DRAM");
                    }else if (sm_bios_memory_type == 3 || memory_type == 3){
                        ram_type_list.Add("Synchronous DRAM");
                    }else if (sm_bios_memory_type == 4 || memory_type == 4){
                        ram_type_list.Add("Cache Ram");
                    }else if (sm_bios_memory_type == 5 || memory_type == 5){
                        ram_type_list.Add("EDO");
                    }else if (sm_bios_memory_type == 6 || memory_type == 6){
                        ram_type_list.Add("EDRAM");
                    }else if (sm_bios_memory_type == 7 || memory_type == 7){
                        ram_type_list.Add("VRAM");
                    }else if (sm_bios_memory_type == 8 || memory_type == 8){
                        ram_type_list.Add("SRAM");
                    }else if (sm_bios_memory_type == 9 || memory_type == 9){
                        ram_type_list.Add("RAM");
                    }else if (sm_bios_memory_type == 10 || memory_type == 10){
                        ram_type_list.Add("ROM");
                    }else if (sm_bios_memory_type == 11 || memory_type == 11){
                        ram_type_list.Add("FLASH");
                    }else if (sm_bios_memory_type == 12 || memory_type == 12){
                        ram_type_list.Add("EEPROM");
                    }else if (sm_bios_memory_type == 13 || memory_type == 13){
                        ram_type_list.Add("FEPROM");
                    }else if (sm_bios_memory_type == 14 || memory_type == 14){
                        ram_type_list.Add("EPROM");
                    }else if (sm_bios_memory_type == 15 || memory_type == 15){
                        ram_type_list.Add("CDRAM");
                    }else if (sm_bios_memory_type == 16 || memory_type == 16){
                        ram_type_list.Add("3DRAM");
                    }else if (sm_bios_memory_type == 17 || memory_type == 17){
                        ram_type_list.Add("SDRAM");
                    }else if (sm_bios_memory_type == 18 || memory_type == 18){
                        ram_type_list.Add("SGRAM");
                    }else if (sm_bios_memory_type == 19 || memory_type == 19){
                        ram_type_list.Add("RDRAM");
                    }else if (sm_bios_memory_type == 20 || memory_type == 20){
                        ram_type_list.Add("DDR");
                    }else if (sm_bios_memory_type == 21 || memory_type == 21){
                        ram_type_list.Add("DDR2");
                    }else if (sm_bios_memory_type == 22 || memory_type == 22){
                        ram_type_list.Add("DDR2 FB-DIMM");
                    }else if (sm_bios_memory_type == 24 || memory_type == 24){
                        ram_type_list.Add("DDR3");
                    }else if (sm_bios_memory_type == 25 || memory_type == 25){
                        ram_type_list.Add("FBD2");
                    }else if (sm_bios_memory_type == 26 || memory_type == 26){
                        ram_type_list.Add("DDR4");
                    }else if (sm_bios_memory_type == 27 || memory_type == 27){
                        ram_type_list.Add("DDR5");
                    }else if (sm_bios_memory_type == 0 || memory_type == 0){
                        ram_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_4").Trim())));
                    }
                    RamType_V.Text = ram_type_list[0];
                }catch (Exception){ }
                try{
                    // RAM SPEED
                    ram_frekans_list.Add(Convert.ToInt32(queryObj["Speed"]) + " MHz");
                    RamFrequency_V.Text = ram_frekans_list[0];
                }catch (Exception){ }
                try{
                    // RAM VOLTAGE
                    string ram_voltaj = Convert.ToString(queryObj["ConfiguredVoltage"]);
                    if (ram_voltaj == "" || ram_voltaj == "0" || ram_voltaj == "0.0" || ram_voltaj == "0.00" || ram_voltaj == string.Empty){
                        ram_voltage_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_5").Trim())));
                    }else{
                        ram_voltage_list.Add(String.Format("{0:0.00} " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_6").Trim())), Convert.ToInt32(ram_voltaj) / 1000.0));
                    }
                    RamVolt_V.Text = ram_voltage_list[0];
                }catch (Exception){ }
                try{
                    // FORM FACTOR
                    int form_factor = Convert.ToInt32(queryObj["FormFactor"]);
                    if (form_factor == 8){
                        ram_form_factor.Add("DIMM");
                    }else if (form_factor == 12){
                        ram_form_factor.Add("SO-DIMM");
                    }else if (form_factor == 0){
                        ram_form_factor.Add("SO-DIMM / " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_7").Trim())));
                    }else{
                        ram_form_factor.Add(form_factor.ToString());
                    }
                    RamFormFactor_V.Text = ram_form_factor[0];
                }catch (Exception){ }
                try{
                    // RAM SERIAL
                    string ram_serial = Convert.ToString(queryObj["SerialNumber"]).Trim();
                    if (ram_serial == "00000000"){
                        ram_serial_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_8").Trim())));
                    }else if (ram_serial == "" || ram_serial == "Unknown" || ram_serial == "unknown" || ram_serial == string.Empty){
                        ram_serial_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_9").Trim())));
                    }else{
                        ram_serial_list.Add(ram_serial);
                    }
                    RamSerial_V.Text = ram_serial_list[0];
                }catch (Exception){ }
                try{
                    // RAM MAN
                    string ram_man = Convert.ToString(queryObj["Manufacturer"]).Trim();
                    if (ram_man == "" || ram_man == string.Empty){
                        ram_manufacturer_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_10").Trim())));
                    }else if (ram_man == "017A"){
                        ram_manufacturer_list.Add("Apacer");
                    }else if (ram_man == "059B"){
                        ram_manufacturer_list.Add("Crucial");
                    }else if (ram_man == "04CD"){
                        ram_manufacturer_list.Add("G.Skill");
                    }else if (ram_man == "0198"){
                        ram_manufacturer_list.Add("HyperX");
                    }else if (ram_man == "029E"){
                        ram_manufacturer_list.Add("Corsair");
                    }else if (ram_man == "04CB"){
                        ram_manufacturer_list.Add("A-DATA");
                    }else if (ram_man == "00CE"){
                        ram_manufacturer_list.Add("Samsung");
                    }else{
                        ram_manufacturer_list.Add(ram_man);
                    }
                    RamManufacturer_V.Text = ram_manufacturer_list[0];
                }catch (Exception){ }
                try{
                    // RAM BANK LABEL
                    string bank_label = Convert.ToString(queryObj["BankLabel"]);
                    if (bank_label == "" || bank_label == string.Empty){
                        ram_bank_label_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_11").Trim())));
                    }else{
                        ram_bank_label_list.Add(bank_label);
                    }
                    RamBankLabel_V.Text = ram_bank_label_list[0];
                }catch (Exception){ }
                try{
                    // RAM TOTAL WIDTH
                    string ram_data_width = Convert.ToString(queryObj["TotalWidth"]);
                    if (ram_data_width == "" || ram_data_width == string.Empty){
                        ram_data_width_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_12").Trim())));
                    }else{
                        ram_data_width_list.Add(ram_data_width + " Bit");
                    }
                    RAMDataWidth_V.Text = ram_data_width_list[0];
                }catch (Exception){ }
                try{
                    // RAM LOCATOR
                    bellek_type_list.Add(Convert.ToString(queryObj["DeviceLocator"]));
                    BellekType_V.Text = bellek_type_list[0];
                }catch (Exception){ }
                try{
                    // PART NUMBER
                    string part_number = Convert.ToString(queryObj["PartNumber"]).Trim();
                    if (part_number == "" || part_number == string.Empty){
                        ram_part_number_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_13").Trim())));
                    }else{
                        ram_part_number_list.Add(part_number);
                    }
                    RamPartNumber_V.Text = ram_part_number_list[0];
                }catch (Exception){ }
            }
            // RAM SELECT
            try{
                int ram_amount = ram_slot_list.Count;
                for (int rs = 1; rs <= ram_amount; rs++){
                    RAMSelectList.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_14").Trim())) + " #" + rs);
                    RAMSelectList.SelectedIndex = 0;
                }
            }catch (Exception){ }
        }
        private void RAMSelectList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int ram_slot = RAMSelectList.SelectedIndex;
                RamMik_V.Text = ram_amount_list[ram_slot];
                RamType_V.Text = ram_type_list[ram_slot];
                RamFrequency_V.Text = ram_frekans_list[ram_slot];
                RamVolt_V.Text = ram_voltage_list[ram_slot];
                RamFormFactor_V.Text = ram_form_factor[ram_slot];
                RamSerial_V.Text = ram_serial_list[ram_slot];
                RamManufacturer_V.Text = ram_manufacturer_list[ram_slot];
                RamBankLabel_V.Text = ram_bank_label_list[ram_slot];
                RAMDataWidth_V.Text = ram_data_width_list[ram_slot];
                BellekType_V.Text = bellek_type_list[ram_slot];
                RamPartNumber_V.Text = ram_part_number_list[ram_slot];
            }catch (Exception){ }
        }
        private void ram_bg_process(){
            try{
                ComputerInfo get_ram_info = new ComputerInfo();
                do{
                    ulong total_ram = ulong.Parse(get_ram_info.TotalPhysicalMemory.ToString());
                    double total_ram_x64 = total_ram / (1024 * 1024);
                    ulong usable_ram = ulong.Parse(get_ram_info.AvailablePhysicalMemory.ToString());
                    double usable_ram_x64 = usable_ram / (1024 * 1024);
                    double ram_process = total_ram_x64 - usable_ram_x64;
                    double usage_ram_percentage = (total_ram_x64 - usable_ram_x64) / total_ram_x64 * 100;
                    if (ram_process > 1024){
                        if ((ram_process / 1024) > 1024){
                            UsageRAMCount_V.Text = String.Format("{0:0.00} TB - ", ram_process / 1024 / 1024) + String.Format("%{0:0.0}", usage_ram_percentage);
                        }else{
                            UsageRAMCount_V.Text = String.Format("{0:0.00} GB - ", ram_process / 1024) + String.Format("%{0:0.0}", usage_ram_percentage);
                        }
                    }else{
                        UsageRAMCount_V.Text = ram_process.ToString() + " MB - " + String.Format("%{0:0.0}", usage_ram_percentage);
                    }
                    if (usable_ram_x64 > 1024){
                        if ((usable_ram_x64 / 1024) > 1024){
                            EmptyRamCount_V.Text = String.Format("{0:0.00} TB", usable_ram_x64 / 1024 / 1024);
                        }else{
                            EmptyRamCount_V.Text = String.Format("{0:0.00} GB", usable_ram_x64 / 1024);
                        }
                    }else{
                        EmptyRamCount_V.Text = usable_ram_x64.ToString() + " MB";
                    }
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        // GPU
        // ======================================================================================================
        List<string> gpu_man_list = new List<string>();
        List<string> gpu_driver_version_list = new List<string>();
        List<string> gpu_driver_date_list = new List<string>();
        List<string> gpu_status_list = new List<string>();
        List<string> gpu_dac_type_list = new List<string>();
        List<string> gpu_drivers_list = new List<string>();
        List<string> gpu_inf_file_list = new List<string>();
        List<string> gpu_inf_file_section_list = new List<string>();
        List<string> gpu_monitor_select_list = new List<string>();
        List<string> gpu_monitor_bounds_list = new List<string>();
        List<string> gpu_monitor_work_list = new List<string>();
        List<string> gpu_monitor_res_list = new List<string>();
        List<string> gpu_monitor_refresh_rate_list = new List<string>();
        List<string> gpu_monitor_virtual_res_list = new List<string>();
        List<string> gpu_monitor_primary_list = new List<string>();
        private void gpu(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_vc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            foreach (ManagementObject query_vc_rotate in search_vc.Get()){
                try{
                    // GPU NAME
                    string gpu_name = Convert.ToString(query_vc_rotate["Name"]);
                    if (gpu_name != "" && gpu_name != string.Empty){
                        GPUSelect.Items.Add(gpu_name);
                        GPUSelect.SelectedIndex = 0;
                    }
                }catch (Exception){ }
                try{
                    // GPU MAN
                    string gpu_man = Convert.ToString(query_vc_rotate["AdapterCompatibility"]).Trim();
                    if (gpu_man != "" && gpu_man != string.Empty){
                        gpu_man_list.Add(gpu_man);
                        GPUManufacturer_V.Text = gpu_man_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU DRIVER VERSION
                    string driver_version = Convert.ToString(query_vc_rotate["DriverVersion"]);
                    if (driver_version != "" && driver_version != string.Empty){
                        gpu_driver_version_list.Add(driver_version);
                        GPUVersion_V.Text = gpu_driver_version_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU DRIVER DATE
                    string gpu_date = Convert.ToString(query_vc_rotate["DriverDate"]);
                    if (gpu_date != "" && gpu_date != string.Empty){
                        string gpu_date_process = gpu_date.Substring(6, 2) + "." + gpu_date.Substring(4, 2) + "." + gpu_date.Substring(0, 4);
                        gpu_driver_date_list.Add(gpu_date_process);
                        GPUDriverDate_V.Text = gpu_driver_date_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU STATUS
                    int gpu_status = Convert.ToInt32(query_vc_rotate["Availability"]);
                    switch (gpu_status){
                        case 1:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_1").Trim())));
                            break;
                        case 2:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_2").Trim())));
                            break;
                        case 3:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_3").Trim())));
                            break;
                        case 4:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_4").Trim())));
                            break;
                        case 5:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_5").Trim())));
                            break;
                        case 6:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_6").Trim())));
                            break;
                        case 7:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_7").Trim())));
                            break;
                        case 8:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_8").Trim())));
                            break;
                        case 9:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_9").Trim())));
                            break;
                        case 10:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_10").Trim())));
                            break;
                        case 11:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_11").Trim())));
                            break;
                        case 12:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_12").Trim())));
                            break;
                        case 13:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_13").Trim())));
                            break;
                        case 14:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_14").Trim())));
                            break;
                        case 15:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_15").Trim())));
                            break;
                        case 16:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_16").Trim())));
                            break;
                        case 17:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_17").Trim())));
                            break;
                        case 18:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_18").Trim())));
                            break;
                        case 19:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_19").Trim())));
                            break;
                        case 20:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_20").Trim())));
                            break;
                        case 21:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_21").Trim())));
                            break;
                        default:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_22").Trim())));
                            break;
                    }
                    GPUStatus_V.Text = gpu_status_list[0];
                }catch (Exception){ }
                try{
                    // GPU DAC TYPE
                    string adaptor_dac_type = Convert.ToString(query_vc_rotate["AdapterDACType"]);
                    if (adaptor_dac_type == ""){
                        gpu_dac_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_23").Trim())));
                    }else{
                        if (adaptor_dac_type == "Integrated RAMDAC"){
                            gpu_dac_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_24").Trim())));
                        }else if (adaptor_dac_type == "Internal"){
                            gpu_dac_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_25").Trim())));
                        }else{
                            gpu_dac_type_list.Add(adaptor_dac_type);
                        }
                    }
                    GPUDacType_V.Text = gpu_dac_type_list[0];
                }catch (Exception){ }
                try{
                    // GPU DRIVERS
                    string gpu_display_drivers = Path.GetFileName(Convert.ToString(query_vc_rotate["InstalledDisplayDrivers"]));
                    if (gpu_display_drivers != "" && gpu_display_drivers != string.Empty){
                        gpu_drivers_list.Add(gpu_display_drivers);
                        GraphicDriversName_V.Text = gpu_drivers_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU INF FILE NAME
                    string gpu_inf_file = Convert.ToString(query_vc_rotate["InfFilename"]);
                    if (gpu_inf_file != "" && gpu_inf_file != string.Empty){
                        gpu_inf_file_list.Add(gpu_inf_file);
                        GpuInfFileName_V.Text = gpu_inf_file_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU INF FILE GPU INFO PARTITION
                    string gpu_inf_section = Convert.ToString(query_vc_rotate["InfSection"]);
                    if (gpu_inf_section != "" && gpu_inf_section != string.Empty){
                        gpu_inf_file_section_list.Add(gpu_inf_section);
                        INFSectionFile_V.Text = gpu_inf_file_section_list[0];
                    }
                }catch (Exception){ }
            }
            try{
                // ALL SCREEN DETECT
                foreach (var all_monitors in Screen.AllScreens){
                    string m_bounds = all_monitors.Bounds.ToString();
                    string m_working_area = all_monitors.WorkingArea.ToString();
                    string m_primary_screen = all_monitors.Primary.ToString();
                    gpu_monitor_select_list.Add(all_monitors.DeviceName.ToString());
                    string m_bounds_v2 = m_bounds.Replace("Width", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_26").Trim())));
                    string m_bounds_v3 = m_bounds_v2.Replace("Height", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_27").Trim())));
                    string m_bounds_v4 = m_bounds_v3.Replace("{", "");
                    string m_bounds_v5 = m_bounds_v4.Replace("}", "");
                    string m_bounds_v6 = m_bounds_v5.Replace(",", ", ");
                    string m_bounds_v7 = m_bounds_v6.Replace("=", " = ");
                    string m_working_area_v2 = m_working_area.Replace("Width", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_28").Trim())));
                    string m_working_area_v3 = m_working_area_v2.Replace("Height", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_29").Trim())));
                    string m_working_area_v4 = m_working_area_v3.Replace("{", "");
                    string m_working_area_v5 = m_working_area_v4.Replace("}", "");
                    string m_working_area_v6 = m_working_area_v5.Replace(",", ", ");
                    string m_working_area_v7 = m_working_area_v6.Replace("=", " = ");
                    string m_primary_screen_v2 = m_primary_screen.Replace("True", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_30").Trim())));
                    string m_primary_screen_v3 = m_primary_screen_v2.Replace("False", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_31").Trim())));
                    gpu_monitor_bounds_list.Add(m_bounds_v7);
                    gpu_monitor_work_list.Add(m_working_area_v7);
                    gpu_monitor_primary_list.Add(m_primary_screen_v3);
                }
            }catch (Exception){ }
            try{
                foreach (Screen screen in Screen.AllScreens){
                    // SCREEN RESOLUTIONS
                    var dm = new DEVMODE { dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)) };
                    EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);
                    gpu_monitor_res_list.Add(dm.dmPelsWidth + " x " + dm.dmPelsHeight);
                    gpu_monitor_virtual_res_list.Add(screen.Bounds.Width + " x " + screen.Bounds.Height);
                    // SCREEN REFRESH RATE
                    var dm_2 = new DEVMODE { dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)) };
                    EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm_2);
                    gpu_monitor_refresh_rate_list.Add(dm_2.dmDisplayFrequency.ToString() + " Hz");
                }
            }catch (Exception){ }
            try{
                // MONITOR SELECT
                int monitor_amount = gpu_monitor_select_list.Count;
                for (int ma = 1; ma <= monitor_amount; ma++){
                    MonitorSelectList.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_32").Trim())) + " #" + ma);
                    MonitorSelectList.SelectedIndex = 0;
                }
            }catch (Exception){ }
            // GPU SELECT
            try { GPUSelect.SelectedIndex = 0; }catch(Exception){ }
        }
        private void GPUSelect_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int gpu_select = GPUSelect.SelectedIndex;
                GPUManufacturer_V.Text = gpu_man_list[gpu_select];
                GPUVersion_V.Text = gpu_driver_version_list[gpu_select];
                GPUDriverDate_V.Text = gpu_driver_date_list[gpu_select];
                GPUStatus_V.Text = gpu_status_list[gpu_select];
                GPUDacType_V.Text = gpu_dac_type_list[gpu_select];
                GraphicDriversName_V.Text = gpu_drivers_list[gpu_select];
                GpuInfFileName_V.Text = gpu_inf_file_list[gpu_select];
                INFSectionFile_V.Text = gpu_inf_file_section_list[gpu_select];
            }catch(Exception){ }
        }
        private void MonitorSelectList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int monitor_select = MonitorSelectList.SelectedIndex;
                MonitorBounds_V.Text = gpu_monitor_bounds_list[monitor_select];
                MonitorWorking_V.Text = gpu_monitor_work_list[monitor_select];
                MonitorResLabel_V.Text = gpu_monitor_res_list[monitor_select];
                ScreenRefreshRate_V.Text = gpu_monitor_refresh_rate_list[monitor_select];
                MonitorVirtualRes_V.Text = gpu_monitor_virtual_res_list[monitor_select];
                MonitorPrimary_V.Text = gpu_monitor_primary_list[monitor_select];
            }catch(Exception){ }
        }
        // DISK
        // ======================================================================================================
        List<string> disk_model_list = new List<string>();
        List<string> disk_man_list = new List<string>();
        List<string> disk_volume_list = new List<string>();
        List<string> disk_volume_name_list = new List<string>();
        List<string> disk_firmware_list = new List<string>();
        List<string> disk_serial_list = new List<string>();
        List<string> disk_size_list = new List<string>();
        List<string> disk_free_space_list = new List<string>();
        List<string> disk_file_system_list = new List<string>();
        List<string> disk_partition_layout_list = new List<string>();
        List<string> disk_type_list = new List<string>();
        List<string> disk_drive_type_list = new List<string>();
        List<string> disk_interface_list = new List<string>();
        List<string> disk_partition_list = new List<string>();
        List<string> disk_uniquq_id_list = new List<string>();
        List<string> disk_location_list = new List<string>();
        List<string> disk_healt_list = new List<string>();
        List<string> disk_boot_list = new List<string>();
        List<string> disk_boot_status_list = new List<string>();
        private void disk(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                DriveInfo[] get_drives = DriveInfo.GetDrives();
                for (int d = 0; d < get_drives.Count(); d++){
                    // DISK VOLUME ID
                    disk_volume_list.Add(get_drives[d].Name);
                    // DISK FILE SYSTEM
                    disk_file_system_list.Add(get_drives[d].DriveFormat);
                    // DISK DRIVE TYPE
                    string disk_drive_type = get_drives[d].DriveType.ToString().Trim();
                    if (disk_drive_type != string.Empty && disk_drive_type != ""){
                        if (disk_drive_type == "Fixed"){
                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                        }else if (disk_drive_type == "CDRom"){
                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_2").Trim())));
                        }else if (disk_drive_type == "Network"){
                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_3").Trim())));
                        }else if (disk_drive_type == "NoRootDirectory"){
                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_4").Trim())));
                        }else if (disk_drive_type == "Ram"){
                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_5").Trim())));
                        }else if (disk_drive_type == "Removable"){
                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_6").Trim())));
                        }else if (disk_drive_type == "Unknown"){
                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_7").Trim())));
                        }
                        DiskDriveType_V.Text = disk_drive_type_list[0];
                    }
                    // DISK VOLUME NAME
                    if (get_drives[d].VolumeLabel == string.Empty || get_drives[d].VolumeLabel == ""){
                        disk_volume_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_8").Trim())));
                    }else{
                        disk_volume_name_list.Add(get_drives[d].VolumeLabel);
                    }
                    // DISK FREE SPACE
                    double disk_free = get_drives[d].AvailableFreeSpace / 1024 / 1024;
                    if (disk_free > 1024){
                        if ((disk_free / 1024) > 1024){
                            disk_free_space_list.Add(String.Format("{0:0.00} TB", disk_free / 1024 / 1024));
                        }else{
                            disk_free_space_list.Add(String.Format("{0:0.00} GB", disk_free / 1024));
                        }
                    }else{
                        disk_free_space_list.Add(String.Format("{0:0.0} MB", disk_free));
                    }
                }
                DiskVolumeID_V.Text = disk_volume_list[0];
                DiskFileSystem_V.Text = disk_file_system_list[0];
                DiskVolumeName_V.Text = disk_volume_name_list[0];
                DiskFreeSpace_V.Text = disk_free_space_list[0];
            }catch (Exception){ }
            ManagementObjectSearcher search_disk = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", "SELECT * FROM MSFT_Disk");
            foreach (ManagementObject query_disk in search_disk.Get()){
                try{
                    // DISK NAME
                    DiskSelectBox.Items.Add(Convert.ToString(query_disk["FriendlyName"]));
                }catch (Exception){ }
                try{
                    // DISK MODEL
                    string disk_model = Convert.ToString(query_disk["Model"]).Trim();
                    if (disk_model == string.Empty || disk_model == ""){
                        disk_model_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_9").Trim())));
                    }else{
                        disk_model_list.Add(disk_model);
                    }
                    DiskModel_V.Text = disk_model_list[0];
                }catch (Exception){ }
                try{
                    // DISK MANUFACTURER
                    string disk_man = Convert.ToString(query_disk["Manufacturer"]).Trim();
                    if (disk_man == string.Empty || disk_man == ""){
                        disk_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_10").Trim())));
                    }else{
                        disk_man_list.Add(disk_man);
                    }
                    DiskMan_V.Text = disk_man_list[0];
                }catch (Exception){ }
                try{
                    // DISK FIMWARE VERSION
                    disk_firmware_list.Add(Convert.ToString(query_disk["FirmwareVersion"]));
                    DiskFirmWare_V.Text = disk_firmware_list[0];
                }catch (Exception){ }
                try{
                    // DISK SERIAL
                    disk_serial_list.Add(Convert.ToString(query_disk["SerialNumber"]).Trim());
                    DiskSeri_V.Text = disk_serial_list[0];
                }catch (Exception){ }
                try{
                    // DISK SIZE
                    double disk_size = Convert.ToDouble(query_disk["Size"]) / 1024 / 1024;
                    if (disk_size > 1024){
                        if ((disk_size / 1024) > 1024){
                            disk_size_list.Add(String.Format("{0:0.00} TB", disk_size / 1024 / 1024));
                        }else{
                            disk_size_list.Add(String.Format("{0:0.00} GB", disk_size / 1024));
                        }
                    }else{
                        disk_size_list.Add(String.Format("{0:0.0} MB", disk_size));
                    }
                    DiskSize_V.Text = disk_size_list[0];
                }catch (Exception){ }
                try{
                    // DISK PARTITON LAYOUT
                    int disk_partition_layout = Convert.ToInt32(query_disk["PartitionStyle"]);
                    if (disk_partition_layout == 0){
                        disk_partition_layout_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_11").Trim())));
                    }else if (disk_partition_layout == 1){
                        disk_partition_layout_list.Add("MBR");
                    }else if (disk_partition_layout == 2){
                        disk_partition_layout_list.Add("GPT");
                    }
                    DiskPartitionLayout_V.Text = disk_partition_layout_list[0];
                }catch (Exception){ }
                try{
                    // DISK TYPE
                    int disk_type = Convert.ToInt32(query_disk["BusType"]);
                    if (disk_type == 0){
                        disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_12").Trim())));
                    }else if (disk_type == 1){
                        disk_interface_list.Add("SCSI");
                    }else if (disk_type == 2){
                        disk_interface_list.Add("ATAPI");
                    }else if (disk_type == 3){
                        disk_interface_list.Add("ATA");
                    }else if (disk_type == 4){
                        disk_interface_list.Add("IEEE 1394");
                    }else if (disk_type == 5){
                        disk_interface_list.Add("SSA");
                    }else if (disk_type == 6){
                        disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_13").Trim())));
                    }else if (disk_type == 7){
                        disk_interface_list.Add("USB");
                    }else if (disk_type == 8){
                        disk_interface_list.Add("RAID");
                    }else if (disk_type == 9){
                        disk_interface_list.Add("iSCSI");
                    }else if (disk_type == 10){
                        disk_interface_list.Add("SAS");
                    }else if (disk_type == 11){
                        disk_interface_list.Add("SATA");
                    }else if (disk_type == 12){
                        disk_interface_list.Add("SD");
                    }else if (disk_type == 13){
                        disk_interface_list.Add("MMC");
                    }else if (disk_type == 14){
                        disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_14").Trim())));
                    }else if (disk_type == 15){
                        disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_15").Trim())));
                    }else if (disk_type == 16){
                        disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_16").Trim())));
                    }else if (disk_type == 17){
                        disk_interface_list.Add("NVMe");
                    }
                    DiskInterFace_V.Text = disk_interface_list[0];
                }catch (Exception){ }
                try{
                    // DISK PARTITION
                    int disk_partition = Convert.ToInt32(query_disk["NumberOfPartitions"]);
                    disk_partition_list.Add(disk_partition.ToString().Trim());
                    DiskPartition_V.Text = disk_partition_list[0];
                }catch (Exception){ }
                try{
                    // DISK UNIQUE ID
                    string disk_unique_id = Convert.ToString(query_disk["UniqueId"]).Trim();
                    if (disk_unique_id == string.Empty || disk_unique_id == ""){
                        disk_uniquq_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_17").Trim())));
                    }else{
                        disk_uniquq_id_list.Add(disk_unique_id);
                    }
                    DiskUniqueID_V.Text = disk_uniquq_id_list[0];
                }catch (Exception){ }
                try{
                    // DISK LOCATION
                    string disk_locaiton = Convert.ToString(query_disk["Location"]).Trim();
                    string disk_process_1 = disk_locaiton.Replace("Integrated", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_18").Trim())));
                    string disk_process_2 = disk_process_1.Replace("Device", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_19").Trim())));
                    string disk_process_3 = disk_process_2.Replace("Function", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_20").Trim())));
                    string disk_process_4 = disk_process_3.Replace("Adapter", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_21").Trim())));
                    disk_location_list.Add(disk_process_4);
                    DiskLocation_V.Text = disk_location_list[0];
                }catch (Exception){ }
                try{
                    // DISK HEALT
                    int disk_healt = Convert.ToInt32(query_disk["HealthStatus"]);
                    if (disk_healt == 0){
                        disk_healt_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_22").Trim())));
                    }else if (disk_healt == 1){
                        disk_healt_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_23").Trim())));
                    }else if (disk_healt == 2){
                        disk_healt_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_24").Trim())));
                    }
                    DiskHealt_V.Text = disk_healt_list[0];
                }catch (Exception){ }
                try{
                    // DISK BOOT STATUS
                    bool boot_status = Convert.ToBoolean(query_disk["BootFromDisk"]);
                    if (boot_status == true){
                        disk_boot_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_25").Trim())));
                    }else if (boot_status == false){
                        disk_boot_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_26").Trim())));
                    }
                    DiskBootPartition_V.Text = disk_boot_list[0];
                }catch (Exception){ }
                try{
                    // DISK BOOTABLE IS BOOT
                    bool disk_bootable_status = Convert.ToBoolean(query_disk["IsBoot"]);
                    if (disk_bootable_status == true){
                        disk_boot_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_27").Trim())));
                    }else if (disk_bootable_status == false){
                        disk_boot_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_28").Trim())));
                    }
                    DiskBootableStatus_V.Text = disk_boot_status_list[0];
                }catch (Exception){ }
            }
            ManagementObjectSearcher search_pd = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", "SELECT * FROM MSFT_PhysicalDisk");
            foreach (ManagementObject query_pdisk in search_pd.Get()){
                try{
                    // DISK TYPE
                    int disk_type = Convert.ToInt32(query_pdisk["MediaType"]);
                    if (disk_type == 0){
                        disk_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_29").Trim())));
                    }else if (disk_type == 3){
                        disk_type_list.Add("HDD");
                    }else if (disk_type == 4){
                        disk_type_list.Add("SSD");
                    }else if (disk_type == 5){
                        disk_type_list.Add("SCM");
                    }
                    DiskType_V.Text = disk_type_list[0];
                }catch (Exception){ }
            }
            // SELECT DISK
            try{ DiskSelectBox.SelectedIndex = 0; }catch(Exception){ }
        }
        private void disklist_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int disk_select = DiskSelectBox.SelectedIndex;
                DiskModel_V.Text = disk_model_list[disk_select];
                DiskMan_V.Text = disk_man_list[disk_select];
                DiskVolumeID_V.Text = disk_volume_list[disk_select];
                DiskVolumeName_V.Text = disk_volume_name_list[disk_select];
                DiskFirmWare_V.Text = disk_firmware_list[disk_select];
                DiskSeri_V.Text = disk_serial_list[disk_select];
                DiskSize_V.Text = disk_size_list[disk_select];
                DiskFreeSpace_V.Text = disk_free_space_list[disk_select];
                DiskPartitionLayout_V.Text = disk_partition_layout_list[disk_select];
                DiskFileSystem_V.Text = disk_file_system_list[disk_select];
                DiskType_V.Text = disk_type_list[disk_select];
                DiskDriveType_V.Text = disk_drive_type_list[disk_select];
                DiskInterFace_V.Text = disk_interface_list[disk_select];
                DiskPartition_V.Text = disk_partition_list[disk_select];
                DiskUniqueID_V.Text = disk_uniquq_id_list[disk_select];
                DiskLocation_V.Text = disk_location_list[disk_select];
                DiskHealt_V.Text = disk_healt_list[disk_select];
                DiskBootPartition_V.Text = disk_boot_list[disk_select];
                DiskBootableStatus_V.Text = disk_boot_status_list[disk_select];
            }catch(Exception){ }
        }
        // NETWORK
        // ======================================================================================================
        List<string> network_mac_adress_list = new List<string>();
        List<string> network_man_list = new List<string>();
        List<string> network_service_name_list = new List<string>();
        List<string> network_adaptor_type_list = new List<string>();
        List<string> network_guid_list = new List<string>();
        List<string> network_connection_type_list = new List<string>();
        List<string> network_dhcp_status_list = new List<string>();
        List<string> network_dhcp_server_list = new List<string>();
        List<string> network_connection_speed_list = new List<string>();
        private void network(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_na = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter");
            ManagementObjectSearcher search_nac = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject query_na_rotate in search_na.Get()){
                try{
                    // NET NAME
                    ListNetwork.Items.Add(Convert.ToString(query_na_rotate["Name"]));
                    ListNetwork.SelectedIndex = 0;
                }catch (Exception){ }
                try{
                    // MAC ADRESS
                    string mac_adress = Convert.ToString(query_na_rotate["MACAddress"]);
                    if (mac_adress == ""){
                        network_mac_adress_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_1").Trim())));
                    }else{
                        network_mac_adress_list.Add(mac_adress);
                    }
                    MacAdress_V.Text = network_mac_adress_list[0];
                }catch (Exception){ }
                try{
                    // NET MAN
                    string net_man = Convert.ToString(query_na_rotate["Manufacturer"]);
                    if (net_man == ""){
                        network_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_2").Trim())));
                    }else{
                        network_man_list.Add(net_man);
                    }
                    NetMan_V.Text = network_man_list[0];
                }catch (Exception){ }
                try{
                    // SERVICE NAME
                    string service_name = Convert.ToString(query_na_rotate["ServiceName"]);
                    if (service_name == ""){
                        network_service_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_3").Trim())));
                    }else{
                        network_service_name_list.Add(service_name);
                    }
                    ServiceName_V.Text = network_service_name_list[0];
                }catch (Exception){ }
                try{
                    // NET ADAPTER TYPE
                    string adaptor_type = Convert.ToString(query_na_rotate["AdapterType"]);
                    if (adaptor_type == ""){
                        network_adaptor_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_4").Trim())));
                    }else{
                        network_adaptor_type_list.Add(adaptor_type);
                    }
                    AdapterType_V.Text = network_adaptor_type_list[0];
                }catch (Exception){ }
                try{
                    // NET GUID
                    string guid = Convert.ToString(query_na_rotate["GUID"]);
                    if (guid == ""){
                        network_guid_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_5").Trim())));
                    }else{
                        network_guid_list.Add(guid);
                    }
                    Guid_V.Text = network_guid_list[0];
                }catch (Exception){ }
                try{
                    // NET CONNECTION ID
                    string net_con_id = Convert.ToString(query_na_rotate["NetConnectionID"]);
                    if (net_con_id == ""){
                        network_connection_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_6").Trim())));
                    }else{
                        network_connection_type_list.Add(net_con_id);
                    }
                    ConnectionType_V.Text = network_connection_type_list[0];
                }catch (Exception){ }
                try{
                    // MODEM CONNECT SPEED
                    string local_con_speed = Convert.ToString(query_na_rotate["Speed"]);
                    if (local_con_speed == "" || local_con_speed == "Unknown"){
                        network_connection_speed_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_7").Trim())));
                    }else{
                        int net_speed_cal = Convert.ToInt32(local_con_speed) / 1000 / 1000;
                        double net_speed_download_cal = Convert.ToDouble(net_speed_cal) / 8;
                        network_connection_speed_list.Add(net_speed_cal.ToString() + " Mbps - (" + String.Format("{0:0.0} MB/s)", net_speed_download_cal));
                    }
                    LocalConSpeed_V.Text = network_connection_speed_list[0];
                }catch (Exception){ }
            }
            foreach (ManagementObject query_nac_rotate in search_nac.Get()){
                try{
                    // DHCP STATUS
                    bool dhcp_enabled = Convert.ToBoolean(query_nac_rotate["DHCPEnabled"]);
                    if (dhcp_enabled == true){
                        network_dhcp_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_8").Trim())));
                    }else if (dhcp_enabled == false){
                        network_dhcp_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_9").Trim())));
                    }
                    Dhcp_status_V.Text = network_dhcp_status_list[0];
                }catch (Exception){ }
                try{
                    // DHCP SERVER STATUS
                    string dhcp_server = Convert.ToString(query_nac_rotate["DHCPServer"]);
                    if (dhcp_server == ""){
                        network_dhcp_server_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_10").Trim())));
                    }else{
                        network_dhcp_server_list.Add(dhcp_server);
                    }
                    Dhcp_server_V.Text = network_dhcp_server_list[0];
                }catch (Exception){ }
            }
            // IPv4 And IPv6 Adress
            try{
                IPHostEntry ip_entry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] adress_x64 = ip_entry.AddressList;
                IPv4Adress_V.Text = adress_x64[adress_x64.Length - 1].ToString();
                if (adress_x64[0].AddressFamily == AddressFamily.InterNetworkV6){
                    IPv6Adress_V.Text = adress_x64[0].ToString();
                }
            }catch (Exception){ }
            // NETWORK SELECT
            try{ ListNetwork.SelectedIndex = 1; }catch (Exception){ ListNetwork.SelectedIndex = 0; }
        }
        private void listnetwork_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int network_select = ListNetwork.SelectedIndex;
                MacAdress_V.Text = network_mac_adress_list[network_select];
                NetMan_V.Text = network_man_list[network_select];
                ServiceName_V.Text = network_service_name_list[network_select];
                AdapterType_V.Text = network_adaptor_type_list[network_select];
                Guid_V.Text = network_guid_list[network_select];
                ConnectionType_V.Text = network_connection_type_list[network_select];
                Dhcp_status_V.Text = network_dhcp_status_list[network_select];
                Dhcp_server_V.Text = network_dhcp_server_list[network_select];
                LocalConSpeed_V.Text = network_connection_speed_list[network_select];
            }catch(Exception){ }
        }
        // SOUND
        // ======================================================================================================
        List<string> sound_driver_launch_status_list = new List<string>();
        List<string> sound_config_list = new List<string>();
        List<string> sound_device_id_list = new List<string>();
        List<string> sound_man_list = new List<string>();
        List<string> sound_power_man_list = new List<string>();
        private void sound(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_sd = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SoundDevice");
            foreach (ManagementObject query_sd_rotate in search_sd.Get()){
                try{
                    // SOUND NAME
                    SoundDriverMainList.Items.Add(Convert.ToString(query_sd_rotate["Name"]));
                    SoundDriverMainList.SelectedIndex = 0;
                }catch (Exception){ }
                try{
                    // SOUND STATUS
                    int sound_driver_launch_status = Convert.ToInt32(query_sd_rotate["ConfigManagerErrorCode"]);
                    if (sound_driver_launch_status == 0){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_1").Trim())));
                    }else if (sound_driver_launch_status == 1){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_2").Trim())));
                    }else if (sound_driver_launch_status == 2){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_3").Trim())));
                    }else if (sound_driver_launch_status == 3){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_4").Trim())));
                    }else if (sound_driver_launch_status == 4){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_5").Trim())));
                    }else if (sound_driver_launch_status == 5){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_6").Trim())));
                    }else if (sound_driver_launch_status == 6){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_7").Trim())));
                    }else if (sound_driver_launch_status == 7){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_8").Trim())));
                    }else if (sound_driver_launch_status == 8){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_9").Trim())));
                    }else if (sound_driver_launch_status == 9){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_10").Trim())));
                    }else if (sound_driver_launch_status == 10){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_11").Trim())));
                    }else if (sound_driver_launch_status == 11){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_12").Trim())));
                    }else if (sound_driver_launch_status == 12){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_13").Trim())));
                    }else if (sound_driver_launch_status == 13){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_14").Trim())));
                    }else if (sound_driver_launch_status == 14){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_15").Trim())));
                    }else if (sound_driver_launch_status == 15){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_16").Trim())));
                    }else if (sound_driver_launch_status == 16){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_17").Trim())));
                    }else if (sound_driver_launch_status == 17){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_18").Trim())));
                    }else if (sound_driver_launch_status == 18){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_19").Trim())));
                    }else if (sound_driver_launch_status == 19){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_20").Trim())));
                    }else if (sound_driver_launch_status == 20){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_21").Trim())));
                    }else if (sound_driver_launch_status == 21){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_22").Trim())));
                    }else if (sound_driver_launch_status == 22){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_23").Trim())));
                    }else if (sound_driver_launch_status == 23){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_24").Trim())));
                    }else if (sound_driver_launch_status == 24){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_25").Trim())));
                    }else if (sound_driver_launch_status == 25){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_26").Trim())));
                    }else if (sound_driver_launch_status == 26){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_27").Trim())));
                    }else if (sound_driver_launch_status == 27){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_28").Trim())));
                    }else if (sound_driver_launch_status == 28){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_29").Trim())));
                    }else if (sound_driver_launch_status == 29){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_30").Trim())));
                    }else if (sound_driver_launch_status == 30){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_31").Trim())));
                    }else if (sound_driver_launch_status == 31){
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_32").Trim())));
                    }else{
                        sound_driver_launch_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_33").Trim())));
                    }
                    SoundDriverStatus_V.Text = sound_driver_launch_status_list[0];
                }catch (Exception){ }
                try{
                    // SOUND CONFIG
                    bool sound_config_manager = Convert.ToBoolean(query_sd_rotate["ConfigManagerUserConfig"]);
                    if (sound_config_manager == true){
                        sound_config_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_34").Trim())));
                    }else if (sound_config_manager == false){
                        sound_config_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_35").Trim())));
                    }else{
                        sound_config_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_36").Trim())));
                    }
                    SoundConfig_V.Text = sound_config_list[0];
                }catch (Exception){ }
                try{
                    // SOUND DEVICE ID
                    string sound_device_id = Convert.ToString(query_sd_rotate["DeviceID"]);
                    if (sound_device_id == ""){
                        sound_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_37").Trim())));
                    }else{
                        sound_device_id_list.Add(sound_device_id.Trim());
                    }
                    SoundDeviceId_V.Text = sound_device_id_list[0];
                }catch (Exception){ }
                try{
                    // SOUND MANUFACTURER
                    string sound_manufacturer = Convert.ToString(query_sd_rotate["Manufacturer"]);
                    if (sound_manufacturer == ""){
                        sound_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_38").Trim())));
                    }else{
                        sound_man_list.Add(sound_manufacturer.Trim());
                    }
                    SoundMan_V.Text = sound_man_list[0];
                }catch (Exception){ }
                try{
                    // SOUND PMS
                    bool sound_power_supported = Convert.ToBoolean(query_sd_rotate["PowerManagementSupported"]);
                    if (sound_power_supported == true){
                        sound_power_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_39").Trim())));
                    }else if (sound_power_supported == false){
                        sound_power_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_40").Trim())));
                    }else{
                        sound_power_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sd_c_41").Trim())));
                    }
                    SoundPowerMan_V.Text = sound_power_man_list[0];
                }catch (Exception){ }
            }
        }
        private void SoundDriverMainList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int sound_driver_select = SoundDriverMainList.SelectedIndex;
                SoundDriverStatus_V.Text = sound_driver_launch_status_list[sound_driver_select];
                SoundConfig_V.Text = sound_config_list[sound_driver_select];
                SoundDeviceId_V.Text = sound_device_id_list[sound_driver_select];
                SoundMan_V.Text = sound_man_list[sound_driver_select];
                SoundPowerMan_V.Text = sound_power_man_list[sound_driver_select];
            }catch (Exception){ }
        }
        // USB
        // ======================================================================================================
        List<string> usb_hub_status_list = new List<string>();
        List<string> usb_hub_config_list = new List<string>();
        List<string> usb_hub_desc_list = new List<string>();
        List<string> usb_device_id_list = new List<string>();
        List<string> usb_name_list = new List<string>();
        List<string> usb_pnp_device_id_list = new List<string>();
        private void usb(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_usbhub = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_USBHub");
            foreach (ManagementObject query_usb_rotate in search_usbhub.Get()){
                try{
                    // USB NAME
                    USBMainList.Items.Add(Convert.ToString(query_usb_rotate["Caption"]).Trim());
                    USBMainList.SelectedIndex = 0;
                }catch (Exception){ }
                try{
                    // USB STATUS
                    int usb_status = Convert.ToInt32(query_usb_rotate["ConfigManagerErrorCode"]);
                    if (usb_status == 0){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_1").Trim())));
                    }else if (usb_status == 1){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
                    }else if (usb_status == 2){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_3").Trim())));
                    }else if (usb_status == 3){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_4").Trim())));
                    }else if (usb_status == 4){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_5").Trim())));
                    }else if (usb_status == 5){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_6").Trim())));
                    }else if (usb_status == 6){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_7").Trim())));
                    }else if (usb_status == 7){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_8").Trim())));
                    }else if (usb_status == 8){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_9").Trim())));
                    }else if (usb_status == 9){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_10").Trim())));
                    }else if (usb_status == 10){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_11").Trim())));
                    }else if (usb_status == 11){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_12").Trim())));
                    }else if (usb_status == 12){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_13").Trim())));
                    }else if (usb_status == 13){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_14").Trim())));
                    }else if (usb_status == 14){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_15").Trim())));
                    }else if (usb_status == 15){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_16").Trim())));
                    }else if (usb_status == 16){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_17").Trim())));
                    }else if (usb_status == 17){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_18").Trim())));
                    }else if (usb_status == 18){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_19").Trim())));
                    }else if (usb_status == 19){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_20").Trim())));
                    }else if (usb_status == 20){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_21").Trim())));
                    }else if (usb_status == 21){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_22").Trim())));
                    }else if (usb_status == 22){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_23").Trim())));
                    }else if (usb_status == 23){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_24").Trim())));
                    }else if (usb_status == 24){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_25").Trim())));
                    }else if (usb_status == 25){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_26").Trim())));
                    }else if (usb_status == 26){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_27").Trim())));
                    }else if (usb_status == 27){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_28").Trim())));
                    }else if (usb_status == 28){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_29").Trim())));
                    }else if (usb_status == 29){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_30").Trim())));
                    }else if (usb_status == 30){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_31").Trim())));
                    }else if (usb_status == 31){
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_32").Trim())));
                    }else{
                        usb_hub_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_33").Trim())));
                    }
                    USBStatus_V.Text = usb_hub_status_list[0];
                }catch (Exception){ }
                try{
                    // USB CONFIG MANAGER
                    bool usb_usage_status = Convert.ToBoolean(query_usb_rotate["ConfigManagerUserConfig"]);
                    if (usb_usage_status == true){
                        usb_hub_config_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_34").Trim())));
                    }else if (usb_usage_status == false){
                        usb_hub_config_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_35").Trim())));
                    }else{
                        usb_hub_config_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_36").Trim())));
                    }
                    USBConfigUsage_V.Text = usb_hub_config_list[0];
                }catch (Exception){ }
                try{
                    // USB DESCRIPTION
                    string usb_hub_description = Convert.ToString(query_usb_rotate["Description"]);
                    if (usb_hub_description == ""){
                        usb_hub_desc_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_37").Trim())));
                    }else{
                        usb_hub_desc_list.Add(usb_hub_description);
                    }
                    USBHubDesc_V.Text = usb_hub_desc_list[0];
                }catch (Exception){ }
                try{
                    // USB DEVICE ID
                    string usb_device_id = Convert.ToString(query_usb_rotate["DeviceID"]);
                    if (usb_device_id == ""){
                        usb_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_38").Trim())));
                    }else{
                        usb_device_id_list.Add(usb_device_id);
                    }
                    USBDeviceID_V.Text = usb_device_id_list[0];
                }catch (Exception){ }
                try{
                    // USB NAME
                    string usb_names = Convert.ToString(query_usb_rotate["Name"]);
                    if (usb_names == ""){
                        usb_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_39").Trim())));
                    }else{
                        usb_name_list.Add(usb_names);
                    }
                    USBNames_V.Text = usb_name_list[0];
                }catch (Exception){ }
                try{
                    // USB PNP DEVICE ID
                    string usb_pnp_device_id = Convert.ToString(query_usb_rotate["PNPDeviceID"]);
                    if (usb_pnp_device_id == ""){
                        usb_pnp_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_40").Trim())));
                    }else{
                        usb_pnp_device_id_list.Add(usb_pnp_device_id);
                    }
                    USBPnpDeviceID_V.Text = usb_pnp_device_id_list[0];
                }
                catch (Exception){ }
            }
        }
        private void USBMainList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int usb_select = USBMainList.SelectedIndex;
                USBStatus_V.Text = usb_hub_status_list[usb_select];
                USBConfigUsage_V.Text = usb_hub_config_list[usb_select];
                USBHubDesc_V.Text = usb_hub_desc_list[usb_select];
                USBDeviceID_V.Text = usb_device_id_list[usb_select];
                USBNames_V.Text = usb_name_list[usb_select];
                USBPnpDeviceID_V.Text = usb_pnp_device_id_list[usb_select];
            }catch (Exception){ }
        }
        // BATTERY
        // ======================================================================================================
        private void battery(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_battery = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");
            foreach (ManagementObject query_battery_rotate in search_battery.Get()){
                try{
                    // BATTERY ID                        
                    BatteryModel_V.Text = Convert.ToString(query_battery_rotate["DeviceID"]).Trim();
                }catch(Exception){ }
                try{
                    // BATTERY NAME
                    BatteryName_V.Text = Convert.ToString(query_battery_rotate["Name"]).Trim();
                }catch(Exception){ }
                try{
                    // BATTERY TYPE
                    int battery_structure = Convert.ToInt32(query_battery_rotate["Chemistry"]);
                    if (battery_structure == 1){
                        BatteryType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_1").Trim()));
                    }else if (battery_structure == 2){
                        BatteryType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_2").Trim()));
                    }else if (battery_structure == 3){
                        BatteryType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_3").Trim()));
                    }else if (battery_structure == 4){
                        BatteryType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_4").Trim()));
                    }else if (battery_structure == 5){
                        BatteryType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_5").Trim()));
                    }else if (battery_structure == 6){
                        BatteryType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_6").Trim()));
                    }else if (battery_structure == 7){
                        BatteryType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_7").Trim()));
                    }else if (battery_structure == 8){
                        BatteryType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_8").Trim()));
                    }
                }catch(ManagementException){ }
            }
        }
        private void battery_visible_off(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            BatteryStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_9").Trim()));
            BatteryModel.Visible = false;
            BatteryModel_V.Visible = false;
            BatteryName.Visible = false;
            BatteryName_V.Visible = false;
            BatteryType.Visible = false;
            BatteryType_V.Visible = false;
            battery_panel_1.Height = 43;
        }
        private void battery_visible_on(){
            BatteryModel.Visible = true;
            BatteryModel_V.Visible = true;
            BatteryName.Visible = true;
            BatteryName_V.Visible = true;
            BatteryType.Visible = true;
            BatteryType_V.Visible = true;
            battery_panel_1.Height = 225;
        }
        private void laptop_bg_process(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                ManagementObjectSearcher search_battery = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");
                PowerStatus power = SystemInformation.PowerStatus;
                do{
                    Single battery = power.BatteryLifePercent;
                    Single battery_process = battery * 100;
                    string battery_status = "%" + Convert.ToString(battery_process);
                    BatteryStatus_V.Text = battery_status;
                    foreach (ManagementObject query_battery_rotate in search_battery.Get()){
                        try{
                            // BATTERY VOLTAGE
                            double battery_voltage = Convert.ToDouble(query_battery_rotate["DesignVoltage"]) / 1000.0;
                            BatteryVoltage_V.Text = String.Format("{0:0.0} Volt", battery_voltage);
                        }catch (Exception){ }
                    }
                    BATTERYRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_10").Trim())) + " - " + battery_status;
                    if (power.PowerLineStatus == PowerLineStatus.Online){
                        if (battery_process == 100){
                            BatteryStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_10").Trim())) + " " + battery_status;
                        }else{
                            BatteryStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_11").Trim())) + " " + battery_status;
                        }
                    }else{
                        BatteryStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_12").Trim())) + " " + battery_status;
                    }
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        // OSD
        // ======================================================================================================
        private void osd(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                ManagementObjectSearcher search_sd = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_SystemDriver");
                foreach (ManagementObject query_sd_rotate in search_sd.Get()){
                    string driver_names = Convert.ToString(query_sd_rotate["Name"]).Trim() + ".sys";
                    string driver_paths = Convert.ToString(query_sd_rotate["DisplayName"]).Trim();
                    string driver_types = Convert.ToString(query_sd_rotate["ServiceType"]).Trim();
                    string driver_starts = Convert.ToString(query_sd_rotate["StartMode"]).Trim();
                    string driver_status = Convert.ToString(query_sd_rotate["State"]).Trim();
                    // DRIVER TYPE TR
                    if (driver_types == "Kernel Driver"){
                        driver_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_1").Trim()));
                    }else if (driver_types == "File System Driver"){
                        driver_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_2").Trim()));
                    }
                    // DRIVER STARTUP TR
                    if (driver_starts == "Boot"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_3").Trim()));
                    }else if (driver_starts == "Manual"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_4").Trim()));
                    }else if (driver_starts == "System"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_5").Trim()));
                    }else if (driver_starts == "Auto"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_6").Trim()));
                    }else if (driver_starts == "Disabled"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_7").Trim()));
                    }
                    // DRIVER STATUS TR
                    if (driver_status == "Stopped"){
                        driver_status = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_8").Trim()));
                    }else if (driver_status == "Running"){
                        driver_status = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_9").Trim()));
                    }
                    string[] driver_infos = { driver_names, driver_paths, driver_types, driver_starts, driver_status };
                    DataMainTable.Rows.Add(driver_infos);
                }
            }catch (ManagementException){ }
            TYSS_V.Text = DataMainTable.Rows.Count.ToString();
            DataMainTable.ClearSelection();
        }
        private void OSD_TextBox_TextChanged(object sender, EventArgs e){
            if (OSD_TextBox.Text == "" || OSD_TextBox.Text == string.Empty){ DataMainTable.ClearSelection(); DataMainTable.FirstDisplayedScrollingRowIndex = DataMainTable.Rows[0].Index; }
            else{ try{ foreach (DataGridViewRow row in DataMainTable.Rows){ if (row.Cells[0].Value.ToString().ToLower().Contains(OSD_TextBox.Text.ToString().Trim().ToLower())){ row.Selected = true; DataMainTable.FirstDisplayedScrollingRowIndex = row.Index; break; } } }catch(Exception){ } }
        }
        // GS SERVICE
        // ======================================================================================================
        private void gs_services(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                ManagementObjectSearcher search_service = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Service");
                foreach (ManagementObject query_service_rotate in search_service.Get()){
                    string service_path_names = Convert.ToString(query_service_rotate["Name"]).Trim();
                    string service_captions = Convert.ToString(query_service_rotate["DisplayName"]).Trim();
                    string service_types = Convert.ToString(query_service_rotate["ServiceType"]).Trim();
                    string service_start_modes = Convert.ToString(query_service_rotate["StartMode"]).Trim();
                    string service_states = Convert.ToString(query_service_rotate["State"]).Trim();
                    // SERVICE TYPE
                    if (service_types == "Kernel Driver"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_1").Trim()));
                    }else if (service_types == "File System Driver"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_2").Trim()));
                    }else if (service_types == "Adapter"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_3").Trim()));
                    }else if (service_types == "Recognizer Driver"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_4").Trim()));
                    }else if (service_types == "Own Process"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_5").Trim()));
                    }else if (service_types == "Share Process"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_6").Trim()));
                    }else if (service_types == "Interactive Process"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_7").Trim()));
                    }else if (service_types == "Unknown"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_8").Trim()));
                    }
                    // START MODE
                    if (service_start_modes == "Boot"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_9").Trim()));
                    }else if (service_start_modes == "Manual"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_10").Trim()));
                    }else if (service_start_modes == "System"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_11").Trim()));
                    }else if (service_start_modes == "Auto"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_12").Trim()));
                    }else if (service_start_modes == "Disabled"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_13").Trim()));
                    }
                    // STATE
                    if (service_states == "Stopped"){
                        service_states = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_14").Trim()));
                    }else if (service_states == "Running"){
                        service_states = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_15").Trim()));
                    }
                    string[] services_infos = { service_path_names, service_captions, service_types, service_start_modes, service_states };
                    ServicesDataGrid.Rows.Add(services_infos);
                }
            }catch (ManagementException){ }
            TYS_V.Text = ServicesDataGrid.Rows.Count.ToString();
            ServicesDataGrid.ClearSelection();
        }
        private void Services_SearchTextBox_TextChanged(object sender, EventArgs e){
            if (Services_SearchTextBox.Text == "" || Services_SearchTextBox.Text == string.Empty){ ServicesDataGrid.ClearSelection(); ServicesDataGrid.FirstDisplayedScrollingRowIndex = ServicesDataGrid.Rows[0].Index; }
            else{ try{ foreach (DataGridViewRow row in ServicesDataGrid.Rows){ if (row.Cells[0].Value.ToString().ToLower().Contains(Services_SearchTextBox.Text.ToString().Trim().ToLower())){ row.Selected = true; ServicesDataGrid.FirstDisplayedScrollingRowIndex = row.Index; break; } } }catch(Exception){ } }
        }
        // BUTTONS ROTATE
        // ======================================================================================================
        private Button active_btn;
        private void active_page(object btn_target){
            disabled_page();
            if (btn_target != null){
                if (active_btn != (Button)btn_target){
                    active_btn = (Button)btn_target;
                    if (theme == 1){ active_btn.BackColor = btn_colors[1]; }
                    else if (theme == 2){ active_btn.BackColor = btn_colors[3]; }
                }
            }
        }
        private void disabled_page(){
            foreach (Control disabled_btn in MenuPanel.Controls){
                if (theme == 1){ disabled_btn.BackColor = btn_colors[0]; }
                else if (theme == 2){ disabled_btn.BackColor = btn_colors[2]; }
            }
        }
        private void OSRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = OS;
            menu_btns = 1;
            menu_rp = 1;
            HeaderImage.Image = Properties.Resources.menu_windows;
            if (OSRotateBtn.BackColor != btn_colors[1] && OSRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_1").Trim()));
        }
        private void MBRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = MB;
            menu_btns = 2;
            menu_rp = 2;
            HeaderImage.Image = Properties.Resources.menu_motherboard;
            if (MBRotateBtn.BackColor != btn_colors[1] && MBRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()));
        }
        private void CPURotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = CPU;
            menu_btns = 3;
            menu_rp = 3;
            HeaderImage.Image = Properties.Resources.menu_cpu;
            if (CPURotateBtn.BackColor != btn_colors[1] && CPURotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()));
        }
        private void RAMRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = RAM;
            menu_btns = 4;
            menu_rp = 4;
            HeaderImage.Image = Properties.Resources.menu_ram;
            if (RAMRotateBtn.BackColor != btn_colors[1] && RAMRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()));
        }
        private void GPURotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = GPU;
            menu_btns = 5;
            menu_rp = 5;
            HeaderImage.Image = Properties.Resources.menu_gpu;
            if (GPURotateBtn.BackColor != btn_colors[1] && GPURotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()));
        }
        private void DISKRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = DISK;
            menu_btns = 6;
            menu_rp = 6;
            HeaderImage.Image = Properties.Resources.menu_disk;
            if (DISKRotateBtn.BackColor != btn_colors[1] && DISKRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()));
        }
        private void NETWORKRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = NETWORK;
            menu_btns = 7;
            menu_rp = 7;
            HeaderImage.Image = Properties.Resources.menu_network;
            if (NETWORKRotateBtn.BackColor != btn_colors[1] && NETWORKRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()));
        }
        private void SOUNDRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = SOUND;
            menu_btns = 8;
            menu_rp = 8;
            HeaderImage.Image = Properties.Resources.menu_sound;
            if (SOUNDRotateBtn.BackColor != btn_colors[1] && SOUNDRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()));
        }
        private void USBRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = USB;
            menu_btns = 9;
            menu_rp = 9;
            HeaderImage.Image = Properties.Resources.menu_usb;
            if (USBRotateBtn.BackColor != btn_colors[1] && USBRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()));
        }
        private void PILRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = BATTERY;
            menu_btns = 10;
            menu_rp = 10;
            HeaderImage.Image = Properties.Resources.menu_battery;
            if (BATTERYRotateBtn.BackColor != btn_colors[1] && BATTERYRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()));
        }
        private void OSDRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = OSD;
            menu_btns = 11;
            menu_rp = 11;
            HeaderImage.Image = Properties.Resources.menu_osd;
            if (OSDRotateBtn.BackColor != btn_colors[1] && OSDRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_11").Trim()));
        }
        private void ServicesRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = GSERVICE;
            menu_btns = 12;
            menu_rp = 12;
            HeaderImage.Image = Properties.Resources.menu_services;
            if (ServicesRotateBtn.BackColor != btn_colors[1] && ServicesRotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_12").Trim()));
        }
        // LANGUAGES SETTINGS
        // ======================================================================================================
        private ToolStripMenuItem selected_lang;
        private void select_lang_active(object target_lang){
            select_lang_deactive();
            if (target_lang != null){
                if (selected_lang != (ToolStripMenuItem)target_lang){
                    selected_lang = (ToolStripMenuItem)target_lang;
                    selected_lang.Checked = true;
                }
            }
        }
        private void select_lang_deactive(){
            foreach (ToolStripMenuItem disabled_lang in dilToolStripMenuItem.DropDownItems){
                disabled_lang.Checked = false;
            }
        }
        // LANG SWAP
        // ======================================================================================================
        private void türkçeToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != 1){ lang_preload("tr"); select_lang_active(sender); } 
        }
        private void englishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != 2){ lang_preload("en"); select_lang_active(sender); }
        }
        private void çinceToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != 3){ lang_preload("zh"); select_lang_active(sender); } 
        }
        private void hintçeToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != 4){ lang_preload("hi"); select_lang_active(sender); } 
        }
        private void ispanyolcaToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != 5){ lang_preload("es"); select_lang_active(sender); } 
        }
        private void lang_preload(string lang_type){
            lang_engine(lang_type);
            try{
                GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                glow_setting_save.GlowWriteSettings("Language", "LanguageStatus", lang_type);
            }catch (Exception){ }
            // LANG CHANGE NOTIFICATION
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            DialogResult lang_change_message = MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LangChange", "le_1").Trim())) + "\n\n" + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LangChange", "le_2").Trim())) + "\n\n" + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LangChange", "le_3").Trim())), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (lang_change_message == DialogResult.Yes){ Application.Restart(); }
        }
        private void lang_engine(string lang_type){
            try{
                switch (lang_type){
                    case "tr":
                        lang = 1;
                        lang_path = glow_lang_tr;
                        break;
                    case "en":
                        lang = 2;
                        lang_path = glow_lang_en;
                        break;
                    case "zh":
                        lang = 3;
                        lang_path = glow_lang_zh;
                        break;
                    case "hi":
                        lang = 4;
                        lang_path = glow_lang_hi;
                        break;
                    case "es":
                        lang = 5;
                        lang_path = glow_lang_es;
                        break;
                }
                // GLOBAL ENGINE
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                // HEADER TITLE
                if (menu_rp == 1){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_1").Trim()));
                }else if (menu_rp == 2){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()));
                }else if (menu_rp == 3){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()));
                }else if (menu_rp == 4){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()));
                }else if (menu_rp == 5){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()));
                }else if (menu_rp == 6){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()));
                }else if (menu_rp == 7){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()));
                }else if (menu_rp == 8){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()));
                }else if (menu_rp == 9){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()));
                }else if (menu_rp == 10){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()));
                }else if (menu_rp == 11){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_11").Trim()));
                }else if (menu_rp == 12){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_12").Trim()));
                }
                // HEADER
                bilgileriYazdırToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_1").Trim()));
                ayarlarToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_2").Trim()));
                temaToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_3").Trim()));
                dilToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_4").Trim()));
                yardımToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_5").Trim()));
                hakkımızdaToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_6").Trim()));
                gitHubSayfasıToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_7").Trim()));
                webSiteToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_8").Trim()));
                destekOlToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_9").Trim()));
                // THEMES
                açıkTemaToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderThemes", "theme_light").Trim()));
                koyuTemaToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderThemes", "theme_dark").Trim()));
                // LANGS
                türkçeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_tr").Trim()));
                englishToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_en").Trim()));
                çinceToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_zh").Trim()));
                hintçeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_hi").Trim()));
                ispanyolcaToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_es").Trim()));
                // MENU
                OSRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_1").Trim()));
                MBRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_2").Trim()));
                CPURotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_3").Trim()));
                RAMRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_4").Trim()));
                GPURotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_5").Trim()));
                DISKRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_6").Trim()));
                NETWORKRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_7").Trim()));
                SOUNDRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_8").Trim())); ;
                USBRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_9").Trim()));
                BATTERYRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_10").Trim()));
                OSDRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_11").Trim()));
                ServicesRotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_12").Trim()));
                // OS
                SystemUser.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_1").Trim()));
                ComputerName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_2").Trim()));
                SystemModel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_3").Trim()));
                OS_SavedUser.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_4").Trim()));
                OSName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_5").Trim()));
                OSManufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_6").Trim()));
                SystemVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_7").Trim()));
                OSBuild.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_8").Trim()));
                SystemArchitectural.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_9").Trim()));
                OSFamily.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_10").Trim()));
                OS_Serial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_11").Trim()));
                OS_Country.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_12").Trim()));
                OS_CharacterSet.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_13").Trim()));
                OS_EncryptionType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_14").Trim()));
                SystemRootIndex.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_15").Trim()));
                SystemBuildPart.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_16").Trim()));
                SystemTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_17").Trim()));
                OS_Install.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_18").Trim()));
                SystemWorkTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_19").Trim()));
                LastBootTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_20").Trim()));
                PortableOS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_21").Trim()));
                MouseWheelStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_22").Trim()));
                ScrollLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_23").Trim()));
                NumLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_24").Trim()));
                CapsLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_25").Trim()));
                BootPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_26").Trim()));
                SystemPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_27").Trim()));
                GPUWallpaper.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_28").Trim()));
                MainToolTip.SetToolTip(GoWallpaperRotate, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_29").Trim())), wp_rotate));
                // MB
                MotherBoardName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_1").Trim()));
                MotherBoardMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_2").Trim()));
                MotherBoardSerial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_3").Trim()));
                MB_Chipset.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_4").Trim()));
                BiosManufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_5").Trim()));
                BiosDate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_6").Trim()));
                BiosVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_7").Trim()));
                SmBiosVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_8").Trim()));
                MB_Model.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_9").Trim()));
                PrimaryBusType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_10").Trim()));
                BiosMajorMinor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_11").Trim()));
                SMBiosMajorMinor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_12").Trim()));
                // CPU
                CPUName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_1").Trim()));
                CPUManufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_2").Trim()));
                CPUArchitectural.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_3").Trim()));
                CPUNormalSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_4").Trim()));
                CPUDefaultSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_5").Trim()));
                CPU_L1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_6").Trim()));
                CPU_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_7").Trim()));
                CPU_L3.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_8").Trim()));
                CPUUsage.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_9").Trim()));
                CPUCoreCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_10").Trim()));
                CPULogicalCore.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_11").Trim()));
                CPUSocketDefinition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_12").Trim()));
                CPUFamily.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_13").Trim()));
                CPUVirtualization.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_14").Trim()));
                CPUVMMonitorExtension.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_15").Trim()));
                CPUSerialName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_16").Trim()));
                // RAM
                TotalRAM.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_1").Trim()));
                UsageRAMCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_2").Trim()));
                EmptyRamCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_3").Trim()));
                TotalVirtualRam.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_4").Trim()));
                EmptyVirtualRam.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_5").Trim()));
                RamSlotStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_6").Trim()));
                RamSlotSelectLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_7").Trim()));
                RamMik.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_8").Trim()));
                RamType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_9").Trim()));
                RamFrequency.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_10").Trim()));
                RamVolt.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_11").Trim()));
                RamFormFactor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_12").Trim()));
                RamSerial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_13").Trim()));
                RamManufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_14").Trim()));
                RamBankLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_15").Trim()));
                RAMDataWidth.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_16").Trim()));
                BellekType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_17").Trim()));
                RamPartNumber.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_18").Trim()));
                // GPU
                GPUName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_1").Trim()));
                GPUManufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_2").Trim()));
                GPUVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_3").Trim()));
                GPUDriverDate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_4").Trim()));
                GPUStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_5").Trim()));
                GPUDacType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_6").Trim()));
                GraphicDriversName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_7").Trim()));
                GpuInfFileName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_8").Trim()));
                INFSectionFile.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_9").Trim()));
                MonitorSelect.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_10").Trim()));
                MonitorBounds.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_11").Trim()));
                MonitorWorking.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_12").Trim()));
                MonitorResLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_13").Trim()));
                MonitorVirtualRes.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_14").Trim()));
                ScreenRefreshRate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_15").Trim()));
                MonitorPrimary.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_16").Trim()));
                // DISK
                DiskName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_1").Trim()));
                DiskModel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_2").Trim()));
                DiskMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_3").Trim()));
                DiskVolumeID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_4").Trim()));
                DiskVolumeName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_5").Trim()));
                DiskFirmWare.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_6").Trim()));
                DiskSeri.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_7").Trim()));
                DiskSize.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_8").Trim()));
                DiskFreeSpace.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_9").Trim()));
                DiskPartitionLayout.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_10").Trim()));
                DiskFileSystem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_11").Trim()));
                DiskType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_12").Trim()));
                DiskDriveType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_13").Trim()));
                DiskInterFace.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_14").Trim()));
                DiskPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_15").Trim()));
                DiskUniqueID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_16").Trim()));
                DiskLocation.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_17").Trim()));
                DiskHealt.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_18").Trim()));
                DiskBootPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_19").Trim()));
                DiskBootableStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_20").Trim()));
                // NETWORK
                ConnType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_1").Trim()));
                MacAdress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_2").Trim()));
                NetMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_3").Trim()));
                ServiceName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_4").Trim()));
                AdapterType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_5").Trim()));
                Guid.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_6").Trim()));
                ConnectionType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_7").Trim()));
                Dhcp_status.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_8").Trim()));
                Dhcp_server.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_9").Trim()));
                LocalConSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_10").Trim()));
                IPv4Adress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_11").Trim()));
                IPv6Adress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_12").Trim()));
                // AUDIO
                SoundDriver.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Audio", "ao_1").Trim()));
                SoundDriverStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Audio", "ao_2").Trim()));
                SoundConfig.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Audio", "ao_3").Trim()));
                SoundDeviceId.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Audio", "ao_4").Trim()));
                SoundMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Audio", "ao_5").Trim()));
                SoundPowerMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Audio", "ao_6").Trim()));
                // USB
                USBHardware.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_1").Trim()));
                USBStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_2").Trim()));
                USBConfigUsage.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_3").Trim()));
                USBHubDesc.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_4").Trim()));
                USBDeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_5").Trim()));
                USBNames.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_6").Trim()));
                USBPnpDeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_7").Trim()));
                // PIL
                BatteryStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_1").Trim()));
                BatteryModel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_2").Trim()));
                BatteryName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_3").Trim()));
                BatteryVoltage.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_4").Trim()));
                BatteryType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_5").Trim()));
                // OSD
                DataMainTable.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_1").Trim()));
                DataMainTable.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_2").Trim()));
                DataMainTable.Columns[2].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_3").Trim()));
                DataMainTable.Columns[3].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_4").Trim()));
                DataMainTable.Columns[4].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_5").Trim()));
                SearchDriverLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_6").Trim()));
                TYSS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_7").Trim()));
                // SERVICES
                ServicesDataGrid.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_1").Trim()));
                ServicesDataGrid.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_2").Trim()));
                ServicesDataGrid.Columns[2].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_3").Trim()));
                ServicesDataGrid.Columns[3].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_4").Trim()));
                ServicesDataGrid.Columns[4].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_5").Trim()));
                ServiceSearchLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_6").Trim()));
                TYS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_7").Trim()));
            }catch (Exception){ }
        }
        // ======================================================================================================
        // THEME SETTINGS
        private ToolStripMenuItem selected_theme;
        private void select_theme_active(object target_theme){
            select_theme_deactive();
            if (target_theme != null){
                if (selected_theme != (ToolStripMenuItem)target_theme){
                    selected_theme = (ToolStripMenuItem)target_theme;
                    selected_theme.Checked = true;
                }
            }
        }
        private void select_theme_deactive(){
            foreach (ToolStripMenuItem disabled_theme in temaToolStripMenuItem.DropDownItems){
                disabled_theme.Checked = false;
            }
        }
        // THEME SWAP
        // ======================================================================================================
        private void açıkTemaToolStripMenuItem_Click(object sender, EventArgs e){
            if (theme != 1){ color_mode(1); select_theme_active(sender); }
        }
        private void koyuTemaToolStripMenuItem_Click(object sender, EventArgs e){
            if (theme != 2){ color_mode(2); select_theme_active(sender); }
        }
        private void color_mode(int ts){
            switch (ts){
                case 1:
                    theme = ts;
                    break;
                case 2:
                    theme = ts;
                    break;
            }
            if (theme == 1){
                // TITLEBAR CHANGE AND HEADER CHANGE
                HeaderMenu.Renderer = new WhiteTheme();
                try{ if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                // CLEAR PRELOAD ITEMS
                if (ui_colors.Count > 0){ ui_colors.Clear(); }
                // HEADER AND TOOLTIP COLOR MODE
                ui_colors.Add(Color.Black);                         // 0
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 1
                // LEFT MENU COLOR MODE
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 2
                ui_colors.Add(Color.WhiteSmoke);                    // 3
                ui_colors.Add(Color.Black);                         // 4
                // CONTENT BG COLOR MODE
                ui_colors.Add(Color.WhiteSmoke);                    // 5
                // UI COLOR MODES
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 6
                ui_colors.Add(Color.Black);                         // 7
                ui_colors.Add(Color.FromArgb(4, 87, 160));          // 8
                ui_colors.Add(Color.WhiteSmoke);                    // 9
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 10
                ui_colors.Add(Color.WhiteSmoke);                    // 11
                ui_colors.Add(Color.Black);                         // 12
                ui_colors.Add(Color.White);                         // 13
                ui_colors.Add(Color.Black);                         // 14
                ui_colors.Add(Color.FromArgb(200, 200, 200));       // 15
                ui_colors.Add(Color.FromArgb(237, 237, 237));       // 16
                ui_colors.Add(Color.FromArgb(4, 87, 160));          // 17
                ui_colors.Add(Color.White);                         // 18
                ui_colors.Add(Color.WhiteSmoke);                    // 19
                // SAVE THEME
                try{
                    GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                    glow_setting_save.GlowWriteSettings("Theme", "ThemeStatus", "light");
                }catch (Exception){ }
            }else if (theme == 2){
                // TITLEBAR CHANGE AND HEADER CHANGE
                HeaderMenu.Renderer = new DarkTheme();
                try{ if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                // CLEAR PRELOAD ITEMS
                if (ui_colors.Count > 0){ ui_colors.Clear(); }
                // HEADER AND TOOLTIP COLOR MODE
                ui_colors.Add(Color.WhiteSmoke);                    // 0
                ui_colors.Add(Color.FromArgb(37, 37, 43));          // 1
                // LEFT MENU COLOR MODE
                ui_colors.Add(Color.FromArgb(37, 37, 43));          // 2
                ui_colors.Add(Color.FromArgb(53, 53, 61));          // 3
                ui_colors.Add(Color.WhiteSmoke);                    // 4
                // CONTENT BG COLOR MODE
                ui_colors.Add(Color.FromArgb(53, 53, 61));          // 5
                // UI COLOR MODES
                ui_colors.Add(Color.FromArgb(37, 37, 43));          // 6
                ui_colors.Add(Color.WhiteSmoke);                    // 7
                ui_colors.Add(Color.FromArgb(81, 171, 255));        // 8
                ui_colors.Add(Color.FromArgb(53, 53, 61));          // 9
                ui_colors.Add(Color.FromArgb(37, 37, 43));          // 10
                ui_colors.Add(Color.FromArgb(37, 37, 43));          // 11
                ui_colors.Add(Color.WhiteSmoke);                    // 12
                ui_colors.Add(Color.FromArgb(37, 37, 43));          // 13
                ui_colors.Add(Color.WhiteSmoke);                    // 14
                ui_colors.Add(Color.FromArgb(65, 65, 65));          // 15
                ui_colors.Add(Color.FromArgb(53, 53, 61));          // 16
                ui_colors.Add(Color.FromArgb(81, 171, 255));        // 17
                ui_colors.Add(Color.FromArgb(37, 37, 45));          // 18
                ui_colors.Add(Color.FromArgb(53, 53, 61));          // 19
                // SAVE THEME
                try{
                    GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                    glow_setting_save.GlowWriteSettings("Theme", "ThemeStatus", "dark");
                }catch (Exception){ }
            }
            theme_engine();
        }
        private void theme_engine(){
            try{
                // TOOLTIP
                MainToolTip.ForeColor = ui_colors[0];
                MainToolTip.BackColor = ui_colors[1];
                // HEADER PANEL
                HeaderPanel.BackColor = ui_colors[1];
                // HEADER PANEL TEXT
                HeaderText.ForeColor = ui_colors[0];
                // HEADER MENU
                HeaderMenu.ForeColor = ui_colors[0];
                HeaderMenu.BackColor = ui_colors[1];
                // HEADER MENU CONTENT
                bilgileriYazdırToolStripMenuItem.ForeColor = ui_colors[0];
                bilgileriYazdırToolStripMenuItem.BackColor = ui_colors[1];
                ayarlarToolStripMenuItem.ForeColor = ui_colors[0];
                ayarlarToolStripMenuItem.BackColor = ui_colors[1];
                temaToolStripMenuItem.BackColor = ui_colors[1];
                temaToolStripMenuItem.ForeColor = ui_colors[0];
                dilToolStripMenuItem.BackColor = ui_colors[1];
                dilToolStripMenuItem.ForeColor = ui_colors[0];
                yardımToolStripMenuItem.BackColor = ui_colors[1];
                yardımToolStripMenuItem.ForeColor = ui_colors[0];
                hakkımızdaToolStripMenuItem.BackColor = ui_colors[1];
                hakkımızdaToolStripMenuItem.ForeColor = ui_colors[0];
                gitHubSayfasıToolStripMenuItem.BackColor = ui_colors[1];
                gitHubSayfasıToolStripMenuItem.ForeColor = ui_colors[0];
                webSiteToolStripMenuItem.BackColor = ui_colors[1];
                webSiteToolStripMenuItem.ForeColor = ui_colors[0];
                destekOlToolStripMenuItem.BackColor = ui_colors[1];
                destekOlToolStripMenuItem.ForeColor = ui_colors[0];
                // THEMES
                açıkTemaToolStripMenuItem.BackColor = ui_colors[1];
                açıkTemaToolStripMenuItem.ForeColor = ui_colors[0];
                koyuTemaToolStripMenuItem.BackColor = ui_colors[1];
                koyuTemaToolStripMenuItem.ForeColor = ui_colors[0];
                // LANGS
                türkçeToolStripMenuItem.BackColor = ui_colors[1];
                türkçeToolStripMenuItem.ForeColor = ui_colors[0];
                englishToolStripMenuItem.BackColor = ui_colors[1];
                englishToolStripMenuItem.ForeColor = ui_colors[0];
                çinceToolStripMenuItem.BackColor = ui_colors[1];
                çinceToolStripMenuItem.ForeColor = ui_colors[0];
                hintçeToolStripMenuItem.BackColor = ui_colors[1];
                hintçeToolStripMenuItem.ForeColor = ui_colors[0];
                ispanyolcaToolStripMenuItem.BackColor = ui_colors[1];
                ispanyolcaToolStripMenuItem.ForeColor = ui_colors[0];
                // LEFT MENU
                MenuPanel.BackColor = ui_colors[2];
                OSRotateBtn.BackColor = ui_colors[2];
                MBRotateBtn.BackColor = ui_colors[2];
                CPURotateBtn.BackColor = ui_colors[2];
                RAMRotateBtn.BackColor = ui_colors[2];
                GPURotateBtn.BackColor = ui_colors[2];
                DISKRotateBtn.BackColor = ui_colors[2];
                NETWORKRotateBtn.BackColor = ui_colors[2];
                SOUNDRotateBtn.BackColor = ui_colors[2];
                USBRotateBtn.BackColor = ui_colors[2];
                BATTERYRotateBtn.BackColor = ui_colors[2];
                OSDRotateBtn.BackColor = ui_colors[2];
                ServicesRotateBtn.BackColor = ui_colors[2];
                // LEFT MENU BORDER
                OSRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                MBRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                CPURotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                RAMRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                GPURotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                DISKRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                NETWORKRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                SOUNDRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                USBRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                BATTERYRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                OSDRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                ServicesRotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                // LEFT MENU MOUSE HOVER
                OSRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                MBRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                CPURotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                RAMRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                GPURotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                DISKRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                NETWORKRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                SOUNDRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                USBRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                BATTERYRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                OSDRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                ServicesRotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                // LEFT MENU MOUSE DOWN
                OSRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                MBRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                CPURotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                RAMRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                GPURotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                DISKRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                NETWORKRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                SOUNDRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                USBRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                BATTERYRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                OSDRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                ServicesRotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                // LEFT MENU BUTTON TEXT COLOR
                OSRotateBtn.ForeColor = ui_colors[4];
                MBRotateBtn.ForeColor = ui_colors[4];
                CPURotateBtn.ForeColor = ui_colors[4];
                RAMRotateBtn.ForeColor = ui_colors[4];
                GPURotateBtn.ForeColor = ui_colors[4];
                DISKRotateBtn.ForeColor = ui_colors[4];
                NETWORKRotateBtn.ForeColor = ui_colors[4];
                SOUNDRotateBtn.ForeColor = ui_colors[4];
                USBRotateBtn.ForeColor = ui_colors[4];
                BATTERYRotateBtn.ForeColor = ui_colors[4];
                OSDRotateBtn.ForeColor = ui_colors[4];
                ServicesRotateBtn.ForeColor = ui_colors[4];
                // CONTENT BG
                BackColor = ui_colors[5];
                OS.BackColor = ui_colors[5];
                MB.BackColor = ui_colors[5];
                CPU.BackColor = ui_colors[5];
                RAM.BackColor = ui_colors[5];
                GPU.BackColor = ui_colors[5];
                DISK.BackColor = ui_colors[5];
                NETWORK.BackColor = ui_colors[5];
                SOUND.BackColor = ui_colors[5];
                USB.BackColor = ui_colors[5];
                BATTERY.BackColor = ui_colors[5];
                OSD.BackColor = ui_colors[5];
                GSERVICE.BackColor = ui_colors[5];
                // OS
                os_panel_1.BackColor = ui_colors[6];
                os_panel_2.BackColor = ui_colors[6];
                os_panel_3.BackColor = ui_colors[6];
                os_panel_4.BackColor = ui_colors[6];
                os_bottom_label.ForeColor = ui_colors[9];
                SystemUser.ForeColor = ui_colors[7];
                SystemUser_V.ForeColor = ui_colors[8];
                ComputerName.ForeColor = ui_colors[7];
                ComputerName_V.ForeColor = ui_colors[8];
                SystemModel.ForeColor = ui_colors[7];
                SystemModel_V.ForeColor = ui_colors[8];
                OS_SavedUser.ForeColor = ui_colors[7];
                OS_SavedUser_V.ForeColor = ui_colors[8];
                OSName.ForeColor = ui_colors[7];
                OSName_V.ForeColor = ui_colors[8];
                OSManufacturer.ForeColor = ui_colors[7];
                OSManufacturer_V.ForeColor = ui_colors[8];
                SystemVersion.ForeColor = ui_colors[7];
                SystemVersion_V.ForeColor = ui_colors[8];
                OSBuild.ForeColor = ui_colors[7];
                OSBuild_V.ForeColor = ui_colors[8];
                SystemArchitectural.ForeColor = ui_colors[7];
                SystemArchitectural_V.ForeColor = ui_colors[8];
                OSFamily.ForeColor = ui_colors[7];
                OSFamily_V.ForeColor = ui_colors[8];
                OS_Serial.ForeColor = ui_colors[7];
                OS_Serial_V.ForeColor = ui_colors[8];
                OS_Country.ForeColor = ui_colors[7];
                OS_Country_V.ForeColor = ui_colors[8];
                OS_CharacterSet.ForeColor = ui_colors[7];
                OS_CharacterSet_V.ForeColor = ui_colors[8];
                OS_EncryptionType.ForeColor = ui_colors[7];
                OS_EncryptionType_V.ForeColor = ui_colors[8];
                SystemRootIndex.ForeColor = ui_colors[7];
                SystemRootIndex_V.ForeColor = ui_colors[8];
                SystemBuildPart.ForeColor = ui_colors[7];
                SystemBuildPart_V.ForeColor = ui_colors[8];
                SystemTime.ForeColor = ui_colors[7];
                SystemTime_V.ForeColor = ui_colors[8];
                OS_Install.ForeColor = ui_colors[7];
                OS_Install_V.ForeColor = ui_colors[8];
                SystemWorkTime.ForeColor = ui_colors[7];
                SystemWorkTime_V.ForeColor = ui_colors[8];
                LastBootTime.ForeColor = ui_colors[7];
                LastBootTime_V.ForeColor = ui_colors[8];
                PortableOS.ForeColor = ui_colors[7];
                PortableOS_V.ForeColor = ui_colors[8];
                MouseWheelStatus.ForeColor = ui_colors[7];
                MouseWheelStatus_V.ForeColor = ui_colors[8];
                ScrollLockStatus.ForeColor = ui_colors[7];
                ScrollLockStatus_V.ForeColor = ui_colors[8];
                NumLockStatus.ForeColor = ui_colors[7];
                NumLockStatus_V.ForeColor = ui_colors[8];
                CapsLockStatus.ForeColor = ui_colors[7];
                CapsLockStatus_V.ForeColor = ui_colors[8];
                BootPartition.ForeColor = ui_colors[7];
                BootPartition_V.ForeColor = ui_colors[8];
                SystemPartition.ForeColor = ui_colors[7];
                SystemPartition_V.ForeColor = ui_colors[8];
                GPUWallpaper.ForeColor = ui_colors[7];
                GPUWallpaper_V.ForeColor = ui_colors[8];
                // MB
                mb_panel_1.BackColor = ui_colors[6];
                mb_panel_2.BackColor = ui_colors[6];
                mb_bottom_1.ForeColor = ui_colors[9];
                MotherBoardName.ForeColor = ui_colors[7];
                MotherBoardName_V.ForeColor = ui_colors[8];
                MotherBoardMan.ForeColor = ui_colors[7];
                MotherBoardMan_V.ForeColor = ui_colors[8];
                MotherBoardSerial.ForeColor = ui_colors[7];
                MotherBoardSerial_V.ForeColor = ui_colors[8];
                MB_Chipset.ForeColor = ui_colors[7];
                MB_Chipset_V.ForeColor = ui_colors[8];
                BiosManufacturer.ForeColor = ui_colors[7];
                BiosManufacturer_V.ForeColor = ui_colors[8];
                BiosDate.ForeColor = ui_colors[7];
                BiosDate_V.ForeColor = ui_colors[8];
                BiosVersion.ForeColor = ui_colors[7];
                BiosVersion_V.ForeColor = ui_colors[8];
                SmBiosVersion.ForeColor = ui_colors[7];
                SmBiosVersion_V.ForeColor = ui_colors[8];
                MB_Model.ForeColor = ui_colors[7];
                MB_Model_V.ForeColor = ui_colors[8];
                PrimaryBusType.ForeColor = ui_colors[7];
                PrimaryBusType_V.ForeColor = ui_colors[8];
                BiosMajorMinor.ForeColor = ui_colors[7];
                BiosMajorMinor_V.ForeColor = ui_colors[8];
                SMBiosMajorMinor.ForeColor = ui_colors[7];
                SMBiosMajorMinor_V.ForeColor = ui_colors[8];
                // CPU
                cpu_panel_1.BackColor = ui_colors[6];
                cpu_panel_2.BackColor = ui_colors[6];
                cpu_bottom_1.ForeColor = ui_colors[9];
                CPUName.ForeColor = ui_colors[7];
                CPUName_V.ForeColor = ui_colors[8];
                CPUManufacturer.ForeColor = ui_colors[7];
                CPUManufacturer_V.ForeColor = ui_colors[8];
                CPUArchitectural.ForeColor = ui_colors[7];
                CPUArchitectural_V.ForeColor = ui_colors[8];
                CPUNormalSpeed.ForeColor = ui_colors[7];
                CPUNormalSpeed_V.ForeColor = ui_colors[8];
                CPUDefaultSpeed.ForeColor = ui_colors[7];
                CPUDefaultSpeed_V.ForeColor = ui_colors[8];
                CPU_L1.ForeColor = ui_colors[7];
                CPU_L1_V.ForeColor = ui_colors[8];
                CPU_L2.ForeColor = ui_colors[7];
                CPU_L2_V.ForeColor = ui_colors[8];
                CPU_L3.ForeColor = ui_colors[7];
                CPU_L3_V.ForeColor = ui_colors[8];
                CPUUsage.ForeColor = ui_colors[7];
                CPUUsage_V.ForeColor = ui_colors[8];
                CPUCoreCount.ForeColor = ui_colors[7];
                CPUCoreCount_V.ForeColor = ui_colors[8];
                CPULogicalCore.ForeColor = ui_colors[7];
                CPULogicalCore_V.ForeColor = ui_colors[8];
                CPUSocketDefinition.ForeColor = ui_colors[7];
                CPUSocketDefinition_V.ForeColor = ui_colors[8];
                CPUFamily.ForeColor = ui_colors[7];
                CPUFamily_V.ForeColor = ui_colors[8];
                CPUVirtualization.ForeColor = ui_colors[7];
                CPUVirtualization_V.ForeColor = ui_colors[8];
                CPUVMMonitorExtension.ForeColor = ui_colors[7];
                CPUVMMonitorExtension_V.ForeColor = ui_colors[8];
                CPUSerialName.ForeColor = ui_colors[7];
                CPUSerialName_V.ForeColor = ui_colors[8];
                // RAM
                ram_panel_1.BackColor = ui_colors[6];
                ram_panel_2.BackColor = ui_colors[6];
                ram_bottom_1.ForeColor = ui_colors[9];
                TotalRAM.ForeColor = ui_colors[7];
                TotalRAM_V.ForeColor = ui_colors[8];
                UsageRAMCount.ForeColor = ui_colors[7];
                UsageRAMCount_V.ForeColor = ui_colors[8];
                EmptyRamCount.ForeColor = ui_colors[7];
                EmptyRamCount_V.ForeColor = ui_colors[8];
                TotalVirtualRam.ForeColor = ui_colors[7];
                TotalVirtualRam_V.ForeColor = ui_colors[8];
                EmptyVirtualRam.ForeColor = ui_colors[7];
                EmptyVirtualRam_V.ForeColor = ui_colors[8];
                RamSlotStatus.ForeColor = ui_colors[7];
                RamSlotStatus_V.ForeColor = ui_colors[8];
                RamSlotSelectLabel.ForeColor = ui_colors[7];
                RAMSelectList.BackColor = ui_colors[10];
                RAMSelectList.ForeColor = ui_colors[8];
                RamMik.ForeColor = ui_colors[7];
                RamMik_V.ForeColor = ui_colors[8];
                RamType.ForeColor = ui_colors[7];
                RamType_V.ForeColor = ui_colors[8];
                RamFrequency.ForeColor = ui_colors[7];
                RamFrequency_V.ForeColor = ui_colors[8];
                RamVolt.ForeColor = ui_colors[7];
                RamVolt_V.ForeColor = ui_colors[8];
                RamFormFactor.ForeColor = ui_colors[7];
                RamFormFactor_V.ForeColor = ui_colors[8];
                RamSerial.ForeColor = ui_colors[7];
                RamSerial_V.ForeColor = ui_colors[8];
                RamManufacturer.ForeColor = ui_colors[7];
                RamManufacturer_V.ForeColor = ui_colors[8];
                RamBankLabel.ForeColor = ui_colors[7];
                RamBankLabel_V.ForeColor = ui_colors[8];
                RAMDataWidth.ForeColor = ui_colors[7];
                RAMDataWidth_V.ForeColor = ui_colors[8];
                BellekType.ForeColor = ui_colors[7];
                BellekType_V.ForeColor = ui_colors[8];
                RamPartNumber.ForeColor = ui_colors[7];
                RamPartNumber_V.ForeColor = ui_colors[8];
                // GPU
                gpu_panel_1.BackColor = ui_colors[6];
                gpu_panel_2.BackColor = ui_colors[6];
                gpu_bottom_1.ForeColor = ui_colors[9];
                GPUName.ForeColor = ui_colors[7];
                GPUSelect.BackColor = ui_colors[10];
                GPUSelect.ForeColor = ui_colors[8];
                GPUManufacturer.ForeColor = ui_colors[7];
                GPUManufacturer_V.ForeColor = ui_colors[8];
                GPUVersion.ForeColor = ui_colors[7];
                GPUVersion_V.ForeColor = ui_colors[8];
                GPUDriverDate.ForeColor = ui_colors[7];
                GPUDriverDate_V.ForeColor = ui_colors[8];
                GPUStatus.ForeColor = ui_colors[7];
                GPUStatus_V.ForeColor = ui_colors[8];
                GPUDacType.ForeColor = ui_colors[7];
                GPUDacType_V.ForeColor = ui_colors[8];
                GraphicDriversName.ForeColor = ui_colors[7];
                GraphicDriversName_V.ForeColor = ui_colors[8];
                GpuInfFileName.ForeColor = ui_colors[7];
                GpuInfFileName_V.ForeColor = ui_colors[8];
                INFSectionFile.ForeColor = ui_colors[7];
                INFSectionFile_V.ForeColor = ui_colors[8];
                MonitorSelectList.BackColor = ui_colors[10];
                MonitorSelectList.ForeColor = ui_colors[8];
                MonitorSelect.ForeColor = ui_colors[7];
                MonitorBounds.ForeColor = ui_colors[7];
                MonitorBounds_V.ForeColor = ui_colors[8];
                MonitorWorking.ForeColor = ui_colors[7];
                MonitorWorking_V.ForeColor = ui_colors[8];
                MonitorResLabel.ForeColor = ui_colors[7];
                MonitorResLabel_V.ForeColor = ui_colors[8];
                MonitorVirtualRes.ForeColor = ui_colors[7];
                MonitorVirtualRes_V.ForeColor = ui_colors[8];
                ScreenRefreshRate.ForeColor = ui_colors[7];
                ScreenRefreshRate_V.ForeColor = ui_colors[8];
                MonitorPrimary.ForeColor = ui_colors[7];
                MonitorPrimary_V.ForeColor = ui_colors[8];
                // DISK
                disk_panel_1.BackColor = ui_colors[6];
                disk_panel_2.BackColor = ui_colors[6];
                disk_bottom_label.ForeColor = ui_colors[9];
                DiskName.ForeColor = ui_colors[7];
                DiskSelectBox.BackColor = ui_colors[10];
                DiskSelectBox.ForeColor = ui_colors[8];
                DiskModel.ForeColor = ui_colors[7];
                DiskModel_V.ForeColor = ui_colors[8];
                DiskMan.ForeColor = ui_colors[7];
                DiskMan_V.ForeColor = ui_colors[8];
                DiskVolumeID.ForeColor = ui_colors[7];
                DiskVolumeID_V.ForeColor = ui_colors[8];
                DiskVolumeName.ForeColor = ui_colors[7];
                DiskVolumeName_V.ForeColor = ui_colors[8];
                DiskFirmWare.ForeColor = ui_colors[7];
                DiskFirmWare_V.ForeColor = ui_colors[8];
                DiskSeri.ForeColor = ui_colors[7];
                DiskSeri_V.ForeColor = ui_colors[8];
                DiskSize.ForeColor = ui_colors[7];
                DiskSize_V.ForeColor = ui_colors[8];
                DiskFreeSpace.ForeColor = ui_colors[7];
                DiskFreeSpace_V.ForeColor = ui_colors[8];
                DiskPartitionLayout.ForeColor = ui_colors[7];
                DiskPartitionLayout_V.ForeColor = ui_colors[8];
                DiskFileSystem.ForeColor = ui_colors[7];
                DiskFileSystem_V.ForeColor = ui_colors[8];
                DiskType.ForeColor = ui_colors[7];
                DiskType_V.ForeColor = ui_colors[8];
                DiskDriveType.ForeColor = ui_colors[7];
                DiskDriveType_V.ForeColor = ui_colors[8];
                DiskInterFace.ForeColor = ui_colors[7];
                DiskInterFace_V.ForeColor = ui_colors[8];
                DiskPartition.ForeColor = ui_colors[7];
                DiskPartition_V.ForeColor = ui_colors[8];
                DiskUniqueID.ForeColor = ui_colors[7];
                DiskUniqueID_V.ForeColor = ui_colors[8];
                DiskLocation.ForeColor = ui_colors[7];
                DiskLocation_V.ForeColor = ui_colors[8];
                DiskBootPartition.ForeColor = ui_colors[7];
                DiskBootPartition_V.ForeColor = ui_colors[8];
                DiskHealt.ForeColor = ui_colors[7];
                DiskHealt_V.ForeColor = ui_colors[8];
                DiskBootableStatus.ForeColor = ui_colors[7];
                DiskBootableStatus_V.ForeColor = ui_colors[8];
                // NETWORK
                network_panel_1.BackColor = ui_colors[6];
                network_bottom_label.ForeColor = ui_colors[9];
                ListNetwork.BackColor = ui_colors[10];
                ListNetwork.ForeColor = ui_colors[8];
                ConnType.ForeColor = ui_colors[7];
                MacAdress.ForeColor = ui_colors[7];
                MacAdress_V.ForeColor = ui_colors[8];
                NetMan.ForeColor = ui_colors[7];
                NetMan_V.ForeColor = ui_colors[8];
                ServiceName.ForeColor = ui_colors[7];
                ServiceName_V.ForeColor = ui_colors[8];
                AdapterType.ForeColor = ui_colors[7];
                AdapterType_V.ForeColor = ui_colors[8];
                Guid.ForeColor = ui_colors[7];
                Guid_V.ForeColor = ui_colors[8];
                ConnectionType.ForeColor = ui_colors[7];
                ConnectionType_V.ForeColor = ui_colors[8];
                Dhcp_status.ForeColor = ui_colors[7];
                Dhcp_status_V.ForeColor = ui_colors[8];
                Dhcp_server.ForeColor = ui_colors[7];
                Dhcp_server_V.ForeColor = ui_colors[8];
                LocalConSpeed.ForeColor = ui_colors[7];
                LocalConSpeed_V.ForeColor = ui_colors[8];
                IPv4Adress.ForeColor = ui_colors[7];
                IPv4Adress_V.ForeColor = ui_colors[8];
                IPv6Adress.ForeColor = ui_colors[7];
                IPv6Adress_V.ForeColor = ui_colors[8];
                // AUDIO
                sound_panel_1.BackColor = ui_colors[6];
                SoundDriverMainList.BackColor = ui_colors[10];
                SoundDriverMainList.ForeColor = ui_colors[8];
                SoundDriver.ForeColor = ui_colors[7];
                SoundDriverStatus.ForeColor = ui_colors[7];
                SoundDriverStatus_V.ForeColor = ui_colors[8];
                SoundConfig.ForeColor = ui_colors[7];
                SoundConfig_V.ForeColor = ui_colors[8];
                SoundDeviceId.ForeColor = ui_colors[7];
                SoundDeviceId_V.ForeColor = ui_colors[8];
                SoundMan.ForeColor = ui_colors[7];
                SoundMan_V.ForeColor = ui_colors[8];
                SoundPowerMan.ForeColor = ui_colors[7];
                SoundPowerMan_V.ForeColor = ui_colors[8];
                // USB
                usb_panel_1.BackColor = ui_colors[6];
                USBMainList.BackColor = ui_colors[10];
                USBMainList.ForeColor = ui_colors[8];
                USBHardware.ForeColor = ui_colors[7];
                USBStatus.ForeColor = ui_colors[7];
                USBStatus_V.ForeColor = ui_colors[8];
                USBConfigUsage.ForeColor = ui_colors[7];
                USBConfigUsage_V.ForeColor = ui_colors[8];
                USBHubDesc.ForeColor = ui_colors[7];
                USBHubDesc_V.ForeColor = ui_colors[8];
                USBDeviceID.ForeColor = ui_colors[7];
                USBDeviceID_V.ForeColor = ui_colors[8];
                USBNames.ForeColor = ui_colors[7];
                USBNames_V.ForeColor = ui_colors[8];
                USBPnpDeviceID.ForeColor = ui_colors[7];
                USBPnpDeviceID_V.ForeColor = ui_colors[8];
                // BATTERY
                battery_panel_1.BackColor = ui_colors[6];
                BatteryStatus.ForeColor = ui_colors[7];
                BatteryStatus_V.ForeColor = ui_colors[8];
                BatteryModel.ForeColor = ui_colors[7];
                BatteryModel_V.ForeColor = ui_colors[8];
                BatteryName.ForeColor = ui_colors[7];
                BatteryName_V.ForeColor = ui_colors[8];
                BatteryVoltage.ForeColor = ui_colors[7];
                BatteryVoltage_V.ForeColor = ui_colors[8];
                BatteryType.ForeColor = ui_colors[7];
                BatteryType_V.ForeColor = ui_colors[8];
                // INSTALLED DRIVERS
                osd_panel_1.BackColor = ui_colors[6];
                OSD_TextBox.BackColor = ui_colors[11];
                OSD_TextBox.ForeColor = ui_colors[12];
                TYSS.ForeColor = ui_colors[7];
                TYSS_V.ForeColor = ui_colors[8];
                SearchDriverLabel.ForeColor = ui_colors[7];
                DataMainTable.BackgroundColor = ui_colors[13];
                DataMainTable.GridColor = ui_colors[15];
                DataMainTable.DefaultCellStyle.BackColor = ui_colors[13];
                DataMainTable.DefaultCellStyle.ForeColor = ui_colors[14];
                DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = ui_colors[16];
                DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = ui_colors[17];
                DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = ui_colors[17];
                DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = ui_colors[18];
                DataMainTable.DefaultCellStyle.SelectionBackColor = ui_colors[17];
                DataMainTable.DefaultCellStyle.SelectionForeColor = ui_colors[18];
                // SERVICES
                service_panel_1.BackColor = ui_colors[6];
                Services_SearchTextBox.BackColor = ui_colors[11];
                Services_SearchTextBox.ForeColor = ui_colors[12];
                TYS.ForeColor = ui_colors[7];
                TYS_V.ForeColor = ui_colors[8];
                ServiceSearchLabel.ForeColor = ui_colors[7];
                ServicesDataGrid.BackgroundColor = ui_colors[13];
                ServicesDataGrid.GridColor = ui_colors[15];
                ServicesDataGrid.DefaultCellStyle.BackColor = ui_colors[13];
                ServicesDataGrid.DefaultCellStyle.ForeColor = ui_colors[14];
                ServicesDataGrid.AlternatingRowsDefaultCellStyle.BackColor = ui_colors[16];
                ServicesDataGrid.ColumnHeadersDefaultCellStyle.BackColor = ui_colors[17];
                ServicesDataGrid.ColumnHeadersDefaultCellStyle.SelectionBackColor = ui_colors[17];
                ServicesDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = ui_colors[18];
                ServicesDataGrid.DefaultCellStyle.SelectionBackColor = ui_colors[17];
                ServicesDataGrid.DefaultCellStyle.SelectionForeColor = ui_colors[18];
                // ROTATE MENU
                if (menu_btns == 1){
                    OSRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 2){
                    MBRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 3){
                    CPURotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 4){
                    RAMRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 5){
                    GPURotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 6){
                    DISKRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 7){
                    NETWORKRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 8){
                    SOUNDRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 9){
                    USBRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 10){
                    BATTERYRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 11){
                    OSDRotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 12){
                    ServicesRotateBtn.BackColor = ui_colors[19];
                }
            }catch (Exception){ }
        }
        // PRINT ENGINES
        // ======================================================================================================
        List<string> PrintEngineList = new List<string>();
        private void bilgileriYazdırToolStripMenuItem_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                print_engine();
            }catch (Exception){
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_1").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // PRINT ENGINE
        private void print_engine(){
            // HEADER
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            PrintEngineList.Add("<------------------------------------------------>");
            PrintEngineList.Add($"<------------- {Application.ProductName.ToUpper()} - {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_2").Trim()))} ------------->");
            PrintEngineList.Add("<------------------------------------------------>" + Environment.NewLine);
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // OS
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_1").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(SystemUser.Text + " " + SystemUser_V.Text);
            PrintEngineList.Add(ComputerName.Text + " " + ComputerName_V.Text);
            PrintEngineList.Add(SystemModel.Text + " " + SystemModel_V.Text);
            PrintEngineList.Add(OS_SavedUser.Text + " " + OS_SavedUser_V.Text);
            PrintEngineList.Add(OSName.Text + " " + OSName_V.Text);
            PrintEngineList.Add(OSManufacturer.Text + " " + OSManufacturer_V.Text);
            PrintEngineList.Add(SystemVersion.Text + " " + SystemVersion_V.Text);
            PrintEngineList.Add(OSBuild.Text + " " + OSBuild_V.Text);
            PrintEngineList.Add(SystemArchitectural.Text + " " + SystemArchitectural_V.Text);
            PrintEngineList.Add(OSFamily.Text + " " + OSFamily_V.Text);
            PrintEngineList.Add(OS_Serial.Text + " " + OS_Serial_V.Text);
            PrintEngineList.Add(OS_Country.Text + " " + OS_Country_V.Text);
            PrintEngineList.Add(OS_CharacterSet.Text + " " + OS_CharacterSet_V.Text);
            PrintEngineList.Add(OS_EncryptionType.Text + " " + OS_EncryptionType_V.Text);
            PrintEngineList.Add(SystemRootIndex.Text + " " + SystemRootIndex_V.Text);
            PrintEngineList.Add(SystemBuildPart.Text + " " + SystemBuildPart_V.Text);
            PrintEngineList.Add(SystemTime.Text + " " + SystemTime_V.Text);
            PrintEngineList.Add(OS_Install.Text + " " + OS_Install_V.Text);
            PrintEngineList.Add(SystemWorkTime.Text + " " + SystemWorkTime_V.Text);
            PrintEngineList.Add(LastBootTime.Text + " " + LastBootTime_V.Text);
            PrintEngineList.Add(PortableOS.Text + " " + PortableOS_V.Text);
            PrintEngineList.Add(MouseWheelStatus.Text + " " + MouseWheelStatus_V.Text);
            PrintEngineList.Add(ScrollLockStatus.Text + " " + ScrollLockStatus_V.Text);
            PrintEngineList.Add(NumLockStatus.Text + " " + NumLockStatus_V.Text);
            PrintEngineList.Add(CapsLockStatus.Text + " " + CapsLockStatus_V.Text);
            PrintEngineList.Add(BootPartition.Text + " " + BootPartition_V.Text);
            PrintEngineList.Add(SystemPartition.Text + " " + SystemPartition_V.Text);
            PrintEngineList.Add(GPUWallpaper.Text + " " + GPUWallpaper_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            // MOTHERBOARD
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(MotherBoardName.Text + " " + MotherBoardName_V.Text);
            PrintEngineList.Add(MotherBoardMan.Text + " " + MotherBoardMan_V.Text);
            PrintEngineList.Add(MotherBoardSerial.Text + " " + MotherBoardSerial_V.Text);
            PrintEngineList.Add(MB_Chipset.Text + " " + MB_Chipset_V.Text);
            PrintEngineList.Add(BiosManufacturer.Text + " " + BiosManufacturer_V.Text);
            PrintEngineList.Add(BiosDate.Text + " " + BiosDate_V.Text);
            PrintEngineList.Add(BiosVersion.Text + " " + BiosVersion_V.Text);
            PrintEngineList.Add(SmBiosVersion.Text + " " + SmBiosVersion_V.Text);
            PrintEngineList.Add(MB_Model.Text + " " + MB_Model_V.Text);
            PrintEngineList.Add(PrimaryBusType.Text + " " + PrimaryBusType_V.Text);
            PrintEngineList.Add(BiosMajorMinor.Text + " " + BiosMajorMinor_V.Text);
            PrintEngineList.Add(SMBiosMajorMinor.Text + " " + SMBiosMajorMinor_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            // CPU
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(CPUName.Text + " " + CPUName_V.Text);
            PrintEngineList.Add(CPUManufacturer.Text + " " + CPUManufacturer_V.Text);
            PrintEngineList.Add(CPUArchitectural.Text + " " + CPUArchitectural_V.Text);
            PrintEngineList.Add(CPUNormalSpeed.Text + " " + CPUNormalSpeed_V.Text);
            PrintEngineList.Add(CPUDefaultSpeed.Text + " " + CPUDefaultSpeed_V.Text);
            PrintEngineList.Add(CPU_L1.Text + " " + CPU_L1_V.Text);
            PrintEngineList.Add(CPU_L2.Text + " " + CPU_L2_V.Text);
            PrintEngineList.Add(CPU_L3.Text + " " + CPU_L3_V.Text);
            PrintEngineList.Add(CPUUsage.Text + " " + CPUUsage_V.Text);
            PrintEngineList.Add(CPUCoreCount.Text + " " + CPUCoreCount_V.Text);
            PrintEngineList.Add(CPULogicalCore.Text + " " + CPULogicalCore_V.Text);
            PrintEngineList.Add(CPUSocketDefinition.Text + " " + CPUSocketDefinition_V.Text);
            PrintEngineList.Add(CPUFamily.Text + " " + CPUFamily_V.Text);
            PrintEngineList.Add(CPUVirtualization.Text + " " + CPUVirtualization_V.Text);
            PrintEngineList.Add(CPUVMMonitorExtension.Text + " " + CPUVMMonitorExtension_V.Text);
            PrintEngineList.Add(CPUSerialName.Text + " " + CPUSerialName_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            // RAM
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(TotalRAM.Text + " " + TotalRAM_V.Text);
            PrintEngineList.Add(UsageRAMCount.Text + " " + UsageRAMCount_V.Text);
            PrintEngineList.Add(EmptyRamCount.Text + " " + EmptyRamCount_V.Text);
            PrintEngineList.Add(TotalVirtualRam.Text + " " + TotalVirtualRam_V.Text);
            PrintEngineList.Add(EmptyVirtualRam.Text + " " + EmptyVirtualRam_V.Text);
            PrintEngineList.Add(RamSlotStatus.Text + " " + RamSlotStatus_V.Text + Environment.NewLine);
            try{
                int ram_slot = RAMSelectList.Items.Count;
                for (int rs = 1; rs <= ram_slot; rs++){
                    RAMSelectList.SelectedIndex = rs - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_7").Trim())) + " #" + rs + Environment.NewLine);
                    PrintEngineList.Add(RamMik.Text + " " + RamMik_V.Text);
                    PrintEngineList.Add(RamType.Text + " " + RamType_V.Text);
                    PrintEngineList.Add(RamFrequency.Text + " " + RamFrequency_V.Text);
                    PrintEngineList.Add(RamVolt.Text + " " + RamVolt_V.Text);
                    PrintEngineList.Add(RamFormFactor.Text + " " + RamFormFactor_V.Text);
                    PrintEngineList.Add(RamSerial.Text + " " + RamSerial_V.Text);
                    PrintEngineList.Add(RamManufacturer.Text + " " + RamManufacturer_V.Text);
                    PrintEngineList.Add(RamBankLabel.Text + " " + RamBankLabel_V.Text);
                    PrintEngineList.Add(RAMDataWidth.Text + " " + RAMDataWidth_V.Text);
                    PrintEngineList.Add(BellekType.Text + " " + BellekType_V.Text);
                    PrintEngineList.Add(RamPartNumber.Text + " " + RamPartNumber_V.Text + Environment.NewLine);
                }
                RAMSelectList.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // GPU
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()))} ------->" + Environment.NewLine);
            try{
                int gpu_amount = GPUSelect.Items.Count;
                for (int gpu_render = 1; gpu_render <= gpu_amount; gpu_render++){
                    GPUSelect.SelectedIndex = gpu_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_17").Trim())) + " #" + gpu_render + Environment.NewLine);
                    PrintEngineList.Add(GPUName.Text + " " + GPUSelect.SelectedItem.ToString());
                    PrintEngineList.Add(GPUManufacturer.Text + " " + GPUManufacturer_V.Text);
                    PrintEngineList.Add(GPUVersion.Text + " " + GPUVersion_V.Text);
                    PrintEngineList.Add(GPUDriverDate.Text + " " + GPUDriverDate_V.Text);
                    PrintEngineList.Add(GPUStatus.Text + " " + GPUStatus_V.Text);
                    PrintEngineList.Add(GPUDacType.Text + " " + GPUDacType_V.Text);
                    PrintEngineList.Add(GraphicDriversName.Text + " " + GraphicDriversName_V.Text);
                    PrintEngineList.Add(GpuInfFileName.Text + " " + GpuInfFileName_V.Text);
                    PrintEngineList.Add(INFSectionFile.Text + " " + INFSectionFile_V.Text + Environment.NewLine);
                }
                GPUSelect.SelectedIndex = 0;
            }catch (Exception){ }
            try{
                int screen_amount = MonitorSelectList.Items.Count;
                for (int sa = 1; sa <= screen_amount; sa++){
                    MonitorSelectList.SelectedIndex = sa - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_18").Trim())) + " #" + sa + Environment.NewLine);
                    PrintEngineList.Add(MonitorBounds.Text + " " + MonitorBounds_V.Text);
                    PrintEngineList.Add(MonitorWorking.Text + " " + MonitorWorking_V.Text);
                    PrintEngineList.Add(MonitorResLabel.Text + " " + MonitorResLabel_V.Text);
                    PrintEngineList.Add(MonitorVirtualRes.Text + " " + MonitorVirtualRes_V.Text);
                    PrintEngineList.Add(ScreenRefreshRate.Text + " " + ScreenRefreshRate_V.Text);
                    PrintEngineList.Add(MonitorPrimary.Text + " " + MonitorPrimary_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
                MonitorSelectList.SelectedIndex = 0;
            }catch (Exception){ }
            // STORAGE
        
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()))} ------->" + Environment.NewLine);
            try{
                int disk_amount = DiskSelectBox.Items.Count;
                for (int disk_render = 1; disk_render <= disk_amount; disk_render++){
                    DiskSelectBox.SelectedIndex = disk_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_21").Trim())) + " #" + disk_render + Environment.NewLine);
                    PrintEngineList.Add(DiskName.Text + " " + DiskSelectBox.SelectedItem.ToString());
                    PrintEngineList.Add(DiskModel.Text + " " + DiskModel_V.Text);
                    PrintEngineList.Add(DiskMan.Text + " " + DiskMan_V.Text);
                    PrintEngineList.Add(DiskVolumeID.Text + " " + DiskVolumeID_V.Text);
                    PrintEngineList.Add(DiskVolumeName.Text + " " + DiskVolumeName_V.Text);
                    PrintEngineList.Add(DiskFirmWare.Text + " " + DiskFirmWare_V.Text);
                    PrintEngineList.Add(DiskSeri.Text + " " + DiskSeri_V.Text);
                    PrintEngineList.Add(DiskSize.Text + " " + DiskSize_V.Text);
                    PrintEngineList.Add(DiskFreeSpace.Text + " " + DiskFreeSpace_V.Text);
                    PrintEngineList.Add(DiskPartitionLayout.Text + " " + DiskPartitionLayout_V.Text);
                    PrintEngineList.Add(DiskFileSystem.Text + " " + DiskFileSystem_V.Text);
                    PrintEngineList.Add(DiskType.Text + " " + DiskType_V.Text);
                    PrintEngineList.Add(DiskDriveType.Text + " " + DiskDriveType_V.Text);
                    PrintEngineList.Add(DiskInterFace.Text + " " + DiskInterFace_V.Text);
                    PrintEngineList.Add(DiskPartition.Text + " " + DiskPartition_V.Text);
                    PrintEngineList.Add(DiskUniqueID.Text + " " + DiskUniqueID_V.Text);
                    PrintEngineList.Add(DiskLocation.Text + " " + DiskLocation_V.Text);
                    PrintEngineList.Add(DiskHealt.Text + " " + DiskHealt_V.Text);
                    PrintEngineList.Add(DiskBootPartition.Text + " " + DiskBootPartition_V.Text);
                    PrintEngineList.Add(DiskBootableStatus.Text + " " + DiskBootableStatus_V.Text + Environment.NewLine);
                }
                DiskSelectBox.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // NETWORK
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()))} ------->" + Environment.NewLine);
            try{
                int net_amount = ListNetwork.Items.Count;
                for (int net_render = 1; net_render <= net_amount; net_render++){
                    ListNetwork.SelectedIndex = net_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_13").Trim())) + " #" + net_render + Environment.NewLine);
                    PrintEngineList.Add(ConnType.Text + " " + ListNetwork.SelectedItem.ToString());
                    PrintEngineList.Add(MacAdress.Text + " " + MacAdress_V.Text);
                    PrintEngineList.Add(NetMan.Text + " " + NetMan_V.Text);
                    PrintEngineList.Add(ServiceName.Text + " " + ServiceName_V.Text);
                    PrintEngineList.Add(AdapterType.Text + " " + AdapterType_V.Text);
                    PrintEngineList.Add(Guid.Text + " " + Guid_V.Text);
                    PrintEngineList.Add(ConnectionType.Text + " " + ConnectionType_V.Text);
                    PrintEngineList.Add(Dhcp_status.Text + " " + Dhcp_status_V.Text);
                    PrintEngineList.Add(Dhcp_server.Text + " " + Dhcp_server_V.Text);
                    PrintEngineList.Add(LocalConSpeed.Text + " " + LocalConSpeed_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add(IPv4Adress.Text + " " + IPv4Adress_V.Text);
                PrintEngineList.Add(IPv6Adress.Text + " " + IPv6Adress_V.Text + Environment.NewLine);
                ListNetwork.SelectedIndex = 1;
            }catch (Exception){ }
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // SOUND
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()))} ------->" + Environment.NewLine);
            try{
                int audio_amount = SoundDriverMainList.Items.Count;
                for (int audio_render = 1; audio_render <= audio_amount; audio_render++){
                    SoundDriverMainList.SelectedIndex = audio_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Audio", "ao_1").Trim())) + " #" + audio_render + Environment.NewLine);
                    PrintEngineList.Add(SoundDriver.Text + " " + SoundDriverMainList.SelectedItem.ToString());
                    PrintEngineList.Add(SoundDriverStatus.Text + " " + SoundDriverStatus_V.Text);
                    PrintEngineList.Add(SoundConfig.Text + " " + SoundConfig_V.Text);
                    PrintEngineList.Add(SoundDeviceId.Text + " " + SoundDeviceId_V.Text);
                    PrintEngineList.Add(SoundMan.Text + " " + SoundMan_V.Text);
                    PrintEngineList.Add(SoundPowerMan.Text + " " + SoundPowerMan_V.Text + Environment.NewLine);
                }
                SoundDriverMainList.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // USB
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()))} ------->" + Environment.NewLine);
            try{
                int usb_amount = USBMainList.Items.Count;
                for (int usb_render = 1; usb_render <= usb_amount; usb_render++){
                    USBMainList.SelectedIndex = usb_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_1").Trim())) + " #" + usb_render + Environment.NewLine);
                    PrintEngineList.Add(USBHardware.Text + " " + USBMainList.SelectedItem.ToString());
                    PrintEngineList.Add(USBStatus.Text + " " + USBStatus_V.Text);
                    PrintEngineList.Add(USBConfigUsage.Text + " " + USBConfigUsage_V.Text);
                    PrintEngineList.Add(USBHubDesc.Text + " " + USBHubDesc_V.Text);
                    PrintEngineList.Add(USBDeviceID.Text + " " + USBDeviceID_V.Text);
                    PrintEngineList.Add(USBNames.Text + " " + USBNames_V.Text);
                    PrintEngineList.Add(USBPnpDeviceID.Text + " " + USBPnpDeviceID_V.Text + Environment.NewLine);
                }
                USBMainList.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // BATTERY
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()))} ------->" + Environment.NewLine);
                PrintEngineList.Add(BatteryStatus.Text + " " + BatteryStatus_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            }else{
                PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()))} ------->" + Environment.NewLine);
                PrintEngineList.Add(BatteryStatus.Text + " " + BatteryStatus_V.Text);
                PrintEngineList.Add(BatteryModel.Text + " " + BatteryModel_V.Text);
                PrintEngineList.Add(BatteryName.Text + " " + BatteryName_V.Text);
                PrintEngineList.Add(BatteryVoltage.Text + " " + BatteryVoltage_V.Text);
                PrintEngineList.Add(BatteryType.Text + " " + BatteryType_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            }
            // OSD
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_11").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_3").Trim())) + Environment.NewLine);
            try{
                for (int i = 0; i < DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add(DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + DataMainTable.Rows[i].Cells[4].Value.ToString() + Environment.NewLine + "---------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_8").Trim())) + " " + TYSS_V.Text + Environment.NewLine);
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // SERVICES
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_12").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_4").Trim())) + Environment.NewLine);
            try{
                for (int i = 0; i < ServicesDataGrid.Rows.Count; i++){
                    PrintEngineList.Add(ServicesDataGrid.Rows[i].Cells[0].Value.ToString() + " | " + ServicesDataGrid.Rows[i].Cells[1].Value.ToString() + " | " + ServicesDataGrid.Rows[i].Cells[2].Value.ToString() + " | " + ServicesDataGrid.Rows[i].Cells[3].Value.ToString() + " | " + ServicesDataGrid.Rows[i].Cells[4].Value.ToString() + Environment.NewLine + "---------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_8").Trim())) + " " + TYSS_V.Text + Environment.NewLine);
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // FOOTER
            PrintEngineList.Add(Application.ProductName + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_5").Trim())) + " " + Application.ProductVersion.Substring(0, 4) + " - 64 Bit");
            PrintEngineList.Add("(C) 2023 Türkay Software.");
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_6").Trim())) + " " + ts_website);
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_7").Trim())) + " " + DateTime.Now.ToString("dd.MM.yyyy - H:mm:ss"));
            SaveFileDialog save_engine = new SaveFileDialog{
                InitialDirectory = @"C:\Users\" + SystemInformation.UserName + @"\Desktop\",
                Title = Application.ProductName + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_8").Trim())),
                DefaultExt = "txt",
                FileName = Application.ProductName + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_9").Trim())),
                Filter = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_10").Trim())) + " (*.txt)|*.txt"
            };
            if (save_engine.ShowDialog() == DialogResult.OK){
                String[] text_engine = new String[PrintEngineList.Count];
                PrintEngineList.CopyTo(text_engine, 0);
                File.WriteAllLines(save_engine.FileName, text_engine);
                DialogResult glow_print_engine_query = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_11").Trim())) + Environment.NewLine + Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_12").Trim())), Application.ProductName, save_engine.FileName), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (glow_print_engine_query == DialogResult.Yes){ Process.Start(save_engine.FileName); }
            }
            PrintEngineList.Clear(); save_engine.Dispose();
        }
        // GLOW ABOUT
        // ======================================================================================================
        private void hakkımızdaToolStripMenuItem_Click(object sender, EventArgs e){ AboutUs about_us = new AboutUs(); about_us.ShowDialog(); }
        // TURKAY SOFTWARE WEBSITE
        // ======================================================================================================
        private void webSiteToolStripMenuItem_Click(object sender, EventArgs e){ Process.Start(ts_website); }
        // GLOW GITHUB PAGE
        // ======================================================================================================
        private void gitHubSayfasıToolStripMenuItem_Click(object sender, EventArgs e){ Process.Start(glow_github); }
        // GLOW ROTATE DONATE PAGE
        // ======================================================================================================
        private void destekOlToolStripMenuItem_Click(object sender, EventArgs e){ GlowDonate glow_donate = new GlowDonate(); glow_donate.ShowDialog(); }
        // GLOW SHORTCUT KEYS
        // ======================================================================================================
        private void Glow_KeyUp(object sender, KeyEventArgs e){ if (e.KeyData == Keys.Escape){ glow_exit(); } }
        // GLOW EXIT
        // ======================================================================================================
        private void glow_exit(){ loop_status = false; Application.Exit(); }
        private void Glow_FormClosing(object sender, FormClosingEventArgs e){ glow_exit(); }
    }
}