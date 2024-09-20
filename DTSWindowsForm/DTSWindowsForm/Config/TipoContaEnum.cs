using System.ComponentModel;

namespace DTSWindowsForm.Config;

public enum TipoContaEnum : int
{
    [Description("PRODUCAO")]
    Prod = 0,
    [Description("DEV")]
    Dev = 1,
    [Description("QA")]
    Qa = 2
}
