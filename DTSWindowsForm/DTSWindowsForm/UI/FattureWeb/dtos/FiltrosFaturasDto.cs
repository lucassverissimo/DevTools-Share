namespace DTSWindowsForm.UI.FattureWeb.dtos
{
    public class FiltrosFaturasDto
    {
        public string FaturaId { get; set; }
        public List<string> Instalacoes { get; set; } = new List<string>();
        public DateTime? MesReferencia { get; set; }
        public string Distribuidora { get; set; }
        public string IdInstalacao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string ConsumoTotal { get; set; }
    }
}
