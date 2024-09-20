using DTSWindowsForm.Config;
using DTSWindowsForm.UI.FattureWeb.dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DTSWindowsForm.UI.FattureWeb
{
    public partial class FattureWebForm : Form
    {
        List<Dado> dados = new List<Dado>();
        string token = string.Empty;
        Settings settings;
        private string ultimaColunaOrdenada = string.Empty;
        private SortOrder direcaoOrdenacao = SortOrder.None;
        private FiltrosFaturasDto filtros = new FiltrosFaturasDto();
        public FattureWebForm()
        {
            settings = Program.AppSettings;
            InitializeComponent();
            pcbLoading.Dock = DockStyle.Fill;
            gridFaturas.AllowUserToOrderColumns = true;
            foreach (DataGridViewColumn column in gridFaturas.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            gridFaturas.ColumnHeaderMouseClick += dataGridView_ColumnHeaderMouseClick;
            IniciaBackground();
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Obtenha o nome da coluna clicada
            string columnName = gridFaturas.Columns[e.ColumnIndex].DataPropertyName;
            var dadosGrid = gridFaturas.DataSource as List<FaturasViewDto>;
            // Se clicou na mesma coluna, alterne a ordem
            if (ultimaColunaOrdenada == columnName)
            {
                direcaoOrdenacao = (direcaoOrdenacao == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                // Se é uma nova coluna, ordene ascendente por padrão
                direcaoOrdenacao = SortOrder.Ascending;
                ultimaColunaOrdenada = columnName;
            }

            // Realize a ordenação com base na coluna clicada e a direção de ordenação
            if (direcaoOrdenacao == SortOrder.Ascending)
            {
                dadosGrid = dadosGrid.OrderBy(d => GetPropertyValue(d, columnName)).ToList();
            }
            else
            {
                dadosGrid = dadosGrid.OrderByDescending(d => GetPropertyValue(d, columnName)).ToList();
            }

            // Atualize a fonte de dados do DataGridView
            gridFaturas.DataSource = null;
            gridFaturas.DataSource = dadosGrid;
        }
        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
        private void IniciaBackground()
        {
            StartLoading();
            cmbBasesDisponiveis.Enabled = false;
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
            txtBaseSelecionada.Text = settings.TipoConta.GetEnumDescription();
            txtQtdFaturas.Text = dados.Count.ToString();
            if (dados.Count > 0)
            {
                var opcoes = new Dictionary<TipoContaEnum, string>
                {
                    { TipoContaEnum.Prod, TipoContaEnum.Prod.GetEnumDescription() },
                    { TipoContaEnum.Dev, TipoContaEnum.Dev.GetEnumDescription() },
                    { TipoContaEnum.Qa, TipoContaEnum.Qa.GetEnumDescription() }
                }.Where(opt => settings.TipoConta != opt.Key).Select(opt => opt.Value);
                foreach (var opco in opcoes)
                {
                    cmbBasesDisponiveis.Items.Add(opco);
                }
                cmbBasesDisponiveis.Enabled = true;
            }
        }

        private void PreencherGridFaturas()
        {
            List<FaturasViewDto> faturas = new List<FaturasViewDto>();
            var dadosFiltrados = FiltrarDados();
            foreach (var dado in dadosFiltrados)
            {
                DateTime dataMesRef = DateTime.Parse(dado.Conteudo.Fatura.MesReferencia);
                var consumoTotal = dado.Conteudo.Fatura.HistoricoFaturamento != null ? dado.Conteudo.Fatura.HistoricoFaturamento.FirstOrDefault().EnergiaAtiva : 0;
                FaturasViewDto fatura = new FaturasViewDto(
                    FaturaId: dado.Conteudo.FaturaId.ToString(),
                    Instalacao: dado.Conteudo.UnidadeConsumidora.Instalacao,
                    MesReferencia: dataMesRef.ToString("MMyyyy"),
                    Distribuidora: dado.Conteudo.Distribuidora.ToString(),
                    IdInstalacao: dado.InstalacaoId.ToString(),
                    ConsumoTotal: consumoTotal.ToString(),
                    DataEmissao: dado.Conteudo.Fatura.DataEmissao, qualquerCoisa: ""
                );
                faturas.Add(fatura);
            }

            gridFaturas.DataSource = faturas;


        }

        private List<Dado> FiltrarDados()
        {
            List<Dado> dadosFiltrados = dados;
            if (!string.IsNullOrEmpty(filtros.FaturaId))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.Conteudo.FaturaId.ToString() == filtros.FaturaId).ToList();
            }

            if (!string.IsNullOrEmpty(filtros.Instalacao))
            {
                dadosFiltrados = dadosFiltrados.Where(x => x.Conteudo.UnidadeConsumidora.Instalacao.ToString() == filtros.Instalacao).ToList();
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

        private void StartLoading()
        {
            pcbLoading.Visible = true;
        }

        private void StopLoading()
        {
            pcbLoading.Visible = false;
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
                { "email", settings.getUsuario() },
                { "senha", settings.getSenha() }
            };
            var contentToken = new StringContent(jsonRequestToken.ToString(), null, "application/json");
            requestToken.Content = contentToken;
            var response = clientToken.Send(requestToken);
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonResponse = JObject.Parse(responseBody);

            if (!jsonResponse.HasValues)
            {
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
            if (cmbBasesDisponiveis.SelectedItem != null)
            {
                string itemSelecionado = cmbBasesDisponiveis.SelectedItem.ToString();
                settings.TipoConta = EnumExtensions.GetEnumByDescription<TipoContaEnum>(itemSelecionado);
                cmbBasesDisponiveis.Items.Clear();
                IniciaBackground();
            }
        }

        private void btnDownloadCsv_Click(object sender, EventArgs e)
        {
            string outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");

            string fileName = $"faturas{settings.TipoConta.GetEnumDescription()}_{timestamp}.csv";
            string filePath = Path.Combine(outputDir, fileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"\"dado.Conteudo.FaturaId\",\"dado.Conteudo.UnidadeConsumidora.Instalacao\",\"dado.Conteudo.Fatura.MesReferencia\",\"dado.Conteudo.Distribuidora\",\"idInstalacao\",\"consumo_total\",\"data_emissao\"");
                foreach (var dado in dados)
                {
                    DateTime dataMesRef = DateTime.Parse(dado.Conteudo.Fatura.MesReferencia);
                    var consumoTotal = dado.Conteudo.Fatura.HistoricoFaturamento != null ? dado.Conteudo.Fatura.HistoricoFaturamento.FirstOrDefault().EnergiaAtiva : 0;
                    writer.WriteLine($"\"{dado.Conteudo.FaturaId}\",\"{dado.Conteudo.UnidadeConsumidora.Instalacao}\",\"{dataMesRef.ToString("MMyyyy")}\",\"{dado.Conteudo.Distribuidora}\",\"{dado.InstalacaoId}\",\"{consumoTotal}\",\"{dado.Conteudo.Fatura.DataEmissao}\"");
                }
            }

            MessageBox.Show("Exportação concluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Pergunta ao usuário se deseja abrir o diretório
            DialogResult result = MessageBox.Show("Deseja abrir o diretório onde o arquivo foi salvo?", "Abrir Diretório", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(filePath));
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            filtros.Instalacao = txtFiltroInstalacao.Text;
            PreencherGridFaturas();
        }
    }
}
