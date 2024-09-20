namespace DTSWindowsForm.UI.FattureWeb.dtos
{
    public record FaturasViewDto(
        string FaturaId,
        string Instalacao,
        string MesReferencia,
        string Distribuidora,
        string IdInstalacao,
        string DataEmissao,
        string ConsumoTotal,
        string qualquerCoisa);
}