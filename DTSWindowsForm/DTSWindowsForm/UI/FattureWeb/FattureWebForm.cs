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

            PreencherComboBoxInstalacao();
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
            PreencherComboBoxInstalacao();
        }

        private void ObtemDadosFatura(FaturasViewDto fatura, Dado dado)
        {
            var contentFatura = dado.Conteudo;

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



        }

        private void PreencherComboBoxInstalacao()
        {
            // Primeiro, limpe qualquer valor existente no ComboBox
            cmbInstalacao.Items.Clear();

            // Crie uma lista de valores únicos
            HashSet<string> instalacoesUnicas = new HashSet<string>() { "" };

            // Percorra as linhas do DataGridView para pegar os valores da coluna "Instalação"
            foreach (var dado in _dados)
            {
                if (!string.IsNullOrEmpty(dado.Conteudo.UnidadeConsumidora.Instalacao))
                {
                    instalacoesUnicas.Add(dado.Conteudo.UnidadeConsumidora.Instalacao);
                }
            }
            // Adicione os valores únicos no ComboBox
            cmbInstalacao.Items.AddRange(instalacoesUnicas.ToArray());
            cmbInstalacao.SelectedItem = _filtros.Instalacoes.Count > 0 ? _filtros.Instalacoes.First() : "";
        }

        private List<Dado> FiltrarDados()
        {
            List<Dado> dadosFiltrados = _dados;
            if (!string.IsNullOrEmpty(_filtros.FaturaId))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.Conteudo.FaturaId.ToString() == _filtros.FaturaId).ToList();
            }

            if (_filtros.Instalacoes != null && _filtros.Instalacoes.Count > 0)
            {
                dadosFiltrados = dadosFiltrados.Where(x => _filtros.Instalacoes.Any(o => o == x.Conteudo.UnidadeConsumidora.Instalacao.ToString())).ToList();
            }

            if (_filtros.MesReferencia.HasValue)
            {
                dadosFiltrados = dadosFiltrados.Where(x => DateTime.Parse(x.Conteudo.Fatura.MesReferencia) == _filtros.MesReferencia.Value).ToList();
            }

            if (!string.IsNullOrEmpty(_filtros.Distribuidora))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.Conteudo.Distribuidora.ToString() == _filtros.Distribuidora).ToList();
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
            var requestToken = new HttpRequestMessage(HttpMethod.Post, "https://api.fattureweb.com.br/auth/login");
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

        public Root? GetFaturasPaginadoAsync()
        {
            Root retorno = new Root("", "", new List<Dado>());

            var clientFaturas = new HttpClient();
            clientFaturas.Timeout = Timeout.InfiniteTimeSpan;
            const int tamanhoMaximoPagina = 1000;
            int limit = tamanhoMaximoPagina;
            int skip = 0;
            bool hasMoreData = true;

            while (hasMoreData)
            {
                try
                {
                    string url = $"https://api.fattureweb.com.br/faturas?limit={limit}&skip={skip}";
                    var requestFaturas = new HttpRequestMessage(HttpMethod.Get, url);
                    requestFaturas.Headers.Add("Fatture-AuthToken", _token);
                    requestFaturas.Headers.Add(
                        "Fatture-SearchFields",
                        "id, instalacao_id, arquivo_id, status_fatura_id, status, data_criacao, data_atualizacao, processamento_id, usuario_id, email_fatura_id, data_processamento, erro_processamento, mes_referencia, data_vencimento, valor_total, conteudo"
                    );

                    var responseFaturas = clientFaturas.Send(requestFaturas);
                    responseFaturas.EnsureSuccessStatusCode();

                    var contentResponseFaturas = responseFaturas.Content.ReadAsStringAsync().Result;
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
            gridFaturas.ExportToXls("faturas", Program.OutputDir);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            _filtros.Instalacoes.Clear();
            if (cmbInstalacao.SelectedItem != null && !string.IsNullOrEmpty(cmbInstalacao.SelectedItem.ToString()))
            {
                _filtros.Instalacoes.Add(cmbInstalacao.SelectedItem.ToString());
            }

            PreencherGridFaturas();
        }

        private void btnBuscarFaturas_Click(object sender, EventArgs e)
        {
            IniciaBackground();
        }

        private void btnLimparFiltros_Click(object sender, EventArgs e)
        {
            LimparFiltros();
        }

        private void LimparFiltros()
        {
            _filtros = new FiltrosFaturasDto();
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

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
