namespace DTSWindowsForm.UI.FattureWeb
{
    partial class FattureWebForm
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
            panel5 = new Panel();
            panel3 = new Panel();
            gridFaturas = new DataGridView();
            panel4 = new Panel();
            txtQtdFaturasFiltradas = new TextBox();
            label1 = new Label();
            txtQtdFaturas = new TextBox();
            lblFaturas = new Label();
            panel2 = new Panel();
            panel6 = new Panel();
            grbFiltros = new GroupBox();
            label4 = new Label();
            txbInstalacao = new TextBox();
            label3 = new Label();
            label5 = new Label();
            lblMesRef = new Label();
            txbMesRef = new TextBox();
            lblDistribuidora = new Label();
            txbDistribuidora = new TextBox();
            chbFaturasDuplicadas = new CheckBox();
            btnLimparFiltros = new Button();
            btnFiltrar = new Button();
            lblInstalacao = new Label();
            btnDownloadCsv = new Button();
            btnBuscarFaturas = new Button();
            txtSenha = new TextBox();
            lblSenha = new Label();
            txtUsuario = new TextBox();
            lblUsuario = new Label();
            faturasViewDtoBindingSource1 = new BindingSource(components);
            faturasViewDtoBindingSource = new BindingSource(components);
            conteudoBindingSource = new BindingSource(components);
            bcwCarregaDados = new System.ComponentModel.BackgroundWorker();
            panel7 = new Panel();
            tabControl1 = new TabControl();
            tpgFaturas = new TabPage();
            panel8 = new Panel();
            tpgCobrancas = new TabPage();
            label2 = new Label();
            txbDescricaoProdutos = new TextBox();
            lblDescricaoProdutos = new Label();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridFaturas).BeginInit();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel6.SuspendLayout();
            grbFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)faturasViewDtoBindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)faturasViewDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)conteudoBindingSource).BeginInit();
            panel7.SuspendLayout();
            tabControl1.SuspendLayout();
            tpgFaturas.SuspendLayout();
            panel8.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(843, 645);
            panel1.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.Controls.Add(panel3);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 221);
            panel5.Name = "panel5";
            panel5.Size = new Size(843, 388);
            panel5.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.AutoScroll = true;
            panel3.Controls.Add(gridFaturas);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(843, 388);
            panel3.TabIndex = 1;
            // 
            // gridFaturas
            // 
            gridFaturas.AllowUserToOrderColumns = true;
            gridFaturas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridFaturas.Dock = DockStyle.Fill;
            gridFaturas.EditMode = DataGridViewEditMode.EditOnEnter;
            gridFaturas.Location = new Point(0, 0);
            gridFaturas.Name = "gridFaturas";
            gridFaturas.RowTemplate.ReadOnly = true;
            gridFaturas.SelectionMode = DataGridViewSelectionMode.CellSelect;
            gridFaturas.Size = new Size(843, 388);
            gridFaturas.TabIndex = 0;
            gridFaturas.DataSourceChanged += gridFaturas_DataSourceChanged;
            // 
            // panel4
            // 
            panel4.Controls.Add(txtQtdFaturasFiltradas);
            panel4.Controls.Add(label1);
            panel4.Controls.Add(txtQtdFaturas);
            panel4.Controls.Add(lblFaturas);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 609);
            panel4.Name = "panel4";
            panel4.Size = new Size(843, 36);
            panel4.TabIndex = 2;
            // 
            // txtQtdFaturasFiltradas
            // 
            txtQtdFaturasFiltradas.Enabled = false;
            txtQtdFaturasFiltradas.Location = new Point(195, 7);
            txtQtdFaturasFiltradas.Name = "txtQtdFaturasFiltradas";
            txtQtdFaturasFiltradas.Size = new Size(75, 23);
            txtQtdFaturasFiltradas.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(132, 10);
            label1.Name = "label1";
            label1.Size = new Size(57, 15);
            label1.TabIndex = 8;
            label1.Text = "Filtrados:";
            // 
            // txtQtdFaturas
            // 
            txtQtdFaturas.Enabled = false;
            txtQtdFaturas.Location = new Point(51, 7);
            txtQtdFaturas.Name = "txtQtdFaturas";
            txtQtdFaturas.Size = new Size(75, 23);
            txtQtdFaturas.TabIndex = 7;
            // 
            // lblFaturas
            // 
            lblFaturas.AutoSize = true;
            lblFaturas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFaturas.Location = new Point(8, 10);
            lblFaturas.Name = "lblFaturas";
            lblFaturas.Size = new Size(37, 15);
            lblFaturas.TabIndex = 2;
            lblFaturas.Text = "Total:";
            // 
            // panel2
            // 
            panel2.Controls.Add(panel6);
            panel2.Controls.Add(btnBuscarFaturas);
            panel2.Controls.Add(txtSenha);
            panel2.Controls.Add(lblSenha);
            panel2.Controls.Add(txtUsuario);
            panel2.Controls.Add(lblUsuario);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(843, 221);
            panel2.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.AutoScroll = true;
            panel6.Controls.Add(grbFiltros);
            panel6.Dock = DockStyle.Bottom;
            panel6.Location = new Point(0, 52);
            panel6.Name = "panel6";
            panel6.Size = new Size(843, 169);
            panel6.TabIndex = 13;
            // 
            // grbFiltros
            // 
            grbFiltros.Controls.Add(label2);
            grbFiltros.Controls.Add(txbDescricaoProdutos);
            grbFiltros.Controls.Add(lblDescricaoProdutos);
            grbFiltros.Controls.Add(label4);
            grbFiltros.Controls.Add(txbInstalacao);
            grbFiltros.Controls.Add(label3);
            grbFiltros.Controls.Add(label5);
            grbFiltros.Controls.Add(lblMesRef);
            grbFiltros.Controls.Add(txbMesRef);
            grbFiltros.Controls.Add(lblDistribuidora);
            grbFiltros.Controls.Add(txbDistribuidora);
            grbFiltros.Controls.Add(chbFaturasDuplicadas);
            grbFiltros.Controls.Add(btnLimparFiltros);
            grbFiltros.Controls.Add(btnFiltrar);
            grbFiltros.Controls.Add(lblInstalacao);
            grbFiltros.Controls.Add(btnDownloadCsv);
            grbFiltros.Dock = DockStyle.Fill;
            grbFiltros.ForeColor = SystemColors.ActiveCaptionText;
            grbFiltros.Location = new Point(0, 0);
            grbFiltros.Name = "grbFiltros";
            grbFiltros.Size = new Size(843, 169);
            grbFiltros.TabIndex = 7;
            grbFiltros.TabStop = false;
            grbFiltros.Text = "Filtros";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 7F);
            label4.ForeColor = SystemColors.ControlDarkDark;
            label4.Location = new Point(84, 59);
            label4.Name = "label4";
            label4.Size = new Size(72, 12);
            label4.TabIndex = 24;
            label4.Text = "separado por ';'";
            // 
            // txbInstalacao
            // 
            txbInstalacao.Location = new Point(8, 33);
            txbInstalacao.Name = "txbInstalacao";
            txbInstalacao.Size = new Size(148, 23);
            txbInstalacao.TabIndex = 23;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 7F);
            label3.ForeColor = SystemColors.ControlDarkDark;
            label3.Location = new Point(244, 59);
            label3.Name = "label3";
            label3.Size = new Size(72, 12);
            label3.TabIndex = 22;
            label3.Text = "separado por ';'";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 7F);
            label5.ForeColor = SystemColors.ControlDarkDark;
            label5.Location = new Point(407, 59);
            label5.Name = "label5";
            label5.Size = new Size(72, 12);
            label5.TabIndex = 21;
            label5.Text = "separado por ';'";
            // 
            // lblMesRef
            // 
            lblMesRef.AutoSize = true;
            lblMesRef.Location = new Point(331, 14);
            lblMesRef.Name = "lblMesRef";
            lblMesRef.Size = new Size(100, 15);
            lblMesRef.TabIndex = 17;
            lblMesRef.Text = "Mês de referência";
            // 
            // txbMesRef
            // 
            txbMesRef.Location = new Point(331, 33);
            txbMesRef.Name = "txbMesRef";
            txbMesRef.Size = new Size(148, 23);
            txbMesRef.TabIndex = 16;
            // 
            // lblDistribuidora
            // 
            lblDistribuidora.AutoSize = true;
            lblDistribuidora.Location = new Point(168, 14);
            lblDistribuidora.Name = "lblDistribuidora";
            lblDistribuidora.Size = new Size(75, 15);
            lblDistribuidora.TabIndex = 15;
            lblDistribuidora.Text = "Distribuidora";
            // 
            // txbDistribuidora
            // 
            txbDistribuidora.Location = new Point(168, 33);
            txbDistribuidora.Name = "txbDistribuidora";
            txbDistribuidora.Size = new Size(148, 23);
            txbDistribuidora.TabIndex = 14;
            // 
            // chbFaturasDuplicadas
            // 
            chbFaturasDuplicadas.AutoSize = true;
            chbFaturasDuplicadas.Location = new Point(485, 35);
            chbFaturasDuplicadas.Name = "chbFaturasDuplicadas";
            chbFaturasDuplicadas.Size = new Size(124, 19);
            chbFaturasDuplicadas.TabIndex = 7;
            chbFaturasDuplicadas.Text = "Faturas duplicadas";
            chbFaturasDuplicadas.UseVisualStyleBackColor = true;
            // 
            // btnLimparFiltros
            // 
            btnLimparFiltros.BackColor = Color.PaleGoldenrod;
            btnLimparFiltros.FlatStyle = FlatStyle.Popup;
            btnLimparFiltros.Location = new Point(109, 135);
            btnLimparFiltros.Name = "btnLimparFiltros";
            btnLimparFiltros.Size = new Size(95, 25);
            btnLimparFiltros.TabIndex = 4;
            btnLimparFiltros.Text = "Limpar Filtros";
            btnLimparFiltros.UseVisualStyleBackColor = false;
            btnLimparFiltros.Click += btnLimparFiltros_Click;
            // 
            // btnFiltrar
            // 
            btnFiltrar.BackColor = Color.GreenYellow;
            btnFiltrar.FlatStyle = FlatStyle.Popup;
            btnFiltrar.Location = new Point(8, 135);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(95, 25);
            btnFiltrar.TabIndex = 2;
            btnFiltrar.Text = "Aplicar Filtros";
            btnFiltrar.UseVisualStyleBackColor = false;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // lblInstalacao
            // 
            lblInstalacao.AutoSize = true;
            lblInstalacao.Location = new Point(8, 14);
            lblInstalacao.Name = "lblInstalacao";
            lblInstalacao.Size = new Size(60, 15);
            lblInstalacao.TabIndex = 1;
            lblInstalacao.Text = "Instalação";
            // 
            // btnDownloadCsv
            // 
            btnDownloadCsv.BackColor = Color.DarkOrange;
            btnDownloadCsv.FlatStyle = FlatStyle.Popup;
            btnDownloadCsv.Font = new Font("Segoe UI", 9F);
            btnDownloadCsv.ForeColor = SystemColors.ControlText;
            btnDownloadCsv.Location = new Point(210, 135);
            btnDownloadCsv.Name = "btnDownloadCsv";
            btnDownloadCsv.Size = new Size(95, 25);
            btnDownloadCsv.TabIndex = 6;
            btnDownloadCsv.Text = "🔼 Exportar";
            btnDownloadCsv.UseVisualStyleBackColor = false;
            btnDownloadCsv.Click += btnDownloadCsv_Click;
            // 
            // btnBuscarFaturas
            // 
            btnBuscarFaturas.BackColor = Color.GreenYellow;
            btnBuscarFaturas.FlatStyle = FlatStyle.Popup;
            btnBuscarFaturas.Location = new Point(288, 21);
            btnBuscarFaturas.Name = "btnBuscarFaturas";
            btnBuscarFaturas.Size = new Size(108, 23);
            btnBuscarFaturas.TabIndex = 12;
            btnBuscarFaturas.Text = "\U0001f7e2 Buscar Faturas";
            btnBuscarFaturas.UseVisualStyleBackColor = false;
            btnBuscarFaturas.Click += btnBuscarFaturas_Click;
            // 
            // txtSenha
            // 
            txtSenha.Location = new Point(170, 21);
            txtSenha.Name = "txtSenha";
            txtSenha.PasswordChar = '●';
            txtSenha.Size = new Size(100, 23);
            txtSenha.TabIndex = 11;
            // 
            // lblSenha
            // 
            lblSenha.AutoSize = true;
            lblSenha.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSenha.Location = new Point(170, 6);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(41, 15);
            lblSenha.TabIndex = 10;
            lblSenha.Text = "Senha";
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(16, 21);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(148, 23);
            txtUsuario.TabIndex = 9;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUsuario.Location = new Point(16, 6);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(36, 15);
            lblUsuario.TabIndex = 8;
            lblUsuario.Text = "Email";
            // 
            // bcwCarregaDados
            // 
            bcwCarregaDados.DoWork += bcwCarregaDados_DoWork;
            bcwCarregaDados.RunWorkerCompleted += bcwCarregaDados_RunWorkerCompleted;
            // 
            // panel7
            // 
            panel7.Controls.Add(tabControl1);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 0);
            panel7.Name = "panel7";
            panel7.Size = new Size(857, 679);
            panel7.TabIndex = 1;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tpgFaturas);
            tabControl1.Controls.Add(tpgCobrancas);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(857, 679);
            tabControl1.TabIndex = 1;
            // 
            // tpgFaturas
            // 
            tpgFaturas.Controls.Add(panel8);
            tpgFaturas.Location = new Point(4, 24);
            tpgFaturas.Name = "tpgFaturas";
            tpgFaturas.Padding = new Padding(3);
            tpgFaturas.Size = new Size(849, 651);
            tpgFaturas.TabIndex = 0;
            tpgFaturas.Text = "Faturas";
            tpgFaturas.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            panel8.Controls.Add(panel1);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(3, 3);
            panel8.Name = "panel8";
            panel8.Size = new Size(843, 645);
            panel8.TabIndex = 0;
            // 
            // tpgCobrancas
            // 
            tpgCobrancas.Location = new Point(4, 24);
            tpgCobrancas.Name = "tpgCobrancas";
            tpgCobrancas.Padding = new Padding(3);
            tpgCobrancas.Size = new Size(849, 510);
            tpgCobrancas.TabIndex = 1;
            tpgCobrancas.Text = "Cobrancas";
            tpgCobrancas.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 7F);
            label2.ForeColor = SystemColors.ControlDarkDark;
            label2.Location = new Point(537, 119);
            label2.Name = "label2";
            label2.Size = new Size(72, 12);
            label2.TabIndex = 27;
            label2.Text = "separado por ';'";
            // 
            // txbDescricaoProdutos
            // 
            txbDescricaoProdutos.Location = new Point(8, 93);
            txbDescricaoProdutos.Name = "txbDescricaoProdutos";
            txbDescricaoProdutos.Size = new Size(601, 23);
            txbDescricaoProdutos.TabIndex = 26;
            // 
            // lblDescricaoProdutos
            // 
            lblDescricaoProdutos.AutoSize = true;
            lblDescricaoProdutos.Location = new Point(8, 74);
            lblDescricaoProdutos.Name = "lblDescricaoProdutos";
            lblDescricaoProdutos.Size = new Size(109, 15);
            lblDescricaoProdutos.TabIndex = 25;
            lblDescricaoProdutos.Text = "Descrição Produtos";
            // 
            // FattureWebForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(857, 679);
            Controls.Add(panel7);
            MinimumSize = new Size(873, 718);
            Name = "FattureWebForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DevTools - Share (FattureWeb)";
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridFaturas).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel6.ResumeLayout(false);
            grbFiltros.ResumeLayout(false);
            grbFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)faturasViewDtoBindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)faturasViewDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)conteudoBindingSource).EndInit();
            panel7.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tpgFaturas.ResumeLayout(false);
            panel8.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private Label lblFaturas;
        private DataGridView gridFaturas;
        private BindingSource conteudoBindingSource;
        private System.ComponentModel.BackgroundWorker bcwCarregaDados;
        private Button btnDownloadCsv;
        private TextBox txtQtdFaturas;
        private DataGridViewTextBoxColumn faturaIdDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn instalacaoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn mesReferenciaDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn distribuidoraDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn idInstalacaoDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn dataEmissaoDataGridViewTextBoxColumn;
        private BindingSource faturasViewDtoBindingSource;
        private Panel panel4;
        private Panel panel5;
        private GroupBox grbFiltros;
        private Button btnFiltrar;
        private Label lblInstalacao;
        private TextBox txtFiltroInstalacao;
        private BindingSource faturasViewDtoBindingSource1;
        private Label lblUsuario;
        private TextBox txtSenha;
        private Label lblSenha;
        private TextBox txtUsuario;
        private Button btnBuscarFaturas;
        private Panel panel6;
        private Panel panel7;
        private TabControl tabControl1;
        private TabPage tpgFaturas;
        private Panel panel8;
        private TabPage tpgCobrancas;
        private Button btnLimparFiltros;
        private TextBox txtQtdFaturasFiltradas;
        private Label label1;
        private CheckBox chbFaturasDuplicadas;
        private Label lblDistribuidora;
        private TextBox txbDistribuidora;
        private Label lblMesRef;
        private TextBox txbMesRef;
        private Label label4;
        private TextBox txbInstalacao;
        private Label label3;
        private Label label5;
        private Label label2;
        private TextBox txbDescricaoProdutos;
        private Label lblDescricaoProdutos;
    }
}