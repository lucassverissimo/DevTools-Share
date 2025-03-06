namespace DTSWindowsForm.UI.FattureWeb.dtos
{
    public class FiltrosFaturasDto
    {
        public string FaturaId { get; set; }
        public List<string> MesReferencia { get; set; }
        public List<string> Instalacao { get; set; }
        public List<string> Distribuidora { get; set; }
        public string IdInstalacao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string ConsumoTotal { get; set; }
    }
}
