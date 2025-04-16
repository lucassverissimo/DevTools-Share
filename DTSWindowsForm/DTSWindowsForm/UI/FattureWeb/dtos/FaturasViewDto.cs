


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
        public string? DataProcessamento { get; set; }
        public string? ConsumoTotal { get; set; }
        public double? EnergiaInjetada { get; set; }
        public decimal? ValorMuc { get; set; }
        public ModelosFaturasEnum ModeloFatura { get; set; }
        public string DebitoAutomatico { get; set; }
        public string DescricaoProdutos { get; set; }
        public string DescricoesOriginais { get; set; }
        public DateTime? DataApresentacao { get; set; }
        public DateTime? DataInsercaoFW { get; set; }
        public DateTime? DataProximaLeitura { get; set; }
        public double? EnquadramentoEnergiaPorcentagem { get; internal set; }
    }
}