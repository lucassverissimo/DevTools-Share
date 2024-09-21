using ClosedXML.Excel;

namespace DTSWindowsForm.Extensions
{
    public static class DataGridViewExtensions
    {
        /// <summary>
        /// Exporta os dados de um DataGridView para um arquivo Excel (.xlsx) com colunas formatadas.
        /// Aplica estilos de cabeçalhos em verde claro com texto em negrito e preto, ajusta automaticamente
        /// a largura das colunas e adiciona filtros nos cabeçalhos.
        /// </summary>
        /// <param name="grid">DataGridView contendo os dados a serem exportados.</param>
        /// <param name="prefixFileName">Prefixo do nome do arquivo gerado. Se vazio, será utilizado "DevToolsShare".</param>
        /// <param name="outputPath">Diretório onde o arquivo será salvo. Se vazio, será salvo em uma pasta "output" no diretório do executável.</param>
        /// <returns>True se o arquivo foi exportado com sucesso, False em caso de erro.</returns>
        /// <exception cref="ArgumentException">Lançada se o DataGridView estiver vazio ou nulo.</exception>
        public static bool ExportToXls(this DataGridView grid, string prefixFileName, string outputPath)
        {
            try
            {
                if (grid == null || grid.Rows.Count == 0)
                {
                    throw new ArgumentException("O DataGridView está vazio ou nulo.");
                }

                if (string.IsNullOrWhiteSpace(prefixFileName))
                {
                    prefixFileName = "DevToolsShare";
                }

                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");

                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                }

                string fileName = $"{prefixFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string fullPath = Path.Combine(outputPath, fileName);


                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(prefixFileName);
                    var headerStyle = worksheet.Range(1, 1, 1, grid.Columns.Count).Style;
                    headerStyle.Font.Bold = true;
                    headerStyle.Font.FontColor = XLColor.White;
                    headerStyle.Fill.BackgroundColor = XLColor.BlueGray;

                    for (int col = 0; col < grid.Columns.Count; col++)
                    {
                        worksheet.Cell(1, col + 1).Value = grid.Columns[col].HeaderText;
                    }

                    for (int row = 0; row < grid.Rows.Count; row++)
                    {
                        for (int col = 0; col < grid.Columns.Count; col++)
                        {
                            worksheet.Cell(row + 2, col + 1).Value = grid.Rows[row].Cells[col].Value?.ToString() ?? string.Empty;
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().SetAutoFilter();
                    workbook.SaveAs(fullPath);
                }

                MessageBox.Show("Exportação concluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult result = MessageBox.Show("Deseja abrir o diretório onde o arquivo foi salvo?", "Abrir Diretório", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(fullPath));
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao exportar para Excel: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
