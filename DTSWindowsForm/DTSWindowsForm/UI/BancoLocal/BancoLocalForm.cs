using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace DTSWindowsForm.UI.BancoLocal
{
    public partial class BancoLocalForm : Form
    {
        public BancoLocalForm()
        {
            InitializeComponent();
        }

        public async Task GravarLog(string mensagem, TipoLog tipoLog = TipoLog.Info)
        {

            if (rtbResultado.InvokeRequired)
            {
                rtbResultado.Invoke(new Action(() => GravarLog(mensagem)));
                return;
            }

            rtbResultado.SelectionStart = rtbResultado.TextLength;
            rtbResultado.SelectionLength = 0;

            switch (tipoLog)
            {
                case TipoLog.Error:
                    rtbResultado.SelectionColor = Color.Red;
                    break;
                case TipoLog.Success:
                    rtbResultado.SelectionColor = Color.Blue;
                    break;
                case TipoLog.Info:
                    rtbResultado.SelectionColor = Color.DarkGray;
                    break;
                default:
                    rtbResultado.SelectionColor = Color.DarkGray;
                    break;
            }



            rtbResultado.AppendText($"{DateTime.Now:HH:mm:ss} - {mensagem}{Environment.NewLine}");
            rtbResultado.SelectionColor = rtbResultado.ForeColor;

            if (ckbAtualizacaoAutomatica.Checked)
            {
                rtbResultado.ScrollToCaret();
            }

            Application.DoEvents();
        }

        private async void btnImportar_Click(object sender, EventArgs e)
        {
            string projectRoot = GetProjectRootDirectory();

            if (string.IsNullOrEmpty(projectRoot))
            {
                await GravarLog("Could not locate the root directory of the project.", TipoLog.Error);
                return;
            }

            // Configura o carregamento do appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectRoot)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Obtém os dados de conexão do appsettings.json
            var connection = configuration.GetSection("ConnectionStrings:DefaultConnection");
            string server = connection["Server"];
            string database = connection["Database"];
            string username = connection["Username"];
            string password = connection["Password"];

            // Diretório dos scripts SQL
            string sqlDir = Path.Combine(projectRoot, "scripts");

            // Obtém data e hora atual para log
            string timestamp = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            string logDir = Path.Combine(projectRoot, "logs");
            string logFile = Path.Combine(logDir, $"logfile_exec_{timestamp}.log");

            // Checa a conexão com o banco de dados
            if (!CheckDatabaseConnection(server, database, username, password))
            {
                await GravarLog("Failed to connect to the database. Please check your configuration details.", TipoLog.Error);
                return;
            }

            await GravarLog("Successfully connected to the database.", TipoLog.Success);

            string pathScript = Path.Combine(projectRoot, "set_identity.py");
            //await PythonExecutor.Run(pathScript, "set_identity.py", logFile);

            await ExecuteStartScripts(projectRoot, logFile, server, database, username, password);
            await ExecuteScripts(sqlDir, logFile, server, database, username, password);
            await ExecuteEndScripts(projectRoot, logFile, server, database, username, password);
        }

        static string GetProjectRootDirectory()
        {
            string baseDirectory = AppContext.BaseDirectory;
            DirectoryInfo directoryInfo = new DirectoryInfo(baseDirectory);
            while (directoryInfo != null && !File.Exists(Path.Combine(directoryInfo.FullName, "DTSWindowsForm.csproj")))
            {
                directoryInfo = directoryInfo.Parent;
            }

            return directoryInfo?.FullName;
        }

        static bool CheckDatabaseConnection(string server, string database, string username, string password)
        {
            string sqlCmdCommand = $"sqlcmd -S {server} -d {database} -U {username} -P {password} -Q \"SELECT 1\"";

            var processInfo = new ProcessStartInfo("cmd.exe", $"/c {sqlCmdCommand}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processInfo);
            process.WaitForExit();

            return process.ExitCode == 0;
        }

        async Task ExecuteStartScripts(string projectRoot, string logFile, string server, string database, string username, string password)
        {
            await GravarLog("Execute Start SQL script execution...");

            string startSqlDir = Path.Combine(projectRoot, "start.sql");
            await ExecuteSqlFile(startSqlDir, logFile, server, database, username, password);

            await GravarLog("START SQL script execution completed.", TipoLog.Success);
        }

        async Task ExecuteScripts(string sqlDir, string logFile, string server, string database, string username, string password)
        {
            await GravarLog("Starting SQL script execution...");

            foreach (var sqlFile in Directory.GetFiles(sqlDir, "*.sql"))
            {
                await ExecuteSqlFile(sqlFile, logFile, server, database, username, password);
            }

            await GravarLog("SQL script execution completed.", TipoLog.Success);
        }

        async Task ExecuteEndScripts(string projectRoot, string logFile, string server, string database, string username, string password)
        {
            await GravarLog("END SQL script execution completed.");
            string endSqlDir = Path.Combine(projectRoot, "end.sql");
            await ExecuteSqlFile(endSqlDir, logFile, server, database, username, password);
            await GravarLog("END SQL script execution completed.", TipoLog.Success);
        }

        async Task ExecuteSqlFile(string sqlFile, string logFile, string server, string database, string username, string password)
        {
            if (File.Exists(sqlFile))
            {
                await GravarLog($"Executing {sqlFile}...");


                string sqlCmdCommand = $"sqlcmd -S {server} -d {database} -U {username} -P {password} -i \"{sqlFile}\" -b";
                var processInfo = new ProcessStartInfo("cmd.exe", $"/c {sqlCmdCommand}")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = Process.Start(processInfo);
                if (process != null)
                {
                    int timeout = 3 * 1000;
                    process.WaitForExit(timeout);

                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();
                    if (process.ExitCode == 0)
                    {
                        await GravarLog($"Successfully executed {sqlFile}", TipoLog.Success);
                    }
                    else
                    {
                        await GravarLog($"Error executing {sqlFile}:#OUTPUT: {output} #ERROR: {error}", TipoLog.Error);
                    }
                }

            }
            else
            {
                await GravarLog($"{sqlFile} not found!", TipoLog.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txbSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Chama o método para procurar o termo
                SearchAllOccurrences(searchTerm);
            }else
            {
                ClearSearchHighlight();
            }
        }
        private int lastSearchPosition = 0;
        private void SearchLog(string searchTerm)
        {
            // Se não houver nenhum termo a ser pesquisado
            if (string.IsNullOrEmpty(searchTerm)) return;

            // Define o início da pesquisa
            int startIndex = lastSearchPosition;

            // Faz a busca, começando pela posição onde parou da última pesquisa
            int index = rtbResultado.Find(searchTerm, startIndex, RichTextBoxFinds.None);

            // Se o termo for encontrado
            if (index != -1)
            {
                // Define a posição para a próxima pesquisa
                lastSearchPosition = index + searchTerm.Length;

                // Seleciona o texto encontrado e aplica a cor de destaque
                rtbResultado.Select(index, searchTerm.Length);
                rtbResultado.SelectionBackColor = Color.Yellow; // Cor de fundo do texto selecionado
            }
            else
            {
                MessageBox.Show("Texto não encontrado.");
            }
        }

        private void SearchAllOccurrences(string searchTerm)
        {
            // Se não houver nenhum termo a ser pesquisado
            if (string.IsNullOrEmpty(searchTerm)) return;

            // Limpa qualquer destaque anterior
            ClearSearchHighlight();

            // Variável de controle para realizar a busca de todas as ocorrências
            int startIndex = 0;

            // Continuar buscando até que todas as ocorrências sejam encontradas
            while (startIndex < rtbResultado.TextLength)
            {
                // Procura a próxima ocorrência da palavra
                int index = rtbResultado.Find(searchTerm, startIndex, RichTextBoxFinds.None);

                // Se uma ocorrência for encontrada
                if (index != -1)
                {
                    // Seleciona o texto e aplica a cor de destaque
                    rtbResultado.Select(index, searchTerm.Length);
                    rtbResultado.SelectionBackColor = Color.Yellow;

                    // Move o início da pesquisa para depois da ocorrência encontrada
                    startIndex = index + searchTerm.Length;
                }
                else
                {
                    // Se não houver mais ocorrências, sai do loop
                    break;
                }
            }
        }

        private void ClearSearchHighlight()
        {
            rtbResultado.BackColor = Color.White;
            rtbResultado.DeselectAll();
        }

        public enum TipoLog
        {
            Error,
            Success,
            Info
        }
    }
}
