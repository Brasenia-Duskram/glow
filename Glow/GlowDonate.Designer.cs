namespace Glow
{
    partial class GlowDonate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowDonate));
            this.DonateBankLogo = new System.Windows.Forms.PictureBox();
            this.IBANCopyBtn = new System.Windows.Forms.Button();
            this.NameSurnameLabel = new System.Windows.Forms.Label();
            this.IBANNoLabel = new System.Windows.Forms.Label();
            this.DonateMiddlePanel = new System.Windows.Forms.Panel();
            this.BankLogoPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.DonateBankLogo)).BeginInit();
            this.DonateMiddlePanel.SuspendLayout();
            this.BankLogoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DonateBankLogo
            // 
            this.DonateBankLogo.BackColor = System.Drawing.Color.Transparent;
            this.DonateBankLogo.BackgroundImage = global::Glow.Properties.Resources.donate_bank_d_1;
            this.DonateBankLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DonateBankLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DonateBankLogo.Location = new System.Drawing.Point(10, 10);
            this.DonateBankLogo.Name = "DonateBankLogo";
            this.DonateBankLogo.Size = new System.Drawing.Size(440, 80);
            this.DonateBankLogo.TabIndex = 0;
            this.DonateBankLogo.TabStop = false;
            // 
            // IBANCopyBtn
            // 
            this.IBANCopyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.IBANCopyBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IBANCopyBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.IBANCopyBtn.FlatAppearance.BorderSize = 0;
            this.IBANCopyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IBANCopyBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.IBANCopyBtn.ForeColor = System.Drawing.Color.White;
            this.IBANCopyBtn.Image = global::Glow.Properties.Resources.copy_iban;
            this.IBANCopyBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.IBANCopyBtn.Location = new System.Drawing.Point(0, 201);
            this.IBANCopyBtn.Name = "IBANCopyBtn";
            this.IBANCopyBtn.Size = new System.Drawing.Size(484, 40);
            this.IBANCopyBtn.TabIndex = 1;
            this.IBANCopyBtn.Text = "IBAN Kopyala";
            this.IBANCopyBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IBANCopyBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.IBANCopyBtn.UseVisualStyleBackColor = false;
            this.IBANCopyBtn.Click += new System.EventHandler(this.IBANCopyBtn_Click);
            // 
            // NameSurnameLabel
            // 
            this.NameSurnameLabel.BackColor = System.Drawing.Color.Transparent;
            this.NameSurnameLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NameSurnameLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10.75F, System.Drawing.FontStyle.Bold);
            this.NameSurnameLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NameSurnameLabel.Location = new System.Drawing.Point(0, 0);
            this.NameSurnameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.NameSurnameLabel.Name = "NameSurnameLabel";
            this.NameSurnameLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.NameSurnameLabel.Size = new System.Drawing.Size(460, 32);
            this.NameSurnameLabel.TabIndex = 32;
            this.NameSurnameLabel.Text = "N/A";
            this.NameSurnameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IBANNoLabel
            // 
            this.IBANNoLabel.BackColor = System.Drawing.Color.Transparent;
            this.IBANNoLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.IBANNoLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10.75F, System.Drawing.FontStyle.Bold);
            this.IBANNoLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.IBANNoLabel.Location = new System.Drawing.Point(0, 32);
            this.IBANNoLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.IBANNoLabel.Name = "IBANNoLabel";
            this.IBANNoLabel.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.IBANNoLabel.Size = new System.Drawing.Size(460, 32);
            this.IBANNoLabel.TabIndex = 33;
            this.IBANNoLabel.Text = "N/A";
            this.IBANNoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DonateMiddlePanel
            // 
            this.DonateMiddlePanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.DonateMiddlePanel.Controls.Add(this.NameSurnameLabel);
            this.DonateMiddlePanel.Controls.Add(this.IBANNoLabel);
            this.DonateMiddlePanel.Location = new System.Drawing.Point(12, 124);
            this.DonateMiddlePanel.Name = "DonateMiddlePanel";
            this.DonateMiddlePanel.Size = new System.Drawing.Size(460, 64);
            this.DonateMiddlePanel.TabIndex = 34;
            // 
            // BankLogoPanel
            // 
            this.BankLogoPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BankLogoPanel.Controls.Add(this.DonateBankLogo);
            this.BankLogoPanel.Location = new System.Drawing.Point(12, 12);
            this.BankLogoPanel.Name = "BankLogoPanel";
            this.BankLogoPanel.Padding = new System.Windows.Forms.Padding(10);
            this.BankLogoPanel.Size = new System.Drawing.Size(460, 100);
            this.BankLogoPanel.TabIndex = 35;
            // 
            // GlowDonate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.ClientSize = new System.Drawing.Size(484, 241);
            this.Controls.Add(this.BankLogoPanel);
            this.Controls.Add(this.DonateMiddlePanel);
            this.Controls.Add(this.IBANCopyBtn);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 280);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 280);
            this.Name = "GlowDonate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowDonate";
            this.Load += new System.EventHandler(this.GlowDonate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DonateBankLogo)).EndInit();
            this.DonateMiddlePanel.ResumeLayout(false);
            this.BankLogoPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox DonateBankLogo;
        private System.Windows.Forms.Button IBANCopyBtn;
        internal System.Windows.Forms.Label NameSurnameLabel;
        internal System.Windows.Forms.Label IBANNoLabel;
        private System.Windows.Forms.Panel DonateMiddlePanel;
        private System.Windows.Forms.Panel BankLogoPanel;
    }
}