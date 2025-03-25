using DTSWindowsForm.Extensions;
using DTSWindowsForm.UI.FattureWeb.dtos;
using DTSWindowsForm.UI.UserControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
                    var item = (FaturasViewDto)gridFaturas.Rows[e.RowIndex].DataBoundItem;
                    if (item == null) return;

                    if (e.ColumnIndex == gridFaturas.Columns["VisualizarFatura"].Index)
                        BaixarPdf(item);
                    else if (e.ColumnIndex == gridFaturas.Columns["VisualizarJson"].Index)
                        BaixarJson(item);
                }
            };
        }

        void BaixarPdf(FaturasViewDto fatura)
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

        void BaixarJson(FaturasViewDto fatura)
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
            var dadosGrid = gridFaturas.DataSource as List<FaturasViewDto>;

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
            List<FaturasViewDto> faturas = new List<FaturasViewDto>();
            _dadosFiltrados = FiltrarDados();
            foreach (var dado in _dadosFiltrados)
            {
                DateTime dataMesRef = DateTime.Parse(dado.Conteudo.Fatura.MesReferencia);
                var consumoTotal = dado.Conteudo.Fatura.HistoricoFaturamento != null ? dado.Conteudo.Fatura.HistoricoFaturamento.FirstOrDefault().EnergiaAtiva : 0;
                FaturasViewDto fatura = new FaturasViewDto();
                fatura.FaturaId = dado.Conteudo.FaturaId.ToString();
                fatura.Instalacao = dado.Conteudo.UnidadeConsumidora.Instalacao;
                fatura.MesReferencia = dataMesRef.ToString("MMyyyy");
                fatura.Distribuidora = dado.Conteudo.Distribuidora.ToString();
                fatura.IdInstalacao = dado.InstalacaoId.ToString();
                fatura.ConsumoTotal = consumoTotal.ToString();
                fatura.DataEmissao = dado.Conteudo.Fatura.DataEmissao;
                fatura.DataProcessamento = dado.DataProcessamento.ToString();
                ObtemDadosFatura(fatura, dado);
                faturas.Add(fatura);
            }

            gridFaturas.DataSource = faturas;
        }

        private void ObtemDadosFatura(FaturasViewDto fatura, Dado dado)
        {
            var contentFatura = dado.Conteudo;
            fatura.DescricaoProdutos = contentFatura.Fatura.GetDescricaoProdutos();
            #region Obtenção dos produtos da fatura
            var produtoConsumoCompensadoKwh = contentFatura.Fatura.GetProdutoPorDescricao("Consumo Compensado kWh");
            var produtoConsumoKwh = contentFatura.Fatura.GetProdutoPorDescricao("Consumo kWh");
            var produtoEnergiaInjetadaKwh = contentFatura.Fatura.GetProdutoPorDescricao("Energia Injetada kWh");
            var produtoEnergiaInjetadaTUSDKwh = contentFatura.Fatura.GetProdutoPorDescricao("Energia Injetada TUSD kWh");
            var produtoEnergiaInjetadaTEKwh = contentFatura.Fatura.GetProdutoPorDescricao("Energia Injetada TE kWh");
            var produtoConsumoTUSDKwh = contentFatura.Fatura.GetProdutoPorDescricao("Consumo TUSD kWh");
            var produtoConsumoTEKwh = contentFatura.Fatura.GetProdutoPorDescricao("Consumo TE kWh");
            var produtoContribuicaoIluminacaoPublica = contentFatura.Fatura.GetProdutoPorDescricao("Contribuição Iluminação Pública");
            var produtoConsumoTEFP = contentFatura.Fatura.GetProdutoPorDescricao("Consumo TE kWh Fora Ponta");
            var produtoConsumoTEP = contentFatura.Fatura.GetProdutoPorDescricao("Consumo TE kWh Ponta");
            var produtoConsumoReativoExcedenteP = contentFatura.Fatura.GetProdutoPorDescricao("Consumo Reativo Excedente kVARh Ponta");
            var produtoConsumoReativoExcedenteFP = contentFatura.Fatura.GetProdutoPorDescricao("Consumo Reativo Excedente kVARh Fora Ponta");
            var produtoDemandaTUSDP = contentFatura.Fatura.GetProdutoPorDescricao("Demanda TUSD kW Ponta");
            var produtoDemandaTUSDFP = contentFatura.Fatura.GetProdutoPorDescricao("Demanda TUSD kW Fora Ponta");
            var produtoConsumoTUSDFP = contentFatura.Fatura.GetProdutoPorDescricao("Consumo TUSD kWh Fora Ponta");
            var produtoConsumoTUSDP = contentFatura.Fatura.GetProdutoPorDescricao("Consumo TUSD kWh Ponta");
            var produtoEnergiaInjetadaTEFP = contentFatura.Fatura.GetProdutoPorDescricao("Energia Injetada TE kWh Fora Ponta");
            var produtoEnergiaInjetadaTUSDFP = contentFatura.Fatura.GetProdutoPorDescricao("Energia Injetada TUSD kWh Fora Ponta");
            var produtoEnergiaInjetadaTEP = contentFatura.Fatura.GetProdutoPorDescricao("Energia Injetada TE kWh Ponta");
            var produtoEnergiaInjetadaTUSDP = contentFatura.Fatura.GetProdutoPorDescricao("Energia Injetada TUSD kWh Ponta");
            var produtoAdicionalBandeiraAmarela = contentFatura.Fatura.GetProdutoPorDescricao("Adic. Bandeira Amarela");
            var produtoAdicionalBandeiraVermelhaP1 = contentFatura.Fatura.GetProdutoPorDescricao("Adic. Bandeira Vermelha P1");
            var produtoAdicionalBandeiraVermelhaP2 = contentFatura.Fatura.GetProdutoPorDescricao("Adic. Bandeira Vermelha P2");
            var produtoAdicionalBandeiraEscassezHidrica = contentFatura.Fatura.GetProdutoPorDescricao("Adic. Bandeira Escassez Hídrica");
            var produtoBandeiraEnergiaInjetadaGDAmarela = contentFatura.Fatura.GetProdutoPorDescricao("Bandeira Energia Injetada GD Amarela");
            var produtoBandeiraEnergiaInjetadaGDVermelhaP1 = contentFatura.Fatura.GetProdutoPorDescricao("Bandeira Energia Injetada GD Vermelha P1");
            var produtoBandeiraEnergiaInjetadaGDVermelhaP2 = contentFatura.Fatura.GetProdutoPorDescricao("Bandeira Energia Injetada GD Vermelha P2");
            var produtoBandeiraEnergiaInjetadaGDEscassesHidrica = contentFatura.Fatura.GetProdutoPorDescricao("Bandeira Energia Injetada GD Escassez Hídrica");
            var produtoAjusteFaturamentoGD_REN_1059_2023 = contentFatura.Fatura.GetProdutoPorDescricao("Ajuste Faturamento GD - REN 1.059/2023");
            #endregion

            ModelosFaturasEnum modeloFatura = contentFatura.ObtemModeloFaturaGdc();
            fatura.ModeloFatura = modeloFatura;
            if (produtoEnergiaInjetadaKwh != null && modeloFatura != ModelosFaturasEnum.Modelo5)
            {
                var produto = produtoEnergiaInjetadaKwh;
                fatura.EnergiaInjetada = produto.Quantidade != null ? Math.Abs(produto.Quantidade.Value) : 0;
            }

            if (produtoConsumoCompensadoKwh != null && modeloFatura == ModelosFaturasEnum.Modelo5)
            {
                var produto = produtoConsumoCompensadoKwh;
                fatura.EnergiaInjetada = produto.Quantidade != null ? Math.Abs(produto.Quantidade.Value) : 0;
            }
            if (produtoEnergiaInjetadaTUSDKwh != null)
            {
                var produto = produtoEnergiaInjetadaTUSDKwh;
                fatura.EnergiaInjetada = produto.Quantidade != null ? Math.Abs(produto.Quantidade.Value) : null;
            }
            if (contentFatura.Outros.DebitoAutomatico.HasValue)
            {
                fatura.DebitoAutomatico = contentFatura.Outros.DebitoAutomatico.Value ? "Sim" : "Não";
            }

            fatura.ValorMuc = contentFatura.Fatura.GetValorMuc();
        }

        private List<Dado> FiltrarDados()
        {
            List<Dado> dadosFiltrados = _dados;
            if (!string.IsNullOrEmpty(_filtros.FaturaId))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.Conteudo.FaturaId.ToString() == _filtros.FaturaId).ToList();
            }

            if (_filtros.Instalacao != null && _filtros.Instalacao.Any())
            {
                dadosFiltrados = dadosFiltrados.Where(x => _filtros.Instalacao.Any(o => o == x.Conteudo.UnidadeConsumidora.Instalacao.ToString())).ToList();
            }

            if (_filtros.MesReferencia != null && _filtros.MesReferencia.Any())
            {
                dadosFiltrados = dadosFiltrados.Where(x => _filtros.MesReferencia.Contains(DateTime.Parse(x.Conteudo.Fatura.MesReferencia).ToString("MMyyyy"))).ToList();
            }

            if (_filtros.Distribuidora != null && _filtros.Distribuidora.Any())
            {
                dadosFiltrados = dadosFiltrados.Where(x => _filtros.Distribuidora.Contains(x.Conteudo.Distribuidora.ToString())).ToList();
            }

            if (!string.IsNullOrEmpty(_filtros.IdInstalacao))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.InstalacaoId.ToString() == _filtros.IdInstalacao).ToList();
            }

            if (!string.IsNullOrEmpty(_filtros.ConsumoTotal))
            {
                dadosFiltrados = dadosFiltrados.Where(x => (x.Conteudo.Fatura.HistoricoFaturamento != null ? x.Conteudo.Fatura.HistoricoFaturamento.FirstOrDefault().EnergiaAtiva : 0).ToString() == _filtros.ConsumoTotal).ToList();
            }

            if (_filtros.DataEmissao.HasValue)
            {
                dadosFiltrados = dadosFiltrados.Where(x => DateTime.Parse(x.Conteudo.Fatura.DataEmissao) == _filtros.DataEmissao.Value).ToList();
            }

            if (chbFaturasDuplicadas.Checked)
            {
                dadosFiltrados = dadosFiltrados
                    .GroupBy(x => new
                    {
                        x.Conteudo.Fatura.MesReferencia,
                        x.Conteudo.UnidadeConsumidora.Instalacao,
                        x.Conteudo.Distribuidora
                    })
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g)
                    .ToList();
            }

            return dadosFiltrados;
        }

        public void CarregarDados()
        {
            _token = realizarLogin();
            var objeto = GetFaturasPaginadoAsync();
            if (objeto != null)
            {
                _dados = objeto.Dados.ToList();
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

        //public Root? GetFaturasPaginadoAsync()
        //{
        //    Root retorno = new Root("", "", new List<Dado>());

        //    var clientFaturas = new HttpClient();
        //    clientFaturas.Timeout = Timeout.InfiniteTimeSpan;
        //    const int tamanhoMaximoPagina = 1000;
        //    int limit = tamanhoMaximoPagina;
        //    int skip = 0;
        //    bool hasMoreData = true;

        //    while (hasMoreData)
        //    {
        //        try
        //        {
        //            string url = $"https://api.fattureweb.com.br/faturas?limit={limit}&skip={skip}";
        //            var requestFaturas = new HttpRequestMessage(HttpMethod.Get, url);
        //            requestFaturas.Headers.Add("Fatture-AuthToken", _token);
        //            requestFaturas.Headers.Add(
        //                "Fatture-SearchFields",
        //                "id, instalacao_id, arquivo_id, status_fatura_id, status, data_criacao, data_atualizacao, processamento_id, usuario_id, email_fatura_id, data_processamento, erro_processamento, mes_referencia, data_vencimento, valor_total, conteudo"
        //            );

        //            var responseFaturas = clientFaturas.Send(requestFaturas);
        //            responseFaturas.EnsureSuccessStatusCode();

        //            var contentResponseFaturas = responseFaturas.Content.ReadAsStringAsync().Result;
        //            var retornoFw = JsonConvert.DeserializeObject<Root>(contentResponseFaturas);

        //            if (retornoFw != null && retornoFw.Dados.Any())
        //            {
        //                retorno.Dados.AddRange(retornoFw.Dados);
        //                skip += limit;
        //                if (retornoFw.Dados.Count < tamanhoMaximoPagina)
        //                {
        //                    hasMoreData = false;
        //                }
        //            }
        //            else
        //            {
        //                hasMoreData = false;
        //            }
        //        }
        //        catch
        //        {
        //            hasMoreData = false;
        //        }
        //    }

        //    return retorno;
        //}
        public Root? GetFaturasPaginadoAsync()
        {
            Root retorno = new Root("", "", new List<Dado>());
            const int tamanhoMaximoPagina = 1000;
            int limit = tamanhoMaximoPagina;
            int skip = 0;
            bool hasMoreData = true;

            while (hasMoreData)
            {
                try
                {
                    string contentResponseFaturas = GetJsonFaturas(limit, skip);
                    var retornoFw = JsonConvert.DeserializeObject<Root>(contentResponseFaturas);

                    if (retornoFw != null && retornoFw.Dados.Any())
                    {
                        retorno.Dados.AddRange(retornoFw.Dados);
                        skip += limit;
                        if (retornoFw.Dados.Count < tamanhoMaximoPagina)
                        {
                            hasMoreData = false;
                        }
                    }
                    else
                    {
                        hasMoreData = false;
                    }
                }
                catch
                {
                    hasMoreData = false;
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

        private void cmbBasesDisponiveis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbBasesDisponiveis.SelectedItem != null)
            //{
            //    string itemSelecionado = cmbBasesDisponiveis.SelectedItem.ToString();
            //    settings.TipoConta = EnumExtensions.GetEnumByDescription<TipoContaEnum>(itemSelecionado);
            //    cmbBasesDisponiveis.Items.Clear();
            //    IniciaBackground();
            //}
        }

        private void btnDownloadCsv_Click(object sender, EventArgs e)
        {
            try
            {
                StartLoading();
                gridFaturas.ExportToXls("faturas", Program.OutputDir);

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
    }
}
