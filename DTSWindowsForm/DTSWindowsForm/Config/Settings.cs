namespace DTSWindowsForm.Config;

public sealed class Settings
{
    public required string Connection { get; set; }
    public required string Usuario { get; set; }
    public required string Senha { get; set; }

    public Settings()
    {

    }
}
