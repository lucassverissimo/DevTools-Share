namespace DTSWindowsForm.UI.FattureWeb.dtos
{
    public record FaturasViewDto()
    {
        public string? FaturaId { get; set; }
        public string? Instalacao { get; set; }
        public string? MesReferencia { get; set; }
        public string? Distribuidora { get; set; }
        public string? IdInstalacao { get; set; }
        public string? DataEmissao { get; set; }
        public string? ConsumoTotal { get; set; }
    }
}