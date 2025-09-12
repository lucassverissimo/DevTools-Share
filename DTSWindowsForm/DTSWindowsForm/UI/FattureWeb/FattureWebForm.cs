using DTSWindowsForm.Extensions;
using DTSWindowsForm.UI.FattureWeb.dtos;
using DTSWindowsForm.UI.UserControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;


namespace DTSWindowsForm.UI.FattureWeb
{
    public partial class FattureWebForm : Form
    {
        List<Dado> _dados = new List<Dado>();
        string _token = string.Empty;

        private string _ultimaColunaOrdenada = string.Empty;
        private SortOrder _direcaoOrdenacao = SortOrder.None;
        private FiltrosFaturasDto _filtros = new FiltrosFaturasDto();
        private LoadingControl _loadingControl;
        private List<Dado> _dadosFiltrados = new List<Dado>();
        private bool _gravarLog = false;
        private readonly string URL_FATTUREWEB = "https://api.fattureweb.com.br/";

        public FattureWebForm()
        {
            InitializeComponent();
            _loadingControl = new LoadingControl();

            foreach (DataGridViewColumn column in gridFaturas.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            gridFaturas.AllowUserToOrderColumns = true;
            gridFaturas.ColumnHeaderMouseClick += dataGridView_ColumnHeaderMouseClick;

            if (Program.AppSettings != null)
            {
                txtUsuario.Text = Program.AppSettings.Usuario;
                txtSenha.Text = Program.AppSettings.Senha;
            }

            // Criar a primeira coluna de botão (Lupa - Visualizar Fatura)
            DataGridViewButtonColumn btnLupa = new DataGridViewButtonColumn();
            btnLupa.Name = "VisualizarFatura";
            btnLupa.HeaderText = "⬇️";
            btnLupa.Text = "🔍"; // Apenas um símbolo para diferenciar
            btnLupa.UseColumnTextForButtonValue = true; // Garante que o botão mostra o texto
            btnLupa.Width = 30;

            // Criar a segunda coluna de botão (Chaves - Visualizar JSON)
            DataGridViewButtonColumn btnJson = new DataGridViewButtonColumn();
            btnJson.Name = "VisualizarJson";
            btnJson.HeaderText = "⬇️";
            btnJson.Text = "{ }"; // Representação do JSON
            btnJson.UseColumnTextForButtonValue = true;
            btnJson.Width = 30;

            // Adicionar as colunas no DataGridView
            gridFaturas.Columns.Insert(0, btnJson);
            gridFaturas.Columns.Insert(0, btnLupa);

            // Adicionar ToolTips
            gridFaturas.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == gridFaturas.Columns["VisualizarFatura"].Index)
                        gridFaturas.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "Baixar Fatura";
                    else if (e.ColumnIndex == gridFaturas.Columns["VisualizarJson"].Index)
                        gridFaturas.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "Baixar JSON";
                }
            };

            // Capturar clique nos botões            
            gridFaturas.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    // Obtém o objeto vinculado à linha
                    var item = (FaturasProdutoViewDto)gridFaturas.Rows[e.RowIndex].DataBoundItem;
                    if (item == null) return;

                    if (e.ColumnIndex == gridFaturas.Columns["VisualizarFatura"].Index)
                        BaixarPdf(item);
                    else if (e.ColumnIndex == gridFaturas.Columns["VisualizarJson"].Index)
                        BaixarJson(item);
                }
            };

            btnBuscarFaturas.MouseUp += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    cmsLog.Show(btnBuscarFaturas, e.Location);
                }
            };

            checkLog.Click += (sender, e) =>
            {
                _gravarLog = checkLog.Checked;
            };
        }

        void BaixarPdf(FaturasProdutoViewDto fatura)
        {
            try
            {
                StartLoading();
                MessageBox.Show($"O download do PDF da fatura: {fatura.FaturaId} será iniciado.");

                using (var client = new HttpClient())
                {
                    var url = $"{URL_FATTUREWEB}faturas/{fatura.FaturaId}/arquivo";
                    var requestArquivo = new HttpRequestMessage(HttpMethod.Get, url);
                    requestArquivo.Headers.Add("Fatture-AuthToken", _token);

                    var responseArquivos = client.Send(requestArquivo);

                    if (responseArquivos.IsSuccessStatusCode)
                    {
                        string responseStringArquivos = responseArquivos.Content.ReadAsStringAsync().Result;
                        FattureWebArquivoResponse arquivos = JsonConvert.DeserializeObject<FattureWebArquivoResponse>(responseStringArquivos);

                        var linkPdf = arquivos.Dados.First().AwsPresignedUrl;

                        try
                        {
                            HttpResponseMessage response = client.GetAsync(linkPdf).Result;
                            response.EnsureSuccessStatusCode();

                            byte[] pdfFatura = response.Content.ReadAsByteArrayAsync().Result;
                            var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Faturas");

                            if (!Directory.Exists(outputPath))
                            {
                                Directory.CreateDirectory(outputPath);
                            }

                            string fileName = $"FAT-{fatura.FaturaId}.pdf";
                            string fullPath = Path.Combine(outputPath, fileName);

                            File.WriteAllBytes(fullPath, pdfFatura);

                            DialogResult result = MessageBox.Show($"Fatura salva em:\n{fullPath}\n\nDeseja abrir o diretório onde o arquivo foi salvo?", "Download Concluído", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(fullPath));
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao baixar o arquivo PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Erro ao obter URL do PDF: {responseArquivos.ReasonPhrase}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StopLoading();
            }
        }

        void BaixarJson(FaturasProdutoViewDto fatura)
        {
            try
            {
                StartLoading();
                MessageBox.Show($"O download do JSON da fatura: {fatura.FaturaId} será iniciado.");

                string json = GetJsonFaturas(idFatura: fatura.FaturaId!);

                var outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Faturas");

                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                string fileName = $"FAT-{fatura.FaturaId}.json";
                string fullPath = Path.Combine(outputPath, fileName);

                // Tratamento de erro ao desserializar JSON
                try
                {
                    string formattedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented);
                    File.WriteAllText(fullPath, formattedJson);

                    DialogResult result = MessageBox.Show($"JSON salvo com sucesso em:\n{fullPath}\n\nDeseja abrir o diretório onde o arquivo foi salvo?", "Download Concluído", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(fullPath));
                    }
                }
                catch (JsonException)
                {
                    MessageBox.Show("Erro ao processar o JSON. O conteúdo pode estar corrompido.", "Erro de JSON", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Erro: Permissão negada para salvar o arquivo. Tente executar o programa como administrador.", "Erro de Permissão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Erro ao acessar o arquivo: {ex.Message}", "Erro de Arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro Desconhecido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StopLoading();
            }

        }

        private void StartLoading()
        {
            _loadingControl.ShowLoading(this, "aguarde...");
        }

        private void StopLoading()
        {
            _loadingControl.HideLoading(this);
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = gridFaturas.Columns[e.ColumnIndex].DataPropertyName;
            var dadosGrid = gridFaturas.DataSource as List<FaturasProdutoViewDto>;

            var ordemColunas = gridFaturas.Columns.Cast<DataGridViewColumn>()
                .OrderBy(c => c.DisplayIndex)
                .Select(c => c.Name)
                .ToList();

            if (_ultimaColunaOrdenada == columnName)
            {
                _direcaoOrdenacao = (_direcaoOrdenacao == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                _direcaoOrdenacao = SortOrder.Ascending;
                _ultimaColunaOrdenada = columnName;
            }

            if (_direcaoOrdenacao == SortOrder.Ascending)
            {
                dadosGrid = dadosGrid.OrderBy(d => GetPropertyValue(d, columnName)).ToList();
            }
            else
            {
                dadosGrid = dadosGrid.OrderByDescending(d => GetPropertyValue(d, columnName)).ToList();
            }
            gridFaturas.DataSource = null;
            gridFaturas.DataSource = dadosGrid;

            foreach (var nomeColuna in ordemColunas)
            {
                gridFaturas.Columns[nomeColuna].DisplayIndex = ordemColunas.IndexOf(nomeColuna);
            }
        }
        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
        private void IniciaBackground()
        {
            StartLoading();
            bcwCarregaDados.RunWorkerAsync();
        }

        private void bcwCarregaDados_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            CarregarDados();
        }

        private void bcwCarregaDados_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            AjustarInterface();
            StopLoading();
        }

        private void AjustarInterface()
        {
            PreencherGridFaturas();
        }

        private void PreencherGridFaturas()
        {
            List<FaturasProdutoViewDto> produtos = new List<FaturasProdutoViewDto>();
            _dadosFiltrados = FiltrarDados();

            foreach (var dado in _dadosFiltrados)
            {
                DateTime dataMesRef = DateTime.Parse(dado.Conteudo.Fatura.MesReferencia);
                foreach (var produto in dado.Conteudo.Fatura.Produtos)
                {
                    if (_filtros.DescricaoProdutos != null && _filtros.DescricaoProdutos.Any())
                    {
                        if (!_filtros.DescricaoProdutos.Any(dp => !string.IsNullOrEmpty(produto.Descricao) && produto.Descricao.IndexOf(dp, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            continue;
                        }
                    }

                    if (_filtros.DescricoesOriginais != null && _filtros.DescricoesOriginais.Any())
                    {
                        if (!(produto.DescricoesOriginais != null && produto.DescricoesOriginais.Any(o => _filtros.DescricoesOriginais.Any(f => o.IndexOf(f, StringComparison.OrdinalIgnoreCase) >= 0))))
                        {
                            continue;
                        }
                    }

                    var view = new FaturasProdutoViewDto
                    {
                        FaturaId = dado.Conteudo.FaturaId.ToString(),
                        Instalacao = dado.Conteudo.UnidadeConsumidora.Instalacao,
                        MesReferencia = dataMesRef.ToString("MMyyyy"),
                        Distribuidora = dado.Conteudo.Distribuidora.ToString(),
                        IdInstalacao = dado.InstalacaoId.ToString(),
                        DataEmissao = dado.Conteudo.Fatura.DataEmissao,
                        DataProcessamento = dado.DataProcessamento.ToString(),
                        Descricao = produto.Descricao,
                        Quantidade = produto.Quantidade,
                        ValorTotal = produto.ValorTotal,
                        ValorSemImpostos = produto.ValorSemImpostos,
                        TarifaComImpostos = produto.TarifaComImpostos,
                        TarifaSemImpostos = produto.TarifaSemImpostos,
                        TaxaDesconto = produto.TaxaDesconto?.ToString(),
                        DescricoesOriginais = produto.DescricoesOriginais != null ? string.Join(";", produto.DescricoesOriginais) : string.Empty
                    };

                    produtos.Add(view);
                }
            }

            gridFaturas.DataSource = produtos;
        }

        // ObtemDadosFatura removido: informações dos produtos agora são tratadas diretamente em PreencherGridFaturas.

        private List<Dado> FiltrarDados()
        {
            IEnumerable<Dado> query = _dados;

            if (!string.IsNullOrEmpty(_filtros.FaturaId))
            {
                query = query.Where(x => x.Conteudo.FaturaId.ToString() == _filtros.FaturaId);
            }

            if (_filtros.Instalacao != null && _filtros.Instalacao.Any())
            {
                query = query.Where(x => _filtros.Instalacao.Any(o => o == x.Conteudo.UnidadeConsumidora.Instalacao.ToString()));
            }

            if (_filtros.MesReferencia != null && _filtros.MesReferencia.Any())
            {
                query = query.Where(x => _filtros.MesReferencia.Contains(DateTime.Parse(x.Conteudo.Fatura.MesReferencia).ToString("MMyyyy")));
            }

            if (_filtros.Distribuidora != null && _filtros.Distribuidora.Any())
            {
                query = query.Where(x => _filtros.Distribuidora.Contains(x.Conteudo.Distribuidora.ToString()));
            }

            if (!string.IsNullOrEmpty(_filtros.IdInstalacao))
            {
                query = query.Where(x => x.InstalacaoId.ToString() == _filtros.IdInstalacao);
            }

            if (!string.IsNullOrEmpty(_filtros.ConsumoTotal))
            {
                query = query.Where(x => (x.Conteudo.Fatura.HistoricoFaturamento != null ? x.Conteudo.Fatura.HistoricoFaturamento.FirstOrDefault().EnergiaAtiva : 0).ToString() == _filtros.ConsumoTotal);
            }

            if (_filtros.DataEmissao.HasValue)
            {
                query = query.Where(x => DateTime.Parse(x.Conteudo.Fatura.DataEmissao) == _filtros.DataEmissao.Value);
            }

            if (_filtros.ModelosFw != null && _filtros.ModelosFw.Any())
            {
                query = query.Where(x => _filtros.ModelosFw.Any(m => m == x.Conteudo.ModeloFatura));
            }

            if (chbFaturasDuplicadas.Checked)
            {
                query = query
                    .GroupBy(x => new
                    {
                        x.Conteudo.Fatura.MesReferencia,
                        x.Conteudo.UnidadeConsumidora.Instalacao,
                        x.Conteudo.Distribuidora
                    })
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g);
            }

            return query.ToList();
        }

        public void CarregarDados()
        {
            _token = realizarLogin();
            if (!string.IsNullOrEmpty(_token))
            {
                var objeto = GetFaturasPaginadoWithLogAsync();
                if (objeto != null)
                {
                    _dados = objeto.Dados.ToList();
                }
            }
        }

        public string realizarLogin()
        {
            var clientToken = new HttpClient();
            var requestToken = new HttpRequestMessage(HttpMethod.Post, $"{URL_FATTUREWEB}auth/login");
            var jsonRequestToken = new JObject
            {
                { "email", txtUsuario.Text },
                { "senha", txtSenha.Text }
            };
            var contentToken = new StringContent(jsonRequestToken.ToString(), null, "application/json");
            requestToken.Content = contentToken;
            var response = clientToken.Send(requestToken);
            //response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JObject.Parse(responseBody);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Não foi possível fazer autenticação com esse usuário/senha.", "Falha login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "";
            }
            else
            {
                var dados = jsonResponse["dados"];
                if (dados != null && dados.Any())
                {
                    var primeiroDado = dados.FirstOrDefault();
                    if (primeiroDado != null)
                    {
                        var token = primeiroDado["token"];
                        if (token != null)
                            return token.ToString();
                    }
                }
                return "";
            }
        }

        public Root? GetFaturasPaginadoWithLogAsync()
        {
            Root retorno = new Root("", "", new List<Dado>());
            const int tamanhoMaximoPagina = 1000;
            int skip = 0;
            bool continuar = true;
            object locker = new object();

            List<Task> tasks = new List<Task>();

            List<(int, string)> logEntries = new List<(int, string)>();

            Stopwatch totalStopwatch = Stopwatch.StartNew();

            logEntries.Add((0, $"Início do processo {DateTime.Now}"));

            while (continuar)
            {
                var localSkip = skip;
                skip += tamanhoMaximoPagina;

                Task task = Task.Run(() =>
                {
                    Stopwatch taskStopwatch = Stopwatch.StartNew();

                    try
                    {
                        string content = GetJsonFaturas(tamanhoMaximoPagina, localSkip);
                        var result = JsonConvert.DeserializeObject<Root>(content);

                        lock (locker)
                        {
                            logEntries.Add((
                                localSkip / tamanhoMaximoPagina + 1,
                                $"Requisição {localSkip / tamanhoMaximoPagina + 1} - Página: {localSkip / tamanhoMaximoPagina + 1} - Tempo: {taskStopwatch.ElapsedMilliseconds / 1000.0:F2}s - Sucesso"
                            ));
                        }

                        if (result?.Dados != null && result.Dados.Any())
                        {
                            lock (locker)
                            {
                                retorno.Dados.AddRange(result.Dados);
                            }

                            if (result.Dados.Count < tamanhoMaximoPagina)
                                continuar = false;
                        }
                        else
                        {
                            continuar = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (locker)
                        {
                            logEntries.Add((
                                localSkip / tamanhoMaximoPagina + 1,
                                $"Requisição {localSkip / tamanhoMaximoPagina + 1} - Página: {localSkip / tamanhoMaximoPagina + 1} - Erro: {ex.Message}"
                            ));
                        }
                        continuar = false;
                    }

                    taskStopwatch.Stop();
                });

                tasks.Add(task);

                if (tasks.Count >= 80)
                {
                    Task.WaitAll(tasks.ToArray());
                    tasks.Clear();
                }
            }

            Task.WaitAll(tasks.ToArray());

            totalStopwatch.Stop();
            logEntries.Add((logEntries.Count + 1, $"Processo concluído. Tempo total: {totalStopwatch.ElapsedMilliseconds / 1000.0:F2}s"));

            var sortedLogs = logEntries.OrderBy(entry => entry.Item1).ToList();

            if (_gravarLog)
            {
                string logFilePath = Path.Combine("logs", $"log{DateTime.Now:yyyyMMdd_HHmmss}.txt");

                if (!Directory.Exists("logs"))
                {
                    Directory.CreateDirectory("logs");
                }
                using (StreamWriter logFile = new StreamWriter(logFilePath, true))
                {
                    foreach (var log in sortedLogs)
                    {
                        logFile.WriteLine(log.Item2);
                    }
                }
            }

            return retorno;
        }

        private string GetJsonFaturas(int limit = 1000, int skip = 0, string idFatura = "")
        {
            using (HttpClient clientFaturas = new HttpClient())
            {
                clientFaturas.Timeout = Timeout.InfiniteTimeSpan;
                string url = $"{URL_FATTUREWEB}faturas?limit={limit}&skip={skip}";
                if (!string.IsNullOrEmpty(idFatura))
                {
                    url += $"&id={idFatura}";
                }

                var requestFaturas = new HttpRequestMessage(HttpMethod.Get, url);
                requestFaturas.Headers.Add("Fatture-AuthToken", _token);
                requestFaturas.Headers.Add(
                    "Fatture-SearchFields",
                    "id, instalacao_id, arquivo_id, status_fatura_id, status, data_criacao, data_atualizacao, processamento_id, usuario_id, email_fatura_id, data_processamento, erro_processamento, mes_referencia, data_vencimento, valor_total, conteudo"
                );

                var responseFaturas = clientFaturas.Send(requestFaturas);
                responseFaturas.EnsureSuccessStatusCode();

                var contentResponseFaturas = responseFaturas.Content.ReadAsStringAsync().Result;
                return contentResponseFaturas;
            }
        }

        private void btnDownloadCsv_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dadosFiltrados.Count > 0)
                {
                    StartLoading();
                    gridFaturas.ExportToXls("faturas", Program.OutputDir);
                }
                else
                {
                    MessageBox.Show("Não há dados para serem exportados.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao exportar arquivo. {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StopLoading();
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                StartLoading();
                _filtros = new FiltrosFaturasDto();
                if (!string.IsNullOrEmpty(txbDistribuidora.Text))
                    _filtros.Distribuidora = txbDistribuidora.Text.Split(';').ToList();

                if (!string.IsNullOrEmpty(txbMesRef.Text))
                    _filtros.MesReferencia = txbMesRef.Text.Split(';').ToList();

                if (!string.IsNullOrEmpty(txbInstalacao.Text))
                    _filtros.Instalacao = txbInstalacao.Text.Split(';').ToList();

                if (!string.IsNullOrEmpty(txbDescricaoProdutos.Text))
                    _filtros.DescricaoProdutos = txbDescricaoProdutos.Text.Split(';').ToList();

                if (!string.IsNullOrEmpty(txbDescricoesOriginais.Text))
                    _filtros.DescricoesOriginais = txbDescricoesOriginais.Text.Split(';').ToList();

                if (!string.IsNullOrEmpty(txbModeloFw.Text))
                    _filtros.ModelosFw = txbModeloFw.Text.Split(';').ToList();

                PreencherGridFaturas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao filtrar faturas. \n{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                StopLoading();
            }
        }

        private void btnBuscarFaturas_Click(object sender, EventArgs e)
        {
            if (_dados == null || !_dados.Any() ||
                MessageBox.Show("Essa ação irá buscar todas as faturas no FattureWeb. \nQuer continuar?", "Deseja realizar uma nova busca?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes
                )
            {
                IniciaBackground();
            }
        }

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            LimparFiltros();
        }

        private void LimparFiltros()
        {
            _filtros = new FiltrosFaturasDto();
            chbFaturasDuplicadas.Checked = false;
            txbDistribuidora.Text = string.Empty;
            txbDescricaoProdutos.Text = string.Empty;
            txbDescricoesOriginais.Text = string.Empty;
            txbInstalacao.Text = string.Empty;
            txbMesRef.Text = string.Empty;

            PreencherGridFaturas();
        }

        private void gridFaturas_DataSourceChanged(object sender, EventArgs e)
        {
            AtualizarTotais();
        }

        private void AtualizarTotais()
        {
            txtQtdFaturas.Text = _dados.Count.ToString();
            txtQtdFaturasFiltradas.Text = _dadosFiltrados.Count().ToString();
        }

        private void btnFiltros_Click(object sender, EventArgs e)
        {
            if (btnFiltros.Text.Contains(">>"))
            {
                btnFiltros.Text = "Filtros <<";
                pnlFiltros.Visible = false;
            }
            else
            {
                btnFiltros.Text = "Filtros >>";
                pnlFiltros.Visible = true;
            }

        }
    }
}
