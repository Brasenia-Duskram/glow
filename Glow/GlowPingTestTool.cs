using System;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using static Glow.GlowExternalModules;

namespace Glow{
    public partial class GlowPingTestTool : Form{
        public GlowPingTestTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // MULTI LANGUAGE
        // ======================================================================================================
        static GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
        // QUADRA VERIABLE
        // ======================================================================================================
        List<int> gfn_ping_count = new List<int>();
        int gfn_total_ping;
        List<int> twitch_ping_count = new List<int>();
        int twitch_total_ping;
        // SERVICES STATUS
        // ======================================================================================================
        bool cloudflare_status = false,
        discord_status = false,
        geforce_now_status = false,
        google_status = false, 
        microsoft_status = false,
        steam_status = false,
        technopat_status = false,
        twitch_status = false,
        twitter_status = false,
        valorant_status = false;
        // IP INFOS
        // ======================================================================================================
        List<string> ip_adress = new List<string>(){
            "1.1.1.1",                      // 0  | CloudFlare
            "www.discord.com",              // 1  | Discord
            "85.29.18.157",                 // 2  | GeForce Now TR - Istanbul
            "85.29.14.164",                 // 3  | GeForce Now TR - Ankara
            "80.84.168.138",                // 4  | GeForce Now EU - West 
            "80.84.167.178",                // 5  | GeForce Now EU - Northwest
            "80.84.166.137",                // 6  | GeForce Now EU - Northeast  
            "80.84.167.175",                // 7  | GeForce Now EU - Central  
            "80.84.169.147",                // 8  | GeForce Now EU - Southwest  
            "80.84.167.136",                // 9  | GeForce Now EU - Southeast  
            "www.google.com",               // 10 | Google
            "www.microsoft.com",            // 11 | Microsoft
            "www.steampowered.com",         // 12 | Steam
            "www.technopat.net",            // 13 | Technopat
            "www.twitch.tv",                // 14 | TwitchTV
            "api.twitch.tv",                // 15 | Twitch API
            "www.twitter.com",              // 16 | Twitter
            "104.160.143.212"               // 17 | Valorant TR
        };
        List<string> services = new List<string>(){
            "CloudFlare",                                                                                                                           // 0  | CloudFlare
            "Discord",                                                                                                                              // 1  | Discord
            "GeForce Now TR - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_4").Trim())),     // 2  | GeForce Now TR - Istanbul
            "GeForce Now TR - Ankara",                                                                                                              // 3  | GeForce Now TR - Ankara
            "GeForce Now EU - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_5").Trim())),     // 4  | GeForce Now EU - West 
            "GeForce Now EU - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_6").Trim())),     // 5  | GeForce Now EU - Northwest
            "GeForce Now EU - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_7").Trim())),     // 6  | GeForce Now EU - Northeast  
            "GeForce Now EU - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_8").Trim())),     // 7  | GeForce Now EU - Central  
            "GeForce Now EU - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_9").Trim())),     // 8  | GeForce Now EU - Southwest  
            "GeForce Now EU - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_10").Trim())),    // 9  | GeForce Now EU - Southeast  
            "Google",                                                                                                                               // 10 | Google
            "Microsoft",                                                                                                                            // 11 | Microsoft
            "Steam",                                                                                                                                // 12 | Steam
            "Technopat",                                                                                                                            // 13 | Technopat
            "Twitch TV",                                                                                                                            // 14 | TwitchTV
            "Twitch API",                                                                                                                           // 15 | Twitch API
            "Twitter",                                                                                                                              // 16 | Twitter
            "Valorant - " +  Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_4").Trim()))           // 17 | Valorant TR
        };
        // PING TEST TOOL LOAD
        // ======================================================================================================
        private void GlowPingTestTool_Load(object sender, EventArgs e){
            // MAIN THEMES
            BackColor = Glow.ui_colors[5];
            if (Glow.theme == 1){
                try{ if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
            }else if (Glow.theme == 2){
                try{ if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
            }
            // LANG
            Text = Application.ProductName + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_1").Trim()));
            StartPingTestBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_2").Trim()));
            ping_test_settings();
            start_ping_test();
        }
        // PING TEST SETTINGS
        // ======================================================================================================
        private void ping_test_settings(){
            try{
                // CLOUDFLARE
                CF_Title.Text = services[0];
                CF_DGV.Columns.Add("x", "x");
                CF_DGV.Columns.Add("x", "x");
                CF_DGV.Columns[1].Width = 75;
                CF_Panel.Width = 369;
                // DISCORD
                DC_Title.Text = services[1];
                DC_DGV.Columns.Add("x", "x");
                DC_DGV.Columns.Add("x", "x");
                DC_DGV.Columns[1].Width = 75;
                DC_Panel.Width = 369;
                // GEFORCE NOW
                GFN_Title.Text = services[3].Replace(" TR - Ankara", string.Empty);
                GFN_DGV.Columns.Add("x", "x");
                GFN_DGV.Columns.Add("x", "x");
                GFN_DGV.Columns[1].Width = 75;
                GFN_Panel.Width = 369;
                // GOOGLE
                G_Title.Text = services[10];
                G_DGV.Columns.Add("x", "x");
                G_DGV.Columns.Add("x", "x");
                G_DGV.Columns[1].Width = 75;
                G_Panel.Width = 369;
                // MICROSOFT
                MS_Title.Text = services[11];
                MS_DGV.Columns.Add("x", "x");
                MS_DGV.Columns.Add("x", "x");
                MS_DGV.Columns[1].Width = 75;
                MS_Panel.Width = 369;
                // STEAM
                Steam_Title.Text = services[12];
                Steam_DGV.Columns.Add("x", "x");
                Steam_DGV.Columns.Add("x", "x");
                Steam_DGV.Columns[1].Width = 75;
                Steam_Panel.Width = 369;
                // TECHNOPAT
                TP_Title.Text = services[13];
                TP_DGV.Columns.Add("x", "x");
                TP_DGV.Columns.Add("x", "x");
                TP_DGV.Columns[1].Width = 75;
                TP_Panel.Width = 369;
                // TWITCH
                TW_Title.Text = services[14];
                TW_DGV.Columns.Add("x", "x");
                TW_DGV.Columns.Add("x", "x");
                TW_DGV.Columns[1].Width = 75;
                TW_Panel.Width = 369;
                // TWITTER
                TWITTER_Title.Text = services[16];
                TWITTER_DGV.Columns.Add("x", "x");
                TWITTER_DGV.Columns.Add("x", "x");
                TWITTER_DGV.Columns[1].Width = 75;
                TWITTER_Panel.Width = 369;
                // VALORANT
                VAL_Title.Text = services[17];
                VAL_DGV.Columns.Add("x", "x");
                VAL_DGV.Columns.Add("x", "x");
                VAL_DGV.Columns[1].Width = 75;
                VAL_Panel.Width = 369;
                // UI COLORS
                // ===================================================================================
                // CLOUDFLARE
                CF_Panel.BackColor = Glow.ui_colors[6];
                CF_Header.BackColor = Glow.ui_colors[6];
                CF_Title.ForeColor = Glow.ui_colors[7];
                CF_DGV.BackgroundColor = Glow.ui_colors[13];
                CF_DGV.GridColor = Glow.ui_colors[15];
                CF_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                CF_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                CF_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                CF_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                CF_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                CF_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                CF_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                CF_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // DISCORD
                DC_Panel.BackColor = Glow.ui_colors[6];
                DC_Header.BackColor = Glow.ui_colors[6];
                DC_Title.ForeColor = Glow.ui_colors[7];
                DC_DGV.BackgroundColor = Glow.ui_colors[13];
                DC_DGV.GridColor = Glow.ui_colors[15];
                DC_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                DC_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                DC_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                DC_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                DC_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                DC_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                DC_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                DC_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // GEFORCE NOW
                GFN_Panel.BackColor = Glow.ui_colors[6];
                GFN_Header.BackColor = Glow.ui_colors[6];
                GFN_Title.ForeColor = Glow.ui_colors[7];
                GFN_DGV.BackgroundColor = Glow.ui_colors[13];
                GFN_DGV.GridColor = Glow.ui_colors[15];
                GFN_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                GFN_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                GFN_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                GFN_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                GFN_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                GFN_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                GFN_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                GFN_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // GOOGLE
                G_Panel.BackColor = Glow.ui_colors[6];
                G_Header.BackColor = Glow.ui_colors[6];
                G_Title.ForeColor = Glow.ui_colors[7];
                G_DGV.BackgroundColor = Glow.ui_colors[13];
                G_DGV.GridColor = Glow.ui_colors[15];
                G_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                G_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                G_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                G_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                G_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                G_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                G_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                G_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // MICROSOFT
                MS_Panel.BackColor = Glow.ui_colors[6];
                MS_Header.BackColor = Glow.ui_colors[6];
                MS_Title.ForeColor = Glow.ui_colors[7];
                MS_DGV.BackgroundColor = Glow.ui_colors[13];
                MS_DGV.GridColor = Glow.ui_colors[15];
                MS_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                MS_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                MS_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                MS_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                MS_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                MS_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                MS_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                MS_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // STEAM
                Steam_Panel.BackColor = Glow.ui_colors[6];
                Steam_Header.BackColor = Glow.ui_colors[6];
                Steam_Title.ForeColor = Glow.ui_colors[7];
                Steam_DGV.BackgroundColor = Glow.ui_colors[13];
                Steam_DGV.GridColor = Glow.ui_colors[15];
                Steam_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                Steam_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                Steam_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                Steam_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                Steam_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                Steam_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                Steam_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                Steam_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // TECHNOPAT
                TP_Panel.BackColor = Glow.ui_colors[6];
                TP_Header.BackColor = Glow.ui_colors[6];
                TP_Title.ForeColor = Glow.ui_colors[7];
                TP_DGV.BackgroundColor = Glow.ui_colors[13];
                TP_DGV.GridColor = Glow.ui_colors[15];
                TP_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                TP_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                TP_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                TP_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                TP_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                TP_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                TP_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                TP_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // TWITCH
                TW_Panel.BackColor = Glow.ui_colors[6];
                TW_Header.BackColor = Glow.ui_colors[6];
                TW_Title.ForeColor = Glow.ui_colors[7];
                TW_DGV.BackgroundColor = Glow.ui_colors[13];
                TW_DGV.GridColor = Glow.ui_colors[15];
                TW_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                TW_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                TW_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                TW_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                TW_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                TW_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                TW_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                TW_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // TWITTER
                TWITTER_Panel.BackColor = Glow.ui_colors[6];
                TWITTER_Header.BackColor = Glow.ui_colors[6];
                TWITTER_Title.ForeColor = Glow.ui_colors[7];
                TWITTER_DGV.BackgroundColor = Glow.ui_colors[13];
                TWITTER_DGV.GridColor = Glow.ui_colors[15];
                TWITTER_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                TWITTER_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                TWITTER_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                TWITTER_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                TWITTER_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                TWITTER_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                TWITTER_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                TWITTER_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // VALORANT
                VAL_Panel.BackColor = Glow.ui_colors[6];
                VAL_Header.BackColor = Glow.ui_colors[6];
                VAL_Title.ForeColor = Glow.ui_colors[7];
                VAL_DGV.BackgroundColor = Glow.ui_colors[13];
                VAL_DGV.GridColor = Glow.ui_colors[15];
                VAL_DGV.DefaultCellStyle.BackColor = Glow.ui_colors[13];
                VAL_DGV.DefaultCellStyle.ForeColor = Glow.ui_colors[14];
                VAL_DGV.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[16];
                VAL_DGV.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[17];
                VAL_DGV.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                VAL_DGV.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[18];
                VAL_DGV.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[17];
                VAL_DGV.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[18];
                // BOTTOM LABEL
                ping_bottom_label.ForeColor = Glow.ui_colors[9];
                // DOUBLE BUFFERED
                typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, MiddlePanel, new object[] { true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, CF_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DC_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, GFN_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, G_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, MS_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, Steam_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, TP_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, TW_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, TWITTER_DGV, new object[]{ true });
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, VAL_DGV, new object[]{ true });
            }catch (Exception){ }
        }
        // START ENGINE
        // ======================================================================================================
        private void start_ping_test(){
            clear_dgvs();
            GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
            try{
                // CLOUDFLARE ASYNC PING SEND
                Task ping_cloudflare = new Task(cloudflare_ping);
                ping_cloudflare.Start();
                // DISCORD ASYNC PING SEND
                Task ping_discord = new Task(discord_ping);
                ping_discord.Start();
                // GEFORCE NOW ASYNC PING SEND
                Task ping_geforce_now = new Task(geforce_now_ping);
                ping_geforce_now.Start();
                // GOOGLE ASYNC PING SEND
                Task ping_google = new Task(google_ping);
                ping_google.Start();
                // MICROSOFT ASYNC PING SEND
                Task ping_microsoft = new Task(microsoft_ping);
                ping_microsoft.Start();
                // STEAM ASYNC PING SEND
                Task ping_steam = new Task(steam_ping);
                ping_steam.Start();
                // TECHNOPAT ASYNC PING SEND
                Task ping_technopat = new Task(technopat_ping);
                ping_technopat.Start();
                // TWITCH ASYNC PING SEND
                Task ping_twitch = new Task(twitch_ping);
                ping_twitch.Start();
                // TWITTER ASYNC PING SEND
                Task ping_twitter = new Task(twitter_ping);
                ping_twitter.Start();
                // VALORANT ASYNC PING SEND
                Task ping_valorant = new Task(valorant_ping);
                ping_valorant.Start();
            }catch (Exception){
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_3").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // ENGINES
        // ======================================================================================================
        // CLOUDFLARE
        private void cloudflare_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[0]);
                    if (ping_reply.Status == IPStatus.Success){
                        CF_DGV.Rows.Add($"{services[0]} ({ip_adress[0]})", $"{ping_reply.RoundtripTime} Ms");
                        CF_DGV.ClearSelection();
                        if (ping_reply.RoundtripTime < 75){
                            CF_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (ping_reply.RoundtripTime > 75 || ping_reply.RoundtripTime < 150){
                            CF_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (ping_reply.RoundtripTime > 150){
                            CF_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    cloudflare_status = true;
                }
            }catch (Exception){ }
        }
        // DISCORD
        private void discord_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[1]);
                    if (ping_reply.Status == IPStatus.Success){
                        DC_DGV.Rows.Add($"{services[1]} ({ip_adress[1]})", $"{ping_reply.RoundtripTime} Ms");
                        DC_DGV.ClearSelection();
                        if (ping_reply.RoundtripTime < 75){
                            DC_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (ping_reply.RoundtripTime > 75 || ping_reply.RoundtripTime < 150){
                            DC_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (ping_reply.RoundtripTime > 150){
                            DC_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    discord_status = true;
                }
            }catch (Exception){ }
        }
        // GeForce Now
        private void geforce_now_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    // GeForce Now TR - Istanbul
                    ping_reply = ping_tool.Send(ip_adress[2]);
                    if (ping_reply.Status == IPStatus.Success){
                        GFN_DGV.Rows.Add($"{services[2]} ({ip_adress[2]})", $"{ping_reply.RoundtripTime} Ms");
                        GFN_DGV.ClearSelection();
                        gfn_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                    }
                    // GeForce Now TR - Ankara
                    ping_reply = ping_tool.Send(ip_adress[3]);
                    if (ping_reply.Status == IPStatus.Success){
                        GFN_DGV.Rows.Add($"{services[3]} ({ip_adress[3]})", $"{ping_reply.RoundtripTime} Ms");
                        GFN_DGV.ClearSelection();
                        gfn_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                    }
                    // GeForce Now EU - West 
                    ping_reply = ping_tool.Send(ip_adress[4]);
                    if (ping_reply.Status == IPStatus.Success){
                        GFN_DGV.Rows.Add($"{services[4]} ({ip_adress[4]})", $"{ping_reply.RoundtripTime} Ms");
                        GFN_DGV.ClearSelection();
                        gfn_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                    }
                    // GeForce Now EU - Northwest
                    ping_reply = ping_tool.Send(ip_adress[5]);
                    if (ping_reply.Status == IPStatus.Success){
                        GFN_DGV.Rows.Add($"{services[5]} ({ip_adress[5]})", $"{ping_reply.RoundtripTime} Ms");
                        GFN_DGV.ClearSelection();
                        gfn_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                    }
                    // GeForce Now EU - Northeast  
                    ping_reply = ping_tool.Send(ip_adress[6]);
                    if (ping_reply.Status == IPStatus.Success){
                        GFN_DGV.Rows.Add($"{services[6]} ({ip_adress[6]})", $"{ping_reply.RoundtripTime} Ms");
                        GFN_DGV.ClearSelection();
                        gfn_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                    }
                    // GeForce Now EU - Central 
                    ping_reply = ping_tool.Send(ip_adress[7]);
                    if (ping_reply.Status == IPStatus.Success){
                        GFN_DGV.Rows.Add($"{services[7]} ({ip_adress[7]})", $"{ping_reply.RoundtripTime} Ms");
                        GFN_DGV.ClearSelection();
                        gfn_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                    }
                    // GeForce Now EU - Southwest 
                    ping_reply = ping_tool.Send(ip_adress[8]);
                    if (ping_reply.Status == IPStatus.Success){
                        GFN_DGV.Rows.Add($"{services[8]} ({ip_adress[8]})", $"{ping_reply.RoundtripTime} Ms");
                        GFN_DGV.ClearSelection();
                        gfn_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                    }
                    // GeForce Now EU - Southeast  
                    ping_reply = ping_tool.Send(ip_adress[9]);
                    if (ping_reply.Status == IPStatus.Success){
                        GFN_DGV.Rows.Add($"{services[9]} ({ip_adress[9]})", $"{ping_reply.RoundtripTime} Ms");
                        GFN_DGV.ClearSelection();
                        gfn_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                        for (int i = 0; i <= gfn_ping_count.Count - 1; i++){
                            gfn_total_ping += gfn_ping_count[i];
                        }
                        int gfn_ping = gfn_total_ping / gfn_ping_count.Count;
                        if (gfn_ping < 75){
                            GFN_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (gfn_ping > 75 || gfn_ping < 150){
                            GFN_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (gfn_ping > 150){
                            GFN_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    geforce_now_status = true;
                }
            }catch (Exception){ }
        }
        // GOOGLE 
        private void google_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[10]);
                    if (ping_reply.Status == IPStatus.Success){
                        G_DGV.Rows.Add($"{services[10]} ({ip_adress[10]})", $"{ping_reply.RoundtripTime} Ms");
                        G_DGV.ClearSelection();
                        if (ping_reply.RoundtripTime < 75){
                            G_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (ping_reply.RoundtripTime > 75 || ping_reply.RoundtripTime < 150){
                            G_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (ping_reply.RoundtripTime > 150){
                            G_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    google_status = true;
                }
            }catch (Exception){ }
        }
        // MICROSOFT 
        private void microsoft_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[11]);
                    if (ping_reply.Status == IPStatus.Success){
                        MS_DGV.Rows.Add($"{services[11]} ({ip_adress[11]})", $"{ping_reply.RoundtripTime} Ms");
                        MS_DGV.ClearSelection();
                        if (ping_reply.RoundtripTime < 75){
                            MS_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (ping_reply.RoundtripTime > 75 || ping_reply.RoundtripTime < 150){
                            MS_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (ping_reply.RoundtripTime > 150){
                            MS_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    microsoft_status = true;
                }
            }catch (Exception){ }
        }
        // STEAM 
        private void steam_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[12]);
                    if (ping_reply.Status == IPStatus.Success){
                        Steam_DGV.Rows.Add($"{services[12]} ({ip_adress[12]})", $"{ping_reply.RoundtripTime} Ms");
                        Steam_DGV.ClearSelection();
                        if (ping_reply.RoundtripTime < 75){
                            Steam_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (ping_reply.RoundtripTime > 75 || ping_reply.RoundtripTime < 150){
                            Steam_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (ping_reply.RoundtripTime > 150){
                            Steam_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    steam_status = true;
                }
            }catch (Exception){ }
        }
        // TECHNOPAT 
        private void technopat_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[13]);
                    if (ping_reply.Status == IPStatus.Success){
                        TP_DGV.Rows.Add($"{services[13]} ({ip_adress[13]})", $"{ping_reply.RoundtripTime} Ms");
                        TP_DGV.ClearSelection();
                        if (ping_reply.RoundtripTime < 75){
                            TP_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (ping_reply.RoundtripTime > 75 || ping_reply.RoundtripTime < 150){
                            TP_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (ping_reply.RoundtripTime > 150){
                            TP_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    technopat_status = true;
                }
            }catch (Exception){ }
        }
        // TWITCH TV 
        private void twitch_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[14]);
                    if (ping_reply.Status == IPStatus.Success){
                        TW_DGV.Rows.Add($"{services[14]} ({ip_adress[14]})", $"{ping_reply.RoundtripTime} Ms");
                        TW_DGV.ClearSelection();
                        twitch_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                    }
                    // Twitch API 
                    ping_reply = ping_tool.Send(ip_adress[15]);
                    if (ping_reply.Status == IPStatus.Success){
                        TW_DGV.Rows.Add($"{services[15]} ({ip_adress[15]})", $"{ping_reply.RoundtripTime} Ms");
                        TW_DGV.ClearSelection();
                        twitch_ping_count.Add(Convert.ToInt32(ping_reply.RoundtripTime));
                        for (int i = 0; i <= twitch_ping_count.Count - 1; i++){
                            twitch_total_ping += twitch_ping_count[i];
                        }
                        int twitch_ping = twitch_total_ping / twitch_ping_count.Count;
                        if (twitch_ping < 75){
                            TW_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (twitch_ping > 75 || twitch_ping < 150){
                            TW_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (twitch_ping > 150){
                            TW_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    twitch_status = true;
                }
            }catch (Exception){ }
        }
        // TWITTER
        private void twitter_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[16]);
                    if (ping_reply.Status == IPStatus.Success){
                        TWITTER_DGV.Rows.Add($"{services[16]} ({ip_adress[16]})", $"{ping_reply.RoundtripTime} Ms");
                        TWITTER_DGV.ClearSelection();
                        if (ping_reply.RoundtripTime < 75){
                            TWITTER_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (ping_reply.RoundtripTime > 75 || ping_reply.RoundtripTime < 150){
                            TWITTER_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (ping_reply.RoundtripTime > 150){
                            TWITTER_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    twitter_status = true;
                }
            }catch (Exception){ }
        }
        // VALORANT
        private void valorant_ping(){
            try{
                using (Ping ping_tool = new Ping()){
                    PingReply ping_reply;
                    ping_reply = ping_tool.Send(ip_adress[17]);
                    if (ping_reply.Status == IPStatus.Success){
                        VAL_DGV.Rows.Add($"{services[17]} ({ip_adress[17]})", $"{ping_reply.RoundtripTime} Ms");
                        VAL_DGV.ClearSelection();
                        if (ping_reply.RoundtripTime < 75){
                            VAL_Status.BackgroundImage = Properties.Resources.ping_s_good;
                        }else if (ping_reply.RoundtripTime > 75 || ping_reply.RoundtripTime < 150){
                            VAL_Status.BackgroundImage = Properties.Resources.ping_s_normal;
                        }else if (ping_reply.RoundtripTime > 150){
                            VAL_Status.BackgroundImage = Properties.Resources.ping_s_bad;
                        }
                    }
                    ping_tool.Dispose();
                    valorant_status = true;
                }
            }catch (Exception){ }
        }
        // START ENGINE BUTTON
        // ======================================================================================================
        private void StartPingTestBtn_Click(object sender, EventArgs e){
            try{
                if (cloudflare_status == true && discord_status == true && geforce_now_status == true && google_status == true &&
                microsoft_status == true && steam_status == true && technopat_status == true && twitch_status == true &&
                twitter_status == true && valorant_status == true){
                    start_ping_test();
                }else{
                    MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PingTestTool", "ping_tt_11").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }catch (Exception){ }
        }
        // CLEAR DATAGRIDVIEW AND LIST
        // ======================================================================================================
        private void clear_dgvs(){
            CF_DGV.Rows.Clear();
            DC_DGV.Rows.Clear();
            GFN_DGV.Rows.Clear();
            G_DGV.Rows.Clear();
            MS_DGV.Rows.Clear();
            Steam_DGV.Rows.Clear();
            TP_DGV.Rows.Clear();
            TW_DGV.Rows.Clear();
            TWITTER_DGV.Rows.Clear();
            VAL_DGV.Rows.Clear();
            // LIST
            gfn_ping_count.Clear();
            twitch_ping_count.Clear();
            // STATUS
            CF_Status.BackgroundImage = null;
            DC_Status.BackgroundImage = null;
            GFN_Status.BackgroundImage = null;
            G_Status.BackgroundImage = null;
            MS_Status.BackgroundImage = null;
            Steam_Status.BackgroundImage = null;
            TP_Status.BackgroundImage = null;
            TW_Status.BackgroundImage = null;
            TWITTER_Status.BackgroundImage = null;
            VAL_Status.BackgroundImage = null;
            // SERVICE STATUS
            cloudflare_status = false;
            discord_status = false;
            geforce_now_status = false;
            google_status = false;
            microsoft_status = false;
            steam_status = false;
            technopat_status = false;
            twitch_status = false;
            twitter_status = false;
            valorant_status = false;
        }
    }
}