namespace DTSWindowsForm.UI.Dashboard
{
    partial class DashboardForm
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
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            panel3 = new Panel();
            ConsultaGrid = new DataGridView();
            panel2 = new Panel();
            btnExecutar = new Button();
            lblSegundos = new Label();
            txtmReload = new MaskedTextBox();
            lblReload = new Label();
            lblConsulta = new Label();
            rtxConsulta = new RichTextBox();
            TimerReload = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ConsultaGrid).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(788, 561);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(ConsultaGrid);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 181);
            panel3.Name = "panel3";
            panel3.Size = new Size(788, 380);
            panel3.TabIndex = 1;
            // 
            // ConsultaGrid
            // 
            ConsultaGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ConsultaGrid.Dock = DockStyle.Fill;
            ConsultaGrid.Location = new Point(0, 0);
            ConsultaGrid.Name = "ConsultaGrid";
            ConsultaGrid.Size = new Size(788, 380);
            ConsultaGrid.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnExecutar);
            panel2.Controls.Add(lblSegundos);
            panel2.Controls.Add(txtmReload);
            panel2.Controls.Add(lblReload);
            panel2.Controls.Add(lblConsulta);
            panel2.Controls.Add(rtxConsulta);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(788, 181);
            panel2.TabIndex = 0;
            // 
            // btnExecutar
            // 
            btnExecutar.Location = new Point(167, 146);
            btnExecutar.Name = "btnExecutar";
            btnExecutar.Size = new Size(75, 23);
            btnExecutar.TabIndex = 6;
            btnExecutar.Text = "▶️ Executar";
            btnExecutar.UseVisualStyleBackColor = true;
            btnExecutar.Click += btnExecutar_Click;
            // 
            // lblSegundos
            // 
            lblSegundos.AutoSize = true;
            lblSegundos.Location = new Point(102, 154);
            lblSegundos.Name = "lblSegundos";
            lblSegundos.Size = new Size(59, 15);
            lblSegundos.TabIndex = 5;
            lblSegundos.Text = "Segundos";
            // 
            // txtmReload
            // 
            txtmReload.Location = new Point(61, 146);
            txtmReload.Mask = "00000";
            txtmReload.Name = "txtmReload";
            txtmReload.Size = new Size(35, 23);
            txtmReload.TabIndex = 4;
            txtmReload.ValidatingType = typeof(int);
            // 
            // lblReload
            // 
            lblReload.AutoSize = true;
            lblReload.Location = new Point(12, 154);
            lblReload.Name = "lblReload";
            lblReload.Size = new Size(43, 15);
            lblReload.TabIndex = 3;
            lblReload.Text = "Reload";
            // 
            // lblConsulta
            // 
            lblConsulta.AutoSize = true;
            lblConsulta.Location = new Point(12, 8);
            lblConsulta.Name = "lblConsulta";
            lblConsulta.Size = new Size(54, 15);
            lblConsulta.TabIndex = 1;
            lblConsulta.Text = "Consulta";
            // 
            // rtxConsulta
            // 
            rtxConsulta.AcceptsTab = true;
            rtxConsulta.Location = new Point(12, 26);
            rtxConsulta.Name = "rtxConsulta";
            rtxConsulta.Size = new Size(760, 114);
            rtxConsulta.TabIndex = 0;
            rtxConsulta.Text = "";
            // 
            // TimerReload
            // 
            TimerReload.Interval = 1000;
            TimerReload.Tick += Timer_Tick;
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(788, 561);
            Controls.Add(panel1);
            MinimumSize = new Size(800, 600);
            Name = "DashboardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DevTools - Share (Dashboard)";
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ConsultaGrid).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private RichTextBox rtxConsulta;
        private Label lblConsulta;
        private System.Windows.Forms.Timer TimerReload;
        private Label lblSegundos;
        private MaskedTextBox txtmReload;
        private Label lblReload;
        private DataGridView ConsultaGrid;
        private Button btnExecutar;
    }
}