using DTSWindowsForm.Extensions;
using DTSWindowsForm.UI.FattureWeb.dtos;
using DTSWindowsForm.UI.UserControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DTSWindowsForm.UI.FattureWeb
{
    public partial class FattureWebForm : Form
    {
        List<Dado> dados = new List<Dado>();
        string token = string.Empty;

        private string ultimaColunaOrdenada = string.Empty;
        private SortOrder direcaoOrdenacao = SortOrder.None;
        private FiltrosFaturasDto filtros = new FiltrosFaturasDto();
        private LoadingControl loadingControl;
        private List<Dado> dadosFiltrados = new List<Dado>();
        public FattureWebForm()
        {
            InitializeComponent();
            loadingControl = new LoadingControl();

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
            loadingControl.ShowLoading(this, "aguarde...");
        }

        private void StopLoading()
        {
            loadingControl.HideLoading(this);
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string columnName = gridFaturas.Columns[e.ColumnIndex].DataPropertyName;
            var dadosGrid = gridFaturas.DataSource as List<FaturasViewDto>;

            if (ultimaColunaOrdenada == columnName)
            {
                direcaoOrdenacao = (direcaoOrdenacao == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                direcaoOrdenacao = SortOrder.Ascending;
                ultimaColunaOrdenada = columnName;
            }

            if (direcaoOrdenacao == SortOrder.Ascending)
            {
                dadosGrid = dadosGrid.OrderBy(d => GetPropertyValue(d, columnName)).ToList();
            }
            else
            {
                dadosGrid = dadosGrid.OrderByDescending(d => GetPropertyValue(d, columnName)).ToList();
            }

            gridFaturas.DataSource = null;
            gridFaturas.DataSource = dadosGrid;
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
            dadosFiltrados = FiltrarDados();
            foreach (var dado in dadosFiltrados)
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
                fatura.qualquerCoisa = "teste";

                faturas.Add(fatura);
            }

            gridFaturas.DataSource = faturas;
            PreencherComboBoxInstalacao();
        }

        private void PreencherComboBoxInstalacao()
        {
            // Primeiro, limpe qualquer valor existente no ComboBox
            cmbInstalacao.Items.Clear();

            // Crie uma lista de valores únicos
            HashSet<string> instalacoesUnicas = new HashSet<string>() { "" };

            // Percorra as linhas do DataGridView para pegar os valores da coluna "Instalação"
            foreach (var dado in dados)
            {
                if (!string.IsNullOrEmpty(dado.Conteudo.UnidadeConsumidora.Instalacao))
                {
                    instalacoesUnicas.Add(dado.Conteudo.UnidadeConsumidora.Instalacao);
                }
            }
            // Adicione os valores únicos no ComboBox
            cmbInstalacao.Items.AddRange(instalacoesUnicas.ToArray());
            cmbInstalacao.SelectedItem = filtros.Instalacoes.Count > 0 ? filtros.Instalacoes.First() : "";
        }

        private List<Dado> FiltrarDados()
        {
            List<Dado> dadosFiltrados = dados;
            if (!string.IsNullOrEmpty(filtros.FaturaId))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.Conteudo.FaturaId.ToString() == filtros.FaturaId).ToList();
            }

            if (filtros.Instalacoes != null && filtros.Instalacoes.Count > 0)
            {
                dadosFiltrados = dadosFiltrados.Where(x => filtros.Instalacoes.Any(o => o == x.Conteudo.UnidadeConsumidora.Instalacao.ToString())).ToList();
            }

            if (filtros.MesReferencia.HasValue)
            {
                dadosFiltrados = dadosFiltrados.Where(x => DateTime.Parse(x.Conteudo.Fatura.MesReferencia) == filtros.MesReferencia.Value).ToList();
            }

            if (!string.IsNullOrEmpty(filtros.Distribuidora))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.Conteudo.Distribuidora.ToString() == filtros.Distribuidora).ToList();
            }

            if (!string.IsNullOrEmpty(filtros.IdInstalacao))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.InstalacaoId.ToString() == filtros.IdInstalacao).ToList();
            }

            if (!string.IsNullOrEmpty(filtros.ConsumoTotal))
            {
                dadosFiltrados = dadosFiltrados.Where(x => (x.Conteudo.Fatura.HistoricoFaturamento != null ? x.Conteudo.Fatura.HistoricoFaturamento.FirstOrDefault().EnergiaAtiva : 0).ToString() == filtros.ConsumoTotal).ToList();
            }

            if (filtros.DataEmissao.HasValue)
            {
                dadosFiltrados = dadosFiltrados.Where(x => DateTime.Parse(x.Conteudo.Fatura.DataEmissao) == filtros.DataEmissao.Value).ToList();
            }

            return dadosFiltrados;
        }

        public void CarregarDados()
        {
            token = realizarLogin();
            var objeto = GetFaturasPaginadoAsync();
            if (objeto != null)
            {
                dados = objeto.Dados.ToList();
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
                    requestFaturas.Headers.Add("Fatture-AuthToken", token);
                    requestFaturas.Headers.Add(
                        "Fatture-SearchFields",
                        "id, instalacao_id, arquivo_id, status_fatura_id, status, data_criacao, data_atualizacao, processamento_id, usuario_id, email_fatura_id, email_fatura_assunto, data_processamento, erro_processamento, mes_referencia, data_vencimento, valor_total, conteudo"
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
            filtros.Instalacoes.Clear();
            if (cmbInstalacao.SelectedItem != null && !string.IsNullOrEmpty(cmbInstalacao.SelectedItem.ToString()))
            {
                filtros.Instalacoes.Add(cmbInstalacao.SelectedItem.ToString());
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
            filtros = new FiltrosFaturasDto();
            PreencherGridFaturas();
        }

        private void gridFaturas_DataSourceChanged(object sender, EventArgs e)
        {
            AtualizarTotais();
        }

        private void AtualizarTotais()
        {
            txtQtdFaturas.Text = dados.Count.ToString();
            txtQtdFaturasFiltradas.Text = dadosFiltrados.Count().ToString();
        }
    }
}
