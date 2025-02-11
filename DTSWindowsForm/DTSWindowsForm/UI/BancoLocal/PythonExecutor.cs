using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSWindowsForm.UI.BancoLocal
{
    public class PythonExecutor
    {
        public static async Task Run(string scriptPath, string fileName, string logFile)
        {
            if (!File.Exists(scriptPath))
            {
                await File.WriteAllTextAsync(logFile, "O arquivo do script Python não foi encontrado.");
                return;
            }

            var start = new ProcessStartInfo
            {
                FileName = "\"C:\\Users\\victo\\AppData\\Local\\Programs\\Python\\Python313\\python.exe\"", // Ou o caminho completo para o executável Python, se necessário
                Arguments = scriptPath, // Caminho completo do script Python
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(start))
            {
                if (process == null)
                {
                    await File.WriteAllTextAsync(logFile, $"Falha ao iniciar o script {fileName}\n");
                    return;
                }

                await File.WriteAllTextAsync(logFile, "Execute Python script execution...\n");
                using (var reader = process.StandardOutput)
                {
                    string result = await reader.ReadToEndAsync();

                    await File.WriteAllTextAsync(logFile, $"{result}\n");
                }

                using (var errorReader = process.StandardError)
                {
                    string errors = await errorReader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(errors))
                    {
                        await File.WriteAllTextAsync(logFile, $"Erros: {errors}\n");
                    }
                }
            }
        }
    }
}
