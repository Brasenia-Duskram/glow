using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using static Glow.GlowExternalModules;

namespace Glow{
    public partial class GlowDonate : Form{
        public GlowDonate(){ InitializeComponent(); }
        // GET THEME
        // ======================================================================================================
        int theme = Glow.theme;
        Color[] light_theme = {
            Color.FromArgb(235, 235, 235),
            Color.WhiteSmoke,
            Color.Black,
        };
        Color[] dark_theme = {
            Color.FromArgb(53, 53, 61),
            Color.FromArgb(37, 37, 43),
            Color.WhiteSmoke
        };
        // DONATE INFO
        // ======================================================================================================
        private readonly string name_surname = "ERAY TÜRKAY";
        private readonly string iban_no = "TR500001000192907464105001";
        // GLOW DONATE LOAD
        // ======================================================================================================
        private void GlowDonate_Load(object sender, EventArgs e){
            try{
                // LANG
                GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
                Text = Application.ProductName + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Donate", "d_1").Trim()));
                NameSurnameLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Donate", "d_2").Trim())) + " " + " " + name_surname;
                IBANNoLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Donate", "d_3").Trim())) + " " + " " + iban_no;
                IBANCopyBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Donate", "d_4").Trim()));
                // THEME
                if (theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } } catch (Exception){ }
                    DonateBankLogo.BackgroundImage = Properties.Resources.donate_bank_d_1;
                    BackColor = light_theme[0];
                    BankLogoPanel.BackColor = light_theme[1];
                    DonateMiddlePanel.BackColor = light_theme[1];
                    NameSurnameLabel.ForeColor = light_theme[2];
                    IBANNoLabel.ForeColor = light_theme[2];
                }else if (theme == 2){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } } catch (Exception){ }
                    DonateBankLogo.BackgroundImage = Properties.Resources.donate_bank_w_1;
                    BackColor = dark_theme[0];
                    BankLogoPanel.BackColor = dark_theme[1];
                    DonateMiddlePanel.BackColor = dark_theme[1];
                    NameSurnameLabel.ForeColor = dark_theme[2];
                    IBANNoLabel.ForeColor = dark_theme[2];
                }
            }catch (Exception){ }
        }
        // COPY IBAN NO
        // ======================================================================================================
        private void IBANCopyBtn_Click(object sender, EventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
            try{
                Clipboard.SetText(iban_no);
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Donate", "d_5").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }catch (Exception){
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Donate", "d_6").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}