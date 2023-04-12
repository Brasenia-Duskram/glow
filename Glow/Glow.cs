using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Reflection;
using System.Management;
using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
using System.Runtime.InteropServices;
using static Glow.GlowExternalModules;

namespace Glow{
    public partial class Glow : Form{
        public Glow(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // ======================================================================================================
        // GLOBAL INT / STRING
        public static int theme;
        public static string lang_path;
        // LOCAL INT / STRING / BOOL
        int menu_btns = 1, menu_rp = 1;
        string lang, wp_rotate, wp_resoulation;
        readonly string glow_github = "https://github.com/turkaysoftware/glow";
        bool loop_status = true;
        // ======================================================================================================
        // COLOR MODES
        public static List<Color> ui_colors = new List<Color>();
        List<Color> btn_colors = new List<Color>(){ Color.FromArgb(235, 235, 235), Color.WhiteSmoke, Color.FromArgb(37, 37, 43), Color.FromArgb(53, 53, 61) };
        static List<Color> header_colors = new List<Color>();
        // ======================================================================================================
        // HEADER SETTINGS
        private class HeaderMenuColors : ToolStripProfessionalRenderer{
            public HeaderMenuColors() : base(new HeaderColors()){ }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e){ e.ArrowColor = header_colors[1]; base.OnRenderArrow(e); }
        }
        private class HeaderColors : ProfessionalColorTable{
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
        // ======================================================================================================
        // TOOLTIP SETTINGS
        private void MainToolTip_Draw(object sender, DrawToolTipEventArgs e){ e.DrawBackground(); e.DrawBorder(); e.DrawText(); }
        // ======================================================================================================
        // GLOW PRELOADER
        private void glow_preloader(){
            try{
                // CHECK GLOW LANG FOLDER
                if (Directory.Exists(glow_lang_folder)){
                    // CHECK LANG FILES
                    int get_langs_file = Directory.GetFiles(glow_lang_folder, "*.ini", SearchOption.AllDirectories).Length;
                    if (get_langs_file > 0){
                        // EN | TR
                        if (!File.Exists(glow_lang_en)){ englishToolStripMenuItem.Enabled = false; }
                        if (!File.Exists(glow_lang_tr)){ turkishToolStripMenuItem.Enabled = false; }
                        // CHECK SETTINGS
                        try{
                            if (File.Exists(glow_sf)){
                                GetGlowSetting();
                            }else{
                                // DETECT SYSTEM THEME
                                GlowSettingsSave glow_settings_save = new GlowSettingsSave(glow_sf);
                                string get_system_theme = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "").ToString().Trim();
                                glow_settings_save.GlowWriteSettings("Theme", "ThemeStatus", get_system_theme);
                                // DETECT SYSTEM LANG
                                string culture_lang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.Trim();
                                glow_settings_save.GlowWriteSettings("Language", "LanguageStatus", culture_lang);
                                GetGlowSetting();
                            }
                        }catch (Exception){ }
                    }else{
                        MessageBox.Show("No language files were found.\nThe program is closing.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }else{
                    MessageBox.Show("Langs folder not found.\nThe program is closing.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }catch (Exception){ }
        }
        // ======================================================================================================
        // GLOW LOAD LANGS SETTINGS
        private void GetGlowSetting(){
            // INSTALLED DRIVERS
            OSD_DataMainTable.Columns.Add("osd_1", "x");
            OSD_DataMainTable.Columns.Add("osd_2", "x");
            OSD_DataMainTable.Columns.Add("osd_3", "x");
            OSD_DataMainTable.Columns.Add("osd_4", "x");
            OSD_DataMainTable.Columns.Add("osd_5", "x");
            // SERVICES
            SERVICE_DataMainTable.Columns.Add("ser_1", "x");
            SERVICE_DataMainTable.Columns.Add("ser_2", "x");
            SERVICE_DataMainTable.Columns.Add("ser_3", "x");
            SERVICE_DataMainTable.Columns.Add("ser_4", "x");
            SERVICE_DataMainTable.Columns.Add("ser_5", "x");
            // ALL DGV AND PANEL WIDTH
            int c1 = 165, c2 = 260, c3 = 120, c4 = 95, c5 = 95;
            // INSTALLED DRIVERS
            OSD_DataMainTable.Columns[0].Width = c1;
            OSD_DataMainTable.Columns[1].Width = c2;
            OSD_DataMainTable.Columns[2].Width = c3;
            OSD_DataMainTable.Columns[3].Width = c4;
            OSD_DataMainTable.Columns[4].Width = c5;
            OSD_DataMainTable.ClearSelection();
            // SERVICES
            SERVICE_DataMainTable.Columns[0].Width = c1;
            SERVICE_DataMainTable.Columns[1].Width = c2;
            SERVICE_DataMainTable.Columns[2].Width = c3;
            SERVICE_DataMainTable.Columns[3].Width = c4;
            SERVICE_DataMainTable.Columns[4].Width = c5;
            SERVICE_DataMainTable.ClearSelection();
            // PANEL WIDTH SETTINGS
            int global_width_1 = 771, global_width_2 = 788, global_width_3 = 787;
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
            battery_panel_1.Width = global_width_3;
            osd_panel_1.Width = global_width_3;
            service_panel_1.Width = global_width_3;
            // DGV DOUBLE BUFFER
            typeof(TabControl).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, MainContent, new object[]{ true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, OSD_DataMainTable, new object[]{ true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, SERVICE_DataMainTable, new object[]{ true });
            // THEME AND LANG PRELOADER
            GlowSettingsSave glow_theme_read = new GlowSettingsSave(glow_sf);
            string theme_mode = glow_theme_read.GlowReadSettings("Theme", "ThemeStatus");
            if (theme_mode == "0"){
                color_mode(2);
                darkThemeToolStripMenuItem.Checked = true;
            }else{
                color_mode(1);
                lightThemeToolStripMenuItem.Checked = true;
            }
            GlowSettingsSave glow_lang_read = new GlowSettingsSave(glow_sf);
            string lang_mode = glow_lang_read.GlowReadSettings("Language", "LanguageStatus");
            switch (lang_mode){
                case "en":
                    lang_engine("en");
                    englishToolStripMenuItem.Checked = true;
                    break;
                case "tr":
                    lang_engine("tr");
                    turkishToolStripMenuItem.Checked = true;
                    break;
                default:
                    lang_engine("en");
                    englishToolStripMenuItem.Checked = true;
                    break;
            }
            glow_load_tasks();
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
            // RAM ASYNC STARTER
            Task task_ram_bg = new Task(ram_bg_process);
            task_ram_bg.Start();
        }
        // ======================================================================================================
        // GLOW LOAD
        private void Glow_Load(object sender, EventArgs e){
            // PRELOAD SETTINGS
            Text = Application.ProductName + " " + Application.ProductVersion.Substring(0, 4);
            HeaderMenu.Cursor = Cursors.Hand;
            // GLOW LAUNCH PROCESS
            glow_preloader();
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
                OS_SystemUser_V.Text = SystemInformation.UserName;
            }catch (Exception){ }
            try{
                // PC NAME
                OS_ComputerName_V.Text = SystemInformation.ComputerName;
            }catch (Exception){ }
            try{
                foreach (ManagementObject query_cs_rotate in search_cs.Get()){
                    // SYSTEM MODEL
                    OS_SystemModel_V.Text = Convert.ToString(query_cs_rotate["Name"]);
                }
            }catch (Exception){ }
            foreach (ManagementObject query_os_rotate in search_os.Get()){
                try{
                    // REGISTERED USER
                    OS_SavedUser_V.Text = Convert.ToString(query_os_rotate["RegisteredUser"]);
                }catch (Exception){ }
                try{
                    // OS NAME
                    OS_Name_V.Text = Convert.ToString(query_os_rotate["Caption"]);
                }catch (Exception){ }
                try{
                    // OS MANUFACTURER
                    OS_Manufacturer_V.Text = Convert.ToString(query_os_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // OS VERSION
                    string os_version_display_version = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", "").ToString().Trim();
                    string os_version_release_id = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString().Trim();
                    if (os_version_display_version != string.Empty && os_version_release_id != string.Empty && os_version_display_version != "" && os_version_release_id != ""){
                        OS_SystemVersion_V.Text = os_version_display_version + " - " + os_version_release_id;
                    }else{
                        OS_SystemVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_1").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // OS BUILD NUMBER
                    object os_build_num = query_os_rotate["Version"];
                    OS_Build_V.Text = os_build_num.ToString();
                }catch (Exception){ }
                try{
                    // OS ARCHITECTURE
                    string system_bit = Convert.ToString(query_os_rotate["OSArchitecture"]).Replace("bit", "");
                    OS_SystemArchitectural_V.Text = system_bit.Trim() + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_2").Trim())) + " - " + string.Format("(x{0})", system_bit.Trim());
                }catch (Exception){ }
                try{
                    // OS FAMILY
                    OS_Family_V.Text = new ComputerInfo().OSPlatform.Trim();
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
                    OS_SystemRootIndex_V.Text = windows_dir.ToString().Replace("WINDOWS", "Windows") + @"\";
                }catch (Exception){ }
                try{
                    // BUILD PARTITON
                    object system_yapi_partition = query_os_rotate["SystemDirectory"];
                    OS_SystemBuildPart_V.Text = system_yapi_partition.ToString().Replace("WINDOWS", "Windows") + @"\";
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
                    OS_LastBootTime_V.Text = last_bt_process;
                }catch (Exception){ }
                try{
                    // PORTABLE OS STATUS
                    bool system_portable_status = Convert.ToBoolean(query_os_rotate["PortableOperatingSystem"]);
                    if (system_portable_status == true){
                        OS_PortableOS_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_5").Trim()));
                    }else if (system_portable_status == false){
                        OS_PortableOS_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_6").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // BOOT PARTITION
                    object boot_device = query_os_rotate["BootDevice"];
                    string boot_device_1 = Convert.ToString(boot_device).Replace(@"\Device\", "");
                    string boot_device_2 = boot_device_1.Replace("HarddiskVolume", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_7").Trim())) + " - ");
                    OS_BootPartition_V.Text = boot_device_2.Trim();
                }catch (Exception){ }
                try{
                    // SYSTEM PARTITION
                    object system_device = query_os_rotate["SystemDevice"];
                    string system_device_1 = Convert.ToString(system_device).Replace(@"\Device\", "");
                    string system_device_2 = system_device_1.Replace("HarddiskVolume", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_8").Trim())) + " - ");
                    OS_SystemPartition_V.Text = system_device_2.ToString();
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
                                OS_Wallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.00} MB", wallpaper_size_x64 / 1024);
                            }else if (wallpaper_size_x64 < 1024){
                                OS_Wallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0} KB", wallpaper_size_x64);
                            }
                            MainToolTip.SetToolTip(OS_WallpaperOpen, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_29").Trim())), wp_rotate));
                        }else{
                            OS_Wallpaper_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_9").Trim()));
                            OS_WallpaperOpen.Visible = false;
                        }
                    }
                }
                if (OS_Wallpaper_V.Text == "N/A" || OS_Wallpaper_V.Text.Trim() == "" || OS_Wallpaper_V.Text == string.Empty){
                    OS_Wallpaper_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_10").Trim()));
                    OS_WallpaperOpen.Visible = false;
                }
            }catch (Exception){ }
        }
        private void os_bg_process(){
            try{
                // DESCRIPTIVE
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                do{
                    if (loop_status == false){ break; }
                    foreach (ManagementObject query_os_rotate in search_os.Get()){
                        // FREE VIRTUAL RAM
                        double free_sanal_ram = Convert.ToDouble(query_os_rotate["FreeVirtualMemory"]) / 1024 / 1024;
                        RAM_EmptyVirtualRam_V.Text = String.Format("{0:0.00} GB", free_sanal_ram);
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
                        OS_SystemWorkTime_V.Text = system_uptime_x64;
                    }
                    // SYSTEM TIME
                    OS_SystemTime_V.Text = DateTime.Now.ToString("dd.MM.yyyy - H:mm:ss");
                    // SYSTEM WORK TIME
                    try{
                        // MOUSE WHEEL SPEED
                        int mouse_wheel_speed = new Computer().Mouse.WheelScrollLines;
                        OS_MouseWheelStatus_V.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_15").Trim())), mouse_wheel_speed);
                    }catch (Exception){ }
                    try{
                        // SCROLL LOCK STATUS
                        bool scroll_lock_status = new Computer().Keyboard.ScrollLock;
                        if (scroll_lock_status == true){
                            OS_ScrollLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (scroll_lock_status == false){
                            OS_ScrollLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    try{
                        // NUMLOCK STATUS
                        bool numlock_status = new Computer().Keyboard.NumLock;
                        if (numlock_status == true){
                            OS_NumLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (numlock_status == false){
                            OS_NumLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    try{
                        // CAPSLOCK STATUS
                        bool capslock_status = new Computer().Keyboard.CapsLock;
                        if (capslock_status == true){
                            OS_CapsLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (capslock_status == false){
                            OS_CapsLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        private void GoWallpaperRotate_Click(object sender, EventArgs e){
            try{
                string wallpaper_start_path = string.Format("/select, \"{0}\"", wp_rotate.Trim().Replace("/", @"\"));
                ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", wallpaper_start_path);
                Process.Start(psi);
            }catch (Exception){
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
                    MB_MotherBoardName_V.Text = Convert.ToString(query_bb_rotate["Product"]);
                }catch (Exception){ }
                try{
                    // MB MAN
                    MB_MotherBoardMan_V.Text = Convert.ToString(query_bb_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // MB SERIAL
                    MB_MotherBoardSerial_V.Text = Convert.ToString(query_bb_rotate["SerialNumber"]);
                }catch (Exception){ }
                try{
                    // MB VERSION
                    MB_Model_V.Text = Convert.ToString(query_bb_rotate["Version"]);
                }catch (Exception){ }
            }
            foreach (ManagementObject query_bios_rotate in search_bios.Get()){
                try{
                    // BIOS MAN
                    MB_BiosManufacturer_V.Text = Convert.ToString(query_bios_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // BIOS DATE
                    string bios_date = Convert.ToString(query_bios_rotate["ReleaseDate"]);
                    string b_date_render = bios_date.Substring(6, 2) + "." + bios_date.Substring(4, 2) + "." + bios_date.Substring(0, 4);
                    MB_BiosDate_V.Text = b_date_render;
                }catch (Exception){ }
                try{
                    // BIOS VERSION
                    MB_BiosVersion_V.Text = Convert.ToString(query_bios_rotate["Caption"]);
                }catch (Exception){ }
                try{
                    // SM BIOS VERSION
                    MB_SmBiosVersion_V.Text = Convert.ToString(query_bios_rotate["Version"]);
                }catch (Exception){ }
                try{
                    // BIOS MAJOR MINOR
                    object bios_major = query_bios_rotate["SystemBiosMajorVersion"];
                    object bios_minor = query_bios_rotate["SystemBiosMinorVersion"];
                    MB_BiosMajorMinor_V.Text = bios_major.ToString() + "." + bios_minor.ToString();
                }catch (Exception){ }
                try{
                    // SM-BIOS MAJOR MINOR
                    object sm_bios_major = query_bios_rotate["SMBIOSMajorVersion"];
                    object sm_bios_minor = query_bios_rotate["SMBIOSMinorVersion"];
                    MB_SMBiosMajorMinor_V.Text = sm_bios_major.ToString() + "." + sm_bios_minor.ToString();
                }catch (Exception){ }
            }
            try{
                foreach (ManagementObject query_md_rotate in search_md.Get()){
                    // PRIMARY BUS TYPE
                    MB_PrimaryBusType_V.Text = Convert.ToString(query_md_rotate["PrimaryBusType"]);
                }
            }catch (Exception){ }
        }
        // CPU
        // ======================================================================================================
        private void cpu(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_process = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject query_process_rotate in search_process.Get()){
                try{
                    // CPU NAME
                    CPU_Name_V.Text = Convert.ToString(query_process_rotate["Name"]).Trim();
                }catch (Exception){ }
                try{
                    // CPU MANUFACTURER
                    string cpu_man = Convert.ToString(query_process_rotate["Manufacturer"]);
                    string cpu_man_sv = "GenuineIntel";
                    bool cpu_man_search = cpu_man.Contains(cpu_man_sv);
                    if (cpu_man_search == true){
                        MB_Chipset_V.Text = "Intel";
                        CPU_Manufacturer_V.Text = "Intel Corporation";
                    }else{
                        MB_Chipset_V.Text = "AMD";
                        CPU_Manufacturer_V.Text = cpu_man;
                    }
                }catch (Exception){ }
                try{
                    // CPU ARCHITECTURE
                    int cpu_arch_num = Convert.ToInt32(query_process_rotate["Architecture"]);
                    string[] cpu_architectures = { "32 " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_1").Trim())) + " - (x86)", "MIPS", "ALPHA", "POWER PC", "ARM", "IA64", "64 " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_1").Trim())) + " - (x64)" };
                    if (cpu_arch_num == 0){
                        CPU_Architectural_V.Text = cpu_architectures[0];
                    }else if (cpu_arch_num == 1){
                        CPU_Architectural_V.Text = cpu_architectures[1];
                    }else if (cpu_arch_num == 2){
                        CPU_Architectural_V.Text = cpu_architectures[2];
                    }else if (cpu_arch_num == 3){
                        CPU_Architectural_V.Text = cpu_architectures[3];
                    }else if (cpu_arch_num == 5){
                        CPU_Architectural_V.Text = cpu_architectures[4];
                    }else if (cpu_arch_num == 6){
                        CPU_Architectural_V.Text = cpu_architectures[5];
                    }else if (cpu_arch_num == 9){
                        CPU_Architectural_V.Text = cpu_architectures[6];
                    }else{
                        CPU_Architectural_V.Text = cpu_arch_num.ToString();
                    }
                }catch (Exception){ }
                try{
                    // CPU NORMAL SPEED
                    double cpu_speed = Convert.ToDouble(query_process_rotate["CurrentClockSpeed"]);
                    if (cpu_speed > 1024){
                        CPU_NormalSpeed_V.Text = String.Format("{0:0.00} GHz", cpu_speed / 1000);
                    }else{
                        CPU_NormalSpeed_V.Text = cpu_speed.ToString() + " MHz";
                    }
                }catch (Exception){ }
                try{
                    // CPU DEFAULT SPEED
                    double cpu_max_speed = Convert.ToDouble(query_process_rotate["MaxClockSpeed"]);
                    if (cpu_max_speed > 1024){
                        CPU_DefaultSpeed_V.Text = String.Format("{0:0.00} GHz", cpu_max_speed / 1000);
                    }else{
                        CPU_DefaultSpeed_V.Text = cpu_max_speed.ToString() + " MHz";
                    }
                }catch (Exception){ }
                try{
                    // L2 CACHE
                    double l2_size = Convert.ToDouble(query_process_rotate["L2CacheSize"]);
                    if (l2_size >= 1024){
                        CPU_L2_V.Text = (l2_size / 1024).ToString() + " MB";
                    }else{
                        CPU_L2_V.Text = l2_size.ToString() + " KB";
                    }
                }catch (Exception){ }
                try{
                    // L3 CACHE
                    double l3_size = Convert.ToDouble(query_process_rotate["L3CacheSize"]);
                    CPU_L3_V.Text = l3_size / 1024 + " MB";
                }catch (Exception){ }
                try{
                    // CPU CORES
                    CPU_CoreCount_V.Text = Convert.ToString(query_process_rotate["NumberOfCores"]);
                }catch (Exception){ }
                try{
                    // CPU LOGICAL CORES
                    string thread_count = Convert.ToString(query_process_rotate["ThreadCount"]);
                    if (thread_count == String.Empty || thread_count == ""){
                        CPU_LogicalCore_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_2").Trim()));
                    }else{
                        CPU_LogicalCore_V.Text = thread_count;
                    }
                }catch (Exception){ }
                try{
                    // CPU SOCKET
                    CPU_SocketDefinition_V.Text = Convert.ToString(query_process_rotate["SocketDesignation"]);
                }catch (Exception){ }
                try{
                    // CPU FAMILY
                    string cpu_description = Convert.ToString(query_process_rotate["Description"]);
                    string cpu_tanim = cpu_description.Replace("Family", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_3").Trim())));
                    string cpu_tanim_2 = cpu_tanim.Replace("Model", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_4").Trim())));
                    string cpu_tanim_3 = cpu_tanim_2.Replace("Stepping", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_5").Trim())));
                    string cpu_tanim_4 = cpu_tanim_3.Replace("64", " X64");
                    CPU_Family_V.Text = cpu_tanim_4;
                }catch (Exception){ }
                try{
                    // CPU VIRTUALIZATION
                    bool cpu_virtual_mod = Convert.ToBoolean(query_process_rotate["VirtualizationFirmwareEnabled"]);
                    if (cpu_virtual_mod == true){
                        CPU_Virtualization_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_6").Trim()));
                    }else if (cpu_virtual_mod == false){
                        CPU_Virtualization_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_7").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // CPU VIRTUALIZATION MONITOR EXTENSIONS
                    bool vm_monitor_ext = Convert.ToBoolean(query_process_rotate["VMMonitorModeExtensions"]);
                    if (vm_monitor_ext == true){
                        CPU_VMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_8").Trim()));
                    }else if (vm_monitor_ext == false){
                        CPU_VMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_9").Trim()));
                    }else{
                        CPU_VMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_10").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // CPU SERIAL ID
                    CPU_SerialName_V.Text = Convert.ToString(query_process_rotate["ProcessorId"]);
                }catch (Exception){ }
            }
            ManagementObjectSearcher search_cm = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_CacheMemory WHERE Level = {3}");
            foreach (ManagementObject query_cm_rotate in search_cm.Get()){
                // L1 CACHE
                double l1_size = Convert.ToDouble(query_cm_rotate["MaxCacheSize"]);
                if (l1_size >= 1024){
                    CPU_L1_V.Text = (l1_size / 1024).ToString() + " MB";
                }else{
                    CPU_L1_V.Text = l1_size.ToString() + " KB";
                }
            }
            // WRITE ENGINE ENABLED
            printInformationToolStripMenuItem.Enabled = true;
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
                        RAM_TotalRAM_V.Text = String.Format("{0:0.00} TB", total_ram_isle / 1024 / 1024);
                    }else{
                        RAM_TotalRAM_V.Text = String.Format("{0:0.00} GB", total_ram_isle / 1024);
                    }
                }else{
                    RAM_TotalRAM_V.Text = total_ram_isle.ToString() + " MB";
                }
            }catch (Exception){ }
            try{
                foreach (ManagementObject query_os_rotate in search_os.Get()){
                    // TOTAL VIRTUAL RAM
                    RAM_TotalVirtualRam_V.Text = String.Format("{0:0.00} GB", Convert.ToDouble(query_os_rotate["TotalVirtualMemorySize"]) / 1024 / 1024);
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
                    RAM_SlotStatus_V.Text = ram_slot_count.Count + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_2").Trim()));
                }catch (Exception){ }
                try{
                    // RAM CAPACITY
                    double ram_amount = Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024;
                    if (ram_amount > 1024){
                        ram_amount_list.Add(ram_amount / 1024 + " GB");
                    }else{
                        ram_amount_list.Add(ram_amount + " MB");
                    }
                    RAM_Amount_V.Text = ram_amount_list[0];
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
                    }else if (sm_bios_memory_type == 27 || memory_type == 27 || sm_bios_memory_type == 28 || memory_type == 28){
                        ram_type_list.Add("DDR5");
                    }else if (sm_bios_memory_type == 0 || memory_type == 0){
                        ram_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_4").Trim())));
                    }
                    RAM_Type_V.Text = ram_type_list[0];
                }catch (Exception){ }
                try{
                    // RAM SPEED
                    ram_frekans_list.Add(Convert.ToInt32(queryObj["Speed"]) + " MHz");
                    RAM_Frequency_V.Text = ram_frekans_list[0];
                }catch (Exception){ }
                try{
                    // RAM VOLTAGE
                    string ram_voltaj = Convert.ToString(queryObj["ConfiguredVoltage"]);
                    if (ram_voltaj == "" || ram_voltaj == "0" || ram_voltaj == "0.0" || ram_voltaj == "0.00" || ram_voltaj == string.Empty){
                        ram_voltage_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_5").Trim())));
                    }else{
                        ram_voltage_list.Add(String.Format("{0:0.00} " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_6").Trim())), Convert.ToInt32(ram_voltaj) / 1000.0));
                    }
                    RAM_Volt_V.Text = ram_voltage_list[0];
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
                    RAM_FormFactor_V.Text = ram_form_factor[0];
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
                    RAM_Serial_V.Text = ram_serial_list[0];
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
                    RAM_Manufacturer_V.Text = ram_manufacturer_list[0];
                }catch (Exception){ }
                try{
                    // RAM BANK LABEL
                    string bank_label = Convert.ToString(queryObj["BankLabel"]);
                    if (bank_label == "" || bank_label == string.Empty){
                        ram_bank_label_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_11").Trim())));
                    }else{
                        ram_bank_label_list.Add(bank_label);
                    }
                    RAM_BankLabel_V.Text = ram_bank_label_list[0];
                }catch (Exception){ }
                try{
                    // RAM TOTAL WIDTH
                    string ram_data_width = Convert.ToString(queryObj["TotalWidth"]);
                    if (ram_data_width == "" || ram_data_width == string.Empty){
                        ram_data_width_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_12").Trim())));
                    }else{
                        ram_data_width_list.Add(ram_data_width + " Bit");
                    }
                    RAM_DataWidth_V.Text = ram_data_width_list[0];
                }catch (Exception){ }
                try{
                    // RAM LOCATOR
                    bellek_type_list.Add(Convert.ToString(queryObj["DeviceLocator"]));
                    RAM_BellekType_V.Text = bellek_type_list[0];
                }catch (Exception){ }
                try{
                    // PART NUMBER
                    string part_number = Convert.ToString(queryObj["PartNumber"]).Trim();
                    if (part_number == "" || part_number == string.Empty){
                        ram_part_number_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_13").Trim())));
                    }else{
                        ram_part_number_list.Add(part_number);
                    }
                    RAM_PartNumber_V.Text = ram_part_number_list[0];
                }catch (Exception){ }
            }
            // RAM SELECT
            try{
                int ram_amount = ram_slot_list.Count;
                for (int rs = 1; rs <= ram_amount; rs++){
                    RAM_SelectList.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_14").Trim())) + " #" + rs);
                    RAM_SelectList.SelectedIndex = 0;
                }
            }catch (Exception){ }
        }
        private void RAMSelectList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int ram_slot = RAM_SelectList.SelectedIndex;
                RAM_Amount_V.Text = ram_amount_list[ram_slot];
                RAM_Type_V.Text = ram_type_list[ram_slot];
                RAM_Frequency_V.Text = ram_frekans_list[ram_slot];
                RAM_Volt_V.Text = ram_voltage_list[ram_slot];
                RAM_FormFactor_V.Text = ram_form_factor[ram_slot];
                RAM_Serial_V.Text = ram_serial_list[ram_slot];
                RAM_Manufacturer_V.Text = ram_manufacturer_list[ram_slot];
                RAM_BankLabel_V.Text = ram_bank_label_list[ram_slot];
                RAM_DataWidth_V.Text = ram_data_width_list[ram_slot];
                RAM_BellekType_V.Text = bellek_type_list[ram_slot];
                RAM_PartNumber_V.Text = ram_part_number_list[ram_slot];
            }catch (Exception){ }
        }
        private void ram_bg_process(){
            try{
                ComputerInfo get_ram_info = new ComputerInfo();
                do{
                    if (loop_status == false){ break; }
                    ulong total_ram = ulong.Parse(get_ram_info.TotalPhysicalMemory.ToString());
                    double total_ram_x64 = total_ram / (1024 * 1024);
                    ulong usable_ram = ulong.Parse(get_ram_info.AvailablePhysicalMemory.ToString());
                    double usable_ram_x64 = usable_ram / (1024 * 1024);
                    double ram_process = total_ram_x64 - usable_ram_x64;
                    double usage_ram_percentage = (total_ram_x64 - usable_ram_x64) / total_ram_x64 * 100;
                    if (ram_process > 1024){
                        if ((ram_process / 1024) > 1024){
                            RAM_UsageRAMCount_V.Text = String.Format("{0:0.00} TB - ", ram_process / 1024 / 1024) + String.Format("%{0:0.0}", usage_ram_percentage);
                        }else{
                            RAM_UsageRAMCount_V.Text = String.Format("{0:0.00} GB - ", ram_process / 1024) + String.Format("%{0:0.0}", usage_ram_percentage);
                        }
                    }else{
                        RAM_UsageRAMCount_V.Text = ram_process.ToString() + " MB - " + String.Format("%{0:0.0}", usage_ram_percentage);
                    }
                    if (usable_ram_x64 > 1024){
                        if ((usable_ram_x64 / 1024) > 1024){
                            RAM_EmptyRamCount_V.Text = String.Format("{0:0.00} TB", usable_ram_x64 / 1024 / 1024);
                        }else{
                            RAM_EmptyRamCount_V.Text = String.Format("{0:0.00} GB", usable_ram_x64 / 1024);
                        }
                    }else{
                        RAM_EmptyRamCount_V.Text = usable_ram_x64.ToString() + " MB";
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
                        GPU_Select.Items.Add(gpu_name);
                        GPU_Select.SelectedIndex = 0;
                    }
                }catch (Exception){ }
                try{
                    // GPU MAN
                    string gpu_man = Convert.ToString(query_vc_rotate["AdapterCompatibility"]).Trim();
                    if (gpu_man != "" && gpu_man != string.Empty){
                        gpu_man_list.Add(gpu_man);
                        GPU_Manufacturer_V.Text = gpu_man_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU DRIVER VERSION
                    string driver_version = Convert.ToString(query_vc_rotate["DriverVersion"]);
                    if (driver_version != "" && driver_version != string.Empty){
                        gpu_driver_version_list.Add(driver_version);
                        GPU_Version_V.Text = gpu_driver_version_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU DRIVER DATE
                    string gpu_date = Convert.ToString(query_vc_rotate["DriverDate"]);
                    if (gpu_date != "" && gpu_date != string.Empty){
                        string gpu_date_process = gpu_date.Substring(6, 2) + "." + gpu_date.Substring(4, 2) + "." + gpu_date.Substring(0, 4);
                        gpu_driver_date_list.Add(gpu_date_process);
                        GPU_DriverDate_V.Text = gpu_driver_date_list[0];
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
                    GPU_Status_V.Text = gpu_status_list[0];
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
                    GPU_DacType_V.Text = gpu_dac_type_list[0];
                }catch (Exception){ }
                try{
                    // GPU DRIVERS
                    string gpu_display_drivers = Path.GetFileName(Convert.ToString(query_vc_rotate["InstalledDisplayDrivers"]));
                    if (gpu_display_drivers != "" && gpu_display_drivers != string.Empty){
                        gpu_drivers_list.Add(gpu_display_drivers);
                        GPU_GraphicDriversName_V.Text = gpu_drivers_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU INF FILE NAME
                    string gpu_inf_file = Convert.ToString(query_vc_rotate["InfFilename"]);
                    if (gpu_inf_file != "" && gpu_inf_file != string.Empty){
                        gpu_inf_file_list.Add(gpu_inf_file);
                        GPU_InfFileName_V.Text = gpu_inf_file_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU INF FILE GPU INFO PARTITION
                    string gpu_inf_section = Convert.ToString(query_vc_rotate["InfSection"]);
                    if (gpu_inf_section != "" && gpu_inf_section != string.Empty){
                        gpu_inf_file_section_list.Add(gpu_inf_section);
                        GPU_INFSectionFile_V.Text = gpu_inf_file_section_list[0];
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
                    GPU_MonitorSelectList.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_32").Trim())) + " #" + ma);
                    GPU_MonitorSelectList.SelectedIndex = 0;
                }
            }catch (Exception){ }
            // GPU SELECT
            try { GPU_Select.SelectedIndex = 0; }catch(Exception){ }
        }
        private void GPUSelect_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int gpu_select = GPU_Select.SelectedIndex;
                GPU_Manufacturer_V.Text = gpu_man_list[gpu_select];
                GPU_Version_V.Text = gpu_driver_version_list[gpu_select];
                GPU_DriverDate_V.Text = gpu_driver_date_list[gpu_select];
                GPU_Status_V.Text = gpu_status_list[gpu_select];
                GPU_DacType_V.Text = gpu_dac_type_list[gpu_select];
                GPU_GraphicDriversName_V.Text = gpu_drivers_list[gpu_select];
                GPU_InfFileName_V.Text = gpu_inf_file_list[gpu_select];
                GPU_INFSectionFile_V.Text = gpu_inf_file_section_list[gpu_select];
            }catch(Exception){ }
        }
        private void MonitorSelectList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int monitor_select = GPU_MonitorSelectList.SelectedIndex;
                GPU_MonitorBounds_V.Text = gpu_monitor_bounds_list[monitor_select];
                GPU_MonitorWorking_V.Text = gpu_monitor_work_list[monitor_select];
                GPU_MonitorResLabel_V.Text = gpu_monitor_res_list[monitor_select];
                GPU_ScreenRefreshRate_V.Text = gpu_monitor_refresh_rate_list[monitor_select];
                GPU_MonitorVirtualRes_V.Text = gpu_monitor_virtual_res_list[monitor_select];
                GPU_MonitorPrimary_V.Text = gpu_monitor_primary_list[monitor_select];
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
                        DISK_DriveType_V.Text = disk_drive_type_list[0];
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
                DISK_VolumeID_V.Text = disk_volume_list[0];
                DISK_FileSystem_V.Text = disk_file_system_list[0];
                DISK_VolumeName_V.Text = disk_volume_name_list[0];
                DISK_FreeSpace_V.Text = disk_free_space_list[0];
            }catch (Exception){ }
            ManagementObjectSearcher search_disk = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", "SELECT * FROM MSFT_Disk");
            foreach (ManagementObject query_disk in search_disk.Get()){
                try{
                    // DISK NAME
                    DISK_SelectBox.Items.Add(Convert.ToString(query_disk["FriendlyName"]));
                }catch (Exception){ }
                try{
                    // DISK MODEL
                    string disk_model = Convert.ToString(query_disk["Model"]).Trim();
                    if (disk_model == string.Empty || disk_model == ""){
                        disk_model_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_9").Trim())));
                    }else{
                        disk_model_list.Add(disk_model);
                    }
                    DISK_Model_V.Text = disk_model_list[0];
                }catch (Exception){ }
                try{
                    // DISK MANUFACTURER
                    string disk_man = Convert.ToString(query_disk["Manufacturer"]).Trim();
                    if (disk_man == string.Empty || disk_man == ""){
                        disk_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_10").Trim())));
                    }else{
                        disk_man_list.Add(disk_man);
                    }
                    DISK_Man_V.Text = disk_man_list[0];
                }catch (Exception){ }
                try{
                    // DISK FIMWARE VERSION
                    disk_firmware_list.Add(Convert.ToString(query_disk["FirmwareVersion"]));
                    DISK_FirmWare_V.Text = disk_firmware_list[0];
                }catch (Exception){ }
                try{
                    // DISK SERIAL
                    disk_serial_list.Add(Convert.ToString(query_disk["SerialNumber"]).Trim());
                    DISK_Seri_V.Text = disk_serial_list[0];
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
                    DISK_Size_V.Text = disk_size_list[0];
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
                    DISK_PartitionLayout_V.Text = disk_partition_layout_list[0];
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
                    DISK_InterFace_V.Text = disk_interface_list[0];
                }catch (Exception){ }
                try{
                    // DISK PARTITION
                    int disk_partition = Convert.ToInt32(query_disk["NumberOfPartitions"]);
                    disk_partition_list.Add(disk_partition.ToString().Trim());
                    DISK_Partition_V.Text = disk_partition_list[0];
                }catch (Exception){ }
                try{
                    // DISK UNIQUE ID
                    string disk_unique_id = Convert.ToString(query_disk["UniqueId"]).Trim();
                    if (disk_unique_id == string.Empty || disk_unique_id == ""){
                        disk_uniquq_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_17").Trim())));
                    }else{
                        disk_uniquq_id_list.Add(disk_unique_id);
                    }
                    DISK_UniqueID_V.Text = disk_uniquq_id_list[0];
                }catch (Exception){ }
                try{
                    // DISK LOCATION
                    string disk_locaiton = Convert.ToString(query_disk["Location"]).Trim();
                    string disk_process_1 = disk_locaiton.Replace("Integrated", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_18").Trim())));
                    string disk_process_2 = disk_process_1.Replace("Device", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_19").Trim())));
                    string disk_process_3 = disk_process_2.Replace("Function", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_20").Trim())));
                    string disk_process_4 = disk_process_3.Replace("Adapter", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_21").Trim())));
                    disk_location_list.Add(disk_process_4);
                    DISK_Location_V.Text = disk_location_list[0];
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
                    DISK_Healt_V.Text = disk_healt_list[0];
                }catch (Exception){ }
                try{
                    // DISK BOOT STATUS
                    bool boot_status = Convert.ToBoolean(query_disk["BootFromDisk"]);
                    if (boot_status == true){
                        disk_boot_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_25").Trim())));
                    }else if (boot_status == false){
                        disk_boot_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_26").Trim())));
                    }
                    DISK_BootPartition_V.Text = disk_boot_list[0];
                }catch (Exception){ }
                try{
                    // DISK BOOTABLE IS BOOT
                    bool disk_bootable_status = Convert.ToBoolean(query_disk["IsBoot"]);
                    if (disk_bootable_status == true){
                        disk_boot_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_27").Trim())));
                    }else if (disk_bootable_status == false){
                        disk_boot_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_28").Trim())));
                    }
                    DISK_BootableStatus_V.Text = disk_boot_status_list[0];
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
                    DISK_Type_V.Text = disk_type_list[0];
                }catch (Exception){ }
            }
            // SELECT DISK
            try{ DISK_SelectBox.SelectedIndex = 0; }catch(Exception){ }
        }
        private void disklist_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int disk_select = DISK_SelectBox.SelectedIndex;
                DISK_Model_V.Text = disk_model_list[disk_select];
                DISK_Man_V.Text = disk_man_list[disk_select];
                DISK_VolumeID_V.Text = disk_volume_list[disk_select];
                DISK_VolumeName_V.Text = disk_volume_name_list[disk_select];
                DISK_FirmWare_V.Text = disk_firmware_list[disk_select];
                DISK_Seri_V.Text = disk_serial_list[disk_select];
                DISK_Size_V.Text = disk_size_list[disk_select];
                DISK_FreeSpace_V.Text = disk_free_space_list[disk_select];
                DISK_PartitionLayout_V.Text = disk_partition_layout_list[disk_select];
                DISK_FileSystem_V.Text = disk_file_system_list[disk_select];
                DISK_Type_V.Text = disk_type_list[disk_select];
                DISK_DriveType_V.Text = disk_drive_type_list[disk_select];
                DISK_InterFace_V.Text = disk_interface_list[disk_select];
                DISK_Partition_V.Text = disk_partition_list[disk_select];
                DISK_UniqueID_V.Text = disk_uniquq_id_list[disk_select];
                DISK_Location_V.Text = disk_location_list[disk_select];
                DISK_Healt_V.Text = disk_healt_list[disk_select];
                DISK_BootPartition_V.Text = disk_boot_list[disk_select];
                DISK_BootableStatus_V.Text = disk_boot_status_list[disk_select];
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
                    NET_ListNetwork.Items.Add(Convert.ToString(query_na_rotate["Name"]));
                    NET_ListNetwork.SelectedIndex = 0;
                }catch (Exception){ }
                try{
                    // MAC ADRESS
                    string mac_adress = Convert.ToString(query_na_rotate["MACAddress"]);
                    if (mac_adress == ""){
                        network_mac_adress_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_1").Trim())));
                    }else{
                        network_mac_adress_list.Add(mac_adress);
                    }
                    NET_MacAdress_V.Text = network_mac_adress_list[0];
                }catch (Exception){ }
                try{
                    // NET MAN
                    string net_man = Convert.ToString(query_na_rotate["Manufacturer"]);
                    if (net_man == ""){
                        network_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_2").Trim())));
                    }else{
                        network_man_list.Add(net_man);
                    }
                    NET_NetMan_V.Text = network_man_list[0];
                }catch (Exception){ }
                try{
                    // SERVICE NAME
                    string service_name = Convert.ToString(query_na_rotate["ServiceName"]);
                    if (service_name == ""){
                        network_service_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_3").Trim())));
                    }else{
                        network_service_name_list.Add(service_name);
                    }
                    NET_ServiceName_V.Text = network_service_name_list[0];
                }catch (Exception){ }
                try{
                    // NET ADAPTER TYPE
                    string adaptor_type = Convert.ToString(query_na_rotate["AdapterType"]);
                    if (adaptor_type == ""){
                        network_adaptor_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_4").Trim())));
                    }else{
                        network_adaptor_type_list.Add(adaptor_type);
                    }
                    NET_AdapterType_V.Text = network_adaptor_type_list[0];
                }catch (Exception){ }
                try{
                    // NET GUID
                    string guid = Convert.ToString(query_na_rotate["GUID"]);
                    if (guid == ""){
                        network_guid_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_5").Trim())));
                    }else{
                        network_guid_list.Add(guid);
                    }
                    NET_Guid_V.Text = network_guid_list[0];
                }catch (Exception){ }
                try{
                    // NET CONNECTION ID
                    string net_con_id = Convert.ToString(query_na_rotate["NetConnectionID"]);
                    if (net_con_id == ""){
                        network_connection_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_6").Trim())));
                    }else{
                        network_connection_type_list.Add(net_con_id);
                    }
                    NET_ConnectionType_V.Text = network_connection_type_list[0];
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
                    NET_LocalConSpeed_V.Text = network_connection_speed_list[0];
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
                    NET_Dhcp_status_V.Text = network_dhcp_status_list[0];
                }catch (Exception){ }
                try{
                    // DHCP SERVER STATUS
                    string dhcp_server = Convert.ToString(query_nac_rotate["DHCPServer"]);
                    if (dhcp_server == ""){
                        network_dhcp_server_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_10").Trim())));
                    }else{
                        network_dhcp_server_list.Add(dhcp_server);
                    }
                    NET_Dhcp_server_V.Text = network_dhcp_server_list[0];
                }catch (Exception){ }
            }
            // IPv4 And IPv6 Adress
            try{
                IPHostEntry ip_entry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] adress_x64 = ip_entry.AddressList;
                NET_IPv4Adress_V.Text = adress_x64[adress_x64.Length - 1].ToString();
                if (adress_x64[0].AddressFamily == AddressFamily.InterNetworkV6){
                    NET_IPv6Adress_V.Text = adress_x64[0].ToString();
                }
            }catch (Exception){ }
            // NETWORK SELECT
            try{ NET_ListNetwork.SelectedIndex = 1; }catch (Exception){ NET_ListNetwork.SelectedIndex = 0; }
        }
        private void listnetwork_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int network_select = NET_ListNetwork.SelectedIndex;
                NET_MacAdress_V.Text = network_mac_adress_list[network_select];
                NET_NetMan_V.Text = network_man_list[network_select];
                NET_ServiceName_V.Text = network_service_name_list[network_select];
                NET_AdapterType_V.Text = network_adaptor_type_list[network_select];
                NET_Guid_V.Text = network_guid_list[network_select];
                NET_ConnectionType_V.Text = network_connection_type_list[network_select];
                NET_Dhcp_status_V.Text = network_dhcp_status_list[network_select];
                NET_Dhcp_server_V.Text = network_dhcp_server_list[network_select];
                NET_LocalConSpeed_V.Text = network_connection_speed_list[network_select];
            }catch(Exception){ }
        }
        // BATTERY
        // ======================================================================================================
        private void battery(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_battery = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");
            foreach (ManagementObject query_battery_rotate in search_battery.Get()){
                try{
                    // BATTERY ID                        
                    BATTERY_Model_V.Text = Convert.ToString(query_battery_rotate["DeviceID"]).Trim();
                }catch(Exception){ }
                try{
                    // BATTERY NAME
                    BATTERY_Name_V.Text = Convert.ToString(query_battery_rotate["Name"]).Trim();
                }catch(Exception){ }
                try{
                    // BATTERY TYPE
                    int battery_structure = Convert.ToInt32(query_battery_rotate["Chemistry"]);
                    if (battery_structure == 1){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_1").Trim()));
                    }else if (battery_structure == 2){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_2").Trim()));
                    }else if (battery_structure == 3){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_3").Trim()));
                    }else if (battery_structure == 4){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_4").Trim()));
                    }else if (battery_structure == 5){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_5").Trim()));
                    }else if (battery_structure == 6){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_6").Trim()));
                    }else if (battery_structure == 7){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_7").Trim()));
                    }else if (battery_structure == 8){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_8").Trim()));
                    }
                }catch(ManagementException){ }
            }
        }
        private void battery_visible_off(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            BATTERY_Status_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_9").Trim()));
            BATTERY_Model.Visible = false;
            BATTERY_Model_V.Visible = false;
            BATTERY_Name.Visible = false;
            BATTERY_Name_V.Visible = false;
            BATTERY_Type.Visible = false;
            BATTERY_Type_V.Visible = false;
            battery_panel_1.Height = 43;
        }
        private void battery_visible_on(){
            BATTERY_Model.Visible = true;
            BATTERY_Model_V.Visible = true;
            BATTERY_Name.Visible = true;
            BATTERY_Name_V.Visible = true;
            BATTERY_Type.Visible = true;
            BATTERY_Type_V.Visible = true;
            battery_panel_1.Height = 225;
        }
        private void laptop_bg_process(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                ManagementObjectSearcher search_battery = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");
                PowerStatus power = SystemInformation.PowerStatus;
                do{
                    if (loop_status == false){ break; }
                    Single battery = power.BatteryLifePercent;
                    Single battery_process = battery * 100;
                    string battery_status = "%" + Convert.ToString(battery_process);
                    BATTERY_Status_V.Text = battery_status;
                    foreach (ManagementObject query_battery_rotate in search_battery.Get()){
                        try{
                            // BATTERY VOLTAGE
                            double battery_voltage = Convert.ToDouble(query_battery_rotate["DesignVoltage"]) / 1000.0;
                            BATTERY_Voltage_V.Text = String.Format("{0:0.0} Volt", battery_voltage);
                        }catch (Exception){ }
                    }
                    BATTERY_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_8").Trim())) + " - " + battery_status;
                    if (power.PowerLineStatus == PowerLineStatus.Online){
                        if (battery_process == 100){
                            BATTERY_Status_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_10").Trim())) + " " + battery_status;
                        }else{
                            BATTERY_Status_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_11").Trim())) + " " + battery_status;
                        }
                    }else{
                        BATTERY_Status_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_12").Trim())) + " " + battery_status;
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
                    OSD_DataMainTable.Rows.Add(driver_infos);
                }
            }catch (ManagementException){ }
            OSD_TYSS_V.Text = OSD_DataMainTable.Rows.Count.ToString();
            OSD_DataMainTable.ClearSelection();
        }
        private void OSD_TextBox_TextChanged(object sender, EventArgs e){
            if (OSD_TextBox.Text == "" || OSD_TextBox.Text == string.Empty){ OSD_DataMainTable.ClearSelection(); OSD_DataMainTable.FirstDisplayedScrollingRowIndex = OSD_DataMainTable.Rows[0].Index; }
            else{ try{ foreach (DataGridViewRow row in OSD_DataMainTable.Rows){ if (row.Cells[0].Value.ToString().ToLower().Contains(OSD_TextBox.Text.ToString().Trim().ToLower())){ row.Selected = true; OSD_DataMainTable.FirstDisplayedScrollingRowIndex = row.Index; break; } } }catch(Exception){ } }
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
                    SERVICE_DataMainTable.Rows.Add(services_infos);
                }
            }catch (ManagementException){ }
            SERVICE_TYS_V.Text = SERVICE_DataMainTable.Rows.Count.ToString();
            SERVICE_DataMainTable.ClearSelection();
        }
        private void Services_SearchTextBox_TextChanged(object sender, EventArgs e){
            if (SERVICE_SearchTextBox.Text == "" || SERVICE_SearchTextBox.Text == string.Empty){ SERVICE_DataMainTable.ClearSelection(); SERVICE_DataMainTable.FirstDisplayedScrollingRowIndex = SERVICE_DataMainTable.Rows[0].Index; }
            else{ try{ foreach (DataGridViewRow row in SERVICE_DataMainTable.Rows){ if (row.Cells[0].Value.ToString().ToLower().Contains(SERVICE_SearchTextBox.Text.ToString().Trim().ToLower())){ row.Selected = true; SERVICE_DataMainTable.FirstDisplayedScrollingRowIndex = row.Index; break; } } }catch(Exception){ } }
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
            foreach (Control disabled_btn in LeftMenuPanel.Controls){
                if (theme == 1){ disabled_btn.BackColor = btn_colors[0]; }
                else if (theme == 2){ disabled_btn.BackColor = btn_colors[2]; }
            }
        }
        private void OSRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = OS;
            menu_btns = 1;
            menu_rp = 1;
            HeaderImage.BackgroundImage = Properties.Resources.menu_windows;
            if (OS_RotateBtn.BackColor != btn_colors[1] && OS_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_1").Trim()));
        }
        private void MBRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = MB;
            menu_btns = 2;
            menu_rp = 2;
            HeaderImage.BackgroundImage = Properties.Resources.menu_motherboard;
            if (MB_RotateBtn.BackColor != btn_colors[1] && MB_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()));
        }
        private void CPURotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = CPU;
            menu_btns = 3;
            menu_rp = 3;
            HeaderImage.BackgroundImage = Properties.Resources.menu_cpu;
            if (CPU_RotateBtn.BackColor != btn_colors[1] && CPU_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()));
        }
        private void RAMRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = RAM;
            menu_btns = 4;
            menu_rp = 4;
            HeaderImage.BackgroundImage = Properties.Resources.menu_ram;
            if (RAM_RotateBtn.BackColor != btn_colors[1] && RAM_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()));
        }
        private void GPURotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = GPU;
            menu_btns = 5;
            menu_rp = 5;
            HeaderImage.BackgroundImage = Properties.Resources.menu_gpu;
            if (GPU_RotateBtn.BackColor != btn_colors[1] && GPU_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()));
        }
        private void DISKRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = DISK;
            menu_btns = 6;
            menu_rp = 6;
            HeaderImage.BackgroundImage = Properties.Resources.menu_disk;
            if (DISK_RotateBtn.BackColor != btn_colors[1] && DISK_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()));
        }
        private void NETWORKRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = NETWORK;
            menu_btns = 7;
            menu_rp = 7;
            HeaderImage.BackgroundImage = Properties.Resources.menu_network;
            if (NET_RotateBtn.BackColor != btn_colors[1] && NET_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()));
        }
        private void PILRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = BATTERY;
            menu_btns = 8;
            menu_rp = 8;
            HeaderImage.BackgroundImage = Properties.Resources.menu_battery;
            if (BATTERY_RotateBtn.BackColor != btn_colors[1] && BATTERY_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()));
        }
        private void OSDRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = OSD;
            menu_btns = 9;
            menu_rp = 9;
            HeaderImage.BackgroundImage = Properties.Resources.menu_osd;
            if (OSD_RotateBtn.BackColor != btn_colors[1] && OSD_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()));
        }
        private void ServicesRotateBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            MainContent.SelectedTab = GSERVICE;
            menu_btns = 10;
            menu_rp = 10;
            HeaderImage.BackgroundImage = Properties.Resources.menu_services;
            if (SERVICES_RotateBtn.BackColor != btn_colors[1] && SERVICES_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()));
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
            foreach (ToolStripMenuItem disabled_lang in languageToolStripMenuItem.DropDownItems){
                disabled_lang.Checked = false;
            }
        }
        // LANG SWAP
        // ======================================================================================================
        private void englishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "en"){ lang_preload("en"); select_lang_active(sender); }
        }
        private void turkishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "tr"){ lang_preload("tr"); select_lang_active(sender); }
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
                    case "en":
                        lang = "en";
                        lang_path = glow_lang_en;
                        break;
                    case "tr":
                        lang = "tr";
                        lang_path = glow_lang_tr;
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
                // PRINT INFORMATION
                printInformationToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_1").Trim()));
                // SETTINGS
                settingsToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_2").Trim()));
                // THEMES
                themeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_3").Trim()));
                lightThemeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderThemes", "theme_light").Trim()));
                darkThemeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderThemes", "theme_dark").Trim()));
                // LANGS
                languageToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_4").Trim()));
                englishToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_en").Trim()));
                turkishToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_tr").Trim()));
                // TOOLS
                toolsToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_5").Trim()));
                pingTestToolToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderTools", "header_t_1").Trim()));
                // HELP
                helpToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_6").Trim()));
                supportToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHelp", "header_h_1").Trim()));
                gitHubToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHelp", "header_h_2").Trim()));
                // MENU
                OS_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_1").Trim()));
                MB_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_2").Trim()));
                CPU_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_3").Trim()));
                RAM_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_4").Trim()));
                GPU_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_5").Trim()));
                DISK_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_6").Trim()));
                NET_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_7").Trim()));
                BATTERY_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_8").Trim()));
                OSD_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_9").Trim()));
                SERVICES_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_10").Trim()));
                // OS
                OS_SystemUser.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_1").Trim()));
                OS_ComputerName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_2").Trim()));
                OS_SystemModel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_3").Trim()));
                OS_SavedUser.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_4").Trim()));
                OS_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_5").Trim()));
                OS_Manufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_6").Trim()));
                OS_SystemVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_7").Trim()));
                OS_Build.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_8").Trim()));
                OS_SystemArchitectural.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_9").Trim()));
                OS_Family.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_10").Trim()));
                OS_Serial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_11").Trim()));
                OS_Country.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_12").Trim()));
                OS_CharacterSet.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_13").Trim()));
                OS_EncryptionType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_14").Trim()));
                OS_SystemRootIndex.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_15").Trim()));
                OS_SystemBuildPart.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_16").Trim()));
                OS_SystemTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_17").Trim()));
                OS_Install.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_18").Trim()));
                OS_SystemWorkTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_19").Trim()));
                OS_LastBootTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_20").Trim()));
                OS_PortableOS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_21").Trim()));
                OS_MouseWheelStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_22").Trim()));
                OS_ScrollLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_23").Trim()));
                OS_NumLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_24").Trim()));
                OS_CapsLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_25").Trim()));
                OS_BootPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_26").Trim()));
                OS_SystemPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_27").Trim()));
                OS_Wallpaper.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_28").Trim()));
                MainToolTip.SetToolTip(OS_WallpaperOpen, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_29").Trim())), wp_rotate));
                // MB
                MB_MotherBoardName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_1").Trim()));
                MB_MotherBoardMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_2").Trim()));
                MB_MotherBoardSerial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_3").Trim()));
                MB_Chipset.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_4").Trim()));
                MB_BiosManufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_5").Trim()));
                MB_BiosDate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_6").Trim()));
                MB_BiosVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_7").Trim()));
                MB_SmBiosVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_8").Trim()));
                MB_Model.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_9").Trim()));
                MB_PrimaryBusType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_10").Trim()));
                MB_BiosMajorMinor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_11").Trim()));
                MB_SMBiosMajorMinor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_12").Trim()));
                // CPU
                CPU_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_1").Trim()));
                CPU_Manufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_2").Trim()));
                CPU_Architectural.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_3").Trim()));
                CPU_NormalSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_4").Trim()));
                CPU_DefaultSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_5").Trim()));
                CPU_L1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_6").Trim()));
                CPU_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_7").Trim()));
                CPU_L3.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_8").Trim()));
                CPU_CoreCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_9").Trim()));
                CPU_LogicalCore.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_10").Trim()));
                CPU_SocketDefinition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_11").Trim()));
                CPU_Family.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_12").Trim()));
                CPU_Virtualization.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_13").Trim()));
                CPU_VMMonitorExtension.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_14").Trim()));
                CPU_SerialName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_15").Trim()));
                // RAM
                RAM_TotalRAM.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_1").Trim()));
                RAM_UsageRAMCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_2").Trim()));
                RAM_EmptyRamCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_3").Trim()));
                RAM_TotalVirtualRam.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_4").Trim()));
                RAM_EmptyVirtualRam.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_5").Trim()));
                RAM_SlotStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_6").Trim()));
                RAM_SlotSelectLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_7").Trim()));
                RAM_Amount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_8").Trim()));
                RAM_Type.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_9").Trim()));
                RAM_Frequency.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_10").Trim()));
                RAM_Volt.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_11").Trim()));
                RAM_FormFactor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_12").Trim()));
                RAM_Serial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_13").Trim()));
                RAM_Manufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_14").Trim()));
                RAM_BankLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_15").Trim()));
                RAM_DataWidth.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_16").Trim()));
                RAM_BellekType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_17").Trim()));
                RAM_PartNumber.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_18").Trim()));
                // GPU
                GPU_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_1").Trim()));
                GPU_Manufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_2").Trim()));
                GPU_Version.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_3").Trim()));
                GPU_DriverDate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_4").Trim()));
                GPU_Status.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_5").Trim()));
                GPU_DacType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_6").Trim()));
                GPU_GraphicDriversName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_7").Trim()));
                GPU_InfFileName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_8").Trim()));
                GPU_INFSectionFile.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_9").Trim()));
                GPU_MonitorSelect.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_10").Trim()));
                GPU_MonitorBounds.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_11").Trim()));
                GPU_MonitorWorking.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_12").Trim()));
                GPU_MonitorResLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_13").Trim()));
                GPU_MonitorVirtualRes.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_14").Trim()));
                GPU_ScreenRefreshRate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_15").Trim()));
                GPU_MonitorPrimary.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_16").Trim()));
                // DISK
                DISK_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_1").Trim()));
                DISK_Model.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_2").Trim()));
                DISK_Man.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_3").Trim()));
                DISK_VolumeID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_4").Trim()));
                DISK_VolumeName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_5").Trim()));
                DISK_FirmWare.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_6").Trim()));
                DISK_Seri.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_7").Trim()));
                DISK_Size.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_8").Trim()));
                DISK_FreeSpace.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_9").Trim()));
                DISK_PartitionLayout.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_10").Trim()));
                DISK_FileSystem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_11").Trim()));
                DISK_Type.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_12").Trim()));
                DISK_DriveType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_13").Trim()));
                DISK_InterFace.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_14").Trim()));
                DISK_Partition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_15").Trim()));
                DISK_UniqueID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_16").Trim()));
                DISK_Location.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_17").Trim()));
                DISK_Healt.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_18").Trim()));
                DISK_BootPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_19").Trim()));
                DISK_BootableStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_20").Trim()));
                // NETWORK
                NET_ConnType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_1").Trim()));
                NET_MacAdress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_2").Trim()));
                NET_NetMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_3").Trim()));
                NET_ServiceName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_4").Trim()));
                NET_AdapterType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_5").Trim()));
                NET_Guid.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_6").Trim()));
                NET_ConnectionType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_7").Trim()));
                NET_Dhcp_status.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_8").Trim()));
                NET_Dhcp_server.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_9").Trim()));
                NET_LocalConSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_10").Trim()));
                NET_IPv4Adress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_11").Trim()));
                NET_IPv6Adress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_12").Trim()));
                // BATTERY
                BATTERY_Status.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_1").Trim()));
                BATTERY_Model.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_2").Trim()));
                BATTERY_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_3").Trim()));
                BATTERY_Voltage.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_4").Trim()));
                BATTERY_Type.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_5").Trim()));
                // OSD
                OSD_DataMainTable.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_1").Trim()));
                OSD_DataMainTable.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_2").Trim()));
                OSD_DataMainTable.Columns[2].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_3").Trim()));
                OSD_DataMainTable.Columns[3].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_4").Trim()));
                OSD_DataMainTable.Columns[4].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_5").Trim()));
                OSD_SearchDriverLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_6").Trim()));
                OSD_TYSS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_7").Trim()));
                // SERVICES
                SERVICE_DataMainTable.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_1").Trim()));
                SERVICE_DataMainTable.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_2").Trim()));
                SERVICE_DataMainTable.Columns[2].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_3").Trim()));
                SERVICE_DataMainTable.Columns[3].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_4").Trim()));
                SERVICE_DataMainTable.Columns[4].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_5").Trim()));
                SERVICE_SearchLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_6").Trim()));
                SERVICE_TYS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_7").Trim()));
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
            foreach (ToolStripMenuItem disabled_theme in themeToolStripMenuItem.DropDownItems){
                disabled_theme.Checked = false;
            }
        }
        // THEME SWAP
        // ======================================================================================================
        private void lightThemeToolStripMenuItem_Click(object sender, EventArgs e){
            if (theme != 1){ color_mode(1); select_theme_active(sender); }
        }
        private void darkThemeToolStripMenuItem_Click(object sender, EventArgs e){
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
                // TITLEBAR CHANGE 
                try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } } catch (Exception){ }
                // CLEAR PRELOAD ITEMS
                if (ui_colors.Count > 0){ ui_colors.Clear(); }
                if (header_colors.Count > 1){ header_colors.Clear(); }
                // HEADER MENU COLOR MODE
                header_colors.Add(Color.FromArgb(210, 210, 210));
                header_colors.Add(Color.FromArgb(53, 53, 61));
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
                    glow_setting_save.GlowWriteSettings("Theme", "ThemeStatus", "1");
                }catch (Exception){ }
            }else if (theme == 2){
                // TITLEBAR CHANGE
                try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } } catch (Exception){ }
                // CLEAR PRELOAD ITEMS
                if (ui_colors.Count > 0){ ui_colors.Clear(); }
                if (header_colors.Count > 1){ header_colors.Clear(); }
                // HEADER MENU COLOR MODE
                header_colors.Add(Color.FromArgb(53, 53, 61));
                header_colors.Add(Color.FromArgb(210, 210, 210));
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
                try{
                    GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                    glow_setting_save.GlowWriteSettings("Theme", "ThemeStatus", "0");
                }catch (Exception){ }
            }
            theme_engine();
        }
        private void theme_engine(){
            try{
                HeaderMenu.Renderer = new HeaderMenuColors();
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
                printInformationToolStripMenuItem.ForeColor = ui_colors[0];
                printInformationToolStripMenuItem.BackColor = ui_colors[1];
                // SETTINGS
                settingsToolStripMenuItem.ForeColor = ui_colors[0];
                settingsToolStripMenuItem.BackColor = ui_colors[1];
                // THEMES
                themeToolStripMenuItem.BackColor = ui_colors[1];
                themeToolStripMenuItem.ForeColor = ui_colors[0];
                lightThemeToolStripMenuItem.BackColor = ui_colors[1];
                lightThemeToolStripMenuItem.ForeColor = ui_colors[0];
                darkThemeToolStripMenuItem.BackColor = ui_colors[1];
                darkThemeToolStripMenuItem.ForeColor = ui_colors[0];
                // LANGS
                languageToolStripMenuItem.BackColor = ui_colors[1];
                languageToolStripMenuItem.ForeColor = ui_colors[0];
                englishToolStripMenuItem.BackColor = ui_colors[1];
                englishToolStripMenuItem.ForeColor = ui_colors[0];
                turkishToolStripMenuItem.BackColor = ui_colors[1];
                turkishToolStripMenuItem.ForeColor = ui_colors[0];
                // TOOLS
                toolsToolStripMenuItem.BackColor = ui_colors[1];
                toolsToolStripMenuItem.ForeColor = ui_colors[0];
                pingTestToolToolStripMenuItem.BackColor = ui_colors[1];
                pingTestToolToolStripMenuItem.ForeColor = ui_colors[0];
                // HELP
                helpToolStripMenuItem.BackColor = ui_colors[1];
                helpToolStripMenuItem.ForeColor = ui_colors[0];
                supportToolStripMenuItem.BackColor = ui_colors[1];
                supportToolStripMenuItem.ForeColor = ui_colors[0];
                gitHubToolStripMenuItem.BackColor = ui_colors[1];
                gitHubToolStripMenuItem.ForeColor = ui_colors[0];
                // LEFT MENU
                LeftMenuPanel.BackColor = ui_colors[2];
                OS_RotateBtn.BackColor = ui_colors[2];
                MB_RotateBtn.BackColor = ui_colors[2];
                CPU_RotateBtn.BackColor = ui_colors[2];
                RAM_RotateBtn.BackColor = ui_colors[2];
                GPU_RotateBtn.BackColor = ui_colors[2];
                DISK_RotateBtn.BackColor = ui_colors[2];
                NET_RotateBtn.BackColor = ui_colors[2];
                BATTERY_RotateBtn.BackColor = ui_colors[2];
                OSD_RotateBtn.BackColor = ui_colors[2];
                SERVICES_RotateBtn.BackColor = ui_colors[2];
                // LEFT MENU BORDER
                OS_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                MB_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                CPU_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                RAM_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                GPU_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                DISK_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                NET_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                BATTERY_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                OSD_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                SERVICES_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                // LEFT MENU MOUSE HOVER
                OS_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                MB_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                CPU_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                RAM_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                GPU_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                DISK_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                NET_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                BATTERY_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                OSD_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                SERVICES_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                // LEFT MENU MOUSE DOWN
                OS_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                MB_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                CPU_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                RAM_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                GPU_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                DISK_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                NET_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                BATTERY_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                OSD_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                SERVICES_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                // LEFT MENU BUTTON TEXT COLOR
                OS_RotateBtn.ForeColor = ui_colors[4];
                MB_RotateBtn.ForeColor = ui_colors[4];
                CPU_RotateBtn.ForeColor = ui_colors[4];
                RAM_RotateBtn.ForeColor = ui_colors[4];
                GPU_RotateBtn.ForeColor = ui_colors[4];
                DISK_RotateBtn.ForeColor = ui_colors[4];
                NET_RotateBtn.ForeColor = ui_colors[4];
                BATTERY_RotateBtn.ForeColor = ui_colors[4];
                OSD_RotateBtn.ForeColor = ui_colors[4];
                SERVICES_RotateBtn.ForeColor = ui_colors[4];
                // CONTENT BG
                BackColor = ui_colors[5];
                OS.BackColor = ui_colors[5];
                MB.BackColor = ui_colors[5];
                CPU.BackColor = ui_colors[5];
                RAM.BackColor = ui_colors[5];
                GPU.BackColor = ui_colors[5];
                DISK.BackColor = ui_colors[5];
                NETWORK.BackColor = ui_colors[5];
                BATTERY.BackColor = ui_colors[5];
                OSD.BackColor = ui_colors[5];
                GSERVICE.BackColor = ui_colors[5];
                // OS
                os_panel_1.BackColor = ui_colors[6];
                os_panel_2.BackColor = ui_colors[6];
                os_panel_3.BackColor = ui_colors[6];
                os_panel_4.BackColor = ui_colors[6];
                os_bottom_label.ForeColor = ui_colors[9];
                OS_SystemUser.ForeColor = ui_colors[7];
                OS_SystemUser_V.ForeColor = ui_colors[8];
                OS_ComputerName.ForeColor = ui_colors[7];
                OS_ComputerName_V.ForeColor = ui_colors[8];
                OS_SystemModel.ForeColor = ui_colors[7];
                OS_SystemModel_V.ForeColor = ui_colors[8];
                OS_SavedUser.ForeColor = ui_colors[7];
                OS_SavedUser_V.ForeColor = ui_colors[8];
                OS_Name.ForeColor = ui_colors[7];
                OS_Name_V.ForeColor = ui_colors[8];
                OS_Manufacturer.ForeColor = ui_colors[7];
                OS_Manufacturer_V.ForeColor = ui_colors[8];
                OS_SystemVersion.ForeColor = ui_colors[7];
                OS_SystemVersion_V.ForeColor = ui_colors[8];
                OS_Build.ForeColor = ui_colors[7];
                OS_Build_V.ForeColor = ui_colors[8];
                OS_SystemArchitectural.ForeColor = ui_colors[7];
                OS_SystemArchitectural_V.ForeColor = ui_colors[8];
                OS_Family.ForeColor = ui_colors[7];
                OS_Family_V.ForeColor = ui_colors[8];
                OS_Serial.ForeColor = ui_colors[7];
                OS_Serial_V.ForeColor = ui_colors[8];
                OS_Country.ForeColor = ui_colors[7];
                OS_Country_V.ForeColor = ui_colors[8];
                OS_CharacterSet.ForeColor = ui_colors[7];
                OS_CharacterSet_V.ForeColor = ui_colors[8];
                OS_EncryptionType.ForeColor = ui_colors[7];
                OS_EncryptionType_V.ForeColor = ui_colors[8];
                OS_SystemRootIndex.ForeColor = ui_colors[7];
                OS_SystemRootIndex_V.ForeColor = ui_colors[8];
                OS_SystemBuildPart.ForeColor = ui_colors[7];
                OS_SystemBuildPart_V.ForeColor = ui_colors[8];
                OS_SystemTime.ForeColor = ui_colors[7];
                OS_SystemTime_V.ForeColor = ui_colors[8];
                OS_Install.ForeColor = ui_colors[7];
                OS_Install_V.ForeColor = ui_colors[8];
                OS_SystemWorkTime.ForeColor = ui_colors[7];
                OS_SystemWorkTime_V.ForeColor = ui_colors[8];
                OS_LastBootTime.ForeColor = ui_colors[7];
                OS_LastBootTime_V.ForeColor = ui_colors[8];
                OS_PortableOS.ForeColor = ui_colors[7];
                OS_PortableOS_V.ForeColor = ui_colors[8];
                OS_MouseWheelStatus.ForeColor = ui_colors[7];
                OS_MouseWheelStatus_V.ForeColor = ui_colors[8];
                OS_ScrollLockStatus.ForeColor = ui_colors[7];
                OS_ScrollLockStatus_V.ForeColor = ui_colors[8];
                OS_NumLockStatus.ForeColor = ui_colors[7];
                OS_NumLockStatus_V.ForeColor = ui_colors[8];
                OS_CapsLockStatus.ForeColor = ui_colors[7];
                OS_CapsLockStatus_V.ForeColor = ui_colors[8];
                OS_BootPartition.ForeColor = ui_colors[7];
                OS_BootPartition_V.ForeColor = ui_colors[8];
                OS_SystemPartition.ForeColor = ui_colors[7];
                OS_SystemPartition_V.ForeColor = ui_colors[8];
                OS_Wallpaper.ForeColor = ui_colors[7];
                OS_Wallpaper_V.ForeColor = ui_colors[8];
                // MB
                mb_panel_1.BackColor = ui_colors[6];
                mb_panel_2.BackColor = ui_colors[6];
                mb_bottom_1.ForeColor = ui_colors[9];
                MB_MotherBoardName.ForeColor = ui_colors[7];
                MB_MotherBoardName_V.ForeColor = ui_colors[8];
                MB_MotherBoardMan.ForeColor = ui_colors[7];
                MB_MotherBoardMan_V.ForeColor = ui_colors[8];
                MB_MotherBoardSerial.ForeColor = ui_colors[7];
                MB_MotherBoardSerial_V.ForeColor = ui_colors[8];
                MB_Chipset.ForeColor = ui_colors[7];
                MB_Chipset_V.ForeColor = ui_colors[8];
                MB_BiosManufacturer.ForeColor = ui_colors[7];
                MB_BiosManufacturer_V.ForeColor = ui_colors[8];
                MB_BiosDate.ForeColor = ui_colors[7];
                MB_BiosDate_V.ForeColor = ui_colors[8];
                MB_BiosVersion.ForeColor = ui_colors[7];
                MB_BiosVersion_V.ForeColor = ui_colors[8];
                MB_SmBiosVersion.ForeColor = ui_colors[7];
                MB_SmBiosVersion_V.ForeColor = ui_colors[8];
                MB_Model.ForeColor = ui_colors[7];
                MB_Model_V.ForeColor = ui_colors[8];
                MB_PrimaryBusType.ForeColor = ui_colors[7];
                MB_PrimaryBusType_V.ForeColor = ui_colors[8];
                MB_BiosMajorMinor.ForeColor = ui_colors[7];
                MB_BiosMajorMinor_V.ForeColor = ui_colors[8];
                MB_SMBiosMajorMinor.ForeColor = ui_colors[7];
                MB_SMBiosMajorMinor_V.ForeColor = ui_colors[8];
                // CPU
                cpu_panel_1.BackColor = ui_colors[6];
                cpu_panel_2.BackColor = ui_colors[6];
                cpu_bottom_1.ForeColor = ui_colors[9];
                CPU_Name.ForeColor = ui_colors[7];
                CPU_Name_V.ForeColor = ui_colors[8];
                CPU_Manufacturer.ForeColor = ui_colors[7];
                CPU_Manufacturer_V.ForeColor = ui_colors[8];
                CPU_Architectural.ForeColor = ui_colors[7];
                CPU_Architectural_V.ForeColor = ui_colors[8];
                CPU_NormalSpeed.ForeColor = ui_colors[7];
                CPU_NormalSpeed_V.ForeColor = ui_colors[8];
                CPU_DefaultSpeed.ForeColor = ui_colors[7];
                CPU_DefaultSpeed_V.ForeColor = ui_colors[8];
                CPU_L1.ForeColor = ui_colors[7];
                CPU_L1_V.ForeColor = ui_colors[8];
                CPU_L2.ForeColor = ui_colors[7];
                CPU_L2_V.ForeColor = ui_colors[8];
                CPU_L3.ForeColor = ui_colors[7];
                CPU_L3_V.ForeColor = ui_colors[8];
                CPU_CoreCount.ForeColor = ui_colors[7];
                CPU_CoreCount_V.ForeColor = ui_colors[8];
                CPU_LogicalCore.ForeColor = ui_colors[7];
                CPU_LogicalCore_V.ForeColor = ui_colors[8];
                CPU_SocketDefinition.ForeColor = ui_colors[7];
                CPU_SocketDefinition_V.ForeColor = ui_colors[8];
                CPU_Family.ForeColor = ui_colors[7];
                CPU_Family_V.ForeColor = ui_colors[8];
                CPU_Virtualization.ForeColor = ui_colors[7];
                CPU_Virtualization_V.ForeColor = ui_colors[8];
                CPU_VMMonitorExtension.ForeColor = ui_colors[7];
                CPU_VMMonitorExtension_V.ForeColor = ui_colors[8];
                CPU_SerialName.ForeColor = ui_colors[7];
                CPU_SerialName_V.ForeColor = ui_colors[8];
                // RAM
                ram_panel_1.BackColor = ui_colors[6];
                ram_panel_2.BackColor = ui_colors[6];
                ram_bottom_1.ForeColor = ui_colors[9];
                RAM_TotalRAM.ForeColor = ui_colors[7];
                RAM_TotalRAM_V.ForeColor = ui_colors[8];
                RAM_UsageRAMCount.ForeColor = ui_colors[7];
                RAM_UsageRAMCount_V.ForeColor = ui_colors[8];
                RAM_EmptyRamCount.ForeColor = ui_colors[7];
                RAM_EmptyRamCount_V.ForeColor = ui_colors[8];
                RAM_TotalVirtualRam.ForeColor = ui_colors[7];
                RAM_TotalVirtualRam_V.ForeColor = ui_colors[8];
                RAM_EmptyVirtualRam.ForeColor = ui_colors[7];
                RAM_EmptyVirtualRam_V.ForeColor = ui_colors[8];
                RAM_SlotStatus.ForeColor = ui_colors[7];
                RAM_SlotStatus_V.ForeColor = ui_colors[8];
                RAM_SlotSelectLabel.ForeColor = ui_colors[7];
                RAM_SelectList.BackColor = ui_colors[10];
                RAM_SelectList.ForeColor = ui_colors[8];
                RAM_Amount.ForeColor = ui_colors[7];
                RAM_Amount_V.ForeColor = ui_colors[8];
                RAM_Type.ForeColor = ui_colors[7];
                RAM_Type_V.ForeColor = ui_colors[8];
                RAM_Frequency.ForeColor = ui_colors[7];
                RAM_Frequency_V.ForeColor = ui_colors[8];
                RAM_Volt.ForeColor = ui_colors[7];
                RAM_Volt_V.ForeColor = ui_colors[8];
                RAM_FormFactor.ForeColor = ui_colors[7];
                RAM_FormFactor_V.ForeColor = ui_colors[8];
                RAM_Serial.ForeColor = ui_colors[7];
                RAM_Serial_V.ForeColor = ui_colors[8];
                RAM_Manufacturer.ForeColor = ui_colors[7];
                RAM_Manufacturer_V.ForeColor = ui_colors[8];
                RAM_BankLabel.ForeColor = ui_colors[7];
                RAM_BankLabel_V.ForeColor = ui_colors[8];
                RAM_DataWidth.ForeColor = ui_colors[7];
                RAM_DataWidth_V.ForeColor = ui_colors[8];
                RAM_BellekType.ForeColor = ui_colors[7];
                RAM_BellekType_V.ForeColor = ui_colors[8];
                RAM_PartNumber.ForeColor = ui_colors[7];
                RAM_PartNumber_V.ForeColor = ui_colors[8];
                // GPU
                gpu_panel_1.BackColor = ui_colors[6];
                gpu_panel_2.BackColor = ui_colors[6];
                gpu_bottom_1.ForeColor = ui_colors[9];
                GPU_Name.ForeColor = ui_colors[7];
                GPU_Select.BackColor = ui_colors[10];
                GPU_Select.ForeColor = ui_colors[8];
                GPU_Manufacturer.ForeColor = ui_colors[7];
                GPU_Manufacturer_V.ForeColor = ui_colors[8];
                GPU_Version.ForeColor = ui_colors[7];
                GPU_Version_V.ForeColor = ui_colors[8];
                GPU_DriverDate.ForeColor = ui_colors[7];
                GPU_DriverDate_V.ForeColor = ui_colors[8];
                GPU_Status.ForeColor = ui_colors[7];
                GPU_Status_V.ForeColor = ui_colors[8];
                GPU_DacType.ForeColor = ui_colors[7];
                GPU_DacType_V.ForeColor = ui_colors[8];
                GPU_GraphicDriversName.ForeColor = ui_colors[7];
                GPU_GraphicDriversName_V.ForeColor = ui_colors[8];
                GPU_InfFileName.ForeColor = ui_colors[7];
                GPU_InfFileName_V.ForeColor = ui_colors[8];
                GPU_INFSectionFile.ForeColor = ui_colors[7];
                GPU_INFSectionFile_V.ForeColor = ui_colors[8];
                GPU_MonitorSelectList.BackColor = ui_colors[10];
                GPU_MonitorSelectList.ForeColor = ui_colors[8];
                GPU_MonitorSelect.ForeColor = ui_colors[7];
                GPU_MonitorBounds.ForeColor = ui_colors[7];
                GPU_MonitorBounds_V.ForeColor = ui_colors[8];
                GPU_MonitorWorking.ForeColor = ui_colors[7];
                GPU_MonitorWorking_V.ForeColor = ui_colors[8];
                GPU_MonitorResLabel.ForeColor = ui_colors[7];
                GPU_MonitorResLabel_V.ForeColor = ui_colors[8];
                GPU_MonitorVirtualRes.ForeColor = ui_colors[7];
                GPU_MonitorVirtualRes_V.ForeColor = ui_colors[8];
                GPU_ScreenRefreshRate.ForeColor = ui_colors[7];
                GPU_ScreenRefreshRate_V.ForeColor = ui_colors[8];
                GPU_MonitorPrimary.ForeColor = ui_colors[7];
                GPU_MonitorPrimary_V.ForeColor = ui_colors[8];
                // DISK
                disk_panel_1.BackColor = ui_colors[6];
                disk_panel_2.BackColor = ui_colors[6];
                disk_bottom_label.ForeColor = ui_colors[9];
                DISK_Name.ForeColor = ui_colors[7];
                DISK_SelectBox.BackColor = ui_colors[10];
                DISK_SelectBox.ForeColor = ui_colors[8];
                DISK_Model.ForeColor = ui_colors[7];
                DISK_Model_V.ForeColor = ui_colors[8];
                DISK_Man.ForeColor = ui_colors[7];
                DISK_Man_V.ForeColor = ui_colors[8];
                DISK_VolumeID.ForeColor = ui_colors[7];
                DISK_VolumeID_V.ForeColor = ui_colors[8];
                DISK_VolumeName.ForeColor = ui_colors[7];
                DISK_VolumeName_V.ForeColor = ui_colors[8];
                DISK_FirmWare.ForeColor = ui_colors[7];
                DISK_FirmWare_V.ForeColor = ui_colors[8];
                DISK_Seri.ForeColor = ui_colors[7];
                DISK_Seri_V.ForeColor = ui_colors[8];
                DISK_Size.ForeColor = ui_colors[7];
                DISK_Size_V.ForeColor = ui_colors[8];
                DISK_FreeSpace.ForeColor = ui_colors[7];
                DISK_FreeSpace_V.ForeColor = ui_colors[8];
                DISK_PartitionLayout.ForeColor = ui_colors[7];
                DISK_PartitionLayout_V.ForeColor = ui_colors[8];
                DISK_FileSystem.ForeColor = ui_colors[7];
                DISK_FileSystem_V.ForeColor = ui_colors[8];
                DISK_Type.ForeColor = ui_colors[7];
                DISK_Type_V.ForeColor = ui_colors[8];
                DISK_DriveType.ForeColor = ui_colors[7];
                DISK_DriveType_V.ForeColor = ui_colors[8];
                DISK_InterFace.ForeColor = ui_colors[7];
                DISK_InterFace_V.ForeColor = ui_colors[8];
                DISK_Partition.ForeColor = ui_colors[7];
                DISK_Partition_V.ForeColor = ui_colors[8];
                DISK_UniqueID.ForeColor = ui_colors[7];
                DISK_UniqueID_V.ForeColor = ui_colors[8];
                DISK_Location.ForeColor = ui_colors[7];
                DISK_Location_V.ForeColor = ui_colors[8];
                DISK_BootPartition.ForeColor = ui_colors[7];
                DISK_BootPartition_V.ForeColor = ui_colors[8];
                DISK_Healt.ForeColor = ui_colors[7];
                DISK_Healt_V.ForeColor = ui_colors[8];
                DISK_BootableStatus.ForeColor = ui_colors[7];
                DISK_BootableStatus_V.ForeColor = ui_colors[8];
                // NETWORK
                network_panel_1.BackColor = ui_colors[6];
                network_bottom_label.ForeColor = ui_colors[9];
                NET_ListNetwork.BackColor = ui_colors[10];
                NET_ListNetwork.ForeColor = ui_colors[8];
                NET_ConnType.ForeColor = ui_colors[7];
                NET_MacAdress.ForeColor = ui_colors[7];
                NET_MacAdress_V.ForeColor = ui_colors[8];
                NET_NetMan.ForeColor = ui_colors[7];
                NET_NetMan_V.ForeColor = ui_colors[8];
                NET_ServiceName.ForeColor = ui_colors[7];
                NET_ServiceName_V.ForeColor = ui_colors[8];
                NET_AdapterType.ForeColor = ui_colors[7];
                NET_AdapterType_V.ForeColor = ui_colors[8];
                NET_Guid.ForeColor = ui_colors[7];
                NET_Guid_V.ForeColor = ui_colors[8];
                NET_ConnectionType.ForeColor = ui_colors[7];
                NET_ConnectionType_V.ForeColor = ui_colors[8];
                NET_Dhcp_status.ForeColor = ui_colors[7];
                NET_Dhcp_status_V.ForeColor = ui_colors[8];
                NET_Dhcp_server.ForeColor = ui_colors[7];
                NET_Dhcp_server_V.ForeColor = ui_colors[8];
                NET_LocalConSpeed.ForeColor = ui_colors[7];
                NET_LocalConSpeed_V.ForeColor = ui_colors[8];
                NET_IPv4Adress.ForeColor = ui_colors[7];
                NET_IPv4Adress_V.ForeColor = ui_colors[8];
                NET_IPv6Adress.ForeColor = ui_colors[7];
                NET_IPv6Adress_V.ForeColor = ui_colors[8];
                // BATTERY
                battery_panel_1.BackColor = ui_colors[6];
                BATTERY_Status.ForeColor = ui_colors[7];
                BATTERY_Status_V.ForeColor = ui_colors[8];
                BATTERY_Model.ForeColor = ui_colors[7];
                BATTERY_Model_V.ForeColor = ui_colors[8];
                BATTERY_Name.ForeColor = ui_colors[7];
                BATTERY_Name_V.ForeColor = ui_colors[8];
                BATTERY_Voltage.ForeColor = ui_colors[7];
                BATTERY_Voltage_V.ForeColor = ui_colors[8];
                BATTERY_Type.ForeColor = ui_colors[7];
                BATTERY_Type_V.ForeColor = ui_colors[8];
                // INSTALLED DRIVERS
                osd_panel_1.BackColor = ui_colors[6];
                OSD_TextBox.BackColor = ui_colors[11];
                OSD_TextBox.ForeColor = ui_colors[12];
                OSD_TYSS.ForeColor = ui_colors[7];
                OSD_TYSS_V.ForeColor = ui_colors[8];
                OSD_SearchDriverLabel.ForeColor = ui_colors[7];
                OSD_DataMainTable.BackgroundColor = ui_colors[13];
                OSD_DataMainTable.GridColor = ui_colors[15];
                OSD_DataMainTable.DefaultCellStyle.BackColor = ui_colors[13];
                OSD_DataMainTable.DefaultCellStyle.ForeColor = ui_colors[14];
                OSD_DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = ui_colors[16];
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = ui_colors[17];
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = ui_colors[17];
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = ui_colors[18];
                OSD_DataMainTable.DefaultCellStyle.SelectionBackColor = ui_colors[17];
                OSD_DataMainTable.DefaultCellStyle.SelectionForeColor = ui_colors[18];
                // SERVICES
                service_panel_1.BackColor = ui_colors[6];
                SERVICE_SearchTextBox.BackColor = ui_colors[11];
                SERVICE_SearchTextBox.ForeColor = ui_colors[12];
                SERVICE_TYS.ForeColor = ui_colors[7];
                SERVICE_TYS_V.ForeColor = ui_colors[8];
                SERVICE_SearchLabel.ForeColor = ui_colors[7];
                SERVICE_DataMainTable.BackgroundColor = ui_colors[13];
                SERVICE_DataMainTable.GridColor = ui_colors[15];
                SERVICE_DataMainTable.DefaultCellStyle.BackColor = ui_colors[13];
                SERVICE_DataMainTable.DefaultCellStyle.ForeColor = ui_colors[14];
                SERVICE_DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = ui_colors[16];
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = ui_colors[17];
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = ui_colors[17];
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = ui_colors[18];
                SERVICE_DataMainTable.DefaultCellStyle.SelectionBackColor = ui_colors[17];
                SERVICE_DataMainTable.DefaultCellStyle.SelectionForeColor = ui_colors[18];
                // ROTATE MENU
                if (menu_btns == 1){
                    OS_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 2){
                    MB_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 3){
                    CPU_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 4){
                    RAM_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 5){
                    GPU_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 6){
                    DISK_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 7){
                    NET_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 8){
                    BATTERY_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 9){
                    OSD_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 10){
                    SERVICES_RotateBtn.BackColor = ui_colors[19];
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
            PrintEngineList.Add(OS_SystemUser.Text + " " + OS_SystemUser_V.Text);
            PrintEngineList.Add(OS_ComputerName.Text + " " + OS_ComputerName_V.Text);
            PrintEngineList.Add(OS_SystemModel.Text + " " + OS_SystemModel_V.Text);
            PrintEngineList.Add(OS_SavedUser.Text + " " + OS_SavedUser_V.Text);
            PrintEngineList.Add(OS_Name.Text + " " + OS_Name_V.Text);
            PrintEngineList.Add(OS_Manufacturer.Text + " " + OS_Manufacturer_V.Text);
            PrintEngineList.Add(OS_SystemVersion.Text + " " + OS_SystemVersion_V.Text);
            PrintEngineList.Add(OS_Build.Text + " " + OS_Build_V.Text);
            PrintEngineList.Add(OS_SystemArchitectural.Text + " " + OS_SystemArchitectural_V.Text);
            PrintEngineList.Add(OS_Family.Text + " " + OS_Family_V.Text);
            PrintEngineList.Add(OS_Serial.Text + " " + OS_Serial_V.Text);
            PrintEngineList.Add(OS_Country.Text + " " + OS_Country_V.Text);
            PrintEngineList.Add(OS_CharacterSet.Text + " " + OS_CharacterSet_V.Text);
            PrintEngineList.Add(OS_EncryptionType.Text + " " + OS_EncryptionType_V.Text);
            PrintEngineList.Add(OS_SystemRootIndex.Text + " " + OS_SystemRootIndex_V.Text);
            PrintEngineList.Add(OS_SystemBuildPart.Text + " " + OS_SystemBuildPart_V.Text);
            PrintEngineList.Add(OS_SystemTime.Text + " " + OS_SystemTime_V.Text);
            PrintEngineList.Add(OS_Install.Text + " " + OS_Install_V.Text);
            PrintEngineList.Add(OS_SystemWorkTime.Text + " " + OS_SystemWorkTime_V.Text);
            PrintEngineList.Add(OS_LastBootTime.Text + " " + OS_LastBootTime_V.Text);
            PrintEngineList.Add(OS_PortableOS.Text + " " + OS_PortableOS_V.Text);
            PrintEngineList.Add(OS_MouseWheelStatus.Text + " " + OS_MouseWheelStatus_V.Text);
            PrintEngineList.Add(OS_ScrollLockStatus.Text + " " + OS_ScrollLockStatus_V.Text);
            PrintEngineList.Add(OS_NumLockStatus.Text + " " + OS_NumLockStatus_V.Text);
            PrintEngineList.Add(OS_CapsLockStatus.Text + " " + OS_CapsLockStatus_V.Text);
            PrintEngineList.Add(OS_BootPartition.Text + " " + OS_BootPartition_V.Text);
            PrintEngineList.Add(OS_SystemPartition.Text + " " + OS_SystemPartition_V.Text);
            PrintEngineList.Add(OS_Wallpaper.Text + " " + OS_Wallpaper_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            // MOTHERBOARD
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(MB_MotherBoardName.Text + " " + MB_MotherBoardName_V.Text);
            PrintEngineList.Add(MB_MotherBoardMan.Text + " " + MB_MotherBoardMan_V.Text);
            PrintEngineList.Add(MB_MotherBoardSerial.Text + " " + MB_MotherBoardSerial_V.Text);
            PrintEngineList.Add(MB_Chipset.Text + " " + MB_Chipset_V.Text);
            PrintEngineList.Add(MB_BiosManufacturer.Text + " " + MB_BiosManufacturer_V.Text);
            PrintEngineList.Add(MB_BiosDate.Text + " " + MB_BiosDate_V.Text);
            PrintEngineList.Add(MB_BiosVersion.Text + " " + MB_BiosVersion_V.Text);
            PrintEngineList.Add(MB_SmBiosVersion.Text + " " + MB_SmBiosVersion_V.Text);
            PrintEngineList.Add(MB_Model.Text + " " + MB_Model_V.Text);
            PrintEngineList.Add(MB_PrimaryBusType.Text + " " + MB_PrimaryBusType_V.Text);
            PrintEngineList.Add(MB_BiosMajorMinor.Text + " " + MB_BiosMajorMinor_V.Text);
            PrintEngineList.Add(MB_SMBiosMajorMinor.Text + " " + MB_SMBiosMajorMinor_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            // CPU
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(CPU_Name.Text + " " + CPU_Name_V.Text);
            PrintEngineList.Add(CPU_Manufacturer.Text + " " + CPU_Manufacturer_V.Text);
            PrintEngineList.Add(CPU_Architectural.Text + " " + CPU_Architectural_V.Text);
            PrintEngineList.Add(CPU_NormalSpeed.Text + " " + CPU_NormalSpeed_V.Text);
            PrintEngineList.Add(CPU_DefaultSpeed.Text + " " + CPU_DefaultSpeed_V.Text);
            PrintEngineList.Add(CPU_L1.Text + " " + CPU_L1_V.Text);
            PrintEngineList.Add(CPU_L2.Text + " " + CPU_L2_V.Text);
            PrintEngineList.Add(CPU_L3.Text + " " + CPU_L3_V.Text);
            PrintEngineList.Add(CPU_CoreCount.Text + " " + CPU_CoreCount_V.Text);
            PrintEngineList.Add(CPU_LogicalCore.Text + " " + CPU_LogicalCore_V.Text);
            PrintEngineList.Add(CPU_SocketDefinition.Text + " " + CPU_SocketDefinition_V.Text);
            PrintEngineList.Add(CPU_Family.Text + " " + CPU_Family_V.Text);
            PrintEngineList.Add(CPU_Virtualization.Text + " " + CPU_Virtualization_V.Text);
            PrintEngineList.Add(CPU_VMMonitorExtension.Text + " " + CPU_VMMonitorExtension_V.Text);
            PrintEngineList.Add(CPU_SerialName.Text + " " + CPU_SerialName_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            // RAM
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(RAM_TotalRAM.Text + " " + RAM_TotalRAM_V.Text);
            PrintEngineList.Add(RAM_UsageRAMCount.Text + " " + RAM_UsageRAMCount_V.Text);
            PrintEngineList.Add(RAM_EmptyRamCount.Text + " " + RAM_EmptyRamCount_V.Text);
            PrintEngineList.Add(RAM_TotalVirtualRam.Text + " " + RAM_TotalVirtualRam_V.Text);
            PrintEngineList.Add(RAM_EmptyVirtualRam.Text + " " + RAM_EmptyVirtualRam_V.Text);
            PrintEngineList.Add(RAM_SlotStatus.Text + " " + RAM_SlotStatus_V.Text + Environment.NewLine);
            try{
                int ram_slot = RAM_SelectList.Items.Count;
                for (int rs = 1; rs <= ram_slot; rs++){
                    RAM_SelectList.SelectedIndex = rs - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_7").Trim())) + " #" + rs + Environment.NewLine);
                    PrintEngineList.Add(RAM_Amount.Text + " " + RAM_Amount_V.Text);
                    PrintEngineList.Add(RAM_Type.Text + " " + RAM_Type_V.Text);
                    PrintEngineList.Add(RAM_Frequency.Text + " " + RAM_Frequency_V.Text);
                    PrintEngineList.Add(RAM_Volt.Text + " " + RAM_Volt_V.Text);
                    PrintEngineList.Add(RAM_FormFactor.Text + " " + RAM_FormFactor_V.Text);
                    PrintEngineList.Add(RAM_Serial.Text + " " + RAM_Serial_V.Text);
                    PrintEngineList.Add(RAM_Manufacturer.Text + " " + RAM_Manufacturer_V.Text);
                    PrintEngineList.Add(RAM_BankLabel.Text + " " + RAM_BankLabel_V.Text);
                    PrintEngineList.Add(RAM_DataWidth.Text + " " + RAM_DataWidth_V.Text);
                    PrintEngineList.Add(RAM_BellekType.Text + " " + RAM_BellekType_V.Text);
                    PrintEngineList.Add(RAM_PartNumber.Text + " " + RAM_PartNumber_V.Text + Environment.NewLine);
                }
                RAM_SelectList.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // GPU
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()))} ------->" + Environment.NewLine);
            try{
                int gpu_amount = GPU_Select.Items.Count;
                for (int gpu_render = 1; gpu_render <= gpu_amount; gpu_render++){
                    GPU_Select.SelectedIndex = gpu_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_17").Trim())) + " #" + gpu_render + Environment.NewLine);
                    PrintEngineList.Add(GPU_Name.Text + " " + GPU_Select.SelectedItem.ToString());
                    PrintEngineList.Add(GPU_Manufacturer.Text + " " + GPU_Manufacturer_V.Text);
                    PrintEngineList.Add(GPU_Version.Text + " " + GPU_Version_V.Text);
                    PrintEngineList.Add(GPU_DriverDate.Text + " " + GPU_DriverDate_V.Text);
                    PrintEngineList.Add(GPU_Status.Text + " " + GPU_Status_V.Text);
                    PrintEngineList.Add(GPU_DacType.Text + " " + GPU_DacType_V.Text);
                    PrintEngineList.Add(GPU_GraphicDriversName.Text + " " + GPU_GraphicDriversName_V.Text);
                    PrintEngineList.Add(GPU_InfFileName.Text + " " + GPU_InfFileName_V.Text);
                    PrintEngineList.Add(GPU_INFSectionFile.Text + " " + GPU_INFSectionFile_V.Text + Environment.NewLine);
                }
                GPU_Select.SelectedIndex = 0;
            }catch (Exception){ }
            try{
                int screen_amount = GPU_MonitorSelectList.Items.Count;
                for (int sa = 1; sa <= screen_amount; sa++){
                    GPU_MonitorSelectList.SelectedIndex = sa - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_18").Trim())) + " #" + sa + Environment.NewLine);
                    PrintEngineList.Add(GPU_MonitorBounds.Text + " " + GPU_MonitorBounds_V.Text);
                    PrintEngineList.Add(GPU_MonitorWorking.Text + " " + GPU_MonitorWorking_V.Text);
                    PrintEngineList.Add(GPU_MonitorResLabel.Text + " " + GPU_MonitorResLabel_V.Text);
                    PrintEngineList.Add(GPU_MonitorVirtualRes.Text + " " + GPU_MonitorVirtualRes_V.Text);
                    PrintEngineList.Add(GPU_ScreenRefreshRate.Text + " " + GPU_ScreenRefreshRate_V.Text);
                    PrintEngineList.Add(GPU_MonitorPrimary.Text + " " + GPU_MonitorPrimary_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
                GPU_MonitorSelectList.SelectedIndex = 0;
            }catch (Exception){ }
            // STORAGE
        
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()))} ------->" + Environment.NewLine);
            try{
                int disk_amount = DISK_SelectBox.Items.Count;
                for (int disk_render = 1; disk_render <= disk_amount; disk_render++){
                    DISK_SelectBox.SelectedIndex = disk_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_21").Trim())) + " #" + disk_render + Environment.NewLine);
                    PrintEngineList.Add(DISK_Name.Text + " " + DISK_SelectBox.SelectedItem.ToString());
                    PrintEngineList.Add(DISK_Model.Text + " " + DISK_Model_V.Text);
                    PrintEngineList.Add(DISK_Man.Text + " " + DISK_Man_V.Text);
                    PrintEngineList.Add(DISK_VolumeID.Text + " " + DISK_VolumeID_V.Text);
                    PrintEngineList.Add(DISK_VolumeName.Text + " " + DISK_VolumeName_V.Text);
                    PrintEngineList.Add(DISK_FirmWare.Text + " " + DISK_FirmWare_V.Text);
                    PrintEngineList.Add(DISK_Seri.Text + " " + DISK_Seri_V.Text);
                    PrintEngineList.Add(DISK_Size.Text + " " + DISK_Size_V.Text);
                    PrintEngineList.Add(DISK_FreeSpace.Text + " " + DISK_FreeSpace_V.Text);
                    PrintEngineList.Add(DISK_PartitionLayout.Text + " " + DISK_PartitionLayout_V.Text);
                    PrintEngineList.Add(DISK_FileSystem.Text + " " + DISK_FileSystem_V.Text);
                    PrintEngineList.Add(DISK_Type.Text + " " + DISK_Type_V.Text);
                    PrintEngineList.Add(DISK_DriveType.Text + " " + DISK_DriveType_V.Text);
                    PrintEngineList.Add(DISK_InterFace.Text + " " + DISK_InterFace_V.Text);
                    PrintEngineList.Add(DISK_Partition.Text + " " + DISK_Partition_V.Text);
                    PrintEngineList.Add(DISK_UniqueID.Text + " " + DISK_UniqueID_V.Text);
                    PrintEngineList.Add(DISK_Location.Text + " " + DISK_Location_V.Text);
                    PrintEngineList.Add(DISK_Healt.Text + " " + DISK_Healt_V.Text);
                    PrintEngineList.Add(DISK_BootPartition.Text + " " + DISK_BootPartition_V.Text);
                    PrintEngineList.Add(DISK_BootableStatus.Text + " " + DISK_BootableStatus_V.Text + Environment.NewLine);
                }
                DISK_SelectBox.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // NETWORK
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()))} ------->" + Environment.NewLine);
            try{
                int net_amount = NET_ListNetwork.Items.Count;
                for (int net_render = 1; net_render <= net_amount; net_render++){
                    NET_ListNetwork.SelectedIndex = net_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_13").Trim())) + " #" + net_render + Environment.NewLine);
                    PrintEngineList.Add(NET_ConnType.Text + " " + NET_ListNetwork.SelectedItem.ToString());
                    PrintEngineList.Add(NET_MacAdress.Text + " " + NET_MacAdress_V.Text);
                    PrintEngineList.Add(NET_NetMan.Text + " " + NET_NetMan_V.Text);
                    PrintEngineList.Add(NET_ServiceName.Text + " " + NET_ServiceName_V.Text);
                    PrintEngineList.Add(NET_AdapterType.Text + " " + NET_AdapterType_V.Text);
                    PrintEngineList.Add(NET_Guid.Text + " " + NET_Guid_V.Text);
                    PrintEngineList.Add(NET_ConnectionType.Text + " " + NET_ConnectionType_V.Text);
                    PrintEngineList.Add(NET_Dhcp_status.Text + " " + NET_Dhcp_status_V.Text);
                    PrintEngineList.Add(NET_Dhcp_server.Text + " " + NET_Dhcp_server_V.Text);
                    PrintEngineList.Add(NET_LocalConSpeed.Text + " " + NET_LocalConSpeed_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add(NET_IPv4Adress.Text + " " + NET_IPv4Adress_V.Text);
                PrintEngineList.Add(NET_IPv6Adress.Text + " " + NET_IPv6Adress_V.Text + Environment.NewLine);
                NET_ListNetwork.SelectedIndex = 1;
            }catch (Exception){ }
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // BATTERY
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()))} ------->" + Environment.NewLine);
                PrintEngineList.Add(BATTERY_Status.Text + " " + BATTERY_Status_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            }else{
                PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()))} ------->" + Environment.NewLine);
                PrintEngineList.Add(BATTERY_Status.Text + " " + BATTERY_Status_V.Text);
                PrintEngineList.Add(BATTERY_Model.Text + " " + BATTERY_Model_V.Text);
                PrintEngineList.Add(BATTERY_Name.Text + " " + BATTERY_Name_V.Text);
                PrintEngineList.Add(BATTERY_Voltage.Text + " " + BATTERY_Voltage_V.Text);
                PrintEngineList.Add(BATTERY_Type.Text + " " + BATTERY_Type_V.Text + Environment.NewLine + Environment.NewLine + "------------------------------------------------------------" + Environment.NewLine);
            }
            // OSD
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_3").Trim())) + Environment.NewLine);
            try{
                for (int i = 0; i < OSD_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add(OSD_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[4].Value.ToString() + Environment.NewLine + "---------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_8").Trim())) + " " + OSD_TYSS_V.Text + Environment.NewLine);
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // SERVICES
            PrintEngineList.Add($"<------- {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()))} ------->" + Environment.NewLine);
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_4").Trim())) + Environment.NewLine);
            try{
                for (int i = 0; i < SERVICE_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add(SERVICE_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[4].Value.ToString() + Environment.NewLine + "---------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_8").Trim())) + " " + OSD_TYSS_V.Text + Environment.NewLine);
            PrintEngineList.Add("------------------------------------------------------------" + Environment.NewLine);
            // FOOTER
            PrintEngineList.Add(Application.ProductName + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_5").Trim())) + " " + Application.ProductVersion.Substring(0, 4) + " - 64 Bit");
            PrintEngineList.Add("(C) 2023 Türkay Software.");
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "or_6").Trim())) + " " + glow_github);
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
        // PING TEST TOOL
        // ======================================================================================================
        private void pingTestToolToolStripMenuItem_Click(object sender, EventArgs e){ GlowPingTestTool glow_ping_test_tool = new GlowPingTestTool(); glow_ping_test_tool.Show(); }
        // ======================================================================================================
        // GLOW DONATE 
        private void supportToolStripMenuItem_Click(object sender, EventArgs e){ GlowDonate glow_donate = new GlowDonate(); glow_donate.ShowDialog(); }
        // ======================================================================================================
        // GLOW GITHUB PAGE
        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e){ Process.Start(glow_github); }
        // ======================================================================================================
        // GLOW EXIT
        private void glow_exit(){ loop_status = false; Application.Exit(); }
        private void Glow_FormClosing(object sender, FormClosingEventArgs e){ glow_exit(); }
    }
}