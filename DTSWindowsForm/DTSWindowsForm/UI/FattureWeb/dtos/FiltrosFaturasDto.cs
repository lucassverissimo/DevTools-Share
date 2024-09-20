namespace DTSWindowsForm.UI.FattureWeb.dtos
{
    public class FiltrosFaturasDto
    {
        public string FaturaId { get; set; }
        public string Instalacao { get; set; }
        public DateTime? MesReferencia { get; set; }
        public string Distribuidora { get; set; }
        public string IdInstalacao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string ConsumoTotal { get; set; }
    }
}
