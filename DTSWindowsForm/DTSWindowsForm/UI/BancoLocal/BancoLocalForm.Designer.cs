namespace DTSWindowsForm.UI.BancoLocal
{
    partial class BancoLocalForm
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
            rtbResultado = new RichTextBox();
            btnImportar = new Button();
            panel1 = new Panel();
            panel3 = new Panel();
            panel2 = new Panel();
            btnSearch = new Button();
            txbSearch = new TextBox();
            ckbAtualizacaoAutomatica = new CheckBox();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // rtbResultado
            // 
            rtbResultado.Dock = DockStyle.Fill;
            rtbResultado.Location = new Point(0, 0);
            rtbResultado.Name = "rtbResultado";
            rtbResultado.ReadOnly = true;
            rtbResultado.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbResultado.Size = new Size(784, 500);
            rtbResultado.TabIndex = 0;
            rtbResultado.Text = "";
            // 
            // btnImportar
            // 
            btnImportar.Location = new Point(12, 17);
            btnImportar.Name = "btnImportar";
            btnImportar.Size = new Size(75, 23);
            btnImportar.TabIndex = 4;
            btnImportar.Text = "Importar dados";
            btnImportar.UseVisualStyleBackColor = true;
            btnImportar.Click += btnImportar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(784, 561);
            panel1.TabIndex = 5;
            // 
            // panel3
            // 
            panel3.Controls.Add(rtbResultado);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(784, 500);
            panel3.TabIndex = 6;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnSearch);
            panel2.Controls.Add(txbSearch);
            panel2.Controls.Add(ckbAtualizacaoAutomatica);
            panel2.Controls.Add(btnImportar);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 500);
            panel2.Name = "panel2";
            panel2.Size = new Size(784, 61);
            panel2.TabIndex = 5;
            // 
            // btnSearch
            // 
            btnSearch.BackgroundImage = Properties.Resources.search;
            btnSearch.BackgroundImageLayout = ImageLayout.Zoom;
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(734, 15);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(28, 26);
            btnSearch.TabIndex = 7;
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // txbSearch
            // 
            txbSearch.Location = new Point(316, 17);
            txbSearch.Name = "txbSearch";
            txbSearch.Size = new Size(412, 23);
            txbSearch.TabIndex = 6;
            // 
            // ckbAtualizacaoAutomatica
            // 
            ckbAtualizacaoAutomatica.AutoSize = true;
            ckbAtualizacaoAutomatica.Location = new Point(97, 20);
            ckbAtualizacaoAutomatica.Name = "ckbAtualizacaoAutomatica";
            ckbAtualizacaoAutomatica.Size = new Size(194, 19);
            ckbAtualizacaoAutomatica.TabIndex = 5;
            ckbAtualizacaoAutomatica.Text = "Atualizar log automaticamente?";
            ckbAtualizacaoAutomatica.UseVisualStyleBackColor = true;
            // 
            // BancoLocalForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(panel1);
            MinimumSize = new Size(800, 600);
            Name = "BancoLocalForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DevTools - Share (Banco Local)";
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox rtbResultado;
        private Button btnImportar;
        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private CheckBox ckbAtualizacaoAutomatica;
        private TextBox txbSearch;
        private Button btnSearch;
    }
}