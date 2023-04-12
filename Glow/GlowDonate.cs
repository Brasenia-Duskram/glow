using System;
using System.Text;
using System.Windows.Forms;
using static Glow.GlowExternalModules;

namespace Glow{
    public partial class GlowDonate : Form{
        public GlowDonate(){ InitializeComponent(); }
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
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } } catch (Exception){ }
                    DonateBankLogo.BackgroundImage = Properties.Resources.donate_bank_d_1;
                
                }else if (Glow.theme == 2){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } } catch (Exception){ }
                    DonateBankLogo.BackgroundImage = Properties.Resources.donate_bank_w_1;
                }
                BackColor = Glow.ui_colors[5];
                BankLogoPanel.BackColor = Glow.ui_colors[6];
                DonateMiddlePanel.BackColor = Glow.ui_colors[6];
                NameSurnameLabel.ForeColor = Glow.ui_colors[7];
                IBANNoLabel.ForeColor = Glow.ui_colors[7];
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