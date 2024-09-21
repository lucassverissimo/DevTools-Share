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
            pcbLoading = new PictureBox();
            gridFaturas = new DataGridView();
            faturaIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            instalacaoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            mesReferenciaDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            distribuidoraDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            idInstalacaoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataEmissaoDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            faturasViewDtoBindingSource1 = new BindingSource(components);
            panel4 = new Panel();
            txtBaseSelecionada = new TextBox();
            txtQtdFaturas = new TextBox();
            lblFaturas = new Label();
            lblBase = new Label();
            panel2 = new Panel();
            grbFiltros = new GroupBox();
            btnFiltrar = new Button();
            lblFiltroInstalacao = new Label();
            txtFiltroInstalacao = new TextBox();
            btnDownloadCsv = new Button();
            lblSelecionarOutraBase = new Label();
            cmbBasesDisponiveis = new ComboBox();
            faturasViewDtoBindingSource = new BindingSource(components);
            conteudoBindingSource = new BindingSource(components);
            bcwCarregaDados = new System.ComponentModel.BackgroundWorker();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pcbLoading).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridFaturas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)faturasViewDtoBindingSource1).BeginInit();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            grbFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)faturasViewDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)conteudoBindingSource).BeginInit();
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
            panel1.Size = new Size(857, 450);
            panel1.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.Controls.Add(panel3);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 100);
            panel5.Name = "panel5";
            panel5.Size = new Size(857, 314);
            panel5.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.Controls.Add(pcbLoading);
            panel3.Controls.Add(gridFaturas);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(857, 314);
            panel3.TabIndex = 1;
            // 
            // pcbLoading
            // 
            pcbLoading.Image = Properties.Resources.Spinner_1x_0_7s_200px_200px;
            pcbLoading.Location = new Point(658, 152);
            pcbLoading.Name = "pcbLoading";
            pcbLoading.Size = new Size(66, 59);
            pcbLoading.SizeMode = PictureBoxSizeMode.CenterImage;
            pcbLoading.TabIndex = 1;
            pcbLoading.TabStop = false;
            // 
            // gridFaturas
            // 
            gridFaturas.AllowUserToOrderColumns = true;
            gridFaturas.AutoGenerateColumns = false;
            gridFaturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridFaturas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridFaturas.Columns.AddRange(new DataGridViewColumn[] { faturaIdDataGridViewTextBoxColumn, instalacaoDataGridViewTextBoxColumn, mesReferenciaDataGridViewTextBoxColumn, distribuidoraDataGridViewTextBoxColumn, idInstalacaoDataGridViewTextBoxColumn, dataEmissaoDataGridViewTextBoxColumn });
            gridFaturas.DataSource = faturasViewDtoBindingSource1;
            gridFaturas.Dock = DockStyle.Fill;
            gridFaturas.Location = new Point(0, 0);
            gridFaturas.Name = "gridFaturas";
            gridFaturas.ReadOnly = true;
            gridFaturas.RowTemplate.ReadOnly = true;
            gridFaturas.SelectionMode = DataGridViewSelectionMode.CellSelect;
            gridFaturas.Size = new Size(857, 314);
            gridFaturas.TabIndex = 0;
            // 
            // faturaIdDataGridViewTextBoxColumn
            // 
            faturaIdDataGridViewTextBoxColumn.DataPropertyName = "FaturaId";
            faturaIdDataGridViewTextBoxColumn.HeaderText = "FaturaId";
            faturaIdDataGridViewTextBoxColumn.Name = "faturaIdDataGridViewTextBoxColumn";
            faturaIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // instalacaoDataGridViewTextBoxColumn
            // 
            instalacaoDataGridViewTextBoxColumn.DataPropertyName = "Instalacao";
            instalacaoDataGridViewTextBoxColumn.HeaderText = "Instalacao";
            instalacaoDataGridViewTextBoxColumn.Name = "instalacaoDataGridViewTextBoxColumn";
            instalacaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mesReferenciaDataGridViewTextBoxColumn
            // 
            mesReferenciaDataGridViewTextBoxColumn.DataPropertyName = "MesReferencia";
            mesReferenciaDataGridViewTextBoxColumn.HeaderText = "MesReferencia";
            mesReferenciaDataGridViewTextBoxColumn.Name = "mesReferenciaDataGridViewTextBoxColumn";
            mesReferenciaDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // distribuidoraDataGridViewTextBoxColumn
            // 
            distribuidoraDataGridViewTextBoxColumn.DataPropertyName = "Distribuidora";
            distribuidoraDataGridViewTextBoxColumn.HeaderText = "Distribuidora";
            distribuidoraDataGridViewTextBoxColumn.Name = "distribuidoraDataGridViewTextBoxColumn";
            distribuidoraDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // idInstalacaoDataGridViewTextBoxColumn
            // 
            idInstalacaoDataGridViewTextBoxColumn.DataPropertyName = "IdInstalacao";
            idInstalacaoDataGridViewTextBoxColumn.HeaderText = "IdInstalacao";
            idInstalacaoDataGridViewTextBoxColumn.Name = "idInstalacaoDataGridViewTextBoxColumn";
            idInstalacaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataEmissaoDataGridViewTextBoxColumn
            // 
            dataEmissaoDataGridViewTextBoxColumn.DataPropertyName = "DataEmissao";
            dataEmissaoDataGridViewTextBoxColumn.HeaderText = "DataEmissao";
            dataEmissaoDataGridViewTextBoxColumn.Name = "dataEmissaoDataGridViewTextBoxColumn";
            dataEmissaoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // faturasViewDtoBindingSource1
            // 
            faturasViewDtoBindingSource1.DataSource = typeof(dtos.FaturasViewDto);
            // 
            // panel4
            // 
            panel4.Controls.Add(txtBaseSelecionada);
            panel4.Controls.Add(txtQtdFaturas);
            panel4.Controls.Add(lblFaturas);
            panel4.Controls.Add(lblBase);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 414);
            panel4.Name = "panel4";
            panel4.Size = new Size(857, 36);
            panel4.TabIndex = 2;
            // 
            // txtBaseSelecionada
            // 
            txtBaseSelecionada.Enabled = false;
            txtBaseSelecionada.Location = new Point(44, 8);
            txtBaseSelecionada.Name = "txtBaseSelecionada";
            txtBaseSelecionada.Size = new Size(100, 23);
            txtBaseSelecionada.TabIndex = 8;
            // 
            // txtQtdFaturas
            // 
            txtQtdFaturas.Enabled = false;
            txtQtdFaturas.Location = new Point(206, 8);
            txtQtdFaturas.Name = "txtQtdFaturas";
            txtQtdFaturas.Size = new Size(75, 23);
            txtQtdFaturas.TabIndex = 7;
            // 
            // lblFaturas
            // 
            lblFaturas.AutoSize = true;
            lblFaturas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFaturas.Location = new Point(150, 11);
            lblFaturas.Name = "lblFaturas";
            lblFaturas.Size = new Size(50, 15);
            lblFaturas.TabIndex = 2;
            lblFaturas.Text = "Faturas:";
            // 
            // lblBase
            // 
            lblBase.AutoSize = true;
            lblBase.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBase.Location = new Point(3, 11);
            lblBase.Name = "lblBase";
            lblBase.Size = new Size(36, 15);
            lblBase.TabIndex = 3;
            lblBase.Text = "Base:";
            // 
            // panel2
            // 
            panel2.Controls.Add(grbFiltros);
            panel2.Controls.Add(btnDownloadCsv);
            panel2.Controls.Add(lblSelecionarOutraBase);
            panel2.Controls.Add(cmbBasesDisponiveis);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(857, 100);
            panel2.TabIndex = 0;
            // 
            // grbFiltros
            // 
            grbFiltros.Controls.Add(btnFiltrar);
            grbFiltros.Controls.Add(lblFiltroInstalacao);
            grbFiltros.Controls.Add(txtFiltroInstalacao);
            grbFiltros.Dock = DockStyle.Bottom;
            grbFiltros.Location = new Point(0, 35);
            grbFiltros.Name = "grbFiltros";
            grbFiltros.Size = new Size(857, 65);
            grbFiltros.TabIndex = 7;
            grbFiltros.TabStop = false;
            grbFiltros.Text = "Filtros";
            // 
            // btnFiltrar
            // 
            btnFiltrar.Dock = DockStyle.Right;
            btnFiltrar.Location = new Point(779, 19);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(75, 43);
            btnFiltrar.TabIndex = 2;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = true;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // lblFiltroInstalacao
            // 
            lblFiltroInstalacao.AutoSize = true;
            lblFiltroInstalacao.Location = new Point(21, 17);
            lblFiltroInstalacao.Name = "lblFiltroInstalacao";
            lblFiltroInstalacao.Size = new Size(60, 15);
            lblFiltroInstalacao.TabIndex = 1;
            lblFiltroInstalacao.Text = "Instalação";
            // 
            // txtFiltroInstalacao
            // 
            txtFiltroInstalacao.Location = new Point(21, 30);
            txtFiltroInstalacao.Name = "txtFiltroInstalacao";
            txtFiltroInstalacao.Size = new Size(100, 23);
            txtFiltroInstalacao.TabIndex = 0;
            // 
            // btnDownloadCsv
            // 
            btnDownloadCsv.BackColor = Color.DarkOrange;
            btnDownloadCsv.FlatStyle = FlatStyle.Popup;
            btnDownloadCsv.Font = new Font("Segoe UI", 9F);
            btnDownloadCsv.ForeColor = Color.Black;
            btnDownloadCsv.Location = new Point(263, 6);
            btnDownloadCsv.Name = "btnDownloadCsv";
            btnDownloadCsv.Size = new Size(82, 23);
            btnDownloadCsv.TabIndex = 6;
            btnDownloadCsv.Text = "🔼 Exportar";
            btnDownloadCsv.UseVisualStyleBackColor = false;
            btnDownloadCsv.Click += btnDownloadCsv_Click;
            // 
            // lblSelecionarOutraBase
            // 
            lblSelecionarOutraBase.AutoSize = true;
            lblSelecionarOutraBase.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSelecionarOutraBase.Location = new Point(5, 9);
            lblSelecionarOutraBase.Name = "lblSelecionarOutraBase";
            lblSelecionarOutraBase.Size = new Size(126, 15);
            lblSelecionarOutraBase.TabIndex = 5;
            lblSelecionarOutraBase.Text = "Selecionar outra base";
            // 
            // cmbBasesDisponiveis
            // 
            cmbBasesDisponiveis.FormattingEnabled = true;
            cmbBasesDisponiveis.Location = new Point(137, 6);
            cmbBasesDisponiveis.Name = "cmbBasesDisponiveis";
            cmbBasesDisponiveis.Size = new Size(121, 23);
            cmbBasesDisponiveis.TabIndex = 4;
            cmbBasesDisponiveis.SelectedIndexChanged += cmbBasesDisponiveis_SelectedIndexChanged;
            // 
            // faturasViewDtoBindingSource
            // 
            faturasViewDtoBindingSource.DataSource = typeof(dtos.FaturasViewDto);
            // 
            // conteudoBindingSource
            // 
            conteudoBindingSource.DataSource = typeof(dtos.Conteudo);
            // 
            // bcwCarregaDados
            // 
            bcwCarregaDados.DoWork += bcwCarregaDados_DoWork;
            bcwCarregaDados.RunWorkerCompleted += bcwCarregaDados_RunWorkerCompleted;
            // 
            // FattureWebForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(857, 450);
            Controls.Add(panel1);
            MinimumSize = new Size(816, 489);
            Name = "FattureWebForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DevTools - Share (FattureWeb)";
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pcbLoading).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridFaturas).EndInit();
            ((System.ComponentModel.ISupportInitialize)faturasViewDtoBindingSource1).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            grbFiltros.ResumeLayout(false);
            grbFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)faturasViewDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)conteudoBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private Panel panel2;
        private Label lblFaturas;
        private Label lblBase;
        private Label lblSelecionarOutraBase;
        private ComboBox cmbBasesDisponiveis;
        private DataGridView gridFaturas;
        private BindingSource conteudoBindingSource;
        private PictureBox pcbLoading;
        private System.ComponentModel.BackgroundWorker bcwCarregaDados;
        private Button btnDownloadCsv;
        private TextBox txtBaseSelecionada;
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
        private Label lblFiltroInstalacao;
        private TextBox txtFiltroInstalacao;
        private BindingSource faturasViewDtoBindingSource1;
    }
}