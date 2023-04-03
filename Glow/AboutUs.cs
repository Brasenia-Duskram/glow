using System;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using static Glow.GlowExternalModules;

namespace Glow{
    public partial class AboutUs : Form{
        public AboutUs(){ InitializeComponent(); }
        // GET THEME
        // ======================================================================================================
        int theme = Glow.theme;
        Color[] light_theme = {
            Color.FromArgb(235, 235, 235),
            Color.FromArgb(205, 205, 205),
            Color.White,
            Color.Black
        };
        Color[] dark_theme = {
            Color.FromArgb(53, 53, 61),
            Color.FromArgb(65, 65, 65),
            Color.FromArgb(37, 37, 43),
            Color.WhiteSmoke
        };
        // ABOUT LOAD
        // ======================================================================================================
        private void AboutUs_Load(object sender, EventArgs e){
            try{
                // ADD COLUMN
                AboutUsDataTable.Columns.Add("about_column_1", "x");
                AboutUsDataTable.Columns.Add("about_column_2", "x");
                AboutUsDataTable.Columns[1].Width = 142;
                // LANG
                GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
                Text = Application.ProductName + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("About", "a_1").Trim()));
                string[] about_us_info_1 = { Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("About", "a_2").Trim())), Application.ProductName };
                AboutUsDataTable.Rows.Add(about_us_info_1);
                string[] about_us_info_2 = { Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("About", "a_3").Trim())), Application.CompanyName };
                AboutUsDataTable.Rows.Add(about_us_info_2);
                string[] about_us_info_3 = { Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("About", "a_4").Trim())), Application.ProductVersion.Substring(0, 4) + " - 64 Bit" };
                AboutUsDataTable.Rows.Add(about_us_info_3);
                AboutUsDataTable.ClearSelection();
                // THEME
                if (theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } } catch (Exception){ }
                    BackColor = light_theme[0];
                    AboutUsDataTable.GridColor = light_theme[1];
                    AboutUsDataTable.BackgroundColor = light_theme[2];
                    AboutUsDataTable.DefaultCellStyle.BackColor = light_theme[2];
                    AboutUsDataTable.DefaultCellStyle.SelectionBackColor = light_theme[2];
                    AboutUsDataTable.AlternatingRowsDefaultCellStyle.BackColor = light_theme[2];
                    AboutUsDataTable.DefaultCellStyle.ForeColor = light_theme[3];
                    AboutUsDataTable.DefaultCellStyle.SelectionForeColor = light_theme[3];
                }else if (theme == 2){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } } catch (Exception){ }
                    BackColor = dark_theme[0];
                    AboutUsDataTable.GridColor = dark_theme[1];
                    AboutUsDataTable.BackgroundColor = dark_theme[2];
                    AboutUsDataTable.DefaultCellStyle.BackColor = dark_theme[2];
                    AboutUsDataTable.DefaultCellStyle.SelectionBackColor = dark_theme[2];
                    AboutUsDataTable.AlternatingRowsDefaultCellStyle.BackColor = dark_theme[2];
                    AboutUsDataTable.DefaultCellStyle.ForeColor = dark_theme[3];
                    AboutUsDataTable.DefaultCellStyle.SelectionForeColor = dark_theme[3];
                }
                // DGV DOUBLE BUFFERING
                typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, AboutUsDataTable, new object[]{ true });
            }catch (Exception){ }
        }
    }
}