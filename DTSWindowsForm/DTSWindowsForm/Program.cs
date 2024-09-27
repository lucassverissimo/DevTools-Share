using DTSWindowsForm.Config;
using Microsoft.Extensions.Configuration;
namespace DTSWindowsForm;

internal static class Program
{
    public static IConfiguration Configuration { get; private set; }
    public static string OutputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
    public static Settings AppSettings { get; private set; }
    [STAThread]
    static void Main()
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();

        // Bind dos valores do appsettings.json no objeto Settings
        AppSettings = new Settings
        {
            Connection = Configuration["Settings:Connection"],
            Usuario = Configuration["Settings:Usuario"],
            Senha = Configuration["Settings:Senha"]
        };

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmDTS());
    }
}
